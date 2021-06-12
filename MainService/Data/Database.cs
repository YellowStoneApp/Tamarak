using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainService.Data.DataClasses;
using MainService.Data.DBModels;
using MainService.Data.Errors;
using MainService.Data.Utils;
using MainService.ExceptionHandling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MainService.Data
{

    /// <summary>
    /// This class makes explicit calls to the database.
    /// This should not hit any other external provider. All calls to any other external provider should be made outside of this class.
    /// </summary>
    public class Database : IDatabase
    {
        private TamarakDbContext _dbContext { get; set; }
        private ILogger<Database> _logger { get; set; }

        public Database(TamarakDbContext context, ILogger<Database> logger) 
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<Customer> GetUserByIdentityKey(string key)
        {
            Customer customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.IdentityKey == key);

            if (customer == null)
            {
                throw new DBReadException($"Cannot find IdentityKey {key}.");
            }

            return customer;
        }

        public async Task CreateFollowerRelationship(string userId, string userToFollowId)
        {
            Customer customer = await _dbContext.Customers.FirstOrDefaultAsync(user => user.IdentityKey == userId);

            Customer toFollow = await _dbContext.Customers.FirstOrDefaultAsync(user => user.IdentityKey == userToFollowId);

            CustomerFollowers customerFollower = new CustomerFollowers
            {
                Customer = customer,
                FollowedCustomer = toFollow
            };

            _dbContext.CustomerFollowers.Add(customerFollower);

            _dbContext.SaveChanges();
        }

        public async Task<Customer> UpdateUserAvatar(string userIdentity, string avatarUrl)
        {
            var user = _dbContext.Customers.FirstOrDefault(user => user.IdentityKey == userIdentity);

            if (user == null)
            {
                throw new Exception($"Cannot find user corresponding to {userIdentity} for UpdateAvatar");
            }

            user.Avatar = avatarUrl;

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Customer> GetProfileByUserName(string userName)
        {
            var user = await _dbContext.Customers.FirstOrDefaultAsync(u => u.Name == userName);

            if (user == null)
            {
                throw new DBReadException($"Username {userName} does not exist");
            }

            return user;
        }

        public Task DeleteFollowerRelationship(string userIdentity, string userToFollowId)
        {
            throw new System.NotImplementedException();
        }
        
        public async Task<Gift> RegisterGift(string customerId, GiftRequest giftRequest)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.IdentityKey == customerId);

            if (customer == null)
            {
                throw new DBReadException($"User does not exist");
            }

            var gift = new Gift();

            PropertyCopy<GiftRequest, Gift>.Copy(giftRequest, gift);

            await _dbContext.Gifts.AddAsync(gift);

            var giftCustomer = new CustomerGift();
            
            PropertyCopy<GiftRequest, CustomerGift>.Copy(giftRequest, giftCustomer);

            giftCustomer.Customer = customer;

            giftCustomer.Gift = gift;
            
            giftCustomer.DateAdded = DateTime.Now;

            giftCustomer.GiftState = GiftState.OpenForPurchase;

            await _dbContext.CustomerGifts.AddAsync(giftCustomer);

            await _dbContext.SaveChangesAsync();

            return gift;
        }

        public async Task<bool> CustomerExists(string customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customerId);

            return customer != null;
        }

        public async Task<Customer> SoftUpdateCustomerInformation(string customerId, Customer customer)
        {
            ValidateCustomerInformation(customer);

            // If customer has been manually edited we don't want to squash manual changes with a soft update like this.
            if (customer.ManuallyEdited)
            {
                return customer;
            }

            var currentRegistered = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customerId);

            if (currentRegistered == null)
            {
                throw new InvalidParametersException($"No customer registered with id {customerId}.");
            }

            currentRegistered.Avatar = customer.Avatar;
            currentRegistered.Name = customer.Name;
            currentRegistered.Email = customer.Email;

            await _dbContext.SaveChangesAsync();

            return currentRegistered;
        }

        public async Task<IEnumerable<GiftResponse>> GetGiftsForCustomer(string customerId)
        {
            var internalCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customerId);

            if (internalCustomer == null)
            {
                throw new InvalidParametersException($"Could not find customer id: {customerId}");
            }

            var customerGifts = await _dbContext.CustomerGifts
                .Where(cg => cg.Customer == internalCustomer && cg.GiftState != GiftState.DeletedByCustomer)
                .Include(cg => cg.Gift)
                .ToListAsync();

            var giftResponses = new List<GiftResponse>();
            
            foreach (var customerGift in customerGifts)
            {
                giftResponses.Add(new GiftResponse()
                {
                    CustomDescription = customerGift.CustomDescription,
                    DateAdded = customerGift.DateAdded,
                    Description = customerGift.Gift.Description,
                    Id = customerGift.Gift.Id,
                    Image = customerGift.Gift.Image,
                    Quantity = customerGift.Quantity,
                    Title = customerGift.Gift.Title,
                    Url = customerGift.Gift.Url,
                    Vendor = customerGift.Gift.Vendor,
                });
            }

            return giftResponses;
        }


        public async Task<Customer> GetCustomerProfilePrivate(string customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customerId);

            if (customer == null)
            {
                throw new InvalidParametersException($"Could not find customer id: {customerId}");
            }
            
            return customer;
        }

        public async Task<CustomerPublic> GetCustomerProfile(string customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customerId);

            if (customer == null)
            {
                throw new InvalidParametersException($"Could not find customer id: {customerId}");
            }

            var publicCustomer = new CustomerPublic();
            
            PropertyCopy<Customer, CustomerPublic>.Copy(customer, publicCustomer);
            
            return publicCustomer;
        }

        public async Task<Customer> UpdateCustomerInformation(Customer customer)
        {
            var current = await _dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityKey == customer.IdentityKey);

            if (customer == null)
            {
                throw new InvalidParametersException($"Could not find customer id: {customer.IdentityKey}");
            }
            
            // explicitly not doing property copy here because not all things _should be_ updated on the fly like this....
            // like email
            current.Avatar = customer.Avatar;
            current.Bio = customer.Bio;
            current.Name = customer.Name;
            current.ManuallyEdited = true;

            await _dbContext.SaveChangesAsync();

            return current;
        }

        public async Task<IEnumerable<GiftResponse>> RemoveGiftForCustomer(string customerId, GiftRemove giftRemove)
        {
            var customerGift = await _dbContext.CustomerGifts
                .FirstOrDefaultAsync(cg => cg.Customer.IdentityKey == customerId && cg.Gift.Id == giftRemove.Id);

            customerGift.GiftState = GiftState.DeletedByCustomer;

            await _dbContext.SaveChangesAsync();

            return await GetGiftsForCustomer(customerId);
        }

        public async Task<CustomerGiftPurchase> RegisterGiftPurchase(GiftPurchaseRequest giftPurchase)
        {
            if (giftPurchase.PurchaserEmail == null || giftPurchase.ReceiverIdentityKey == null)
            {
                throw new InvalidParametersException(
                    $"Could not get PurchaserEmail: {giftPurchase.PurchaserEmail} or Receiver's id: {giftPurchase.ReceiverIdentityKey}.");
            }

            var customerGift = await _dbContext.CustomerGifts
                    .FirstOrDefaultAsync(
                        cg => cg.Customer.IdentityKey == giftPurchase.ReceiverIdentityKey && cg.Gift.Id == giftPurchase.GiftId);
            
            if (customerGift == null)
            {
                throw new InvalidParametersException(
                    $"Could not find customer gift for customer: {giftPurchase.ReceiverIdentityKey} and gift: ${giftPurchase.GiftId}");
            }

            var customerGiftPurchase = new CustomerGiftPurchase()
            {
                CustomerGift = customerGift,
                ConfirmationRequestTimestamp = null,
                ConfirmedPurchase = false,
                PurchaserEmail = giftPurchase.PurchaserEmail,
                GiftPurchaseRequestTimestamp = DateTime.Now,
            };
            await _dbContext.CustomerGiftPurchases.AddAsync(customerGiftPurchase);
            await _dbContext.SaveChangesAsync();
            return customerGiftPurchase;
        }

        public async Task<Customer> RegisterCustomer(string userIdentity, Customer customer)
        {
            ValidateCustomerInformation(customer);

            customer.IdentityKey = userIdentity;

            await _dbContext.Customers.AddAsync(customer);

            await _dbContext.SaveChangesAsync();

            return customer;
        }

        private void ValidateCustomerInformation(Customer customer)
        {
            // this might need to become log statements. 
            if (customer.Name == null)
            {
                throw new InvalidParametersException("No customer name present in login.");
            }
            if (customer.Email == null)
            {
                throw new InvalidParametersException("No customer email present in login.");
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainService.Data.DataClasses;
using MainService.Data.DBModels;

namespace MainService.Data
{
    
    /// <summary>
    /// Interface for Db object. 
    /// 
    /// Validation should be done in the db function. If there is an error the DB function should throw that error. 
    /// There should be no return type validation.
    /// 
    /// </summary>
    public interface IDatabase
    {
        Task<Customer> GetUserByIdentityKey(string key);
        Task CreateFollowerRelationship(string userIdentity, string userToFollowId);
        Task DeleteFollowerRelationship(string userIdentity, string userToFollowId);
        Task<Customer> RegisterCustomer(string userIdentity, Customer customer);
        Task<Customer> UpdateUserAvatar(string userIdentity, string avatarUrl);
        Task<Customer> GetProfileByUserName(string userName);
        Task<Gift> RegisterGift(string userId, GiftRequest gift);
        Task<bool> CustomerExists(string customerId);
        Task<Customer> SoftUpdateCustomerInformation(string customerId, Customer customer);
        Task<IEnumerable<GiftResponse>> GetGiftsForCustomer(string customerId);
        Task<CustomerPublic> GetCustomerProfile(string customerId);
        Task<Customer> GetCustomerProfilePrivate(string customerId);
        Task<Customer> UpdateCustomerInformation(Customer customer);
        Task<IEnumerable<GiftResponse>> RemoveGiftForCustomer(string customerId, GiftRemove giftRemove);
        Task<CustomerGiftPurchase> RegisterGiftPurchase(GiftPurchaseRequest giftPurchase);
    }
}
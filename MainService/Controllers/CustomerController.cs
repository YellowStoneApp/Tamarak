using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MainService.Data;
using MainService.Data.DBModels;
using System.Threading.Tasks;
using MainService.ReturnObjects;

namespace MainService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly IDatabase _database;

        public CustomerController(ILogger<CustomerController> logger, IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            _logger.LogInformation($"Getting customer");
            
           var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
           var customer = await _database.GetCustomerProfilePrivate(customerId);

           return new OkObjectResult(customer);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ObjectResult> UpdateCustomer([FromBody] Customer customer)
        {
           _logger.LogInformation($"Updating customer {customer.IdentityKey}");

           var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
           if (customerId != customer.IdentityKey)
           {
               var message = "Customer identity key does not match authenticated customer identity key";
               _logger.LogError(message);
               return new BadRequestObjectResult(message);
           }

           var updatedCustomer = await _database.UpdateCustomerInformation(customer);

           return new OkObjectResult(updatedCustomer);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ObjectResult> RegisterCustomer([FromBody] Customer customer)
        {
            _logger.LogInformation($"Registering customer {customer.Name}, {customer.Avatar}, {customer.Email}");
            
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            _logger.LogInformation($"For customer id {customerId}.");

            Customer registered;
            
            if (await _database.CustomerExists(customerId))
            {
                _logger.LogInformation("Customer exists updating information");
                // this only updates if values haven't been updated by customer
                registered = await _database.SoftUpdateCustomerInformation(customerId, customer);
            }
            else
            {
                _logger.LogInformation("Customer does not exist creating");
                registered = await _database.RegisterCustomer(customerId, customer);
            }

            return new OkObjectResult(registered);
        }
    }
}
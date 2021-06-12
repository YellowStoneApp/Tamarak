using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MainService.Data;
using MainService.Data.DBModels;
using System.Threading.Tasks;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerPublicController : ControllerBase
    {

        private readonly ILogger<CustomerPublicController> _logger;
        private readonly IDatabase _database;

        public CustomerPublicController(ILogger<CustomerPublicController> logger, IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public async Task<OkObjectResult> Get(string customerId)
        {
            _logger.LogInformation($"Getting customer profile for {customerId}");
            
            var customer = await _database.GetCustomerProfile(customerId);

            _logger.LogInformation($"Got Profile for customer {customer.Name}");
            
            return new OkObjectResult(customer);
        }
    }
}
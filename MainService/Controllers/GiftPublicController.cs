using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MainService.Data;
using MainService.Data.DBModels;
using System.Threading.Tasks;
using MainService.Data.DataClasses;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftPublicController : ControllerBase
    {

        private readonly ILogger<GiftPublicController> _logger;
        private readonly IDatabase _database;

        public GiftPublicController(ILogger<GiftPublicController> logger, IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public async Task<OkObjectResult> Get(string customerId)
        {
            _logger.LogInformation($"Getting Gifts for customer {customerId}.");

            var gifts = await _database.GetGiftsForCustomer(customerId);
            
            return new OkObjectResult(gifts);
        }

        [HttpPost]
        [Route("registerPurchase")]
        public async Task<ObjectResult> RegisterPurchase([FromBody] GiftPurchaseRequest giftPurchaseRequest)
        {
            _logger.LogInformation($"Registering gift purchase for gift: {giftPurchaseRequest.GiftId} by: {giftPurchaseRequest.PurchaserEmail}");

            var result = await _database.RegisterGiftPurchase(giftPurchaseRequest);

            _logger.LogInformation("Registered Purchase");
            
            return new OkObjectResult(result);
        }
    }
}
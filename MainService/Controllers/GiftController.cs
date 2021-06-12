using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MainService.Data;
using MainService.Data.DBModels;
using System.Threading.Tasks;
using MainService.Data.DataClasses;
using MainService.UrlUnderstanding;

namespace MainService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : ControllerBase
    {

        private readonly ILogger<GiftController> _logger;
        private readonly IDatabase _database;
        private readonly IUrlProvider _urlProvider;

        public GiftController(ILogger<GiftController> logger, IDatabase database, IUrlProvider urlProvider)
        {
            _logger = logger;
            _database = database;
            _urlProvider = urlProvider;
        }

        [HttpPost]
        [Route("register")]
        public async Task<OkObjectResult> RegisterGift([FromBody] GiftRequest gift)
        {
            _logger.LogInformation($"Registering gift Url {gift.Url}");
            
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            _logger.LogInformation($"For User {customerId}.");
            
            var urlInfo = _urlProvider.Extract(gift.Url);
            
            gift.Vendor = urlInfo.Vendor;

            var registeredGift = await _database.RegisterGift(customerId, gift);

            return new OkObjectResult(registeredGift);
        }

        [HttpPost]
        [Route("remove")]
        public async Task<OkObjectResult> RemoveGift([FromBody] GiftRemove giftRemove)
        {
            _logger.LogInformation($"Removing gift {giftRemove.Id}");
            
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            _logger.LogInformation($"For User {customerId}.");
            
            var updateGifts = await _database.RemoveGiftForCustomer(customerId, giftRemove);
            
            return new OkObjectResult(updateGifts);
        }
    }
}
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MainService.Data;
using MainService.ReturnObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IDatabase _database;

        public ProfileController(ILogger<ProfileController> logger, IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public async Task<OkObjectResult> Get() 
        {
            _logger.Log(LogLevel.Information, "Getting user profile");

            // for now just return the current user's profile
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                var printable = User.ToString();
                throw new Exception($"Cannot find Current authorized user in Claims {printable}");
            }

            var user = await _database.GetUserByIdentityKey(userId);

            return new OkObjectResult(user);


        }

        [HttpGet]
        [Route("user")]
        public async Task<OkObjectResult> GetProfileForUser(string username)
        {
            if (username == null)
            {
                _logger.LogError("No username provided in request");
                return new OkObjectResult(new InvalidRequest("No username provided in request"));
            }

            var profile = await _database.GetProfileByUserName(username);

            return new OkObjectResult(profile);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainService.Controllers
{
    [Route("/")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public OkObjectResult Get()
        {
            _logger.LogInformation("Health Endpoint Hit");
            return new OkObjectResult("Healthy");
        }

    }
}
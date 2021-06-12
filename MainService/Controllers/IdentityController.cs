using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MainService
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
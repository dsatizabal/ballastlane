using Microsoft.AspNetCore.Mvc;

namespace BallastLane.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("OK");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        // GET /health
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new { Status = "API is running", Time = DateTime.Now });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Services;
using System.Text.RegularExpressions;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET /Weather/{city}
        // Retrieves weather data for a specific city
        [HttpGet("{city}")]
        public async Task<ActionResult<WeatherResponse>> GetWeather(string city)
        {
            // ===== Validation =====
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(new { Message = "City is required." });

            // Only English letters and spaces are allowed.
            if (!Regex.IsMatch(city, @"^[a-zA-Z\s]+$"))
                return BadRequest(new { Message = "City name must contain only letters." });

            try
            {
                var result = await _weatherService.GetWeatherAsync(city);

                if (result == null)
                    return NotFound(new { Message = $"City '{city}' not found." });

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                // If the external API cannot be reached
                return StatusCode(503, new { Message = "Unable to reach weather service.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // GET /Weather/recent
        // Retrieves the last 5 searched cities
        [HttpGet("recent")]
        public ActionResult<List<string>> GetRecentSearches()
        {
            var recent = _weatherService.GetRecentSearches();

            if (recent == null || recent.Count == 0)
                return Ok(new List<string>()); // Return empty list if no searches

            return Ok(recent);
        }
        // DELETE /Weather/recent
        [HttpDelete("recent")]
        public IActionResult ClearRecentSearches()
        {
            _weatherService.ClearRecentSearches();
            return Ok(new { Message = "Recent searches cleared." });
        }
    }
}

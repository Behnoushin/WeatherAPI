using WeatherAPI.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WeatherAPI.Tests
{
    public class FakeWeatherService
    {
        public Task<WeatherResponse> GetWeatherAsync(string city)
        {
            return Task.FromResult(new WeatherResponse
            {
                Temperature = 25,
                Humidity = 50,
                WindSpeed = 3.5,
                AQI = 2,
                Pollutants = new Dictionary<string, double> { { "PM2.5", 12 } },
                Latitude = 35.6892,
                Longitude = 51.3890
            });
        }
    }

    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetWeatherAsync_ReturnsResult_ForKnownCity()
        {
            var service = new FakeWeatherService();

            var result = await service.GetWeatherAsync("Tehran");

            Assert.NotNull(result);
            Assert.Equal(25, result.Temperature);
        }
    }
}


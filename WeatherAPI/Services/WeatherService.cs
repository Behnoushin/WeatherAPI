using Newtonsoft.Json;
using WeatherAPI.Models;
using Microsoft.Extensions.Caching.Distributed; 
using System.Text;

namespace WeatherAPI.Services
{
    public class WeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache; 

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IDistributedCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cache = cache; 
        }

        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            // -------------------------------
            // Check the cache
            // -------------------------------
            var cachedData = await _cache.GetStringAsync(city);
            if (cachedData != null)
            {
                return JsonConvert.DeserializeObject<WeatherResponse>(cachedData);
            }

            // -------------------------------
            // If there was no cache → External API request
            // -------------------------------
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var client = _httpClientFactory.CreateClient();

            // 1. Get weather data
            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            var weatherResponse = await client.GetAsync(weatherUrl);

            if (!weatherResponse.IsSuccessStatusCode)
                return null;

            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
            dynamic weatherData = JsonConvert.DeserializeObject(weatherJson);

            // 2. Get coordinates
            double lat = weatherData.coord.lat;
            double lon = weatherData.coord.lon;

            // 3. Get air pollution data
            var airUrl = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={apiKey}";
            var airResponse = await client.GetAsync(airUrl);

            var pollutants = new Dictionary<string, double>();
            int aqi = -1;

            if (airResponse.IsSuccessStatusCode)
            {
                var airJson = await airResponse.Content.ReadAsStringAsync();
                dynamic airData = JsonConvert.DeserializeObject(airJson);

                foreach (var item in airData.list[0].components)
                {
                    pollutants.Add(item.Name, (double)item.Value);
                }

                aqi = airData.list[0].main.aqi;
            }

            var result = new WeatherResponse
            {
                Temperature = weatherData.main.temp,
                Humidity = weatherData.main.humidity,
                WindSpeed = weatherData.wind.speed,
                AQI = aqi,
                Pollutants = pollutants,
                Latitude = lat,
                Longitude = lon
            };

            // -------------------------------
            // Cache with a TTL of 5 minutes
            // -------------------------------
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(city, JsonConvert.SerializeObject(result), cacheOptions);

            return result;
        }
    }
}

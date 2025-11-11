using Newtonsoft.Json;
using WeatherAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace WeatherAPI.Services
{
    public class WeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<WeatherService> _logger;

        private const int CACHE_TTL_MINUTES = 5; 

        public WeatherService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IDistributedCache cache,
            IMemoryCache memoryCache,
            ILogger<WeatherService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cache = cache;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(string city)
        {
            _logger.LogInformation($"Request received for city: {city} at {DateTime.Now}");

            // -------------------------------
            // Update recent searches (MemoryCache)
            // -------------------------------
            const string RECENT_SEARCHES_KEY = "RecentSearches";
            var recentSearches = _memoryCache.Get<List<string>>(RECENT_SEARCHES_KEY) ?? new List<string>();

            recentSearches.Remove(city);
            recentSearches.Insert(0, city);
            if (recentSearches.Count > 5)
                recentSearches = recentSearches.Take(5).ToList();
            _memoryCache.Set(RECENT_SEARCHES_KEY, recentSearches);

            // -------------------------------
            // Check the cache
            // -------------------------------
            var cachedData = await _cache.GetStringAsync(city);
            if (cachedData != null)
            {
                _logger.LogInformation($"Returning cached data for city: {city}");

                var cachedResult = JsonConvert.DeserializeObject<WeatherResponse>(cachedData);
                if (cachedResult != null)
                {
                    // Calculate remaining TTL based on elapsed time
                    var elapsedMinutes = (int)(DateTime.Now - cachedResult.RequestTime).TotalMinutes;
                    cachedResult.CacheTTLMinutes = Math.Max(0, CACHE_TTL_MINUTES - elapsedMinutes);
                }

                return cachedResult;
            }

            _logger.LogInformation($"Fetching new data from API for city: {city}");

            // -------------------------------
            // External API request
            // -------------------------------
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var client = _httpClientFactory.CreateClient();

            // 1. Get weather data
            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            var weatherResponse = await client.GetAsync(weatherUrl);

            if (!weatherResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Weather API request failed for city: {city}");
                return null;
            }

            var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
            dynamic weatherData = JsonConvert.DeserializeObject(weatherJson);

            if (weatherData == null || weatherData.coord == null)
            {
                _logger.LogWarning($"Weather API returned null or missing coordinates for city: {city}");
                return null;
            }

            // 2. Get coordinates
            double lat = weatherData.coord.lat ?? 0.0;
            double lon = weatherData.coord.lon ?? 0.0;

            // 3. Get air pollution data
            var airUrl = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={apiKey}";
            var airResponse = await client.GetAsync(airUrl);

            var pollutants = new Dictionary<string, double>();
            int aqi = -1;

            if (airResponse.IsSuccessStatusCode)
            {
                var airJson = await airResponse.Content.ReadAsStringAsync();
                dynamic airData = JsonConvert.DeserializeObject(airJson);

                if (airData?.list != null && airData.list.Count > 0)
                {
                    foreach (var item in airData.list[0].components)
                        pollutants.Add(item.Name, (double)item.Value);

                    aqi = airData.list[0].main.aqi;
                }
            }
            else
            {
                _logger.LogWarning($"Air pollution API request failed for city: {city}");
            }

            // 4. Create response object
            var result = new WeatherResponse
            {
                Temperature = weatherData.main.temp,
                Humidity = weatherData.main.humidity,
                WindSpeed = weatherData.wind.speed,
                AQI = aqi,
                Pollutants = pollutants,
                Latitude = lat,
                Longitude = lon,
                RequestTime = DateTime.Now,
                CacheTTLMinutes = CACHE_TTL_MINUTES
            };

            // -------------------------------
            // Cache with TTL
            // -------------------------------
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_TTL_MINUTES)
            };
            await _cache.SetStringAsync(city, JsonConvert.SerializeObject(result), cacheOptions);
            _logger.LogInformation($"Cached data for city: {city}");

            return result;
        }

        // -------------------------------
        // Get last 5 searches
        // -------------------------------
        public List<string> GetRecentSearches()
        {
            const string RECENT_SEARCHES_KEY = "RecentSearches";
            return _memoryCache.Get<List<string>>(RECENT_SEARCHES_KEY) ?? new List<string>();
        }
        
        // -------------------------------
        // Clear recent searches
        // -------------------------------
        public void ClearRecentSearches()
        {
            const string RECENT_SEARCHES_KEY = "RecentSearches";
            _memoryCache.Remove(RECENT_SEARCHES_KEY);
            _logger.LogInformation("Recent searches cleared from memory cache.");
        }
    }
}

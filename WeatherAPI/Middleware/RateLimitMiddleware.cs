using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WeatherAPI.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;
        private readonly ILogger<RateLimitMiddleware> _logger;
        private readonly int _limit = 10; // Max requests
        private readonly TimeSpan _period = TimeSpan.FromMinutes(1); // per minute

        public RateLimitMiddleware(RequestDelegate next, IDistributedCache cache, ILogger<RateLimitMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = $"RateLimit-{ip}";
            var countString = await _cache.GetStringAsync(key);
            int count = string.IsNullOrEmpty(countString) ? 0 : int.Parse(countString);

            if (count >= _limit)
            {
                context.Response.StatusCode = 429;
                context.Response.Headers["Retry-After"] = _period.TotalSeconds.ToString();
                await context.Response.WriteAsync("Too many requests. Try again later.");
                _logger.LogWarning($"Rate limit exceeded for IP {ip}");
                return;
            }

            count++;
            await _cache.SetStringAsync(key, count.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _period
            });

            await _next(context);
        }
    }
}

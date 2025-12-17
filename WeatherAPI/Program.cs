using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAPI.Services;
using WeatherAPI.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Logging -------------------
// Clear default providers
builder.Logging.ClearProviders();

// Log to console
builder.Logging.AddConsole();

// Log to file (a separate file each day)
builder.Logging.AddFile("Logs/weather-log-{Date}.txt");

// ------------------- Services -------------------
// Add Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6380";
    options.InstanceName = "weather-cache:";
});

builder.Services.AddMemoryCache();

// Add Controllers
builder.Services.AddControllers();

// Add HttpClient for external API calls
builder.Services.AddHttpClient();

// Add WeatherService as scoped dependency
builder.Services.AddScoped<WeatherService>();

// ------------------- Swagger / OpenAPI -------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------- Middleware -------------------
// Enable Swagger UI only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI V1");
        c.RoutePrefix = string.Empty; // Swagger UI at root URL
    });
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// ------------------- Rate Limiting -------------------
app.UseMiddleware<RateLimitMiddleware>();

// Enable authorization middleware (if needed in future)
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Run the application
app.Run();

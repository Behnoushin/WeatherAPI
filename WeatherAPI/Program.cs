using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Services -------------------
// Add Controllers
builder.Services.AddControllers();

// Add HttpClient for external API calls
builder.Services.AddHttpClient();

// Add WeatherService as scoped dependency
builder.Services.AddScoped<WeatherService>();

// Add Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------- Middleware -------------------
// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable authorization middleware (if needed in future)
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Run the application
app.Run();
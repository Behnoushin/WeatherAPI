# WeatherAPI 🌤️

**WeatherAPI** is a **.NET 9 Web API project** that provides current weather and air quality information for a given city using the [OpenWeatherMap API](https://openweathermap.org/api).  
This project also includes **Docker setup, Redis caching, and request caching** to improve performance.

## Features 🚀

- Get current **temperature**, **humidity**, and **wind speed**.  
- Get **Air Quality Index (AQI)** and major **pollutants** (PM2.5, CO, NO2, etc.).  
- Retrieve **latitude and longitude** of the city.  
- Clean architecture with **Controllers**, **Services**, and **Models**.  
- **Swagger UI** support for easy API testing.  
- **Caching using Redis** for faster responses (5-minute expiration per city).  
- Simple **unit tests** using xUnit.  
- **Dockerized** setup for both WeatherAPI and Redis.


## Installation 💻

1. Clone the repository:
```bash
git clone https://github.com/Behnoushin/WeatherAPI.git
cd WeatherAPI/WeatherAPI
```

2. Set your OpenWeatherMap API key:
Set it in WeatherAPI/appsettings.Development.json:
```json
{
  "OpenWeatherMap": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```
✅ Note: API Key is never exposed in Docker Compose; it’s read securely from appsettings.

3. Docker & Redis Setup

Make sure Docker is installed on your machine.

- Start Redis and WeatherAPI using Docker Compose:
```bash
docker compose up -d --build
```

- WeatherAPI service ports:
```bash
HTTP: 5002
HTTPS: 5003
Redis: 6380
```

- **Redis caching:**  
  - First request to a city fetches data from OpenWeatherMap (slower).  
  - Subsequent requests return cached data from Redis (much faster).  
  - Cache expires automatically after 5 minutes.


4. Restore dependencies:
```bash
dotnet restore
```

5. Run the project:
```bash
dotnet run
```

## API Endpoints 🌐
### Get Weather by City

Request:
```bash
GET /Weather/{city}
```

Response Example:
```json
{
  "temperature": 25,
  "humidity": 50,
  "windSpeed": 3.5,
  "aqi": 2,
  "pollutants": {
    "PM2.5": 12,
    "CO": 0.3,
    "NO2": 14
  },
  "latitude": 35.6892,
  "longitude": 51.3890
}
```

## Testing 🧪

Unit tests are located in the WeatherAPI.Tests folder.
A fake service is used for testing to avoid relying on the external API:
```bash
cd WeatherAPI.Tests
dotnet test
```

## Technologies Used ⚙️
- .NET 9
- C#
- ASP.NET Core Web API
- Newtonsoft.Json
- Redis
- Docker & Docker Compose
- xUnit (for unit testing)
- Swagger/OpenAPI


## Author 👩‍💻
Behnoush Shahraeini - Backend Developer
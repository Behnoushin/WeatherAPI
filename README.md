# WeatherAPI 🌤️

**WeatherAPI** is a **.NET 9 Web API project** that provides current weather and air quality information for a given city using the [OpenWeatherMap API](https://openweathermap.org/api).  
This project also includes **Docker setup**, **Redis caching**, **logging**, and **Swagger UI** for an enhanced developer experience.

---

## 🌟 Features

### 🌤 Core Functionality
- Get current **temperature**, **humidity**, and **wind speed**.  
- Get **Air Quality Index (AQI)** and main **pollutants** (PM2.5, CO, NO2, etc.).  
- Retrieve **latitude** and **longitude** for the city.  
- Includes **clean architecture**: Controllers, Services, and Models.  
- Fully **Dockerized** setup with **Redis** support.  
- Simple **unit tests** using xUnit.

### ⚙️ Extended Features
- **Swagger UI** — beautiful API documentation for testing endpoints.  
- **Health Check Endpoint** (`/health`) — confirms that the API is running.  
- **Redis Caching** — stores weather data for 5 minutes per city to boost performance.  
- **Memory Cache** — keeps the last 5 searched cities in memory:
  - `GET /Weather/recent` → returns recent searches.  
  - `DELETE /Weather/recent` → clears the search history.  
- **Cache TTL Countdown** — shows how many minutes of cache validity remain.  
- **Request Time Field** — every response includes the time of the data retrieval (`RequestTime`).  
- **Logging to File & Console** — all API requests and warnings are logged.  
  - Daily logs saved under `/Logs/weather-log-{Date}.txt`.  
- **Input Validation** — returns `BadRequest` if:
  - The city name is empty or null  
  - The city name includes numbers or invalid characters  


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
  "longitude": 51.3890,
  "requestTime": "2025-11-11T09:42:00Z",
  "cacheTTLMinutes": 3
}

```
🔹 Get Recent Searches:
```bash
GET /Weather/recent
```
Returns the last 5 searched cities.

🔹 Clear Recent Searches:
```bash
DELETE /Weather/recent
```
Clears all stored city searches.

🔹 Health Check:
```bash
GET /health
```
- Response:
- ```json
- { "status": "API is running ✅" }
- ```

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
- ILogger (File + Console Logging)
- xUnit (for unit testing)
- Swagger/OpenAPI


## Author 👩‍💻
Behnoush Shahraeini - Backend Developer
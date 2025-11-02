# WeatherAPI 🌤️

A simple **.NET 9 Web API** that provides current weather and air quality information for a given city using the [OpenWeatherMap API](https://openweathermap.org/api).  

---

## Features 🚀

- Get current **temperature**, **humidity**, and **wind speed**.  
- Get **Air Quality Index (AQI)** and major **pollutants** (PM2.5, CO, NO2, etc.).  
- Retrieve **latitude and longitude** of the city.  
- Clean architecture with **Controllers**, **Services**, and **Models**.  
- **Swagger UI** support for easy API testing.  
- Simple **unit tests** using xUnit.

---

## Installation 💻

1. Clone the repository:
```bash
git clone https://github.com/Behnoushin/WeatherAPI.git
cd WeatherAPI/WeatherAPI
```

Set your OpenWeatherMap API key in appsettings.Development.json or use an .env approach:

```json
{
  "OpenWeatherMap": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

Restore dependencies:
```bash
dotnet restore
```

Run the project:
```bash
dotnet run
```

## API Endpoints 🌐
Get Weather by City

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
- xUnit (for unit testing)
- Swagger/OpenAPI


## Author 👩‍💻
Behnoush Shahraeeni - Backend Developer
namespace WeatherAPI.Models
{
    public class WeatherResponse
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int AQI { get; set; }
        public Dictionary<string, double> Pollutants { get; set; } = new Dictionary<string, double>();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime RequestTime { get; set; }
        public int CacheTTLMinutes { get; set; }
    }
}

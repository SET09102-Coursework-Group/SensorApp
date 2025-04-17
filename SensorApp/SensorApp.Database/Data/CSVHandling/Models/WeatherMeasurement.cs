namespace SensorApp.Database.Data.CSVHandling.Models
{
    public class WeatherMeasurement
    {
        public DateTime Timestamp { get; set; }
        public float? Temperature { get; set; }
        public float? Relative_humidity { get; set; }
        public float? Wind_speed { get; set; }
        public float? Wind_direction { get; set; }
    }
}
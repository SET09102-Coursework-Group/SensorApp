namespace SensorApp.Database.Data.CSVHandling.Models;

public class WeatherMeasurement
{
    public DateTime Timestamp { get; set; }
    public float? Temperature { get; set; }
    public float? RelativeHumidity { get; set; }
    public float? WindSpeed { get; set; }
    public float? WindDirection { get; set; }
}
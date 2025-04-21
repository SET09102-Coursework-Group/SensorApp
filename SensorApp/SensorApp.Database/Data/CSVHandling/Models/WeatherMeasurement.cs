namespace SensorApp.Database.Data.CSVHandling.Models;

/// <summary>
/// Represents a record of weather measurements at a specific point in time, capturing various weather parameters
/// such as temperature, relative humidity, wind speed, and wind direction.
/// This class is used for parsing CSV files containing weather data.
/// </summary>
public class WeatherMeasurement
{
    public DateTime Timestamp { get; set; }
    public float? Temperature { get; set; }
    public float? RelativeHumidity { get; set; }
    public float? WindSpeed { get; set; }
    public float? WindDirection { get; set; }
}
namespace SensorApp.Database.Data.CSVHandling.Models;

/// <summary>
/// Represents a record of water quality measurements at a specific point in time, capturing various water quality parameters
/// such as nitrate, nitrite, phosphate, and electrical conductivity (EC).
/// This class is used for parsing CSV files containing water quality data.
/// </summary>
public class WaterQualityMeasurement
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public float? Nitrate { get; set; }
    public float? Nitrite { get; set; }
    public float? Phosphate { get; set; }
    public float? EC { get; set; }
}
namespace SensorApp.Database.Data.CSVHandling.Models;

/// <summary>
/// Represents a record of air quality measurements from a sensor at a specific point in time, capturing various air quality parameters
/// such as nitrogen dioxide, sulphur dioxide, and particulate matter concentrations.
/// This class is used for parsing CSV files containing air quality data.
/// </summary>
public class AirQualityMeasurement
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public float? NitrogenDioxide { get; set; }
    public float? SulphurDioxide { get; set; }
    public float? PM25ParticulateMatter { get; set; }
    public float? PM10ParticulateMatter { get; set; }
}
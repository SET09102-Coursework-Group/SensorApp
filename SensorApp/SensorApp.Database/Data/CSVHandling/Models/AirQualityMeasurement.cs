namespace SensorApp.Database.Data.CSVHandling.Models;

public class AirQualityMeasurement
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public float? Nitrogen_dioxide { get; set; }
    public float? Sulphur_dioxide { get; set; }
    public float? PM25_particulate_matter { get; set; }
    public float? PM10_particulate_matter { get; set; }
}
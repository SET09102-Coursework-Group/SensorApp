namespace SensorApp.Database.Data.CSVHandling.Models;

public class AirQualityMeasurement
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public float? NitrogenDioxide { get; set; }
    public float? SulphurDioxide { get; set; }
    public float? PM25ParticulateMatter { get; set; }
    public float? PM10ParticulateMatter { get; set; }
}
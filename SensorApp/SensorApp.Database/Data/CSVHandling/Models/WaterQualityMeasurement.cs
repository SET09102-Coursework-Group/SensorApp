namespace SensorApp.Database.Data.CSVHandling.Models;

public class WaterQualityMeasurement
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public float? Nitrate { get; set; }
    public float? Nitrite { get; set; }
    public float? Phosphate { get; set; }
    public float? EC { get; set; }
}
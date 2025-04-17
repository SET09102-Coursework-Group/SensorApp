namespace SensorApp.Shared.Models;

public class SensorModel
{
    public int Id { get; set; }
    public string Type { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public string? Site_zone { get; set; }
    public string Status { get; set; }
    public ICollection<MeasurementModel> Measurements { get; set; } = new List<MeasurementModel>();

    public MeasurementModel? LatestMeasurement =>
        Measurements?.OrderByDescending(m => m.Timestamp).FirstOrDefault();
}
namespace SensorApp.Shared.Models;
public class SensorModel : BaseEntity
{
    public string Type { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public string? Site_zone { get; set; }
    public string Status { get; set; }
    public ICollection<MeasurementModel> Measurements { get; set; } = new List<MeasurementModel>();

    public Dictionary<int, MeasurementModel> LatestMeasurementsByType =>
       Measurements
           .GroupBy(m => m.Measurement_type_id)
           .ToDictionary(
               g => g.Key,
               g => g.OrderByDescending(m => m.Timestamp).First()
           );
    public bool IsThresholdBreached =>
        LatestMeasurementsByType.Values.Any(m =>
            m.MeasurementType != null &&
            (m.Value < m.MeasurementType.Min_safe_threshold || m.Value > m.MeasurementType.Max_safe_threshold));
}
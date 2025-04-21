namespace SensorApp.Shared.Models;

/// <summary>
/// Represents a measurement taken by a sensor for a specific measurand at a given time.
/// </summary>
public class MeasurementModel : BaseEntity
{
    public int Sensor_id { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }
    public int Measurement_type_id { get; set; }
    public MeasurandModel MeasurementType { get; set; }
}
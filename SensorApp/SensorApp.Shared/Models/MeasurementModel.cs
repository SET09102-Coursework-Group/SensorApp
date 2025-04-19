namespace SensorApp.Shared.Models;

public class MeasurementModel : BaseEntity
{
    public int Sensor_id { get; set; }
    public SensorModel Sensor { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }
    public int Measurement_type_id { get; set; }
    public MeasurandModel MeasurementType { get; set; }
}
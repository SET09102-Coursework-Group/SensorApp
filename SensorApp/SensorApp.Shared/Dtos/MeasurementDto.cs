namespace SensorApp.Shared.Dtos;

public class MeasurementDto
{
    public int Id { get; set; }
    public int Sensor_id { get; set; }
    public SensorDto Sensor { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }
    public int Measurement_type_id { get; set; }
    public MeasurandDto MeasurementType { get; set; }
}
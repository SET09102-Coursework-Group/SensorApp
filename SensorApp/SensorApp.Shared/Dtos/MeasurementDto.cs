namespace SensorApp.Shared.Dtos;

/// <summary>
/// Data Transfer Object (DTO) for representing a measurement.
/// Used for transferring measurement data between layers or systems.
/// </summary>
public class MeasurementDto
{
    public int Id { get; set; }
    public int Sensor_id { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }
    public int Measurement_type_id { get; set; }
    public MeasurandDto MeasurementType { get; set; }
}
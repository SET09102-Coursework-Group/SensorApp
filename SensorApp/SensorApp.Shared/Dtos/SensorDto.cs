namespace SensorApp.Shared.Dtos;

public class SensorDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public string? Site_zone { get; set; }
    public string Status { get; set; }
    public List<MeasurementDto> Measurements { get; set; } = new List<MeasurementDto>();
}
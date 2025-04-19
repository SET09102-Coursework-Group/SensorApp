using System.ComponentModel.DataAnnotations;

namespace SensorApp.Shared.Dtos;

public class MeasurandDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public float? Min_safe_threshold { get; set; }
    public float? Max_safe_threshold { get; set; }
    public List<MeasurementDto> Measurements { get; set; } = new List<MeasurementDto>();
}
namespace SensorApp.Shared.Dtos;

/// <summary>
/// Data Transfer Object (DTO) for representing a measurand.
/// Used for transferring measurand data between layers or systems.
/// </summary>
public class MeasurandDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public float? Min_safe_threshold { get; set; }
    public float? Max_safe_threshold { get; set; }
}
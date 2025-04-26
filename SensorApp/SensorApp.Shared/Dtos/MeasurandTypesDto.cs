namespace SensorApp.Shared.Dtos;

/// <summary>
/// Data Transfer Object (DTO) representing a type of measurand.
/// A measurand describes a measurable property for instance, temperature, humidity and its unit.
/// </summary>
public class MeasurandTypesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
}
using System;
using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Dtos.Incident;

/// <summary>
/// Data Transfer Object (DTO) used for creating a new incident.
/// This DTO contains all the necessary information to create an incident in the system.
/// </summary>
public class CreateIncidentDto
{
    public required string Type { get; set; }
    public required string Status { get; set; }
    public required int SensorId { get; set; }
    public required string Priority { get; set; }
    public string? Comments { get; set; }
}

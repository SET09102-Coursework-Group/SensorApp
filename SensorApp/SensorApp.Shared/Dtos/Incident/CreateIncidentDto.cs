using System;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;

namespace SensorApp.Shared.Dtos.Incident;

/// <summary>
/// Data Transfer Object (DTO) used for creating a new incident.
/// This DTO contains all the necessary information to create an incident in the system.
/// </summary>
public class CreateIncidentDto
{
    public required IncidentType Type { get; set; }
    public required IncidentStatus Status { get; set; }
    public int SensorId { get; set; }
    public required IncidentPriority Priority { get; set; }
    public string? Comments { get; set; }
}

using System;
using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Dtos.Incident;

public class CreateIncidentDto
{
    public string Type { get; set; }
    public string Status { get; set; }
    public int SensorId { get; set; }
    public string Priority { get; set; }
    public string? Comments { get; set; }
}

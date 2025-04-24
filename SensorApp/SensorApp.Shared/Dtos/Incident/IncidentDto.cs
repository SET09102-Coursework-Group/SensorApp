using System;
using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Dtos.Incident;

public class IncidentDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public SensorDto Sensor { get; set; }
    public int Sensor_id { get; set; }
    public DateTime Creation_date { get; set; }
    public string Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public string Responder_id { get; set; }
    public UserWithRoleDto Responder { get; set; }
    public string? Comments { get; set; }
    public string? Resolution_comments { get; set; }
}

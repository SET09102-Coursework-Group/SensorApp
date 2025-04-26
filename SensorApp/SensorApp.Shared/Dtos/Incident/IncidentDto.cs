using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;

namespace SensorApp.Shared.Dtos.Incident;


public class IncidentDto
{
    public required int Id { get; set; }
    public required IncidentType Type { get; set; }
    public required IncidentStatus Status { get; set; }
    public SensorDto Sensor { get; set; }
    public required int Sensor_id { get; set; }
    public required DateTime Creation_date { get; set; }
    public required IncidentPriority Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public string Responder_id { get; set; }
    public UserWithRoleDto Responder { get; set; }
    public string? Comments { get; set; }
    public string? Resolution_comments { get; set; }
}

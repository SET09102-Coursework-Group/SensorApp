using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Dtos.Incident;


public class IncidentDto
{
    public required int Id { get; set; }
    public required string Type { get; set; }
    public required string Status { get; set; }
    public SensorDto Sensor { get; set; }
    public required int Sensor_id { get; set; }
    public required DateTime Creation_date { get; set; }
    public required string Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public string Responder_id { get; set; }
    public UserWithRoleDto Responder { get; set; }
    public string? Comments { get; set; }
    public string? Resolution_comments { get; set; }
}

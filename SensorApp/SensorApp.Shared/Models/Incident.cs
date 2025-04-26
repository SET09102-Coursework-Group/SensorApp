namespace SensorApp.Shared.Models;
using SensorApp.Shared.Dtos.Admin;

/// <summary>
/// Represents an incident in the system.
/// This model contains all the details related to a specific incident, including the type, status, sensor, priority, and resolution details.
/// It inherits from <see cref="BaseEntity"/> which includes the common property Id.
/// </summary>
public class Incident : BaseEntity
{
    public required string Type { get; set; }
    public required string Status { get; set; }
    public required SensorModel Sensor { get; set; }
    public required int Sensor_id { get; set; }
    public required DateTime Creation_date { get; set; }
    public required string Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public required string Responder_id { get; set; }
    public required UserWithRoleDto Responder { get; set; }
    public string? Comments { get; set; }
}

namespace SensorApp.Shared.Models;
using SensorApp.Shared.Dtos.Admin;
public class Incident : BaseEntity
{
    public string Type { get; set; }
    public string Status { get; set; }
    public SensorModel Sensor { get; set; }
    public int Sensor_id { get; set; }
    public DateTime Creation_date { get; set; }
    public string Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public string Responder_id { get; set; }
    public UserWithRoleDto Responder { get; set; }
    public string? Comments { get; set; }
}

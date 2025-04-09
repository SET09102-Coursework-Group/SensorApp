using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SensorApp.Infrastructure.Data.Models;

[Table("sensor")]
public class Sensor : BaseEntity
{
    [Required]
    public string Type { get; set; } = default!;

    public DateTime DeploymentDate { get; set; }

    public float Longitude { get; set; }
    public float Latitude { get; set; }
}
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SET09102_coursework.Database.Models;

[Table("sensor")]
[PrimaryKey(nameof(Sensor_id))]
public class Sensor
{
    public int Sensor_id { get; set; }
    [Required]
    public string Sensor_type { get; set; }
    [Required]
    public DateTime Deployment_date { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
}

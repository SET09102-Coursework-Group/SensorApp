using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SensorApp.Database.Models;

[Table("sensor")]
public class Sensor : BaseEntity
{
    [Required]
    public string Type { get; set; }
    [Required]
    public float Longitude { get; set; }
    [Required]
    public float Latitude { get; set; }
    public string? Site_zone { get; set; }
    [Required]
    public string Status { get; set; }
    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
}
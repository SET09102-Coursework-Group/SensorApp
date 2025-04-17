using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SensorApp.Database.Models;

[Table("measurement")]
public class Measurement : BaseEntity
{
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public float Value { get; set; }
    [Required]
    public int Id { get; set; }
    [ForeignKey("Id")]
    public Sensor Sensor { get; set; }
    public int Measurement_type_id { get; set; }
    [ForeignKey("Measurement_type_id")]
    public Measurand Measurement_type { get; set; }
}
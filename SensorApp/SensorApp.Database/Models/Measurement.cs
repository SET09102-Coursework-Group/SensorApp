using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SensorApp.Database.Models;

/// <summary>
/// Represents a recorded measurement taken by a sensor at a specific point in time.
/// Each measurement is linked to a specific sensor and measurement type (measurand), alongside its value and timestamp.
/// </summary>
[Table("measurement")]
public class Measurement : BaseEntity
{
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public float Value { get; set; }
    public int Sensor_id { get; set; }
    [ForeignKey("Sensor_id")]
    public Sensor Sensor { get; set; }
    public int Measurement_type_id { get; set; }
    [ForeignKey("Measurement_type_id")]
    public Measurand Measurement_type { get; set; }
}
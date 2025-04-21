using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SensorApp.Database.Models;

/// <summary>
/// Represents a type of measurement that can be recorded by a sensor. 
/// The <see cref="Measurand"/> model includes metadata about the measurement, such as its name, unit of measurement, 
/// and acceptable threshold ranges for safety.
/// </summary>
[Table("measurand")]
public class Measurand : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Unit { get; set; }
    public float? Min_safe_threshold { get; set; }
    public float? Max_safe_threshold { get; set; }
    public ICollection<Measurement> Measurements { get; set; }
}
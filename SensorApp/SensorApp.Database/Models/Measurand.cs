using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace SensorApp.Database.Models;

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
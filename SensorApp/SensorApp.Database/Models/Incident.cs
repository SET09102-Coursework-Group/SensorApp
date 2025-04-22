using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;

namespace SensorApp.Database.Models;

/// <summary>
/// Represents an incident or anomaly that can occur at a sensor. 
/// The <see cref="Incident"/> model includes metadata about the incident itself, its related sensor and individuals related to it.
/// </summary>
[Table("incident")]
public class Incident : BaseEntity
{
    [Required]
    public string Type { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public Sensor Sensor { get; set; }
    [Required]
    public int Sensor_id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    public IdentityUser Responder { get; set; }
    public int Responder_id { get; set; }
    public string? Comments { get; set; }
}
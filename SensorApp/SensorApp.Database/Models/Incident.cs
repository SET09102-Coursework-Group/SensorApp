using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using SensorApp.Shared.Enums;

namespace SensorApp.Database.Models;

/// <summary>
/// Represents an incident or anomaly that can occur in relation to a sensor. 
/// The <see cref="Incident"/> model includes metadata about the incident itself, its related sensor and users related to it.
/// </summary>
[Table("incident")]
public class Incident : BaseEntity
{
    [Required]
    public IncidentType Type { get; set; }
    [Required]
    public IncidentStatus Status { get; set; }
    [Required]
    [ForeignKey("Sensor_id")]
    public Sensor Sensor { get; set; }
    [Required]
    public int Sensor_id { get; set; }
    [Required]
    public DateTime Creation_date { get; set; }
    [Required]
    public IncidentPriority Priority { get; set; }
    public DateTime? Resolution_date { get; set; }
    [ForeignKey("Responder_id")]
    public IdentityUser Responder { get; set; }
    public string Responder_id { get; set; }
    public string? Comments { get; set; }
}
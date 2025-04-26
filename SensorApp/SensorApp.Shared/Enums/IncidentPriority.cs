using System.ComponentModel.DataAnnotations;

namespace SensorApp.Shared.Enums;

public enum IncidentPriority
{
    [Display(Name = "High")]
    High,
    [Display(Name = "Medium")]
    Medium,
    [Display(Name = "Low")]
    Low
}

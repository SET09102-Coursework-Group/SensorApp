using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SensorApp.Shared.Enums;

public enum IncidentStatus
{
    [Display(Name = "Open")]
    Open,
    [Display(Name = "Resolved")]
    Resolved
}
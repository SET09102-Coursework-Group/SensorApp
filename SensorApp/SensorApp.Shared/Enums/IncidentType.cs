using System.ComponentModel.DataAnnotations;

namespace SensorApp.Shared.Enums;

public enum IncidentType
{
    [Display(Name = "Max threshold breached")]
    MaxThresholdBreached,
    [Display(Name = "Min threshold breached")]
    MinThresholdBreached,
    [Display(Name = "Sensor unresponsive")]
    SensorUnresponsive
}
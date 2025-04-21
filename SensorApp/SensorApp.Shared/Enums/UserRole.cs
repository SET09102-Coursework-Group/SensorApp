using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SensorApp.Shared.Enums;

public enum UserRole
{
    [Display(Name = "Administrator")]
    Administrator,

    [Display(Name = "Operations Manager")]
    OperationsManager,

    [Display(Name = "Environmental Scientist")]
    EnvironmentalScientist
}
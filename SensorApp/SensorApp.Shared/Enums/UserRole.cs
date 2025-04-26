using System.ComponentModel.DataAnnotations;

namespace SensorApp.Shared.Enums;

/// <summary>
/// Defines the different user roles available within the application.
/// Each role controls access permissions and feature visibility.
/// </summary>
public enum UserRole
{
    [Display(Name = "Administrator")]
    Administrator,

    [Display(Name = "Operations Manager")]
    OperationsManager,

    [Display(Name = "Environmental Scientist")]
    EnvironmentalScientist
}
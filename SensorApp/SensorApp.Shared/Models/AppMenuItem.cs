namespace SensorApp.Shared.Models;

/// <summary>
/// This model is used by menu factories (like AdminMenuFactory) and is consumed by the UI to  build role-based navigation options.
/// </summary>
public class AppMenuItem
{
    public string Title { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
}
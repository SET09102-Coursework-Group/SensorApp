namespace SensorApp.Maui.Helpers.MenuRoles.Interfaces;

/// <summary>
/// Interface for menu factories that produce lists of FlyoutItems.
/// </summary>
public interface IMenuFactory
{
    /// <summary>
    /// Creates the menu structure.
    /// </summary>
    List<FlyoutItem> CreateMenu();
}
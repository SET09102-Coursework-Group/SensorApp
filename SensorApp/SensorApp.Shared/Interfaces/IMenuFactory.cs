using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface IMenuFactory
{
    /// <summary>
    /// Creates the list of menu items  that a specific user role should see. This is role specific, for example AdminMenuFactory only returns the navigation for admin users.
    /// </summary>
    /// <returns>A collection of <see cref="AppMenuItem"/> to be rendered in the UI navigation menu.</returns>
    IEnumerable<AppMenuItem> CreateMenu();
}
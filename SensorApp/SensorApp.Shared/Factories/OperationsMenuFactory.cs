using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

/// <summary>
/// Factory class responsible for creating the navigation menu items 
/// that are available to Operations Manager users within the application.
/// </summary>
public class OperationsMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a collection of menu items specific to Operations Manager users that will appear on the app's side menu
    /// </summary>
    /// <returns>A list of <see cref="AppMenuItem"/> representing Environmental Scientist navigation options.</returns>
    public IEnumerable<AppMenuItem> CreateMenu()
    {
        return
        [
            new AppMenuItem
            {
                Title = "Sensor Map",
                Route = "SensorMapPage"
            },
            new AppMenuItem
            {
                Title = "Incident List",
                Route = "IncidentList"
            }
        ];
    }
}
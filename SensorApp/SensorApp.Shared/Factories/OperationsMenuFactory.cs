using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

public class OperationsMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a collection of menu items specific to Operations Manager users that will appear on the apps side menu
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
            }
        ];
    }
}
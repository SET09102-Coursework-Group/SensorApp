using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

public class EnvironmentalMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a collection of menu items specific to Environmental Scientist users that will appear on the app's side menu
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
                Title = "Historical Data",        
                Route = "HistoricalDataPage"   
            }
        ];
    }
}
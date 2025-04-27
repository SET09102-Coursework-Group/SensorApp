using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

/// <summary>
/// Factory class responsible for creating the navigation menu items 
/// that are available to environmental Scientist users within the application.
/// </summary>
public class EnvironmentalMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a collection of menu items specific to Environmental Scientist users
    /// that will appear in the app's side menu.
    /// </summary>
    /// <returns>
    /// A list of <see cref="AppMenuItem"/> representing the Environmental Scientist's navigation options.
    /// </returns>
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
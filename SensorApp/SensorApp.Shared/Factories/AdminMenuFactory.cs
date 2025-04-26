using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

/// <summary>
/// Factory class responsible for creating the navigation menu items 
/// that are available to administrative users within the application.
/// </summary>
public class AdminMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a predefined collection of menu items specifically for admin users. 
    /// Each menu item corresponds to a page route in the application  and provides access to administrative and general app functionality.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="AppMenuItem"/> representing navigation options for administrative users.
    /// </returns>
    public IEnumerable<AppMenuItem> CreateMenu()
    {
    
        return
        [
            new AppMenuItem
            {
                Title = "Admin - Manage Users",
                Route = "AdminUsersPage"
            },
            new AppMenuItem
            {
                Title = "Sensor Map",
                Route = "SensorMapPage"
            },
            new AppMenuItem
            {
                Title = "Historical Data",        
                Route = "HistoricalDataPage" 
            },
            new AppMenuItem
            {
                Title = "Incident List",
                Route = "IncidentList"
            }
        ];
    }
}
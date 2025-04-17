using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

public class AdminMenuFactory : IMenuFactory
{
    /// <summary>
    /// Creates a collection of menu items specific to admin users that will appear on the apps side menu
    /// </summary>
    /// <returns>A list of <see cref="AppMenuItem"/> representing admin navigation options.</returns>
    public IEnumerable<AppMenuItem> CreateMenu()
    {
    
        return
        [
            new AppMenuItem
            {
                Title = "Admin - Manage Users",
                Route = "AdminUsersPage"
            }
        ];
    }
}
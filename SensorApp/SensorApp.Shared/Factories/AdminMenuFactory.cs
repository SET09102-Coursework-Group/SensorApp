using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Factories;

/// <summary>
/// Factory for creating menu items for administrator users.
/// </summary>
public class AdminMenuFactory : IMenuFactory
{
    public IEnumerable<AppMenuItem> CreateMenu()
    {
        // Return a list of domain-level menu items for Admin.
        // This list is easy to test since it's pure data.
        return new List<AppMenuItem>
        {
            new AppMenuItem
            {
                Title = "Admin - Manage Users",
                Route = "AdminUsersPage"
            }
            // Add other admin routes if needed
        };
    }
}
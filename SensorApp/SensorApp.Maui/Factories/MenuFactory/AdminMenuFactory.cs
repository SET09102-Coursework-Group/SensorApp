using SensorApp.Maui.Helpers.MenuRoles.Interfaces;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.Factories.MenuFactory;

/// <summary>
/// Factory for creating menu items for administrator users.
/// </summary>
public class AdminMenuFactory : IMenuFactory
{
    public List<FlyoutItem> CreateMenu()
    {
        return new List<FlyoutItem>
        {
            new FlyoutItem
            {
                Title = "Admin - Manage Users",
                Route = nameof(AdminUsersPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                Items =
                {
                    new ShellContent
                    {
                        Title = "User Table",
                        ContentTemplate = new DataTemplate(typeof(AdminUsersPage))
                    }
                }
            }
        };
    }
}
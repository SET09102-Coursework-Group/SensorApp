using SensorApp.Maui.Views.Controls;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.Helpers.MenuRoles;

/// <summary>
/// Builds and configures the menu for the application based on the current user role.
/// </summary>
public static class MenuBuilder
{
    public static void BuildMenu()
    {
        Shell.Current.Items.Clear();
        Shell.Current.FlyoutHeader = new FlyOutHeader();

        var roleString = App.UserInfo?.Role;
        if (string.IsNullOrWhiteSpace(roleString))
            throw new InvalidOperationException("User role is not set.");


        if (!Enum.TryParse<UserRole>(roleString, out var role))
            throw new InvalidOperationException("Invalid user role.");

        var factory = MenuFactoryResolver.Resolve(role);
        var menuItems = factory.CreateMenu();

        foreach (var item in menuItems)
        {
            Shell.Current.Items.Add(item);
        }


        var logoutFlyoutItem = new FlyoutItem()
        {
            Title = "Logout",
            Route = nameof(LogoutPage),
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
                {
                    new ShellContent
                    {
                        Icon = "dotnet_bot.svg",
                        Title = "Logout",
                        ContentTemplate = new DataTemplate(typeof(LogoutPage))
                    }
                }
        };

        if (!Shell.Current.Items.Contains(logoutFlyoutItem))
        {
            Shell.Current.Items.Add(logoutFlyoutItem);
        }
    }
}
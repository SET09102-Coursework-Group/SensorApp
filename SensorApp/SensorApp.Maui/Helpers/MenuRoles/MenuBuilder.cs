using SensorApp.Maui.Views.Controls;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.Helpers.MenuRoles;

public static class MenuBuilder
{
    public static void BuildMenu()
    {
        Shell.Current.Items.Clear();
        Shell.Current.FlyoutHeader = new FlyOutHeader();

        var role = App.UserInfo?.Role;

        if (string.IsNullOrWhiteSpace(role))
            throw new InvalidOperationException("User role is not set.");

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
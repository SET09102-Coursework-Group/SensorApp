using SensorApp.Maui.Controls;
using SensorApp.Maui.Pages;

namespace SensorApp.Maui.Helpers;

public static class MenuBuilder
{
    public static void BuildMenu()
    {
        Shell.Current.Items.Clear();

        Shell.Current.FlyoutHeader = new FlyOutHeader();

        var role = App.UserInfo?.Role;

        if (role.Equals("Administrator"))
        {
            var flyOutItem = new FlyoutItem
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
            };

            if (!Shell.Current.Items.Contains(flyOutItem))
                Shell.Current.Items.Add(flyOutItem);
        }

        if (role.Equals("User"))
        {
            var flyOutItem = new FlyoutItem()
            {
                Title = "User Sensor Management",
                Route = nameof(MainPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                Items =
                {
                   new ShellContent
                   {
                       Icon = "dotnet_bot.svg",
                       Title = "User Page 1",
                       ContentTemplate = new DataTemplate(typeof(MainPage))
                   },
                }
            };

            if (!Shell.Current.Items.Contains(flyOutItem))
            {
                Shell.Current.Items.Add(flyOutItem);
            }
        }

        var logoutFyloutItem = new FlyoutItem()
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

        if (!Shell.Current.Items.Contains(logoutFyloutItem))
        {
            Shell.Current.Items.Add(logoutFyloutItem);
        }
    }
}
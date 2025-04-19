using SensorApp.Maui.Views.Controls;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Helpers.MenuRoles;

public class MenuBuilder(IServiceProvider serviceProvider) : IMenuBuilder
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Builds the application's navigation menu dynamically based on the user's role.
    /// Works with the <see cref="IMenuFactory"/> to get the user's menu base on their specific role, and converts them to UI elements that are shown in the app Shell.
    /// </summary>
    /// <param name="userInfo">Information about the currently logged-in user in the app.</param>
    public void BuildMenu(UserInfo userInfo)
    {
        if (userInfo is null)
        {
            throw new InvalidOperationException("User info is required.");
        }

        Shell.Current.Items.Clear();
        Shell.Current.FlyoutHeader = new FlyOutHeader();

        var role = Enum.Parse<UserRole>(userInfo.Role.Replace(" ", ""));
        var menuItems = MenuFactoryResolver.Resolve(role).CreateMenu();

        foreach (var item in menuItems)
        {
            var flyoutItem = CreateFlyoutItem(item);
            Shell.Current.Items.Add(flyoutItem);
        }

        AddLogoutItem();
    }

    /// <summary>
    /// Creates a <see cref="FlyoutItem"/> for the given menu item.
    /// </summary>
    private FlyoutItem CreateFlyoutItem(AppMenuItem item)
    {
        var pageType = Type.GetType($"SensorApp.Maui.Views.Pages.{item.Route}") ?? throw new InvalidOperationException($"Page type '{item.Route}' not found.");

        return new FlyoutItem
        {
            Title = item.Title,
            Route = item.Route,
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
            Items =
            {
                new ShellContent
                {
                    Title = item.Title,
                    ContentTemplate = new DataTemplate(() =>
                        _serviceProvider.GetRequiredService(pageType) as Page)
                }
            }
        };
    }

    /// <summary>
    /// Adds a fixed Logout item to the end of the menu.
    /// </summary>
    private void AddLogoutItem()
    {
        Shell.Current.Items.Add(new FlyoutItem
        {
            Title = "Logout",
            Route = nameof(LogoutPage),
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {
                new ShellContent
                {
                    Title = "Logout",
                    Icon = "dotnet_bot.svg",
                    ContentTemplate = new DataTemplate(() => _serviceProvider.GetRequiredService<LogoutPage>())
                }
            }
        });
    }
}
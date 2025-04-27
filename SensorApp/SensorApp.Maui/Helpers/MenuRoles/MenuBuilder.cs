using SensorApp.Maui.Views.Controls;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Helpers.MenuRoles;

/// <summary>
/// Builds the application's navigation menu dynamically based on the user's role.
/// Utilizes the <see cref="IMenuFactory"/> to generate role-specific menu items and attaches them to the app shell.
/// </summary>
public class MenuBuilder : IMenuBuilder
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBuilder"/> class.
    /// </summary>
    /// <param name="serviceProvider">The application's service provider for resolving page instances.</param>
    public MenuBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Builds the navigation menu based on the authenticated user's role.
    /// Clears existing shell items, creates new flyout menu items, and adds a logout item.
    /// </summary>
    /// <param name="userInfo">Information about the currently authenticated user, including their role.</param>
    public void BuildMenu(UserInfo userInfo)
    {
        if (userInfo is null)
        {
            throw new InvalidOperationException("User info is required.");
        }

        Shell.Current.Items.Clear();
        Shell.Current.FlyoutHeader = new FlyOutHeader();

        var menuItems = MenuFactoryResolver.Resolve(userInfo.Role).CreateMenu();

        foreach (var item in menuItems)
        {
            var flyoutItem = CreateFlyoutItem(item);
            Shell.Current.Items.Add(flyoutItem);
        }

        AddLogoutItem();
    }

    /// <summary>
    /// Creates a <see cref="FlyoutItem"/> for a given application menu item.
    /// Dynamically resolves the associated page type from the service provider.
    /// </summary>
    /// <param name="item">The application menu item to convert into a flyout item.</param>
    /// <returns>A configured <see cref="FlyoutItem"/> for the Shell menu.</returns>
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
    /// Adds a fixed logout option to the Shell menu,
    /// allowing the user to sign out from the application.
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
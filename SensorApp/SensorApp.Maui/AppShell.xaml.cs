using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(LogoutPage), typeof(LogoutPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(AdminUsersPage), typeof(AdminUsersPage));
        Routing.RegisterRoute(nameof(NewUserPage), typeof(NewUserPage));
    }
}

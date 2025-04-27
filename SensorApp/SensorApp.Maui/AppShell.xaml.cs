using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui;

/// <summary>
/// Defines the application's main navigation structure using Shell.
/// Registers all page routes for navigation throughout the app.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppShell"/> class.
    /// Sets up all page routes for the application.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(LogoutPage), typeof(LogoutPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(AdminUsersPage), typeof(AdminUsersPage));
        Routing.RegisterRoute(nameof(NewUserPage), typeof(NewUserPage));
        Routing.RegisterRoute(nameof(SensorMapPage), typeof(SensorMapPage));
        Routing.RegisterRoute(nameof(EditUserPage), typeof(EditUserPage));
        Routing.RegisterRoute(nameof(HistoricalDataPage), typeof(HistoricalDataPage));
        Routing.RegisterRoute(nameof(IncidentList), typeof(IncidentList));
        Routing.RegisterRoute(nameof(CreateIncidentPage), typeof(CreateIncidentPage));
        Routing.RegisterRoute(nameof(IncidentDetailPage), typeof(IncidentDetailPage));

    }
}

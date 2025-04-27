using SensorApp.Shared.Models;

namespace SensorApp.Maui;

/// <summary>
/// The main entry point of the application.
/// Sets up global application state and initializes the main navigation shell.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Holds the authenticated user's information during the application's lifecycle.
    /// </summary>
    public static UserInfo UserInfo { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// Sets up the main navigation structure of the application.
    /// </summary>
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}

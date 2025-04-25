using SensorApp.Shared.Models;

namespace SensorApp.Maui;
public partial class App : Application
{
    public static UserInfo UserInfo { get; set; } = new();

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}

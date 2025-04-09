using SensorApp.Maui.Models;

namespace SensorApp.Maui;
public partial class App : Application
{
    public static UserInfo UserInfo;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}

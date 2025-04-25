using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.ViewModels;

public partial class LogoutViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task LogoutAsync()
    {
        SecureStorage.Remove("Token");
        App.UserInfo = null;

        Application.Current.MainPage = new AppShell();

        await Task.Delay(100);

        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        else
        {
            Console.WriteLine("Navigation failed: Shell.Current is null.");
        }
    }
}
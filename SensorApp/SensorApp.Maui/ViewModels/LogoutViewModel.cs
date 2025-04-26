using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel responsible for handling user logout functionality,
/// including clearing secure storage, resetting user session,
/// and navigating back to the login page.
/// </summary>
public partial class LogoutViewModel : BaseViewModel
{
    /// <summary>
    /// Command that performs the logout operation.
    /// Clears authentication tokens, resets user information,
    /// resets the application's main page, and navigates to the login screen.
    /// </summary>
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
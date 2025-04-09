using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Pages;

namespace SensorApp.Maui.ViewModels;

public partial class LogoutViewModel : BaseViewModel
{
    public LogoutViewModel()
    {
        Logout();
    }


    [RelayCommand]
    async void Logout()
    {
        SecureStorage.Remove("Token");
        App.UserInfo = null;
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
}
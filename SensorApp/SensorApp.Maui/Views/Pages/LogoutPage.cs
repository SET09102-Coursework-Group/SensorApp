using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class LogoutPage : ContentPage
{
    private readonly LogoutViewModel _logoutViewModel;

    public LogoutPage(LogoutViewModel logoutViewModel)
    {


        _logoutViewModel = logoutViewModel;

        BindingContext = logoutViewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _logoutViewModel.LogoutAsync();
    }
}

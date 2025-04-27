using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page responsible for logging the user out of the application.
/// Binds to <see cref="LogoutViewModel"/> which handles the logout process and navigation.
/// </summary>
public partial class LogoutPage : ContentPage
{
    private readonly LogoutViewModel _logoutViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutPage"/> class.
    /// </summary>
    /// <param name="logoutViewModel">The view model responsible for managing the logout process.</param>
    public LogoutPage(LogoutViewModel logoutViewModel)
    {


        _logoutViewModel = logoutViewModel;

        BindingContext = logoutViewModel;

    }

    /// <summary>
    /// Called when the page appears.
    /// Automatically triggers the logout process through the view model.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _logoutViewModel.LogoutAsync();
    }
}

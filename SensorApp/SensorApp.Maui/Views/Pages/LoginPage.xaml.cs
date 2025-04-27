using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page for allowing users to log into the application.
/// Binds to <see cref="LoginViewModel"/> which handles user input, authentication, and navigation after login.
/// </summary>
public partial class LoginPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginPage"/> class.
    /// </summary>
    /// <param name="loginViewModel">The view model responsible for managing login logic and authentication flow.</param>
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}
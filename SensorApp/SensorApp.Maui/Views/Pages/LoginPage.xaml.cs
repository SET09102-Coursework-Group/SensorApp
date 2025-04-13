using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Helpers;
using SensorApp.Maui.Models;
using SensorApp.Maui.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Maui.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel(AuthService authService)
    {
        this.authService = authService;
    }

    [ObservableProperty]
    string username;

    [ObservableProperty]
    string password;
    private AuthService authService;

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayLoginMessage("Invalid Login Attempt");
        }
        else
        {
            var loginModel = new LoginModel(username, password);

            var response = await authService.Login(loginModel);

            await DisplayLoginMessage(authService.StatusMessage);

            if (!string.IsNullOrEmpty(response.Token))
            {
                await SecureStorage.SetAsync("Token", response.Token);

                var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;

                var role = jsonToken.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Role))?.Value;

                App.UserInfo = new UserInfo()
                {
                    Username = Username,
                    Role = role
                };


                MenuBuilder.BuildMenu();
                await Shell.Current.GoToAsync($"{nameof(MainPage)}");

            }
            else
            {
                await DisplayLoginMessage("Invalid Login Attempt");
            }
        }
    }

    async Task DisplayLoginMessage(string message)
    {
        await Shell.Current.DisplayAlert("Login Attempt Result", message, "OK");
        Password = string.Empty;
    }
}
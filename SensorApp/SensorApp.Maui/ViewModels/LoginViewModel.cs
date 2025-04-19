using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Maui.ViewModels;

public partial class LoginViewModel(IAuthService authService, IMenuBuilder menuBuilder) : BaseViewModel
{
    private readonly IAuthService _authService = authService;
    private readonly IMenuBuilder _menuBuilder = menuBuilder;


    [ObservableProperty]
    string username;

    [ObservableProperty]
    string password;

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayLoginMessage("Invalid Login Attempt");
            return;
        }

        var loginDto = new LoginDto(username, password);
        var response = await _authService.Login(loginDto);

        await DisplayLoginMessage(_authService.StatusMessage);

        if (!string.IsNullOrEmpty(response.Token))
        {
            await SecureStorage.SetAsync("Token", response.Token);

            var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;

            var role = jsonToken?.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Role)?.Value;
            var email = jsonToken?.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Email)?.Value;

            App.UserInfo = new UserInfo
            {
                Username = email ?? Username,
                Role = role
            };

            _menuBuilder.BuildMenu(App.UserInfo);
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
        else
        {
            await DisplayLoginMessage("Invalid Login Attempt");
        }
    }

    async Task DisplayLoginMessage(string message)
    {
        await Shell.Current.DisplayAlert("Login Attempt Result", message, "OK");
        Password = string.Empty;
    }
}
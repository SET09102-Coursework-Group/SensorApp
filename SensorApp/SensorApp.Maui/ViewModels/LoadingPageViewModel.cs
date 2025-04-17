using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Maui.ViewModels;

public partial class LoadingPageViewModel : BaseViewModel
{
    private readonly IMenuBuilder _menuBuilder;

    public LoadingPageViewModel(IMenuBuilder menuBuilder)
    {
        _menuBuilder = menuBuilder;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await CheckUserLoginDetailsAsync();
    }
    private async Task CheckUserLoginDetailsAsync()
    {
        var token = await SecureStorage.GetAsync("Token");

        if (string.IsNullOrEmpty(token))
        {
            await GoToLoginPage();
            return;
        }

        if (new JwtSecurityTokenHandler().ReadToken(token) is not JwtSecurityToken jwt || jwt.ValidTo < DateTime.UtcNow)
        {
            SecureStorage.Remove("Token");
            await GoToLoginPage();
            return;
        }

        App.UserInfo = LoadingPageViewModel.ParseUserInfo(jwt);
        _menuBuilder.BuildMenu(App.UserInfo);
        await GoToMainPage();
    }

    private static UserInfo ParseUserInfo(JwtSecurityToken token) => new()
    {
        Username = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        Role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
    };

    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }

    private async Task GoToMainPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }
}
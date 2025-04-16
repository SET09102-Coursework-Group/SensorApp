using SensorApp.Maui.Helpers.MenuRoles;
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
        CheckUserLoginDetails();
    }

    private async void CheckUserLoginDetails()
    {
        var token = await SecureStorage.GetAsync("Token");
        if (string.IsNullOrEmpty(token))
        {
            await GoToLoginPage();
        }
        else
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jsonToken.ValidTo < DateTime.UtcNow)
            {
                SecureStorage.Remove("Token");
                await GoToLoginPage();
            }
            else
            {
                var role = jsonToken.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Role)?.Value;

                App.UserInfo = new UserInfo
                {
                    Username = jsonToken.Claims.FirstOrDefault(q => q.Type == ClaimTypes.Email)?.Value,
                    Role = role
                };

                _menuBuilder.BuildMenu(App.UserInfo);
                await GoToMainPage();
            }
        }
    }

    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }

    private async Task GoToMainPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }
}
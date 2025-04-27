using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for the loading page that verifies user authentication and navigates accordingly.
/// </summary>
public partial class LoadingPageViewModel : BaseViewModel
{
    private readonly IMenuBuilder _menuBuilder;
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingPageViewModel"/> class.
    /// Starts the authentication validation process.
    /// </summary>
    /// <param name="menuBuilder">Service responsible for dynamically building the application menu based on user roles.</param>
    /// <param name="tokenProvider">Service for retrieving and managing authentication tokens.</param>
    public LoadingPageViewModel(IMenuBuilder menuBuilder, ITokenProvider tokenProvider)
    {
        _menuBuilder = menuBuilder;
        _tokenProvider = tokenProvider;
        _ = InitializeAsync();
    }

    /// <summary>
    /// Starts the initial authentication check.
    /// </summary>
    private async Task InitializeAsync()
    {
        await CheckUserLoginDetailsAsync();
    }

    /// <summary>
    /// Verifies the user's login details by checking for a valid and non-expired token.
    /// If valid, parses user info and navigates to the main page; otherwise, navigates to the login page.
    /// </summary>
    private async Task CheckUserLoginDetailsAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();

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

    /// <summary>
    /// Parses a JWT token to extract user information such as username and role.
    /// </summary>
    /// <param name="token">The JWT security token to parse.</param>
    /// <returns>A <see cref="UserInfo"/> object containing user details extracted from the token.</returns>
    private static UserInfo ParseUserInfo(JwtSecurityToken token) => new()
    {
        Username = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        Role = Enum.Parse<UserRole>(
            token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value?.Replace(" ", ""))
    };

    /// <summary>
    /// Navigates the user to the login page.
    /// </summary>
    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }

    /// <summary>
    /// Navigates the user to the main page after successful login validation.
    /// </summary>
    private async Task GoToMainPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Maui.ViewModels;

public partial class LoginViewModel(IAuthService authService, IMenuBuilder menuBuilder) : BaseViewModel
{
    private readonly IAuthService _authService = authService;
    private readonly IMenuBuilder _menuBuilder = menuBuilder;


    [ObservableProperty]
    string username = string.Empty;

    [ObservableProperty]
    string password = string.Empty;


    /// <summary>
    /// Command to perform the login operation.
    /// Validates credentials, authenticates the user,
    /// parses JWT token, and navigates to the main page if successful.
    /// </summary>
    [RelayCommand]
    async Task Login()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await DisplayLoginMessage("Please enter both username and password.");
                return;
            }

            var loginDto = new LoginDto(Username, Password);
            var response = await _authService.Login(loginDto);

            if (string.IsNullOrEmpty(response.Token))
            {
                await DisplayLoginMessage("Invalid login attempt. Please try again.");
                return;
            }

            await SecureStorage.SetAsync("Token", response.Token);

            var userInfo = ParseToken(response.Token);
            if (userInfo == null)
            {
                await DisplayLoginMessage("Login failed: unable to process authentication token.");
                return;
            }

            App.UserInfo = userInfo;

            _menuBuilder.BuildMenu(App.UserInfo);
            await Shell.Current.GoToAsync(nameof(MainPage));
        }
        catch (Exception ex)
        {
            await DisplayLoginMessage("An unexpected error occurred during login. Please try again.");
        }
        finally
        {
            Password = string.Empty;
        }
    }

    /// <summary>
    /// Parses the JWT token and extracts user information.
    /// </summary>
    /// <param name="token">The JWT token string.</param>
    /// <returns>Parsed <see cref="UserInfo"/> object or null if parsing fails.</returns>
    private UserInfo? ParseToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(roleClaim))
                return null;

            var parsedRole = Enum.Parse<UserRole>(roleClaim.Replace(" ", ""), ignoreCase: true);

            return new UserInfo
            {
                Username = emailClaim ?? Username,
                Role = parsedRole
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Displays an alert to inform the user about the login result.
    /// </summary>
    /// <param name="message">Message to display.</param>
    private async Task DisplayLoginMessage(string message)
    {
        await Shell.Current.DisplayAlert("Login Attempt Result", message, "OK");
    }
}
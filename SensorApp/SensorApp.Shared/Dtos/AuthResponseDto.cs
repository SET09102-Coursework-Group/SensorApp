namespace SensorApp.Shared.Dtos;

/// <summary>
/// Data Transfer Object returned by the AuthService after a successful login.
/// This DTO is used in the Maui frontend after authentication to retrieve the token,which will be used in following authorized API calls.
/// </summary>
public class AuthResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

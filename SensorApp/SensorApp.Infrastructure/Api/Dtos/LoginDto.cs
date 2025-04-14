namespace SensorApp.Infrastructure.Api.Dtos;

/// <summary>
/// Data transfer object used when a user attempts to login.
/// Carries the username and password provided by the user.
/// </summary>
internal class LoginDto
{
    public string Username { get; set; } 
    public string Password { get; set; }
}
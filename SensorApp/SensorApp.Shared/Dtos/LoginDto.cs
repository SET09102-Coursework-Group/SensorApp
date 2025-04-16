namespace SensorApp.Shared.Dtos;

/// <summary>
/// Represents the login credentials required to authenticate a user.
/// </summary>
public class LoginDto
{
    public LoginDto(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

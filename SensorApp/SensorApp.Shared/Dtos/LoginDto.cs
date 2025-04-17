namespace SensorApp.Shared.Dtos;

/// <summary>
/// Data Transfer Object used to send user login information from the frontend to the backend authentication API (/login).
/// Contains user input for username and password
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

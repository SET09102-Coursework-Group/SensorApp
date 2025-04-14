namespace SensorApp.Maui.Models;

/// <summary>
/// Represents the login credentials required to authenticate a user.
/// </summary>
public class LoginModel
{
    public LoginModel(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}

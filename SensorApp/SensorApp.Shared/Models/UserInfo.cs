namespace SensorApp.Shared.Models;

/// <summary>
/// Represents the essential information about a logged in user, extracted from the JWT token after a successful login.
/// </summary>
public class UserInfo
{
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

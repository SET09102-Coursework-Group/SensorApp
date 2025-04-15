namespace SensorApp.Maui.Models;

/// <summary>
/// Represents the authentication response received from the server.
/// </summary>
public class AuthResponseModel
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

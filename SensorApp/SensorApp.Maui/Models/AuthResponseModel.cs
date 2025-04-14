namespace SensorApp.Maui.Models;

/// <summary>
/// Represents the authentication response received from the server.
/// </summary>
public class AuthResponseModel
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
}

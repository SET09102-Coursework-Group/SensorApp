namespace SensorApp.Shared.Dtos;

/// <summary>
/// Represents the authentication response received from the server.
/// </summary>
public class AuthResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

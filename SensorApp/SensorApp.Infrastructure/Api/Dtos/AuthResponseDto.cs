namespace SensorApp.Infrastructure.Api.Dtos;

/// <summary>
/// Data transfer object returned after successful authentication.
/// Contains the user's ID, username, and the generated JWT token.
/// /// </summary>
internal class AuthResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;    
    public string Token { get; set; } = string.Empty;
}
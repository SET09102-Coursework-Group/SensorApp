namespace SensorApp.Shared.Dtos.Admin;

/// <summary>
/// Payload used by an administrator to create a new user.
/// </summary>
public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}
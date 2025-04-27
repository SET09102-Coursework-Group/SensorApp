using SensorApp.Shared.Enums;

namespace SensorApp.Shared.Dtos.Admin;

/// <summary>
/// Payload used by an administrator to update an existing user's details.
/// Fields are optional, allowing partial updates.
/// </summary>
public class UpdateUserDto
{
    public string? Username { get; set; }
    public string?   Email { get; set; }
    public string? Password { get; set; }
    public required UserRole Role { get; set; }
}
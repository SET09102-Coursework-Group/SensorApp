using SensorApp.Shared.Enums;

namespace SensorApp.Shared.Dtos.Admin;

/// <summary>
/// Data Transfer Object representing a user along with their assigned role.
/// Used to populate the admin user management tables in the application.
/// </summary>
public class UserWithRoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
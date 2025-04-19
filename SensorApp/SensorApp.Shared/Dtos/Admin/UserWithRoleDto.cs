namespace SensorApp.Shared.Dtos.Admin;

/// <summary>
/// Data Transfer Object representing a user and their associated role.
/// This is used in the Admin section of the app (AdminUsersViewModel) to populate the user management table in the frontend.
/// </summary>
public class UserWithRoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
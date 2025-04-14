namespace SensorApp.Infrastructure.Api.Dtos;

/// <summary>
/// Data transfer object representing a user along with their role.
/// Used by administrator endpoints to return user details.
/// </summary>
internal class UserWithRoleDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
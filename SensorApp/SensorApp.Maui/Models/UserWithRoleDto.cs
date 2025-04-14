namespace SensorApp.Maui.Models;


/// <summary>
/// Data transfer object representing a user along with their assigned role.
/// </summary>
public class UserWithRoleDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
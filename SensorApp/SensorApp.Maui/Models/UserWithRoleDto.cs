namespace SensorApp.Maui.Models;


/// <summary>
/// Data transfer object representing a user along with their assigned role.
/// </summary>
public class UserWithRoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; }    
}
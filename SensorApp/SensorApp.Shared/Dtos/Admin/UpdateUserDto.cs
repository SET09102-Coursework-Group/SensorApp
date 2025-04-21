using SensorApp.Shared.Enums;

namespace SensorApp.Shared.Dtos.Admin;

public class UpdateUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public required UserRole Role { get; set; }
}
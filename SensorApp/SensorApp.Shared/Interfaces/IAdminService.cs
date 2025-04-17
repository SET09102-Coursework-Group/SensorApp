using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Interfaces;

public interface IAdminService
{
    Task<List<UserWithRoleDto>> GetAllUsersAsync(string token);
    Task<bool> AddUserAsync(string token, CreateUserDto newUser);
}
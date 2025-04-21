using SensorApp.Shared.Dtos.Admin;

namespace SensorApp.Shared.Interfaces;

public interface IAdminService
{
    Task<UserWithRoleDto?> GetUserByIdAsync(string token, string userId);
    Task<List<UserWithRoleDto>> GetAllUsersAsync(string token);
    Task<bool> AddUserAsync(string token, CreateUserDto newUser);
    Task<bool> DeleteUserAsync(string token, string userId);
    Task<bool> UpdateUserAsync(string token, string userId, UpdateUserDto updatedUser);
}
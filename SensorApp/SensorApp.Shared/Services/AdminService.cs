using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service class that handles administrative user operations,
/// including retrieving, creating, updating, and deleting users via API calls.
/// </summary>
public class AdminService : IAdminService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminService"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to communicate with the backend API.</param>
    public AdminService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserWithRoleDto?> GetUserByIdAsync(string token, string userId)
    {
        var req = HttpRequestHelper.Create(HttpMethod.Get, $"/admin/users/{userId}", token);
        return await HttpRequestHelper.SendAsync<UserWithRoleDto>(_httpClient, req);
    }

    public async Task<List<UserWithRoleDto>> GetAllUsersAsync(string token)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Get, "/admin/users", token);
        return await HttpRequestHelper.SendAsync<List<UserWithRoleDto>>(_httpClient, request) ?? [];
    }

    public async Task<bool> AddUserAsync(string token, CreateUserDto newUser)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Post, "/admin/users", token, newUser);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }

    public async Task<bool> DeleteUserAsync(string token, string userId)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Delete, $"/admin/users/{userId}", token);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }

    public async Task<bool> UpdateUserAsync(string token, string userId, UpdateUserDto updatedUser)
    {
        var req = HttpRequestHelper.Create(HttpMethod.Put, $"/admin/users/{userId}", token, updatedUser);
        return await HttpRequestHelper.SendAsync(_httpClient, req);
    }
}
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Shared.Services;

public class AdminService(HttpClient httpClient) : IAdminService
{
    private readonly HttpClient _httpClient = httpClient;

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
}
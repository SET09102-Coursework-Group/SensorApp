using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

/// <summary>
/// Service that provides administrator-specific operations
/// </summary>
public class AdminService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;


    public AdminService(HttpClient httpClient, TokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Retrieves a list of all users with their roles by making an API call.
    /// </summary>
    /// <returns>A list of UserWithRoleDto objects. Returns an empty list if the response is null.</returns>
    public async Task<List<UserWithRoleDto>> GetAllUsersAsync()
    {
        await _tokenService.SetAuthHeaderAsync(_httpClient);

        var result = await _httpClient.GetFromJsonAsync<List<UserWithRoleDto>>("/admin/users");
        return result ?? [];
    }
}
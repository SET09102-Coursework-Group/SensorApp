using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

public class AdminService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public AdminService(HttpClient httpClient, TokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<List<UserWithRoleDto>> GetAllUsersAsync()
    {
        await _tokenService.SetAuthHeaderAsync(_httpClient);

        var result = await _httpClient.GetFromJsonAsync<List<UserWithRoleDto>>("/admin/users");
        return result ?? [];
    }
}
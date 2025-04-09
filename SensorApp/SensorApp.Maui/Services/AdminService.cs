using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

public class AdminService
{
    private readonly HttpClient _httpClient;

    public AdminService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(AuthService.BaseAddress)
        };
    }

    //TODO separate in its own class
    private async Task SetAuthTokenAsync()
    {
        var token = await SecureStorage.GetAsync("Token");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<UserWithRoleDto>> GetAllUsersAsync()
    {
        await SetAuthTokenAsync();
        var result = await _httpClient.GetFromJsonAsync<List<UserWithRoleDto>>("/admin/users");
        return result ?? new List<UserWithRoleDto>();
    }
}
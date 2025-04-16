using SensorApp.Shared.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service that provides administrator-specific operations
/// </summary>
public class AdminService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// Retrieves a list of all users with their roles by calling the admin endpoint.
    /// </summary>
    public async Task<List<UserWithRoleDto>> GetAllUsersAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetFromJsonAsync<List<UserWithRoleDto>>("/admin/users");
            return result ?? [];
        }
        catch (HttpRequestException)
        {
            return [];
        }
        catch (Exception)
        {
            return [];
        }
    }
}
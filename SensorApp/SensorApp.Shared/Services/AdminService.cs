using SensorApp.Shared.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SensorApp.Shared.Services;

public class AdminService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// Fetches a list of all users along with their assigned roles. Intended for administrators only through the jwt token. 
    /// </summary>
    /// <param name="token">The JWT token of the currently loggedin user.</param>
    /// <returns>A list of users with their roles, or an empty list if the request fails.</returns>
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
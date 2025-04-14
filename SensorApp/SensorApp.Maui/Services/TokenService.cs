using System.Net.Http.Headers;

namespace SensorApp.Maui.Services;

/// <summary>
/// Service for managing the authentication token and setting it on HTTP requests.
/// </summary>
public class TokenService
{
    public async Task<string?> GetAuthTokenAsync()
    {
        return await SecureStorage.GetAsync("Token");
    }

    public async Task<bool> SetAuthHeaderAsync(HttpClient httpClient)
    {
        var token = await GetAuthTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }
        return false;
    }
}
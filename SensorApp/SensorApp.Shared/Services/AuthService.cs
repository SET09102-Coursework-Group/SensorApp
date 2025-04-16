using Newtonsoft.Json;
using SensorApp.Shared.Dtos;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service for handling authentication requests.
/// </summary>
public class AuthService
{
    private readonly HttpClient _httpClient;

    public string StatusMessage { get; private set; } = string.Empty;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Attempts to log in the user by sending their credentials to the API.
    /// </summary>
    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginDto);
            response.EnsureSuccessStatusCode();

            StatusMessage = "Login Successful";
            var jsonContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponseDto>(jsonContent)
                               ?? new AuthResponseDto();

            return authResponse;
        }
        catch (Exception)
        {
            StatusMessage = "Attempt failed";
            return new AuthResponseDto();
        }
    }
}
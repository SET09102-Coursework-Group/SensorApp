using Newtonsoft.Json;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Interfaces;
using System.Net.Http.Json;

namespace SensorApp.Shared.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;

    public string StatusMessage { get; private set; } = string.Empty;


    /// <summary>
    /// Sends the user's login credentials to the backend and processes the response
    /// </summary>
    /// <param name="loginDto">The login credentials entered by the user.</param>
    /// <returns> a <see cref="AuthResponseDto"/> with JWT  and basic user info
    /// </returns>
    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginDto);
            response.EnsureSuccessStatusCode();

            StatusMessage = "Login Successful";
            var jsonContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponseDto>(jsonContent) ?? new AuthResponseDto();

            return authResponse;
        }
        catch (Exception)
        {
            StatusMessage = "Attempt failed";
            return new AuthResponseDto();
        }
    }
}
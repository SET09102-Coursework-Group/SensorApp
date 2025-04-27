using SensorApp.Shared.Dtos;
using SensorApp.Shared.Interfaces;
using System.Net.Http.Json;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service responsible for handling user authentication by communicating with the backend API.
/// Implements the <see cref="IAuthService"/> interface to perform login operations and track authentication status.
/// </summary>
public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to send authentication requests.</param>
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string StatusMessage { get; private set; } = string.Empty;

    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginDto);
            response.EnsureSuccessStatusCode();

            StatusMessage = "Login Successful";
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();


            return authResponse ?? new AuthResponseDto();
        }
        catch (Exception)
        {
            StatusMessage = "Attempt failed";
            return new AuthResponseDto();
        }
    }
}
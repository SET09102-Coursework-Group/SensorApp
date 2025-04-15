using Newtonsoft.Json;
using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

/// <summary>
/// Service for handling authentication requests.
/// </summary>
public class AuthService
{
    private readonly HttpClient _httpClient;

    public string StatusMessage = null!;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Attempts to log in the user by sending their credentials to the API.
    /// </summary>
    /// <param name="loginModel">Contains username and password.</param>
    /// <returns>
    /// An AuthResponseModel containing the user ID, username, and token on successful login.
    /// Returns an empty model on failure.
    /// </returns>

    public async Task<AuthResponseModel> Login(LoginModel loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginModel);
            response.EnsureSuccessStatusCode();
            StatusMessage = "Login Successful";

            var jsonContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponseModel>(jsonContent);

            if (authResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize the authentication response.");
            }

            return authResponse;
        }
        catch (Exception ex)
        {
            StatusMessage = "Attempt failed";
            return new AuthResponseModel();
        }
    }
}

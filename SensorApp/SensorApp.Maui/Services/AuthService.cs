using Newtonsoft.Json;
using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public string StatusMessage = null!;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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

using Newtonsoft.Json;
using SensorApp.Maui.Models;
using System.Net.Http.Json;

namespace SensorApp.Maui.Services;

public class AuthService
{
    HttpClient _httpClient;

    //TODO extract this to a config file
    public static string BaseAddress = "https://0efa-77-97-219-86.ngrok-free.app";

    public string StatusMessage;

    public AuthService()
    {
        _httpClient = new() { BaseAddress = new Uri(BaseAddress) };
    }

    public async Task<AuthResponseModel> Login(LoginModel loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/login", loginModel);
            response.EnsureSuccessStatusCode();
            StatusMessage = "Login Successful";

            return JsonConvert.DeserializeObject<AuthResponseModel>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            StatusMessage = "Attempt failed";
            return new AuthResponseModel();
        }
    }

    public async Task SetAuthToken()
    {
        var token = await SecureStorage.GetAsync("Token");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

}

using SensorApp.Shared.Dtos;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

public class TestUserBuilder
{
    private readonly HttpClient _client;
    private string _email = "admin@sensor.com";
    private string _password = "MyP@ssword123";

    public TestUserBuilder(HttpClient client)
    {
        _client = client;
    }

    public TestUserBuilder WithCredentials(string email, string password)
    {
        _email = email;
        _password = password;
        return this;
    }

    public async Task<string> BuildTokenAsync()
    {
        var auth = await BuildAsync();
        return auth.Token;
    }

    public async Task<AuthResponseDto> BuildAsync()
    {
        var loginDto = new LoginDto(_email, _password);
        var response = await _client.PostAsJsonAsync("/login", loginDto);
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!;
    }

}

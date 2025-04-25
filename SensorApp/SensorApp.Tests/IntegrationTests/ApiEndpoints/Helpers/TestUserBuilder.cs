using SensorApp.Shared.Dtos;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

/// <summary>
/// A builder class to easily create and authenticate test users for integration tests.
/// </summary>
public class TestUserBuilder
{
    private readonly HttpClient _client;
    private string _email = "admin@sensor.com";
    private string _password = "MyP@ssword123";

    public TestUserBuilder(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Overrides the default email and password with custom test credentials.
    /// </summary>
    /// <param name="email">The test user's email.</param>
    /// <param name="password">The test user's password.</param>
    /// <returns>The updated <see cref="TestUserBuilder"/> instance for chaining.</returns>
    public TestUserBuilder WithCredentials(string email, string password)
    {
        _email = email;
        _password = password;
        return this;
    }

    /// <summary>
    /// Builds and retrieves a JWT token for the test user
    /// </summary>
    /// <returns>A JWT token as a string.</returns>
    public async Task<string> BuildTokenAsync()
    {
        var auth = await BuildAsync();
        return auth.Token;
    }

    /// <summary>
    /// Builds the authentication response (AuthResponseDto) for the test user.
    /// </summary>
    /// <returns>The <see cref="AuthResponseDto"/> containing user info and token.</returns>
    public async Task<AuthResponseDto> BuildAsync()
    {
        var loginDto = new LoginDto(_email, _password);
        var response = await _client.PostAsJsonAsync("/login", loginDto);
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!;
    }
}

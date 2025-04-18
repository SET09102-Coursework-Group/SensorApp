using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SensorApp.Api;
using SensorApp.Shared.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints;


/// <summary>
/// Integration tests for the /login authentication endpoint.
/// These tests verify the behavior by making real HTTP requests against the running API in-memory
/// </summary>
public class AuthEndpointTests(WebApplicationFactoryForTests factory) : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task HappyPath_UserLogin_WithValidCredentials_SystemReturnsToken()
    {
        // Arrange
        var loginDto = new LoginDto("admin@sensor.com", "MyP@ssword123");

        // Act
        var response = await _client.PostAsJsonAsync("/login", loginDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Username.Should().Be("admin@sensor.com");
    }

    [Fact]
    public async Task UserLogin_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var loginDto = new LoginDto("admin@sensor.com", "RandomWrongPassword123");

        var response = await _client.PostAsJsonAsync("/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        var loginDto = new LoginDto("administrator@sensor.com", "MyP@ssword123");

        var response = await _client.PostAsJsonAsync("/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

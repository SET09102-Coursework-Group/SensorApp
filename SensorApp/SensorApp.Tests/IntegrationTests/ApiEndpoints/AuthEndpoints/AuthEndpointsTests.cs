using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SensorApp.Api;
using SensorApp.Shared.Dtos;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;


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
        // Arrange & Act
        var auth = await new TestUserBuilder(_client).WithCredentials(TestUsers.AdminEmail, TestUsers.AdminPassword).BuildAsync();

        // Assert
        auth.Should().NotBeNull();
        auth.Token.Should().NotBeNullOrEmpty();
        auth.Username.Should().Be(TestUsers.AdminEmail);
    }

    [Fact]
    public async Task UserLogin_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto(TestUsers.AdminEmail, "RandomWrongPassword123");

        // Act
        var response = await _client.PostAsJsonAsync("/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto("nonexistent@sensor.com", TestUsers.AdminPassword);

        // Act
        var response = await _client.PostAsJsonAsync("/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

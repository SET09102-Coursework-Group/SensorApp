using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SensorApp.Api;
using SensorApp.Shared.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints;

/// <summary>
/// Integration tests for the protected /admin/users endpoint.
/// These tests verify that role-based authorisation is working as expected using the in-memory API.
/// </summary>
public class AdminEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task HappyPath_GetUsers_ReturnsUserList_ForAdmin()
    {
        // Arrange
        var token = await LoginAndGetToken("admin@sensor.com", "MyP@ssword123");

        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        users.Should().NotBeNull();
        users!.Count.Should().BeGreaterThan(0);
        users!.Any(u => u.Username == "admin@sensor.com").Should().BeTrue();
    }

    [Fact]
    public async Task NonAdmin_IsForbidden_FromSeeingUsers_When_LoggedIn()
    {
        // Arrange
        var token = await LoginAndGetToken("ops@sensor.com", "MyP@ssword123");

        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task WithoutToken_GetUsers_IsUnauthorized_WhenLoggedIn()
    {
        //Arrange has no token
        
        //Act
        var response = await _client.GetAsync("/admin/users");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<string> LoginAndGetToken(string username, string password)
    {
        var loginDto = new LoginDto(username, password);
        var response = await _client.PostAsJsonAsync("/login", loginDto);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!.Token;
    }
}
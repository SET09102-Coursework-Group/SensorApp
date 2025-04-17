using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SensorApp.Api;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
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

    [Fact]
    public async Task Admin_CreatesNewUser_ReturnsSuccess()
    {
        //Arrange
        var token = await LoginAndGetToken("admin@sensor.com", "MyP@ssword123");
        var uniqueId = Guid.NewGuid();
        var email = $"testUser_{uniqueId}@sensor.com";

        var newUser = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "TestP@ssword123",
            Role = UserRole.OperationsManager.ToString()
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/admin/users")
        {
            Content = JsonContent.Create(newUser)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //Assert
        var created = await response.Content.ReadFromJsonAsync<UserWithRoleDto>();
        created.Should().NotBeNull();
        created!.Email.Should().Be(newUser.Email);
        created.Role.Should().Be(newUser.Role);
    }

    [Fact]
    public async Task Admin_CannotCreateADuplicateUser_ReturnsConflict()
    {
        //Arrange
        var token = await LoginAndGetToken("admin@sensor.com", "MyP@ssword123");

        var duplicateUser = new CreateUserDto
        {
            Username = "admin@sensor.com",
            Email = "admin@sensor.com",
            Password = "MyP@ssword123",
            Role = UserRole.Administrator.ToString()
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/admin/users")
        {
            Content = JsonContent.Create(duplicateUser)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await _client.SendAsync(request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
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
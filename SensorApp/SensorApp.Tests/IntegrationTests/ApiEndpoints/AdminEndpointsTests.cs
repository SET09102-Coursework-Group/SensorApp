using FluentAssertions;
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
public class AdminEndpointTests(WebApplicationFactoryForTests factory) : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client = factory.CreateClient();
    private const string _adminEmail = "admin@sensor.com";
    private const string _adminPassword = "MyP@ssword123";

    [Fact]
    public async Task HappyPath_GetUsers_ReturnsUserList_ForAdmin()
    {
        // Arrange
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);

        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        users.Should().NotBeNull();
        users!.Count.Should().BeGreaterThan(0);
        users!.Any(u => u.Username == _adminEmail).Should().BeTrue();
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
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);
        var uniqueId = Guid.NewGuid();
        var email = $"testUser_{uniqueId}@sensor.com";

        var newUser = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "TestP@ssword123",
            Role = UserRole.OperationsManager
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
        created.Role.Should().Be(newUser.Role.ToString());
    }

    [Fact]
    public async Task Admin_CannotCreateADuplicateUser_ReturnsConflict()
    {
        //Arrange
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);

        var duplicateUser = new CreateUserDto
        {
            Username = _adminEmail,
            Email = _adminEmail,
            Password = _adminPassword,
            Role = UserRole.Administrator
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

    [Fact]
    public async Task Admin_DeletesExistingUser_Successfully()
    {
        // Arrange
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);
        var uniqueId = Guid.NewGuid();
        var email = $"delete_test_{uniqueId}@sensor.com";

        var newUser = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "DeleteMe123!",
            Role = UserRole.OperationsManager
        };

        var createRequest = new HttpRequestMessage(HttpMethod.Post, "/admin/users")
        {
            Content = JsonContent.Create(newUser)
        };
        createRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.SendAsync(createRequest);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserWithRoleDto>();
        createdUser.Should().NotBeNull();

        // Act
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/admin/users/{createdUser!.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var deleteResponse = await _client.SendAsync(deleteRequest);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify user no longer exists
        var exists = await UserExistsAsync(createdUser.Id, token);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Admin_DeletesNonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);
        var nonExistentId = Guid.NewGuid().ToString();

        var exists = await UserExistsAsync(nonExistentId, token);
        exists.Should().BeFalse("The user id doesn't exist");

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/admin/users/{nonExistentId}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(deleteRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Admin_CannotDeleteOwnAccount_ReturnsBadRequest()
    {
        // Arrange
        var token = await LoginAndGetToken(_adminEmail, _adminPassword);

        // Get the current user's ID
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/admin/users");
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var getResponse = await _client.SendAsync(getRequest);
        var users = await getResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();

        var currentUser = users!.FirstOrDefault(u => u.Email == _adminEmail);
        currentUser.Should().NotBeNull();

        // Act
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/admin/users/{currentUser!.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var deleteResponse = await _client.SendAsync(deleteRequest);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }



    private async Task<string> LoginAndGetToken(string username, string password)
    {
        var loginDto = new LoginDto(username, password);
        var response = await _client.PostAsJsonAsync("/login", loginDto);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!.Token;
    }

    private async Task<bool> UserExistsAsync(string userId, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        return users!.Any(u => u.Id == userId);
    }

}
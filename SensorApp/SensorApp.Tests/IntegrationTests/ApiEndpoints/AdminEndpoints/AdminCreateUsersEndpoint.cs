using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

public class AdminCreateUsersEndpoint : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client;
    private readonly TokenProvider _tokenProvider;

    public AdminCreateUsersEndpoint(WebApplicationFactoryForTests factory)
    {
        _client = factory.CreateClient();
        _tokenProvider = new TokenProvider(_client);
    }

    [Fact]
    public async Task Admin_CreatesNewUser_ReturnsSuccess()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var uniqueEmail = $"testUser_{Guid.NewGuid()}@sensor.com";

        var newUser = new CreateUserDto
        {
            Username = uniqueEmail,
            Email = uniqueEmail,
            Password = "TestP@ssword123",
            Role = UserRole.OperationsManager
        };

        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Post, newUser);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<UserWithRoleDto>();
        created.Should().NotBeNull();
        created!.Email.Should().Be(newUser.Email);
        created.Role.Should().Be(newUser.Role.ToString());
    }

    [Fact]
    public async Task Admin_CannotCreateADuplicateUser_ReturnsConflict()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();

        var duplicateUser = new CreateUserDto
        {
            Username = TestUsers.AdminEmail,
            Email = TestUsers.AdminEmail,
            Password = TestUsers.AdminPassword,
            Role = UserRole.Administrator
        };

        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Post, duplicateUser);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Admin_GetsUserById_ReturnsUser()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();

        var userListRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var response = await _client.SendAsync(userListRequest);
        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();

        var targetUser = users!.FirstOrDefault(u => u.Email != TestUsers.AdminEmail);
        targetUser.Should().NotBeNull();

        var getByIdRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{targetUser!.Id}", token, HttpMethod.Get);

        // Act
        var getByIdResponse = await _client.SendAsync(getByIdRequest);

        // Assert
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await getByIdResponse.Content.ReadFromJsonAsync<UserWithRoleDto>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(targetUser.Id);
        user.Email.Should().Be(targetUser.Email);
        user.Role.Should().Be(targetUser.Role);
    }

    [Fact]
    public async Task Admin_GetsUserById_ReturnsNotFound_IfUserDoesNotExist()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var nonExistentId = Guid.NewGuid().ToString();

        var request = TestHelpers.CreateAuthorizedRequest($"/admin/users/{nonExistentId}", token, HttpMethod.Get);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task NonAdminUser_CannotAccess_GetUserById_ReturnsForbidden()
    {
        // Arrange
        var token = await _tokenProvider.GetOpsTokenAsync();
        var request = TestHelpers.CreateAuthorizedRequest($"/admin/users/{Guid.NewGuid()}", token, HttpMethod.Get);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UnauthorizedUser_CannotAccess_GetUserById_ReturnsUnauthorized()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/admin/users/{Guid.NewGuid()}");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

public class AdminGetUsersEndpoint(WebApplicationFactoryForTests factory) : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly TokenProvider _tokenProvider = new(factory.CreateClient());

    private const string _adminEmail = "admin@sensor.com";


    [Fact]
    public async Task HappyPath_Admin_GetAllUsers_ReturnsOK()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();

        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token);

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
        var token = await _tokenProvider.GetOpsTokenAsync();

        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task WithoutToken_GetUsers_IsUnauthorized_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync("/admin/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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
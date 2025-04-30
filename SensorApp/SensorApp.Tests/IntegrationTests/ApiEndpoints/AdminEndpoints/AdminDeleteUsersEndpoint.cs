using FluentAssertions;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

/// <summary>
/// Integration tests for the admin endpoint responsible for deleting users.
/// </summary>
public class AdminDeleteUsersEndpoint : IClassFixture<WebApplicationFactoryForTests>
{
    private readonly HttpClient _client;
    private readonly TokenProvider _tokenProvider;

    public AdminDeleteUsersEndpoint(WebApplicationFactoryForTests factory)
    {
        _client = factory.CreateClient();
        _tokenProvider = new TokenProvider(_client);
    }

    [Fact]
    public async Task Admin_DeletesExistingUser_Successfully()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var email = $"delete_test_{Guid.NewGuid()}@sensor.com";

        var newUser = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "DeleteMe123!",
            Role = UserRole.OperationsManager
        };

        var createRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Post, newUser);
        var createResponse = await _client.SendAsync(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserWithRoleDto>();
        createdUser.Should().NotBeNull();

        // Act
        var deleteRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{createdUser!.Id}", token, HttpMethod.Delete);
        var deleteResponse = await _client.SendAsync(deleteRequest);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var exists = await UserApiHelper.UserExistsAsync(_client, token, createdUser.Id);
        exists.Should().BeFalse("The user should no longer exist after deletion.");
    }

    [Fact]
    public async Task Admin_DeletesNonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();
        var nonExistentId = Guid.NewGuid().ToString();

        var exists = await UserApiHelper.UserExistsAsync(_client, token, nonExistentId);
        exists.Should().BeFalse("The user ID should not exist before the delete attempt.");

        var deleteRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{nonExistentId}", token, HttpMethod.Delete);

        // Act
        var response = await _client.SendAsync(deleteRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Admin_CannotDeleteOwnAccount_ReturnsBadRequest()
    {
        // Arrange
        var token = await _tokenProvider.GetAdminTokenAsync();

        var getRequest = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var getResponse = await _client.SendAsync(getRequest);
        var users = await getResponse.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();

        var currentUser = users!.FirstOrDefault(u => u.Email == TestUsers.AdminEmail);
        currentUser.Should().NotBeNull("Admin user must exist in the list");

        // Act
        var deleteRequest = TestHelpers.CreateAuthorizedRequest($"/admin/users/{currentUser!.Id}", token, HttpMethod.Delete);
        var deleteResponse = await _client.SendAsync(deleteRequest);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NonAdmin_Cannot_DeleteUser_ReturnsForbidden()
    {
        // Arrange 
        var adminToken = await _tokenProvider.GetAdminTokenAsync();
        var opsToken = await _tokenProvider.GetOpsTokenAsync();
        var email = $"cantDelete_{Guid.NewGuid()}@sensor.com";
        var dto = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "DeleteMe123!",
            Role = UserRole.EnvironmentalScientist
        };
        var createReq = TestHelpers.CreateAuthorizedRequest("/admin/users", adminToken, HttpMethod.Post, dto);
        var createResp = await _client.SendAsync(createReq);
        var created = await createResp.Content.ReadFromJsonAsync<UserWithRoleDto>();

        // Act – 
        var delReq = TestHelpers.CreateAuthorizedRequest($"/admin/users/{created!.Id}", opsToken, HttpMethod.Delete);
        var delResp = await _client.SendAsync(delReq);

        // Assert
        delResp.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

}
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
}
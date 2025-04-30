using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SensorApp.Database.Data;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.AdminEndpoints;

/// <summary>
/// Integration tests for the admin endpoint responsible for creating new users.
/// Verifies that an administrator can successfully create users through the API.
/// </summary>
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
        created!.Id.Should().NotBeNullOrWhiteSpace();                       
        created.Username.Should().Be(newUser.Username);                        
        created.Email.Should().Be(newUser.Email);                          
        created.Role.Should().Be(newUser.Role);                         
    }


    [Fact]
    public async Task Admin_CreateUser_WithDuplicateEmail_ReturnsConflict()
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
    public async Task Admin_Cannot_Create_DuplicateUsername_ReturnsConflict()
    {
        // Arrange
        var adminToken = await _tokenProvider.GetAdminTokenAsync();
        var dupName = $"dupUser_{Guid.NewGuid()}";
        var firstDto = new CreateUserDto
        {
            Username = dupName,
            Email = dupName + "@sensor.com",
            Password = "Dup1icatePw!",
            Role = UserRole.EnvironmentalScientist
        };
        var secondDto = new CreateUserDto
        {
            Username = dupName,
            Email = "different_" + dupName + "@sensor.com",
            Password = "Dup1icatePw!",
            Role = UserRole.EnvironmentalScientist
        };

        var firstReq = TestHelpers.CreateAuthorizedRequest("/admin/users", adminToken, HttpMethod.Post, firstDto);
        var firstResp = await _client.SendAsync(firstReq);
        firstResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondReq = TestHelpers.CreateAuthorizedRequest("/admin/users", adminToken, HttpMethod.Post, secondDto);
        var secondResp = await _client.SendAsync(secondReq);

        // Assert
        secondResp.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    [Fact]
    public async Task NonAdmin_Cannot_Create_User_ReturnsForbidden()
    {
        // Arrange
        var opsToken = await _tokenProvider.GetOpsTokenAsync();
        var email = $"opsCreate_{Guid.NewGuid()}@sensor.com";
        var dto = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "Temporary123!",
            Role = UserRole.EnvironmentalScientist
        };
        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", opsToken, HttpMethod.Post, dto);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Admin_CreateUser_WithWeakPassword_ReturnsBadRequest()
    {
        // Arrange 
        var adminToken = await _tokenProvider.GetAdminTokenAsync();
        var email = $"weakpw_{Guid.NewGuid()}@sensor.com";
        var dto = new CreateUserDto
        {
            Username = email,
            Email = email,
            Password = "123",
            Role = UserRole.OperationsManager
        };
        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", adminToken, HttpMethod.Post, dto);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task UnauthenticatedUser_Cannot_CreateUser_ReturnsUnauthorized()
    {
        var dto = new CreateUserDto
        {
            Username = $"anon_{Guid.NewGuid()}",
            Email = $"anon_{Guid.NewGuid()}@sensor.com",
            Password = "ValidP@ss123",
            Role = UserRole.EnvironmentalScientist
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/admin/users")
        {
            Content = JsonContent.Create(dto)
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

}
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SensorApp.Tests.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="AdminService"/>, responsible for calling the admin user API endpoints in the API project
/// </summary>
public class AdminServiceTests
{
    [Fact]
    public async Task HappyPath_GetAllUsersEndpoint_ReturnsList_WhenSuccessfulResponse()
    {
        // Arrange
        var mockUsers = new List<UserWithRoleDto>
        {
            new() { Id = "1", Username = "admin", Email = "admin@sensor.com",    Role = UserRole.Administrator },
            new() { Id = "2", Username = "ops",   Email = "ops@sensor.com",      Role = UserRole.OperationsManager }
        };

        var json = JsonSerializer.Serialize(mockUsers);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetAllUsersAsync("goodToken");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsEmptyList_WhenUnauthorized_Following401Response()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetAllUsersAsync("noToken");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsEmptyList_WhenExceptionThrown()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("Something went wrong"));
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetAllUsersAsync("noToken");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddUser_ReturnsTrue_WhenSuccessful()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Created);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var newUser = new CreateUserDto
        {
            Username = "test",
            Email = "test@sensor.com",
            Password = "TestP@ssword123",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.AddUserAsync("goodToken", newUser);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddUser_ReturnsFalse_WhenConflict()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Conflict);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var newUser = new CreateUserDto
        {
            Username = "existing",
            Email = "admin@sensor.com",
            Password = "TestP@ss123",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.AddUserAsync("goodToken", newUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddUser_ReturnsFalse_WhenUnauthorized()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var newUser = new CreateUserDto
        {
            Username = "unauthorized",
            Email = "unauthorized@sensor.com",
            Password = "InvalidToken123",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.AddUserAsync("badToken", newUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsTrue_WhenSuccessful()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.DeleteUserAsync("goodToken", "randomId");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsFalse_WhenUserNotFound()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.DeleteUserAsync("goodToken", "nonExistentUserId");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsFalse_WhenUnauthorized()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.DeleteUserAsync("badToken", "randomUserId");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenSuccessful()
    {
        // Arrange
        var user = new UserWithRoleDto
        {
            Id = "123",
            Username = "testuser",
            Email = "test@sensor.com",
            Role = UserRole.Administrator
        };

        var json = JsonSerializer.Serialize(user);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetUserByIdAsync("goodToken", "123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("123", result!.Id);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("test@sensor.com", result.Email);
    }

    [Fact]
    public async Task GetUserById_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetUserByIdAsync("goodToken", "nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserById_ReturnsNull_WhenException()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("Failure"));
        var service = new AdminService(httpClient);

        // Act
        var result = await service.GetUserByIdAsync("badToken", "123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsTrue_WhenSuccessful()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NoContent);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var updatedUser = new UpdateUserDto
        {
            Username = "updateduser",
            Email = "updated@sensor.com",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.UpdateUserAsync("goodToken", "123", updatedUser);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsFalse_WhenBadRequest()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var updatedUser = new UpdateUserDto
        {
            Username = "baduser",
            Email = "bad@sensor.com",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.UpdateUserAsync("goodToken", "123", updatedUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsFalse_WhenException()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("exception"));
        var service = new AdminService(httpClient);

        var updatedUser = new UpdateUserDto
        {
            Username = "failuser",
            Email = "fail@sensor.com",
            Role = UserRole.Administrator
        };

        // Act
        var result = await service.UpdateUserAsync("badToken", "123", updatedUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsFalse_WhenUnauthorized()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var updatedUser = new UpdateUserDto
        {
            Username = "unauthorizedUser",
            Email = "unauth@sensor.com",
            Role = UserRole.EnvironmentalScientist
        };

        // Act
        var result = await service.UpdateUserAsync("nonAdminToken", "123", updatedUser);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUser_ReturnsFalse_WhenForbidden()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        var httpClient = HttpClientTestFactory.Create(response);
        var service = new AdminService(httpClient);

        var updatedUser = new UpdateUserDto
        {
            Username = "forbiddenUser",
            Email = "forbidden@sensor.com",
            Role = UserRole.OperationsManager
        };

        // Act
        var result = await service.UpdateUserAsync("opsToken", "123", updatedUser);

        // Assert
        Assert.False(result);
    }


}
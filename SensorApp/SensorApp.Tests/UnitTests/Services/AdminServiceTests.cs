using SensorApp.Shared.Dtos;
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
            new() { Id = "1", Username = "admin", Email = "admin@sensor.com", Role = "Administrator" },
            new() { Id = "2", Username = "ops", Email = "ops@sensor.com", Role = "Operations Manager" }
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
}
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SensorApp.Tests.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="AuthService"/> which handles login and token retrieval from the API
/// </summary>
public class AuthServiceTests
{
    [Fact]
    public async Task Login_ReturnsAuthResponse_WhenSuccessful()
    {
        // Arrange
        var loginDto = new LoginDto("admin", "password");

        var responseDto = new AuthResponseDto
        {
            UserId = "1",
            Username = "admin",
            Token = "ARandomGuidToken"
        };

        var responseContent = new StringContent(JsonSerializer.Serialize(responseDto), Encoding.UTF8, "application/json");

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = responseContent
        };

        var httpClient = HttpClientTestFactory.Create(response);
        var authService = new AuthService(httpClient);

        // Act
        var result = await authService.Login(loginDto);

        // Assert
        Assert.Equal("admin", result.Username);
        Assert.Equal("ARandomGuidToken", result.Token);
    }

    [Fact]
    public async Task Login_ReturnsEmpty_WhenRequestFails()
    {
        // Arrange
        var loginDto = new LoginDto("admin", "wrongpassword");

        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var httpClient = HttpClientTestFactory.Create(response);
        var authService = new AuthService(httpClient);

        // Act
        var result = await authService.Login(loginDto);

        // Assert
        Assert.True(string.IsNullOrEmpty(result.Token));
    }

    [Fact]
    public async Task Login_ReturnsEmpty_WhenExceptionThrown()
    {
        // Arrange
        var httpClient = HttpClientTestFactory.CreateWithException(new HttpRequestException("Exception"));
        var authService = new AuthService(httpClient);
        var loginDto = new LoginDto("admin", "password");

        // Act
        var result = await authService.Login(loginDto);

        // Assert
        Assert.True(string.IsNullOrEmpty(result.Token));
    }
}
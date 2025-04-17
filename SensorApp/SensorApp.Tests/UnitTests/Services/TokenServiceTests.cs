using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SensorApp.Core.Services.Auth;
using SensorApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SensorApp.Tests.UnitTests.Services;

/// <summary>
/// Unit tests for the <see cref="TokenService"/>, the responsible for generating JWT tokens for authentication
/// </summary>
public class TokenServiceTests
{
    private readonly JwtSettings _jwtSettings = new()
    {
        Key = "random_guid_secret_key_for_testing_1234", 
        Issuer = "SensorApp",
        Audience = "SensorUsers",
        DurationInMinutes = 60
    };

    private TokenService CreateTokenService()
    {
        var options = Options.Create(_jwtSettings);
        return new TokenService(options);
    }

    [Fact]
    public void GenerateToken_IncludingRoleClaims()
    {
        // Arrange
        var tokenService = CreateTokenService();
        var user = new IdentityUser
        {
            Id = "user-guid-123",
            Email = "test@sensor.com",
        };
        var roles = new List<string> { "Administrator" };

        // Act
        var token = tokenService.GenerateToken(user, roles);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(_jwtSettings.Issuer, jwt.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwt.Audiences.First());
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Email && c.Value == "test@sensor.com");
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Administrator");
    }

    [Fact]
    public void GenerateToken_ThrowsException_WhenUserIsNull()
    {
        // Arrange
        var tokenService = CreateTokenService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tokenService.GenerateToken(null!, []));
    }
}
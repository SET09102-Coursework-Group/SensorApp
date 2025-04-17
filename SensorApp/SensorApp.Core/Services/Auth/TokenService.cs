using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SensorApp.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Implementation for <see cref="ITokenService"/> that generates JWT tokens
/// Uses application defined <see cref="JwtSettings"/> for token configuration.
/// </summary>
public class TokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _jwtSettings = options.Value;

    public string GenerateToken(IdentityUser user, IList<string> roles)
    {
        return user == null
            ? throw new ArgumentNullException(nameof(user))
            : GenerateToken(user.Id, user.Email ?? string.Empty, roles);
    }

    /// <summary>
    /// This method builds the JWT token using provided user info and roles
    /// </summary>
    /// <param name="userId">The user's unique id.</param>
    /// <param name="email">The user's email address through email claim</param>
    /// <param name="roles">List of roles to include in the token.</param>
    /// <returns>A JWT token string signed with the configured secret key.</returns>
    private string GenerateToken(string userId, string email, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenClaims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, userId),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, email),
        };

        tokenClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: tokenClaims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
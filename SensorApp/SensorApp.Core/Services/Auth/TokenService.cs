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
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenClaims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, user.Email ?? string.Empty),
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
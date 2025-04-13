using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SensorApp.Infrastructure.Services.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(IdentityUser user, IList<string> roles, IList<Claim> additionalClaims)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }
        if (roles == null)
        {
            throw new ArgumentNullException(nameof(roles), "Roles list cannot be null.");
        }


        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>
            {
                    new (JwtRegisteredClaimNames.Sub, user.Id),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (ClaimTypes.Email, user.Email!),
                    new ("email_confirmed", user.EmailConfirmed.ToString())
            };

            tokenClaims.AddRange(additionalClaims);
            tokenClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while generating the JWT token.", ex);
        }
    }
}
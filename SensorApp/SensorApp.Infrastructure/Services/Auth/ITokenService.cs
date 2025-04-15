using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SensorApp.Infrastructure.Services.Auth;

/// <summary>
/// This service generates JWT tokens based on user credentials.
/// </summary>
public interface ITokenService
{
    string GenerateToken(IdentityUser user, IList<string> roles, IList<Claim> additionalClaims);
}

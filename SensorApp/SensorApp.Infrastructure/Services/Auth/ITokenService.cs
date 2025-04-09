using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SensorApp.Infrastructure.Services.Auth;

public interface ITokenService
{
    string GenerateToken(IdentityUser user, IList<string> roles, IList<Claim> additionalClaims);
}

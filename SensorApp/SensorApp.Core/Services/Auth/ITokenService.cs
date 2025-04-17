using Microsoft.AspNetCore.Identity;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Interface for generating JWT tokens for authenticated users
/// </summary>
public interface ITokenService
{
    string GenerateToken(IdentityUser user, IList<string> roles);
}
using Microsoft.AspNetCore.Identity;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Interface for generating JWT tokens for authenticated users
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a signed JWT token for an authenticated user.
    /// The token includes the user's ID, email address, and all assigned role claims.
    /// </summary>
    /// <param name="user">The authenticated <see cref="IdentityUser"/> object.</param>
    /// <param name="roles">A list of roles associated with the user.</param>
    /// <returns>A signed JWT token string containing the user's claims.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the user is null.</exception>
    string GenerateToken(IdentityUser user, IList<string> roles);
}
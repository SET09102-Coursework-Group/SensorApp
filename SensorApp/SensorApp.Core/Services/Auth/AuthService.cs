using Microsoft.AspNetCore.Identity;
using SensorApp.Shared.Dtos;

namespace SensorApp.Core.Services.Auth;

/// <summary>
/// Service responsible for authenticating users and generating JWT tokens upon successful login.
/// Implements the <see cref="IAuthService"/> interface.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _users;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> users, ITokenService tokenService) => (_users, _tokenService) = (users, tokenService);

    public async Task<AuthResponseDto?> AuthenticateAsync(LoginDto login)
    {
        var user = await _users.FindByNameAsync(login.Username);
        if (user == null || !await _users.CheckPasswordAsync(user, login.Password))
        {
            return null;
        }

        var roles = await _users.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.UserName!,
            Token = token
        };
    }
}
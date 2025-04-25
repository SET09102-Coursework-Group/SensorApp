using SensorApp.Shared.Dtos;

namespace SensorApp.Core.Services.Auth;

public interface IAuthService
{
    Task<AuthResponseDto?> AuthenticateAsync(LoginDto login);
}

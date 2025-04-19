using SensorApp.Shared.Dtos;

namespace SensorApp.Shared.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> Login(LoginDto loginDto);
    string StatusMessage { get; }
}

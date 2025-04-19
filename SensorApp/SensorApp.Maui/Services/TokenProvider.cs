using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Services;

public class TokenProvider : ITokenProvider
{
    public Task<string?> GetTokenAsync()
    {
        return SecureStorage.GetAsync("Token");
    }
}

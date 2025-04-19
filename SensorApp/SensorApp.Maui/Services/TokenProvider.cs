using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Services;

public class TokenProvider : ITokenProvider
{
    public async Task<string?> GetTokenAsync()
    {
        var token = await SecureStorage.GetAsync("Token");
        return token;
    }

}

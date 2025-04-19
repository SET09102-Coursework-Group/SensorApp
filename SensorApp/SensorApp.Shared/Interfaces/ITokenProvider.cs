namespace SensorApp.Shared.Interfaces;
public interface ITokenProvider
{
    Task<string?> GetTokenAsync();
}

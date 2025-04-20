using SensorApp.Tests.IntegrationTests.ApiEndpoints.AuthEndpoints;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

public class TokenProvider
{
    private readonly HttpClient _client;

    public TokenProvider(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetTokenAsync(string email, string password)
    {
        return await new TestUserBuilder(_client).WithCredentials(email, password).BuildTokenAsync();
    }

    public Task<string> GetAdminTokenAsync()
    {
        return GetTokenAsync(TestUsers.AdminEmail, TestUsers.AdminPassword);
    }

    public Task<string> GetOpsTokenAsync()
    {
        return GetTokenAsync(TestUsers.OpsEmail, TestUsers.OpsPassword);
    }
}
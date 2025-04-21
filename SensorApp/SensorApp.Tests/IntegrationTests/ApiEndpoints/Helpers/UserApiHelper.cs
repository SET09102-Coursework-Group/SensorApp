using SensorApp.Shared.Dtos.Admin;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

public static class UserApiHelper
{
    public static async Task<bool> UserExistsAsync(HttpClient client, string token, string userId)
    {
        var request = TestHelpers.CreateAuthorizedRequest("/admin/users", token, HttpMethod.Get);
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<UserWithRoleDto>>();
        return users!.Any(u => u.Id == userId);
    }
}


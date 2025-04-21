using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

internal class TestHelpers
{
    public static HttpRequestMessage CreateAuthorizedRequest(string url, string token, HttpMethod? method = null, object? content = null)
    {
        var httpMethod = method ?? HttpMethod.Get;
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (content is not null)
        {
            request.Content = JsonContent.Create(content);
        }

        return request;
    }

}

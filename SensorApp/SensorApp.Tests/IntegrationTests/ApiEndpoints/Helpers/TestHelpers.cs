using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SensorApp.Tests.IntegrationTests.ApiEndpoints.Helpers;

public class TestHelpers
{
    /// <summary>
    /// Creates an authorized HTTP request by attaching a Bearer token and optional JSON content.
    /// </summary>
    /// <param name="url">The target URL for the request.</param>
    /// <param name="token">The JWT token to include in the Authorization header.</param>
    /// <param name="method">The HTTP method to use (defaults to GET if null).</param>
    /// <param name="content">Optional object to serialize and send as JSON in the request body.</param>
    /// <returns>A fully prepared <see cref="HttpRequestMessage"/>.</returns>
    public static HttpRequestMessage CreateAuthorizedRequest(string url, string token, HttpMethod? method = null, object? content = null)
    {
        var httpMethod = method ?? HttpMethod.Get;
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // If content is provided, serialize it as JSON and attach it to the request
        if (content != null)
        {
            request.Content = JsonContent.Create(content);
        }

        return request;
    }

}

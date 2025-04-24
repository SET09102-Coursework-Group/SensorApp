using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SensorApp.Shared.Helpers;

/// <summary>
/// Helper class for creating and sending HTTP requests with optional authentication and JSON serialization.
/// Provides utility methods for common HTTP client operations.
/// </summary>
public static class HttpRequestHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Creates an <see cref="HttpRequestMessage"/> with the specified HTTP method, URL, Bearer token, and optional content.
    /// </summary>
    /// <param name="method">The HTTP method to use (e.g., GET, POST, PUT).</param>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="token">The JWT token to include in the Authorization header.</param>
    /// <param name="content">Optional object to serialize and send as JSON in the request body.</param>
    /// <returns>A configured <see cref="HttpRequestMessage"/> ready to be sent.</returns>
    public static HttpRequestMessage Create(HttpMethod method, string url, string token, object? content = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Console.WriteLine("Built request: " + request?.RequestUri);

        if (content is not null)
        {
            var json = JsonSerializer.Serialize(content, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    /// <summary>
    /// Sends an HTTP request and attempts to deserialize the JSON response into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized response object.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to send the request.</param>
    /// <param name="request">The prepared <see cref="HttpRequestMessage"/> to send.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/> if successful; otherwise, <c>default</c>.</returns>
    public static async Task<T?> SendAsync<T>(HttpClient httpClient, HttpRequestMessage request)
    {
        try
        {
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error response: {error}");
                return default;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
            return default;
        }
    }

    /// <summary>
    /// Sends an HTTP request and returns a boolean indicating if the operation was successful.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to send the request.</param>
    /// <param name="request">The prepared <see cref="HttpRequestMessage"/> to send.</param>
    /// <returns><c>true</c> if the response was successful; otherwise, <c>false</c>.</returns>
    public static async Task<bool> SendAsync(HttpClient httpClient, HttpRequestMessage request)
    {
        try
        {
            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
            return false;
        }
    }
}
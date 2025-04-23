using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SensorApp.Shared.Helpers;

public static class HttpRequestHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static HttpRequestMessage Create(HttpMethod method, string url, string token, object? content = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (content is not null)
        {
            var json = JsonSerializer.Serialize(content, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

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
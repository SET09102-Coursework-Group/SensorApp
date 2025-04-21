using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SensorApp.Shared.Helpers;

public static class HttpRequestHelper
{
    public static HttpRequestMessage Create(HttpMethod method, string url, string token, object? content = null)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (content is not null)
        {
            var json = JsonConvert.SerializeObject(content);
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
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
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
        catch
        {
            return false;
        }
    }
}
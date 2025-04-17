using SensorApp.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SensorApp.Shared.Services;

public class SensorApiService
{
    private readonly HttpClient _httpClient;

    public SensorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }

    public async Task<List<SensorModel>> GetSensorsAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/sensors");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SensorModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching sensors: {ex.Message}");
            return new List<SensorModel>();
        }
    }
}
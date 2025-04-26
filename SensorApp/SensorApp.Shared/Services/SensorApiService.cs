using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SensorApp.Shared.Services;

/// <summary>
/// Provides services for interacting with the API to retrieve sensor data.
/// </summary>
public class SensorApiService : ISensorApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorApiService"/> class with the provided HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient used to make API requests.</param>
    public SensorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }

    /// <summary>
    /// Fetches a list of sensors from the API using the provided authentication token.
    /// </summary>
    /// <param name="token">The authentication token used for authorization in the request.</param>
    /// <returns>A list of <see cref="SensorModel"/> representing the sensors, or an empty list if an error occurs.</returns>
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
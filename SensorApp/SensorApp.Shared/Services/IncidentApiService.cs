using System.Net.Http.Headers;
using System.Text.Json;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;
public class IncidentApiService : IIncidentApiService
{
    private readonly HttpClient _httpClient;

    public IncidentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }

    public async Task<List<IncidentDto>> GetAllIncidentsAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/incidents");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<IncidentDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching incidents: {ex.Message}");
            return new List<IncidentDto>();
        }
    }

    public async Task<bool> AddIncidentAsync(string token, CreateIncidentDto newIncident)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Post, "/incident/create", token, newIncident);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }

    public async Task<bool> ResolveIncidentAsync(string token, int incidentId, IncidentResolutionDto dto)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Put, $"/incident/resolve/{incidentId}", token, dto);
        return await HttpRequestHelper.SendAsync(_httpClient, request);
    }
}
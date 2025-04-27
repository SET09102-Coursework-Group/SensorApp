using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service responsible for retrieving measurands (measurable properties)
/// associated with a specific sensor by communicating with the backend API.
/// </summary>
public class MeasurandService : IMeasurandService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurandService"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to communicate with the backend API.</param>
    public MeasurandService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Get, $"/sensors/{sensorId}/measurands", token);

        var result = await HttpRequestHelper.SendAsync<List<MeasurandModel>>(_httpClient, request);

        return result ?? [];
    }
}
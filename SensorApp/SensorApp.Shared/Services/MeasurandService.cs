using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service responsible for retrieving measurands (measurable properties)
/// associated with a specific sensor by communicating with the backend API.
/// </summary>
public class MeasurandService(HttpClient httpClient) : IMeasurandService
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// Retrieves a list of measurands available for a specific sensor.
    /// </summary>
    /// <param name="token">Authentication token required to authorize the request.</param>
    /// <param name="sensorId">The unique identifier of the sensor whose measurands are requested.</param>
    /// <returns>
    /// A list of <see cref="MeasurandModel"/> representing the measurable properties for the specified sensor.
    /// If no measurands are found or the request fails, an empty list is returned.
    /// </returns>
    public async Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Get, $"/sensors/{sensorId}/measurands", token);

        var result = await HttpRequestHelper.SendAsync<List<MeasurandModel>>(_httpClient, request);

        return result ?? [];
    }
}
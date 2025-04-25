using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

public class MeasurandService(HttpClient httpClient) : IMeasurandService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<MeasurandModel>> GetMeasurandsAsync(string token, int sensorId)
    {
        var request = HttpRequestHelper.Create(HttpMethod.Get, $"/sensors/{sensorId}/measurands", token);

        var result = await HttpRequestHelper.SendAsync<List<MeasurandModel>>(_httpClient, request);

        return result ?? [];
    }
}
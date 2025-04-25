using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

public class MeasurementService(HttpClient httpClient) : IMeasurementService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<MeasurementModel>> GetMeasurementsAsync(int? sensorId = null, int? measurandId = null, DateTime? from = null, DateTime? to = null, string token = "")
    {
        var queryParams = new List<string>();
        if (sensorId != null)
        {
            queryParams.Add($"sensorId={sensorId}");
        }
        if (measurandId != null)
        {
            queryParams.Add($"measurementTypeId={measurandId}");
        }
        if (from != null)
        {
            var fromStr = from.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            queryParams.Add($"from={Uri.EscapeDataString(fromStr)}");
        }
        if (to != null)
        {
            var toStr = to.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            queryParams.Add($"to={Uri.EscapeDataString(toStr)}");
        }

        var url = "measurements" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "");

        var request = HttpRequestHelper.Create(HttpMethod.Get, url, token);
        var result = await HttpRequestHelper.SendAsync<List<MeasurementModel>>(_httpClient, request);

        return result ?? [];
    }
}
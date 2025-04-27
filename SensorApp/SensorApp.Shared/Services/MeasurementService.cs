using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;

/// <summary>
/// Service responsible for retrieving measurement data from the backend API,
/// supporting optional filtering by sensor, measurand, and date range.
/// </summary>
public class MeasurementService : IMeasurementService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurementService"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to communicate with the backend API.</param>
    public MeasurementService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<MeasurementModel>> GetMeasurementsAsync(int? sensorId = null, int? measurandId = null, DateTime? from = null, DateTime? to = null, string token = "")
    {
        var queryParameters = new List<string>();
        if (sensorId != null)
        {
            queryParameters.Add($"sensorId={sensorId}");
        }
        if (measurandId != null)
        {
            queryParameters.Add($"measurementTypeId={measurandId}");
        }
        if (from != null)
        {
            var fromDateString = from.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            queryParameters.Add($"from={Uri.EscapeDataString(fromDateString)}");
        }
        if (to != null)
        {
            var toDateString = to.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            queryParameters.Add($"to={Uri.EscapeDataString(toDateString)}");
        }

        var measurementsEndpoint = "measurements" + (queryParameters.Count > 0 ? "?" + string.Join("&", queryParameters) : "");

        var apiRequest = HttpRequestHelper.Create(HttpMethod.Get, measurementsEndpoint, token);
        var measurementsResponse = await HttpRequestHelper.SendAsync<List<MeasurementModel>>(_httpClient, apiRequest);

        return measurementsResponse ?? [];
    }
}
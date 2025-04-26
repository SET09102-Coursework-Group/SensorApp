using SensorApp.Shared.Helpers;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Services;
/// <summary>
/// Service responsible for retrieving measurement data from the backend API,
/// supporting optional filtering by sensor, measurand, and date range.
/// </summary>
public class MeasurementService(HttpClient httpClient) : IMeasurementService
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// Asynchronously retrieves a list of measurements from the API.
    /// Supports optional filters by sensor ID, measurand ID, and a specific time range.
    /// </summary>
    /// <param name="sensorId">Optional sensor ID to filter measurements for a specific sensor.</param>
    /// <param name="measurandId">Optional measurand (measurement type) ID to filter by specific data type.</param>
    /// <param name="from">Optional start date to filter measurements recorded after this time.</param>
    /// <param name="to">Optional end date to filter measurements recorded before this time.</param>
    /// <param name="token">Authentication token required for authorized API access.</param>
    /// <returns>
    /// A read-only list of <see cref="MeasurementModel"/> representing the measurements retrieved from the API.
    /// Returns an empty list if no data is found or if the request fails.
    /// </returns>
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
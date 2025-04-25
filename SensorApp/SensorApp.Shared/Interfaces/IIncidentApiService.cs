using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Interface for incident-related API service.
/// Defines methods for interacting with the incident API, such as retrieving, adding, resolving, and deleting incidents.
/// </summary>
public interface IIncidentApiService
{
    /// <summary>
    /// Asynchronously retrieves all incidents from the API.
    /// </summary>
    /// <param name="token">The authorization token required to make the request.</param>
    /// <returns>A list of <see cref="IncidentDto"/> representing the incidents.</returns>
    Task<List<IncidentDto>> GetAllIncidentsAsync(string token);
    /// <summary>
    /// Asynchronously adds a new incident via the API.
    /// </summary>
    /// <param name="token">The authorization token required to make the request.</param>
    /// <param name="newIncident">The incident data to be created.</param>
    /// <returns>True if the incident was successfully added; otherwise, false.</returns>
    Task<bool> AddIncidentAsync(string token, CreateIncidentDto newIncident);
    /// <summary>
    /// Asynchronously resolves an incident via the API.
    /// </summary>
    /// <param name="token">The authorization token required to make the request.</param>
    /// <param name="incidentId">The unique identifier of the incident to resolve.</param>
    /// <param name="dto">The resolution details for the incident.</param>
    /// <returns>True if the incident was successfully resolved; otherwise, false.</returns>
    Task<bool> ResolveIncidentAsync(string token, int incidentId, IncidentResolutionDto dto);
    /// <summary>
    /// Asynchronously deletes an incident via the API.
    /// </summary>
    /// <param name="token">The authorization token required to make the request.</param>
    /// <param name="incidentId">The unique identifier of the incident to delete.</param>
    /// <returns>True if the incident was successfully deleted; otherwise, false.</returns>
    Task<bool> DeleteIncidentAsync(string token, int incidentId);
}
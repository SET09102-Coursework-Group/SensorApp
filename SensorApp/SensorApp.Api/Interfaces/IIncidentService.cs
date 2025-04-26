using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Api.Interfaces;

/// <summary>
/// Defines the contract for incident-related operations.
/// </summary>
public interface IIncidentService
{
    /// <summary>
    /// Fetches all incidents from the data source asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="IncidentDto"/> representing all incidents.</returns>
    Task<List<IncidentDto>> GetAllIncidentsAsync();
    /// <summary>
    /// Creates a new incident in the data source asynchronously.
    /// </summary>
    /// <param name="dto">The data transfer object (DTO) containing incident creation details.</param>
    /// <param name="responderId">The ID of the user who is creating the incident.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<IncidentDto?> CreateIncidentAsync(CreateIncidentDto dto, string? responderId);
    /// <summary>
    /// Marks an existing incident as resolved by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the incident to resolve.</param>
    /// <param name="dto">The data transfer object (DTO) containing resolution details.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if the incident was resolved, otherwise false.</returns>
    Task<bool> ResolveIncidentAsync(int id, IncidentResolutionDto dto);
    /// <summary>
    /// Deletes an existing incident by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the incident to delete.</param>
    /// <returns>A task representing the asynchronous operation. Returns true if the incident was successfully deleted, otherwise false.</returns>
    Task<bool> DeleteIncidentAsync(int id);
}

using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Shared.Interfaces;

public interface IIncidentApiService
{
    Task<List<IncidentDto>> GetAllIncidentsAsync(string token);
    Task<bool> AddIncidentAsync(string token, CreateIncidentDto newIncident);
    Task<bool> ResolveIncidentAsync(string token, int incidentId, IncidentResolutionDto dto);
}
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Api.Services.Interfaces;

public interface IIncidentService
{
    Task<List<IncidentDto>> GetAllIncidentsAsync();
    Task CreateIncidentAsync(CreateIncidentDto dto, string? responderId);
    Task<bool> ResolveIncidentAsync(int id, IncidentResolutionDto dto);
    Task<bool> DeleteIncidentAsync(int id);
}

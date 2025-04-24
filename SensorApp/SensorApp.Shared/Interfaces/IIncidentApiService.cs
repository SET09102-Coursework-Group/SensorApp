using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Shared.Interfaces;

public interface IIncidentApiService
{
    Task<List<IncidentDto>> GetAllIncidentsAsync(string token);
}
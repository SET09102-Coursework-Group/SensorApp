using SensorApp.Infrastructure.Domain.Models;
using SensorApp.Infrastructure.Repositories;

namespace SensorApp.Infrastructure.Services.SensorService;

public class SensorService
{
    private readonly IRepository<Sensor> _sensorRepository;

    public SensorService(IRepository<Sensor> sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<List<Sensor>> GetAllSensorsAsync()
    {
        return await _sensorRepository.GetAllAsync();
    }
}
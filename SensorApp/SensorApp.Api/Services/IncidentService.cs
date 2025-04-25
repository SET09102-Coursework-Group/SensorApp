using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Api.Services.Interfaces;
using SensorApp.Database.Data;
using SensorApp.Database.Models;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Api.Services;

public class IncidentService : IIncidentService
{
    private readonly SensorDbContext _db;

    public IncidentService(SensorDbContext db)
    {
        _db = db;
    }

    public async Task<List<IncidentDto>> GetAllIncidentsAsync()
    {
        return await _db.Incidents
            .Include(i => i.Sensor)
            .Include(i => i.Responder)
            .Select(i => new IncidentDto
            {
                Id = i.Id,
                Type = i.Type,
                Status = i.Status,
                Sensor_id = i.Sensor_id,
                Sensor = new SensorDto
                {
                    Id = i.Sensor.Id,
                    Type = i.Sensor.Type,
                    Latitude = i.Sensor.Latitude,
                    Longitude = i.Sensor.Longitude,
                    Site_zone = i.Sensor.Site_zone,
                    Status = i.Sensor.Status
                },
                Creation_date = i.Creation_date,
                Priority = i.Priority,
                Resolution_date = i.Resolution_date,
                Responder_id = i.Responder_id,
                Responder = new UserWithRoleDto
                {
                    Id = i.Responder.Id,
                    Username = i.Responder.UserName
                },
                Comments = i.Comments
            })
            .ToListAsync();
    }

    public async Task CreateIncidentAsync(CreateIncidentDto dto, string? responderId)
    {
        var incident = new Incident
        {
            Type = dto.Type,
            Status = dto.Status,
            Sensor_id = dto.SensorId,
            Creation_date = DateTime.UtcNow,
            Priority = dto.Priority,
            Comments = dto.Comments,
            Responder_id = responderId
        };

        _db.Incidents.Add(incident);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ResolveIncidentAsync(int id, IncidentResolutionDto dto)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident == null) return false;

        incident.Status = "Resolved";
        incident.Resolution_date = DateTime.UtcNow;
        incident.Comments = dto.ResolutionComments;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteIncidentAsync(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident == null) return false;

        _db.Incidents.Remove(incident);
        await _db.SaveChangesAsync();
        return true;
    }
}

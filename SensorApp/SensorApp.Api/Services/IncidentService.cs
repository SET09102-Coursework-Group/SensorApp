using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Api.Interfaces;
using SensorApp.Database.Data;
using SensorApp.Database.Models;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Enums;

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
                Comments = i.Comments,
                Resolution_comments = i.Resolution_comments
            })
            .ToListAsync();
    }

    public async Task<IncidentDto?> CreateIncidentAsync(CreateIncidentDto dto, string? responderId)
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
        try
        {
            await _db.SaveChangesAsync();
            return new IncidentDto
            {
                Id = incident.Id,
                Type = dto.Type,
                Status = dto.Status,
                Sensor_id = dto.SensorId,
                Creation_date = DateTime.UtcNow,
                Priority = dto.Priority,
                Comments = dto.Comments,
                Responder_id = responderId
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error saving changes: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> ResolveIncidentAsync(int id, IncidentResolutionDto dto)
    {
        var incident = await GetIncidentByIdAsync(id);
        if (incident == null) return false;

        incident.Status = IncidentStatus.Resolved;
        incident.Resolution_date = DateTime.UtcNow;
        incident.Resolution_comments = dto.ResolutionComments;

        try
        {
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error saving changes: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteIncidentAsync(int id)
    {
        var incident = await GetIncidentByIdAsync(id);
        if (incident == null) return false;

        _db.Incidents.Remove(incident);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error saving changes: {ex.Message}");
            return false;
        }
        return true;
    }

    private async Task<Incident?> GetIncidentByIdAsync(int id)
    {
        return await _db.Incidents.FindAsync(id);
    }
}

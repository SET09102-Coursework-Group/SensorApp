using Microsoft.EntityFrameworkCore;
using SensorApp.Api.Services;
using SensorApp.Database.Data;
using SensorApp.Database.Models;
using SensorApp.Shared.Dtos.Incident;
using Microsoft.AspNetCore.Identity;

public class IncidentServiceTests
{
    private SensorDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<SensorDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new SensorDbContext(options);
    }

    [Fact]
    public async Task GetAllIncidentsAsync_ReturnsList()
    {
        // Arrange
        var context = GetDbContext("GetAllIncidentsDb");
        var incident = new Incident
        {
            Id = 1,
            Type = "Max threshold breach",
            Status = "Open",
            Sensor_id = 1,
            Priority = "High",
            Creation_date = DateTime.UtcNow,
            Comments = "High concentrations of Nitrogen Dioxide",
            Responder_id = "responder",
            Sensor = new Sensor
            {
                Id = 1,
                Type = "Air quality",
                Latitude = 52f,
                Longitude = 10f,
                Site_zone = "Zone ",
                Status = "Active"
            },
            Responder = new IdentityUser
            {
                Id = "responder",
                UserName = "ResponderUser"
            }
        };
        context.Incidents.Add(incident);
        await context.SaveChangesAsync();

        var service = new IncidentService(context);

        // Act
        var result = await service.GetAllIncidentsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Max threshold breach", result[0].Type);
        Assert.Equal("ResponderUser", result[0].Responder.Username);
        Assert.Equal("Air quality", result[0].Sensor.Type);
    }

    [Fact]
    public async Task CreateIncidentAsync_AddsIncident()
    {
        //Arrange
        var context = GetDbContext("CreateIncidentDb");
        var service = new IncidentService(context);

        var dto = new CreateIncidentDto
        {
            Type = "Sensor unresponsive",
            Status = "Open",
            SensorId = 2,
            Priority = "High",
            Comments = "No measurements in 24h"
        };

        // Act
        await service.CreateIncidentAsync(dto, "responder2");

        // Assert
        var incident = await context.Incidents.FirstOrDefaultAsync();
        Assert.NotNull(incident);
        Assert.Equal("Sensor unresponsive", incident.Type);
        Assert.Equal("responder2", incident.Responder_id);
    }

    [Fact]
    public async Task ResolveIncidentAsync_UpdatesIncident()
    {
        //Arrange
        var context = GetDbContext("ResolveIncidentDb");
        var incident = new Incident
        {
            Id = 1,
            Type = "Min threshold breach",
            Status = "Open",
            Comments = "Low quantities of nitrogen dioxide",
            Creation_date = DateTime.UtcNow,
            Priority = "High", 
            Responder_id = "responderID"
        };
        context.Incidents.Add(incident);
        await context.SaveChangesAsync();

        var service = new IncidentService(context);

        var dto = new IncidentResolutionDto { ResolutionComments = "Incident resolved" };

        // Act
        var result = await service.ResolveIncidentAsync(1, dto);

        // Assert
        Assert.True(result);
        var updated = await context.Incidents.FindAsync(1);
        Assert.Equal("Resolved", updated.Status);
        Assert.Equal("Incident resolved", updated.Comments);
        Assert.NotNull(updated.Resolution_date);
    }

    [Fact]
    public async Task ResolveIncidentAsync_ReturnsFalse_IfNotFound()
    {
        //Arrange
        var context = GetDbContext("ResolveNotFoundDb");
        var service = new IncidentService(context);

        //Act
        var result = await service.ResolveIncidentAsync(404, new IncidentResolutionDto());

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteIncidentAsync_RemovesIncident()
    {
        //Arrange
        var context = GetDbContext("DeleteIncidentDb");
        context.Incidents.Add(new Incident { Id = 1, Type = "Max threshold breach", Priority = "Medium", Responder_id = "responderID", Status = "Open" });
        await context.SaveChangesAsync();

        var service = new IncidentService(context);

        //Act
        var result = await service.DeleteIncidentAsync(1);

        //Assert
        Assert.True(result);
        Assert.Empty(await context.Incidents.ToListAsync());
    }

    [Fact]
    public async Task DeleteIncidentAsync_ReturnsFalse_IfNotFound()
    {
        //Arrange
        var context = GetDbContext("DeleteNotFoundDb");
        var service = new IncidentService(context);

        //Act
        var result = await service.DeleteIncidentAsync(404);

        //Assert
        Assert.False(result);
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data.DataSeeder;
using System.Diagnostics.Metrics;
using SensorApp.Database.Models;

namespace SensorApp.Database.Data;

/// <summary>
/// The application's database context which extends <see cref="IdentityDbContext"/> to include ASP.NET Identity support for logging in
/// This context is used to configure and interact with the database using Entity Framework Core.
/// </summary>
public class SensorDbContext(DbContextOptions<SensorDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Measurand> Measurand { get; set; }
    public DbSet<Measurement> Measurement { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SeedIdentity();
        builder.SeedMeasurands();
        builder.SeedSensors();
        builder.SeedIncidents();
    }
}
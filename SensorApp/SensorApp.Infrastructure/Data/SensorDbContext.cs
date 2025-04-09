using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Data.DataSeeder;
using SensorApp.Infrastructure.Domain.Models;

namespace SensorApp.Infrastructure.Data;

public class SensorDbContext : IdentityDbContext
{
    public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options)
    {
    }

    public DbSet<Sensor> Sensors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.SeedIdentity();
        modelBuilder.SeedSensors();
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Data.DataSeeder;

namespace SensorApp.Infrastructure.Data;

/// <summary>
/// DbContext that inherits from IdentityDbContext to include Identity support from Microsoft Identity NuGet package. 
/// The SensorDbContext is used to interact with the database.
/// </summary>
public class SensorDbContext : IdentityDbContext
{
    public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.SeedIdentity();
    }
}
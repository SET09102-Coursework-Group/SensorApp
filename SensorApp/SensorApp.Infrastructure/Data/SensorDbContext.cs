using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Data.DataSeeder;

namespace SensorApp.Infrastructure.Data;

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
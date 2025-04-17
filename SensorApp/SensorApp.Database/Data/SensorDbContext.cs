using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SensorApp.Database.Data.DataSeeder;

namespace SensorApp.Database.Data;

/// <summary>
/// The application's database context which extends <see cref="IdentityDbContext"/> to include ASP.NET Identity support for logging in
/// This context is used to configure and interact with the database using Entity Framework Core.
/// </summary>
public class SensorDbContext(DbContextOptions<SensorDbContext> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.SeedIdentity();
    }
}
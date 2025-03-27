using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SET09102_coursework.Database.Models;

namespace SET09102_coursework.Database.Data;

public class CourseworkDbContext : DbContext
{

    public CourseworkDbContext()
    { }
    public CourseworkDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Sensor> Sensors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var ConnectionName = "TestConnection";
        var connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{ConnectionName}");

        if (string.IsNullOrEmpty(connectionString))
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("SET09102-coursework.Database.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                connectionString = config.GetConnectionString(ConnectionName);
            }
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        optionsBuilder.UseSqlServer(
            connectionString,
            m => m.MigrationsAssembly("SET09102-coursework.Migrations"));
    }
}

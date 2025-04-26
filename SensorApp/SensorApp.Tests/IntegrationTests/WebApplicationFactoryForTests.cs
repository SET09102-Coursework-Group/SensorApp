using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorApp.Api;
using SensorApp.Database.Data;

namespace SensorApp.Tests.IntegrationTests;

/// <summary>
/// A custom WebApplicationFactory that configures the SensorApp API for integration testing.
/// It uses an in-memory SQLite database and fake JWT settings, ensuring tests are isolated from production.
/// </summary>
public sealed class WebApplicationFactoryForTests : WebApplicationFactory<Program>, IDisposable
{
    private readonly SqliteConnection _sqliteConnection = new("Data Source=:memory:");

    public WebApplicationFactoryForTests()
    {
        _sqliteConnection.Open();
    }

    /// <summary>
    /// Configures the web host specifically for testing purposes.
    /// Sets the environment, overrides configuration values, and replaces the real database with an in-memory version.
    /// </summary>
    /// <param name="builder">The <see cref="IWebHostBuilder"/> used to build the application for testing.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration((context, config) => ConfigureTestJwt(config))
            .ConfigureServices(services => ReplaceDatabase(services));          //Replacing the database with in-memory database
    }

    /// <summary>
    /// Configures dummy JWT settings for testing
    /// </summary>
    /// <param name="config">The application's configuration builder.</param>
    private static void ConfigureTestJwt(IConfigurationBuilder config)
    {
        config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JwtSettings:Issuer"] = "SensorApiTest",
            ["JwtSettings:Audience"] = "SensorAppTests",
            ["JwtSettings:Key"] = Guid.NewGuid().ToString(),
            ["JwtSettings:DurationInMinutes"] = "60"
        });
    }

    /// <summary>
    /// Replaces the application's real database with an in-memory SQLite database for integration testing.
    /// Ensures the database schema is created before tests are run.
    /// </summary>
    /// <param name="services">The collection of services configured for the application.</param>
    private void ReplaceDatabase(IServiceCollection services)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<SensorDbContext>));
        services.Remove(descriptor);

        services.AddDbContext<SensorDbContext>(options => options.UseSqlite(_sqliteConnection));

        // Build the service provider and create the database schema
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();
        db.Database.EnsureCreated();
    }

    /// <summary>
    /// Cleans up resources after tests are finished.
    /// Specifically, it closes and disposes of the in-memory SQLite connection.
    /// </summary>
    /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sqliteConnection.Dispose();
        }

        base.Dispose(disposing);
    }
}

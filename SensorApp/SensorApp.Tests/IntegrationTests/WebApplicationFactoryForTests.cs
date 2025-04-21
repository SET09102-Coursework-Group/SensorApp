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
/// A custom WebApplicationFactory that configures the API for integration testing.
/// It sets up an in-memory database and dummy JWT settings to isolate tests from the production environment.
/// </summary>
public sealed class WebApplicationFactoryForTests : WebApplicationFactory<Program>, IDisposable
{
    private readonly SqliteConnection _sqliteConnection;
    public WebApplicationFactoryForTests()
    {
        _sqliteConnection = new SqliteConnection("Data Source=:memory:");
        _sqliteConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
          .UseEnvironment("Testing")
          .ConfigureAppConfiguration((ctx, cb) => {
              cb.AddInMemoryCollection(new Dictionary<string, string?>
              {
                  ["JwtSettings:Issuer"] = "SensorApiTest",
                  ["JwtSettings:Audience"] = "SensorAppTests",
                  ["JwtSettings:Key"] = Guid.NewGuid().ToString(),
                  ["JwtSettings:DurationInMinutes"] = "60"
              });
          })
          .ConfigureServices(services =>
          {
              var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<SensorDbContext>));
              services.Remove(descriptor);

              services.AddDbContext<SensorDbContext>(options => options.UseSqlite(_sqliteConnection)
              );

              var sp = services.BuildServiceProvider();
              using var scope = sp.CreateScope();
              var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();
              db.Database.Migrate();
          });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();
        }
    }
}
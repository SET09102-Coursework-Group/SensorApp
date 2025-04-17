using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SensorApp.Api;
using SensorApp.Database.Data;

namespace SensorApp.Tests.IntegrationTests;

/// <summary>
/// A custom WebApplicationFactory that configures the API for integration testing.
/// It sets up a throwaway in-memory database and dummy JWT settings to isolate tests from the production environment.
/// </summary>
public sealed class WebApplicationFactoryForTests : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Here we are injecting the JWT configuration into the test environment so that JWT validation passes without requiring connection appsettings.json in the program.cs
        builder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
        {
            if (hostingContext.HostingEnvironment.IsEnvironment("Testing"))
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["JwtSettings:Issuer"] = "SensorApiTest",
                    ["JwtSettings:Audience"] = "SensorAppTests",
                    ["JwtSettings:Key"] = Guid.NewGuid().ToString(),
                    ["JwtSettings:DurationInMinutes"] = "60"
                });
            }
        });

        // This is to override the default service registrations
        builder.ConfigureServices(services =>
        {
            //When your Program.cs runs, it registers a DbContext pointing to a SQLite file (the real database), so we need to remove it here to not populate the ral db with the test data
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<SensorDbContext>));
            services.Remove(descriptor);

            services.AddDbContext<SensorDbContext>(o => o.UseInMemoryDatabase("SensorAppTests"));

            //EnsureCreated() builds the database schema (tables, constraints) from your EF model, and creates the db in memory to see if the test pass or fail
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
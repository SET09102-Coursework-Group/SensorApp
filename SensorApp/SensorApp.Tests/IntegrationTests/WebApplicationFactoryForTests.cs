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
/// It sets up an in-memory database and dummy JWT settings to isolate tests from the production environment.
/// </summary>
public sealed class WebApplicationFactoryForTests : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //Point the content-root to the API project
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionRoot = Directory.GetParent(currentDirectory)!.Parent!.Parent!.Parent!.FullName;        

        var apiProjectPath = Path.Combine(solutionRoot, "SensorApp.Api");

        builder.UseEnvironment("Testing").UseContentRoot(apiProjectPath)
            .ConfigureAppConfiguration((hostingContext, configBuilder) =>
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
            })
            .ConfigureServices(services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<SensorDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<SensorDbContext>(o => o.UseInMemoryDatabase("SensorAppTests"));

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SensorDbContext>();
                db.Database.EnsureCreated();
            });
    }
}
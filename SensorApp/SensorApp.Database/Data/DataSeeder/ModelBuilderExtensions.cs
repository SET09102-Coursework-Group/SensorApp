using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Shared.Enums;
using SensorApp.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace SensorApp.Database.Data.DataSeeder;

/// <summary>
/// Extension methods for seeding data into the database using ModelBuilder.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Seeds the Identity roles into the database. These roles are used for user authentication and authorization.
    /// </summary>
    public static void SeedIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "3d52c1e5-6aec-45de-91c1-e0ebf20464e3",
                Name = UserRole.Administrator.ToString(),
                NormalizedName = UserRole.Administrator.ToString().ToUpper()
            },
            new IdentityRole
            {
                Id = "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c",
                Name = UserRole.OperationsManager.ToString(),
                NormalizedName = UserRole.OperationsManager.ToString().ToUpper()
            },
            new IdentityRole
            {
                Id = "9b7f193f-bfc4-4eb7-927f-55960e45a82a",
                Name = UserRole.EnvironmentalScientist.ToString(),
                NormalizedName = UserRole.EnvironmentalScientist.ToString().ToUpper()
            }
        );

        var hasher = new PasswordHasher<IdentityUser>();

        var adminUser = new IdentityUser
        {
            Id = "fab66dad-9f12-45a0-9fd8-6352336a696d",
            Email = "admin@sensor.com",
            NormalizedEmail = "ADMIN@SENSOR.COM",
            UserName = "admin@sensor.com",
            NormalizedUserName = "ADMIN@SENSOR.COM",
            EmailConfirmed = true
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "MyP@ssword123");

        var operationsManagerUser = new IdentityUser
        {
            Id = "99166c0c-7f14-442b-8c57-9141f3ac1681",
            Email = "ops@sensor.com",
            NormalizedEmail = "OPS@SENSOR.COM",
            UserName = "ops@sensor.com",
            NormalizedUserName = "OPS@SENSOR.COM",
            EmailConfirmed = true
        };
        operationsManagerUser.PasswordHash = hasher.HashPassword(operationsManagerUser, "MyP@ssword123");

        var envScientistUser = new IdentityUser
        {
            Id = "1243c642-7fdf-4224-9404-02dd6ac95bc5",
            Email = "scientist@sensor.com",
            NormalizedEmail = "SCIENTIST@SENSOR.COM",
            UserName = "scientist@sensor.com",
            NormalizedUserName = "SCIENTIST@SENSOR.COM",
            EmailConfirmed = true
        };
        envScientistUser.PasswordHash = hasher.HashPassword(envScientistUser, "MyP@ssword123");

        modelBuilder.Entity<IdentityUser>().HasData(adminUser, operationsManagerUser, envScientistUser);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "3d52c1e5-6aec-45de-91c1-e0ebf20464e3",
                UserId = "fab66dad-9f12-45a0-9fd8-6352336a696d"
            },
            new IdentityUserRole<string>
            {
                RoleId = "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c",
                UserId = "99166c0c-7f14-442b-8c57-9141f3ac1681"
            },
            new IdentityUserRole<string>
            {
                RoleId = "9b7f193f-bfc4-4eb7-927f-55960e45a82a",
                UserId = "1243c642-7fdf-4224-9404-02dd6ac95bc5"
            }
        );
    }

    /// <summary>
    /// Seeds predefined measurands (such as pollutants, temperature, etc.) into the database.
    /// </summary>
    public static void SeedMeasurands(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurand>().HasData(
            new Measurand
            {
                Id = 1,
                Name = "Nitrogen Dioxide",
                Unit = "ug/m3",
                Min_safe_threshold = 10f,
                Max_safe_threshold = 50.0f
            },
            new Measurand
            {
                Id = 2,
                Name = "Sulphur Dioxide",
                Unit = "ug/m3",
                Min_safe_threshold = 0.8f,
                Max_safe_threshold = 2f
            },
            new Measurand
            {
                Id = 3,
                Name = "PM2.5 Particulate Matter",
                Unit = "ug/m3",
                Min_safe_threshold = 1.5f,
                Max_safe_threshold = 20f
            },
            new Measurand
            {
                Id = 4,
                Name = "PM10 Particulate Matter",
                Unit = "ug/m3",
                Min_safe_threshold = 2f,
                Max_safe_threshold = 12f
            },
            new Measurand
            {
                Id = 5,
                Name = "Nitrate",
                Unit = "mg/l",
                Min_safe_threshold = 20f,
                Max_safe_threshold = 25f
            },
            new Measurand
            {
                Id = 6,
                Name = "Nitrite",
                Unit = "mg/l",
                Min_safe_threshold = 1.1f,
                Max_safe_threshold = 1.5f
            },
            new Measurand
            {
                Id = 7,
                Name = "Phosphate",
                Unit = "mg/l",
                Min_safe_threshold = 0.02f,
                Max_safe_threshold = 0.07f
            },
            new Measurand
            {
                Id = 8,
                Name = "Escherichia coli",
                Unit = "cfu/100ml",
                Min_safe_threshold = 0f,
                Max_safe_threshold = 0.1f
            },
            new Measurand
            {
                Id = 9,
                Name = "Temperature",
                Unit = "C",
                Min_safe_threshold = -10f,
                Max_safe_threshold = 40f
            },
            new Measurand
            {
                Id = 10,
                Name = "Relative Humidity",
                Unit = "%",
                Min_safe_threshold = 80f,
                Max_safe_threshold = 100f
            },
            new Measurand
            {
                Id = 11,
                Name = "Wind Speed",
                Unit = "m/s",
                Min_safe_threshold = 0f,
                Max_safe_threshold = 30f
            },
            new Measurand
            {
                Id = 12,
                Name = "Wind Direction",
                Unit = "degree",
                Min_safe_threshold = null,
                Max_safe_threshold = null
            }
        );
    }

    /// <summary>
    /// Seeds predefined sensor data into the database. These sensors are used to collect measurement data of each measurand.
    /// </summary>
    public static void SeedSensors(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sensor>().HasData(
            new Sensor
            {
                Id = 1,
                Type = "Air Quality",
                Longitude = -3.183991f,
                Latitude = 55.94476f,
                Site_zone = "Central Scotland",
                Status = "Active",
            },
                new Sensor
                {
                    Id = 2,
                    Type = "Water Quality",
                    Longitude = -3.253889f,
                    Latitude = 55.861111f,
                    Site_zone = "Glencorse B",
                    Status = "Active",
                },
                new Sensor
                {
                    Id = 3,
                    Type = "Weather",
                    Longitude = -3.5856323f,
                    Latitude = 55.008785f,
                    Status = "Active",
                }
            );
    }

    public static void SeedIncidents(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Incident>().HasData(
            new Incident
            {
                Id = 1,
                Type = IncidentType.MaxThresholdBreached,
                Status = 0,
                Sensor_id = 1,
                Creation_date = DateTime.UtcNow,
                Priority = IncidentPriority.High,
                Resolution_date = null,
                Responder_id = "99166c0c-7f14-442b-8c57-9141f3ac1681",
                Comments = "Max threshold breach for Nitrogen Dioxide",
                Resolution_comments = null
            }
         );
    }
}


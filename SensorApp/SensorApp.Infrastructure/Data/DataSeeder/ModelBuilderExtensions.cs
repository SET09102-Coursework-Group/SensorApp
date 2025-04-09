using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Domain.Models;

namespace SensorApp.Infrastructure.Data.DataSeeder;

public static class ModelBuilderExtensions
{
    public static void SeedIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "3d52c1e5-6aec-45de-91c1-e0ebf20464e3",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole
            {
                Id = "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c",
                Name = "Operations Manager",
                NormalizedName = "OPERATIONS MANAGER"
            },
            new IdentityRole
            {
                Id = "9b7f193f-bfc4-4eb7-927f-55960e45a82a",
                Name = "Environmental Scientist",
                NormalizedName = "ENVIRONMENTAL SCIENTIST"
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
    public static void SeedSensors(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sensor>().HasData(
            new Sensor
            {
                Id = 1,
                Type = "Temperature",
                DeploymentDate = new DateTime(2023, 1, 1),
                Longitude = 35.6895f,
                Latitude = 139.6917f
            },
            new Sensor
            {
                Id = 2,
                Type = "Humidity",
                DeploymentDate = new DateTime(2023, 2, 15),
                Longitude = -74.0060f,
                Latitude = 40.7128f
            },
            new Sensor
            {
                Id = 3,
                Type = "Pressure",
                DeploymentDate = new DateTime(2023, 3, 10),
                Longitude = 2.3522f,
                Latitude = 48.8566f
            },
             new Sensor
             {
                 Id = 4,
                 Type = "Pressure",
                 DeploymentDate = new DateTime(2023, 3, 10),
                 Longitude = 2.3522f,
                 Latitude = 48.8566f
             }

        );
    }
}


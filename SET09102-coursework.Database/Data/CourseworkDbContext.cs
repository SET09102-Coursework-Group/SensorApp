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
     var a = Assembly.GetExecutingAssembly();
     var resources = a.GetManifestResourceNames();
     using var stream = a.GetManifestResourceStream("SET09102_coursework.Database.appsettings.json");
    
     var config = new ConfigurationBuilder()
         .AddJsonStream(stream)
         .Build();
    
     optionsBuilder.UseSqlServer(
         config.GetConnectionString("DevelopmentConnection"),
         m => m.MigrationsAssembly("SET09102_coursework.Migrations")
     );
 }

}

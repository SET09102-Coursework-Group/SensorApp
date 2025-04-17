using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SensorApp.Api.Endpoints;
using SensorApp.Api.Swagger;
using SensorApp.Core.Services.Auth;
using SensorApp.Database.Data;
using SensorApp.Shared.Models;
using Serilog;
using System.Text;
using SensorApp.Database.Data.CSVHandling;

namespace SensorApp.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerWithJwt();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });

        var jwtSection = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(jwtSection);
        builder.Services.AddOptions<JwtSettings>().ValidateDataAnnotations().ValidateOnStart();
        var jwtSettings = jwtSection.Get<JwtSettings>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<SensorDbContext>(options => options.UseSqlite(connectionString));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<SensorDbContext>().AddDefaultTokenProviders();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });



        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
        });


        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

        var app = builder.Build();

        await MeasurementSeeder.SeedDatabaseAsync(app.Services);

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAll");

        app.MapAuthEndpoints();
        app.MapAdminEndpoints();

        app.Run();
    }
}
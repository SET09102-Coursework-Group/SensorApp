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

namespace SensorApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerWithJwt();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });

        builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));
        builder.Services.AddOptions<JwtSettings>().ValidateDataAnnotations().ValidateOnStart();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        var isTestEnviroment = builder.Environment.IsEnvironment("Testing");
        builder.Services.AddDbContext<SensorDbContext>(options =>
        {
            if (isTestEnviroment)
            {
                options.UseInMemoryDatabase("SensorAppTests");
            }
            else
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlite(connectionString);
            }
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<SensorDbContext>().AddDefaultTokenProviders();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = jwt.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
        });


        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

        var app = builder.Build();

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
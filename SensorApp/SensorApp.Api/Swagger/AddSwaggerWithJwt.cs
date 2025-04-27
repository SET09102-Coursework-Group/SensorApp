using Microsoft.OpenApi.Models;

namespace SensorApp.Api.Swagger;

/// <summary>
/// Provides extension methods for configuring Swagger with JWT authentication support.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Adds Swagger generation services to the application with JWT Bearer token security configured.
    /// This enables API documentation and testing through Swagger UI, requiring JWT tokens for authorized endpoints.
    /// </summary>
    /// <param name="services">The service collection to which the Swagger services will be added.</param>
    public static void AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sensor API", Version = "v1" });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, Array.Empty<string>() }
            });
        });
    }
}
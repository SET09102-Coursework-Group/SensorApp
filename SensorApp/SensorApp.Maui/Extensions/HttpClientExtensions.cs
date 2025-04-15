using Microsoft.Extensions.Configuration;

namespace SensorApp.Maui.Extensions;


public static class HttpClientExtensions
{
    /// <summary>
    /// Configures the HttpClient with the API base address defined in the application configuration.
    /// </summary>
    /// <param name="builder">The IHttpClientBuilder used to configure the HttpClient.</param>
    /// <returns>An IHttpClientBuilder with the API base address configured.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API base address is not properly configured.</exception>
    public static IHttpClientBuilder ConfigureApiHttpClient(this IHttpClientBuilder builder)
    {
        return builder.ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var baseAddress = configuration["ApiSettings:BaseAddress"];

            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new InvalidOperationException("API base address is not configured.");
            }

            httpClient.BaseAddress = new Uri(baseAddress);
        });
    }
}
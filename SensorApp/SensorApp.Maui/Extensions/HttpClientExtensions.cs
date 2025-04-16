using Microsoft.Extensions.Configuration;

namespace SensorApp.Maui.Extensions;

public static class HttpClientExtensions
{
    /// <summary>
    /// Configures an <see cref="HttpClient"/> with a base address under the appsettings key "ApiSettings:BaseAddress", note that in dev we are using ngrok (https://ngrok.com/)
    /// </summary>
    /// <param name="builder">The HTTP client builder used in DI registration.</param>
    /// <returns>The updated <see cref="IHttpClientBuilder"/> with the base address configured.</returns>
    /// <exception cref="InvalidOperationException"> is thrown if the base address is not found or is empty in configuration.
    /// </exception>
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
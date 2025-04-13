using Microsoft.Extensions.Configuration;

namespace SensorApp.Maui.Extensions;
public static class HttpClientExtensions
{
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
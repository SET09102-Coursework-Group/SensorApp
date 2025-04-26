using Moq.Protected;
using Moq;

namespace SensorApp.Tests.UnitTests;

/// <summary>
/// A utility factory for creating mocked <see cref="HttpClient"/> instances with predefined responses or exceptions.
/// Useful for simulating API calls during unit testing without needing a real HTTP server.
/// Based on guidance from: https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
/// </summary>
public static class HttpClientTestFactory
{
    /// <summary>
    /// Creates a mocked <see cref="HttpClient"/> that always returns the specified <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="response">The HTTP response that should be returned for any request.</param>
    /// <param name="baseUrl">Optional base URL to set for the created client (default is "https://unit-test.com").</param>
    /// <returns>A configured <see cref="HttpClient"/> instance ready for testing.</returns>
    public static HttpClient Create(HttpResponseMessage response, string baseUrl = "https://unit-test.com")
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    /// <summary>
    /// Creates a mocked <see cref="HttpClient"/> that throws the specified exception when a request is sent.
    /// Useful for testing how code handles HTTP request failures.
    /// </summary>
    /// <param name="ex">The exception to throw when a request is made.</param>
    /// <param name="baseUrl">Optional base URL to set for the created client (default is "https://unit-test.com").</param>
    /// <returns>A configured <see cref="HttpClient"/> instance that throws on use.</returns>
    public static HttpClient CreateWithException(Exception ex, string baseUrl = "https://unit-test.com")
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(ex);


        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }
}
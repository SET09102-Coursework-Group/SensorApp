using Moq.Protected;
using Moq;

namespace SensorApp.Tests.UnitTests;

/// <summary>
/// A test utility factory for creating mocked <see cref="HttpClient"/> instances with predefined responses and exceptions, used to simulate API calls in unit tests
/// Set up following guidance from: https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
/// </summary>
public static class HttpClientTestFactory
{
    /// <summary>
    /// Creates an <see cref="HttpClient"/> that returns the specified HTTP response for any request sent during testing.
    /// </summary>
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
    /// Creates an <see cref="HttpClient"/> that throws an exception when a request is sent, used to test a fail request
    /// </summary>
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
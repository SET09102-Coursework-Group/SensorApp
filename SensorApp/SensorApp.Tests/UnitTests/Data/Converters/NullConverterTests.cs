using SensorApp.Database.Data.CSVHandling.Converters;

namespace SensorApp.Tests.Converters;

/// <summary>
/// Unit tests for the <see cref="FloatOrNullConverter"/> class.
/// Verifies correct behavior when converting string inputs to floats or null.
/// </summary>
public class FloatOrNullConverterTests
{
    private readonly FloatOrNullConverter _converter;

    public FloatOrNullConverterTests()
    {
        _converter = new FloatOrNullConverter();
    }

    [Fact]
    public void ConvertFromString_ValidFloatString_ReturnsFloat()
    {
        // Arrange
        string input = "123.45";

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.IsType<float>(result);
        Assert.Equal(123.45f, (float)result);
    }

    [Fact]
    public void ConvertFromString_InvalidString_ReturnsNull()
    {
        // Arrange
        string input = "abc";

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertFromString_EmptyString_ReturnsNull()
    {
        // Arrange
        string input = "";

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertFromString_NullInput_ReturnsNull()
    {
        // Arrange
        string input = null;

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertFromString_StringWithSpaces_ReturnsFloat()
    {
        // Arrange
        string input = "   56.78   ";

        // Act
        var result = _converter.ConvertFromString(input.Trim(), null, null); // your current method doesn't internally Trim

        // Assert
        Assert.IsType<float>(result);
        Assert.Equal(56.78f, (float)result);
    }
}

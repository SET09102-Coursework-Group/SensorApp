using SensorApp.Database.Data.CSVHandling.Converters;
using CsvHelper.TypeConversion;

namespace SensorApp.Tests.Converters;

/// <summary>
/// Unit tests for the <see cref="DateConverter"/> class.
/// Verifies correct behavior when converting string inputs to <see cref="DateTime"/> objects.
/// </summary>
public class DateConverterTests
{
    private readonly DateConverter _converter;

    public DateConverterTests()
    {
        _converter = new DateConverter();
    }

    [Fact]
    public void ConvertFromString_ValidDateString_ReturnsDateTime()
    {
        // Arrange
        string input = "25/04/2025";

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.IsType<DateTime>(result);
        Assert.Equal(new DateTime(2025, 4, 25), (DateTime)result);
    }

    [Fact]
    public void ConvertFromString_InvalidDateString_ThrowsTypeConverterException()
    {
        // Arrange
        string input = "04-25-2025";

        // Act & Assert
        Assert.Throws<TypeConverterException>(() => _converter.ConvertFromString(input, null, null));
    }

    [Fact]
    public void ConvertFromString_NonDateString_ThrowsTypeConverterException()
    {
        // Arrange
        string input = "notadate";

        // Act & Assert
        Assert.Throws<TypeConverterException>(() => _converter.ConvertFromString(input, null, null));
    }

    [Fact]
    public void ConvertFromString_EmptyString_ThrowsTypeConverterException()
    {
        // Arrange
        string input = "";

        // Act & Assert
        Assert.Throws<TypeConverterException>(() => _converter.ConvertFromString(input, null, null));
    }

    [Fact]
    public void ConvertFromString_NullInput_ThrowsTypeConverterException()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.Throws<TypeConverterException>(() => _converter.ConvertFromString(input, null, null));
    }

    [Fact]
    public void ConvertFromString_ValidDateStringWithSpaces_ReturnsDateTime()
    {
        // Arrange
        string input = "   01/01/2024   ";

        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        Assert.IsType<DateTime>(result);
        Assert.Equal(new DateTime(2024, 1, 1), (DateTime)result);
    }
}

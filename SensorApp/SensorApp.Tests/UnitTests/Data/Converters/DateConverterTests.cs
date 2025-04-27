using Xunit;
using SensorApp.Database.Data.CSVHandling.Converters;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System;
using CsvHelper.TypeConversion;

namespace SensorApp.Tests.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="DateConverter"/> class.
    /// Verifies correct behavior when converting string inputs to <see cref="DateTime"/> objects.
    /// </summary>
    public class DateConverterTests
    {
        private readonly DateConverter _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateConverterTests"/> class.
        /// </summary>
        public DateConverterTests()
        {
            _converter = new DateConverter();
        }

        /// <summary>
        /// Tests that a valid date string in "dd/MM/yyyy" format is correctly converted to a <see cref="DateTime"/>.
        /// </summary>
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
    }
}

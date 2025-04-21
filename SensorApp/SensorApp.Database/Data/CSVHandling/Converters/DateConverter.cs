using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Converters;

/// <summary>
/// Provides a custom converter for CSV parsing that converts a date string in the format "dd/MM/yyyy"
/// into a <see cref="DateTime"/> object. This is useful when importing data from CSV files where dates
/// are not in a standard ISO format.
/// </summary>
public class DateConverter : DefaultTypeConverter
{

    private readonly string inputFormat = "dd/MM/yyyy";

    /// <summary>
    /// Converts a date string from a CSV file into a <see cref="DateTime"/> object using the specified format.
    /// </summary>
    /// <param name="text">The text representation of the date from the CSV field.</param>
    /// <param name="row">The current row being read from the CSV file.</param>
    /// <param name="memberMapData">The mapping data for the member being set.</param>
    /// <returns>
    /// A <see cref="DateTime"/> object parsed from the input string if it matches the expected format.
    /// </returns>
    /// <exception cref="TypeConverterException">
    /// Thrown when the input string does not match the expected format "dd/MM/yyyy".
    /// </exception>

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        var trimmedText = text?.Trim();
        if (DateTime.TryParseExact(trimmedText, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid date format. Expected {inputFormat}");
    }
}
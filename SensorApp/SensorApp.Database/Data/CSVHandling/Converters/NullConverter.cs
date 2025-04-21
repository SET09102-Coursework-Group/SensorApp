using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Converters;

public class FloatOrNullConverter : DefaultTypeConverter
{
    /// <summary>
    /// Custom converter for handling text to <see cref="float"/> conversion from a CSV.
    /// This converter attempts to parse the text as a <see cref="float"/>. If parsing is successful,
    /// it returns the parsed <see cref="float"/> value. Otherwise, it returns <c>null</c> for invalid or 
    /// non-numeric text.
    /// </summary>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        /// <summary>
        /// Converts a string from a CSV file to a <see cref="float"/> or <c>null</c>.
        /// The method attempts to parse the provided text as a floating-point number.
        /// If parsing is successful, the parsed <see cref="float"/> is returned.
        /// If parsing fails (i.e., the text cannot be converted to a valid <see cref="float"/>),
        /// the method returns <c>null</c>.
        /// </summary>
        /// <param name="text">The text from the CSV that is to be converted, which may represent a floating-point number.</param>
        /// <param name="row">The current row being processed from the CSV file.</param>
        /// <param name="memberMapData">The mapping data for the member being populated with the converted value.</param>
        /// <returns>
        /// A <see cref="float"/> value if the string can be parsed as a valid number; otherwise, <c>null</c>.
        /// </returns>
        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            return result;
        }

        return null;
    }
}
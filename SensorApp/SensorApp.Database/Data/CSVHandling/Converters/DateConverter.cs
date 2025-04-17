using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Converters;

public class DateConverter : DefaultTypeConverter
{

    private readonly string inputFormat = "dd/MM/yyyy";

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
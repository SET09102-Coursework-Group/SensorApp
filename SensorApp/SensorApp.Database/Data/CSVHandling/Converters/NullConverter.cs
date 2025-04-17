using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Converters;

public class NullConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.Equals(text, "No data", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        {
            return result;
        }

        return null;
    }
}
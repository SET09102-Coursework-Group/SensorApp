using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Helpers;
public static class CsvReaderHelper
{
    public static List<T> ReadCsv<T, TMap>(string filePath)
        where TMap : ClassMap<T>
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TMap>();
        return csv.GetRecords<T>().ToList();
    }
}

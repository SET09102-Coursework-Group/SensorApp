using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace SensorApp.Database.Data.CSVHandling.Helpers;

/// <summary>
/// Provides helper methods for reading CSV files using CsvHelper, with class mappings for the records.
/// </summary>
public static class CsvReaderHelper
{
    /// <summary>
    /// Reads a CSV file and maps each record to a strongly-typed list of objects.
    /// The method uses the specified class map for the type <typeparamref name="T"/> to correctly map CSV columns to object properties.
    /// </summary>
    /// <typeparam name="T">The type of the object to map each CSV record to.</typeparam>
    /// <typeparam name="TMap">The class map used to map the CSV columns to the <typeparamref name="T"/> type.</typeparam>
    /// <param name="filePath">The path to the CSV file to read.</param>
    /// <returns>A list of objects of type <typeparamref name="T"/> populated with data from the CSV file.</returns>
    public static List<T> ReadCsv<T, TMap>(string filePath)
        where TMap : ClassMap<T>
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<TMap>();

        return csv.GetRecords<T>().ToList();
    }
}

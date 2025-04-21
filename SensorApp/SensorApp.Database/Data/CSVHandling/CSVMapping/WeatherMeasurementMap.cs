using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

/// <summary>
/// Maps the <see cref="WeatherMeasurement"/> class to CSV fields for correct conversion.
/// Each property is mapped to its respective column, with custom type converters applied where necessary.
/// </summary>
public class WeatherMeasurementMap : ClassMap<WeatherMeasurement>
{
    public WeatherMeasurementMap()
    {
        Map(m => m.Temperature).TypeConverter<FloatOrNullConverter>();
        Map(m => m.RelativeHumidity).TypeConverter<FloatOrNullConverter>();
        Map(m => m.WindSpeed).TypeConverter<FloatOrNullConverter>();
        Map(m => m.WindDirection).TypeConverter<FloatOrNullConverter>();
        Map(m => m.Timestamp);
    }
}
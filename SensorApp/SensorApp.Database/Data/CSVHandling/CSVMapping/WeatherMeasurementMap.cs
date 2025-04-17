using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

public class WeatherMeasurementMap : ClassMap<WeatherMeasurement>
{
    public WeatherMeasurementMap()
    {
        Map(m => m.Temperature).TypeConverter<NullConverter>();
        Map(m => m.RelativeHumidity).TypeConverter<NullConverter>();
        Map(m => m.WindSpeed).TypeConverter<NullConverter>();
        Map(m => m.WindDirection).TypeConverter<NullConverter>();
        Map(m => m.Timestamp);
    }
}
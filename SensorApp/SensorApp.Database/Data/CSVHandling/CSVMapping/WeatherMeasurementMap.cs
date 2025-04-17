using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping
{
    public class WeatherMeasurementMap : ClassMap<WeatherMeasurement>
    {
        public WeatherMeasurementMap()
        {
            Map(m => m.Temperature).TypeConverter<NullConverter>();
            Map(m => m.Relative_humidity).TypeConverter<NullConverter>();
            Map(m => m.Wind_speed).TypeConverter<NullConverter>();
            Map(m => m.Wind_direction).TypeConverter<NullConverter>();
            Map(m => m.Timestamp);
        }
    }
}
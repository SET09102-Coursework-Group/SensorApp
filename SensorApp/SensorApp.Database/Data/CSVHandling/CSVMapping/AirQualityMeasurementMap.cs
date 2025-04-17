using System.ComponentModel;
using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping
{
    public class AirQualityMeasurementMap : ClassMap<AirQualityMeasurement>
    {
        public AirQualityMeasurementMap()
        {
            Map(m => m.Nitrogen_dioxide).TypeConverter<NullConverter>();
            Map(m => m.Sulphur_dioxide).TypeConverter<NullConverter>();
            Map(m => m.PM25_particulate_matter).TypeConverter<NullConverter>();
            Map(m => m.PM10_particulate_matter).TypeConverter<NullConverter>();
            Map(m => m.Date).TypeConverter<DateConverter>();
            Map(m => m.Time);
        }
    }
}
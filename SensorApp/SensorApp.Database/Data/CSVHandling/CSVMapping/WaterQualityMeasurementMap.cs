using System.ComponentModel;
using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

public class WaterQualityMeasurementMap : ClassMap<WaterQualityMeasurement>
{
    public WaterQualityMeasurementMap()
    {
        Map(m => m.Nitrate).TypeConverter<NullConverter>();
        Map(m => m.Nitrite).TypeConverter<NullConverter>();
        Map(m => m.Phosphate).TypeConverter<NullConverter>();
        Map(m => m.EC).TypeConverter<NullConverter>();
        Map(m => m.Date).TypeConverter<DateConverter>();
        Map(m => m.Time);
    }
}
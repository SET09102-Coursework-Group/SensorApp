using System.ComponentModel;
using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

/// <summary>
/// Maps the <see cref="WaterQualityMeasurement"/> class to CSV fields for correct conversion.
/// Each property is mapped to its respective column with custom type converters where necessary.
/// </summary>
public class WaterQualityMeasurementMap : ClassMap<WaterQualityMeasurement>
{
    public WaterQualityMeasurementMap()
    {
        Map(m => m.Nitrate).TypeConverter<FloatOrNullConverter>();
        Map(m => m.Nitrite).TypeConverter<FloatOrNullConverter>();
        Map(m => m.Phosphate).TypeConverter<FloatOrNullConverter>();
        Map(m => m.EC).TypeConverter<FloatOrNullConverter>();
        Map(m => m.Date).TypeConverter<DateConverter>();
        Map(m => m.Time);
    }
}
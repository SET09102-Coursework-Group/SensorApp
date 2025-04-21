using System.ComponentModel;
using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

/// <summary>
/// Maps the <see cref="AirQualityMeasurement"/> class to CSV fields for correct conversion.
/// Each property is mapped to its respective column, with custom type converters applied where necessary.
/// </summary>
/// 
public class AirQualityMeasurementMap : ClassMap<AirQualityMeasurement>
{
    public AirQualityMeasurementMap()
    {
        Map(m => m.NitrogenDioxide).TypeConverter<FloatOrNullConverter>();
        Map(m => m.SulphurDioxide).TypeConverter<FloatOrNullConverter>();
        Map(m => m.PM25ParticulateMatter).TypeConverter<FloatOrNullConverter>();
        Map(m => m.PM10ParticulateMatter).TypeConverter<FloatOrNullConverter>();
        Map(m => m.Date).TypeConverter<DateConverter>();
        Map(m => m.Time);
    }
}
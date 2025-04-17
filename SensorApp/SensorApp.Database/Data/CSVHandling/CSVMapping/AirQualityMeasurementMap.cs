using System.ComponentModel;
using CsvHelper.Configuration;
using SensorApp.Database.Data.CSVHandling.Converters;
using SensorApp.Database.Data.CSVHandling.Models;

namespace SensorApp.Database.Data.CSVHandling.CSVMapping;

public class AirQualityMeasurementMap : ClassMap<AirQualityMeasurement>
{
    public AirQualityMeasurementMap()
    {
        Map(m => m.NitrogenDioxide).TypeConverter<NullConverter>();
        Map(m => m.SulphurDioxide).TypeConverter<NullConverter>();
        Map(m => m.PM25ParticulateMatter).TypeConverter<NullConverter>();
        Map(m => m.PM10ParticulateMatter).TypeConverter<NullConverter>();
        Map(m => m.Date).TypeConverter<DateConverter>();
        Map(m => m.Time);
    }
}
using SensorApp.Database.Data.CSVHandling.CSVMapping;
using SensorApp.Database.Data.CSVHandling.Models;
using CsvHelper;
using System.Globalization;
using SensorApp.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using SensorApp.Database.Data.CSVHandling.Helpers;

namespace SensorApp.Database.Data.CSVHandling;

public class MeasurementSeeder
{
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SensorDbContext>();

        if (!context.Measurement.Any())
        {
            context.ChangeTracker.Clear();

            var airRecords = CsvReaderHelper.ReadCsv<AirQualityMeasurement, AirQualityMeasurementMap>(
                @"../SensorApp.Database/Data/Resources/Air_quality.csv");

            var waterRecords = CsvReaderHelper.ReadCsv<WaterQualityMeasurement, WaterQualityMeasurementMap>(
                @"../SensorApp.Database/Data/Resources/Water_quality.csv");

            var weatherRecords = CsvReaderHelper.ReadCsv<WeatherMeasurement, WeatherMeasurementMap>(
                @"../SensorApp.Database/Data/Resources/Weather.csv");

            await MeasurementSeederHelper.SeedMeasurementsAsync(
            context,
            airRecords,
            r => r.NitrogenDioxide.HasValue,
            r => new Measurement
            {
                Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                Value = r.NitrogenDioxide.Value,
                Sensor_id = 1,
                Measurement_type_id = 1
            });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
            context,
            airRecords,
            r => r.SulphurDioxide.HasValue,
            r => new Measurement
            {
                Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                Value = r.SulphurDioxide.Value,
                Sensor_id = 1,
                Measurement_type_id = 2
            });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
            context,
            airRecords,
            r => r.PM25ParticulateMatter.HasValue,
            r => new Measurement
            {
                Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                Value = r.PM25ParticulateMatter.Value,
                Sensor_id = 1,
                Measurement_type_id = 3
            });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
            context,
            airRecords,
            r => r.PM10ParticulateMatter.HasValue,
            r => new Measurement
            {
                Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                Value = r.PM10ParticulateMatter.Value,
                Sensor_id = 1,
                Measurement_type_id = 4
            });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
            context,
            waterRecords,
            r => r.Nitrate.HasValue,
            r => new Measurement
            {
                Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                Value = r.Nitrate.Value,
                Sensor_id = 2,
                Measurement_type_id = 5
            });


            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                waterRecords,
                r => r.Nitrite.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                    Value = r.Nitrite.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 6
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                waterRecords,
                r => r.Phosphate.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                    Value = r.Phosphate.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 7
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                waterRecords,
                r => r.EC.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                    Value = r.EC.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 8
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                weatherRecords,
                r => r.Temperature.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Timestamp,
                    Value = r.Temperature.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 9
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                weatherRecords,
                r => r.RelativeHumidity.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Timestamp,
                    Value = r.RelativeHumidity.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 10
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                weatherRecords,
                r => r.WindSpeed.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Timestamp,
                    Value = r.WindSpeed.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 11
                });

            await MeasurementSeederHelper.SeedMeasurementsAsync(
                context,
                weatherRecords,
                r => r.WindDirection.HasValue,
                r => new Measurement
                {
                    Timestamp = r.Timestamp,
                    Value = r.WindDirection.Value,
                    Sensor_id = 2,
                    Measurement_type_id = 12
                });
        }
    }
}
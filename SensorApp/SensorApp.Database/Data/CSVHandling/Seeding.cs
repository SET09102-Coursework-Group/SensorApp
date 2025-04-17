using SensorApp.Database.Data.CSVHandling.CSVMapping;
using SensorApp.Database.Data.CSVHandling.Models;
using CsvHelper;
using System.Globalization;
using SensorApp.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Formats.Asn1;
using SensorApp.Database.Data;

namespace SensorApp.Infrastructure.Data.CSVHandling.Seeding
{
    public class MeasurementSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SensorDbContext>();

            if (!context.Measurement.Any())
            {
                context.ChangeTracker.Clear();

                List<AirQualityMeasurement> airRecords;
                List<WaterQualityMeasurement> waterRecords;
                List<WeatherMeasurement> weatherRecords;

                using (var streamReader = new StreamReader(@"../SensorApp.Database/Data/Resources/Air_quality.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<AirQualityMeasurementMap>();
                        airRecords = csvReader.GetRecords<AirQualityMeasurement>().ToList();
                    }
                }

                using (var streamReader = new StreamReader(@"../SensorApp.Database/Data/Resources/Water_quality.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<WaterQualityMeasurementMap>();
                        waterRecords = csvReader.GetRecords<WaterQualityMeasurement>().ToList();
                    }
                }

                using (var streamReader = new StreamReader(@"../SensorApp.Database/Data/Resources/Weather.csv"))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Context.RegisterClassMap<WeatherMeasurementMap>();
                        weatherRecords = csvReader.GetRecords<WeatherMeasurement>().ToList();
                    }
                }

                var nitrogenMeasurements = airRecords
                    .Where(r => r.Nitrogen_dioxide.HasValue)
                    .Select((r, i) => new Measurement
                    {
                        Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                        Value = r.Nitrogen_dioxide.Value,
                        Sensor_id = 1,
                        Measurement_type_id = 1
                    }).ToList();

                context.Measurement.AddRange(nitrogenMeasurements);
                await context.SaveChangesAsync();

                var sulphurMeasurements = airRecords
                    .Where(r => r.Sulphur_dioxide.HasValue)
                    .Select((r, i) => new Measurement
                    {
                        Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                        Value = r.Sulphur_dioxide.Value,
                        Sensor_id = 1,
                        Measurement_type_id = 2
                    }).ToList();

                context.Measurement.AddRange(sulphurMeasurements);
                await context.SaveChangesAsync();

                var PM25Measurements = airRecords
                   .Where(r => r.PM25_particulate_matter.HasValue)
                   .Select((r, i) => new Measurement
                   {
                       Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                       Value = r.PM25_particulate_matter.Value,
                       Sensor_id = 1,
                       Measurement_type_id = 3
                   }).ToList();

                context.Measurement.AddRange(PM25Measurements);
                await context.SaveChangesAsync();

                var PM10Measurements = airRecords
                   .Where(r => r.PM10_particulate_matter.HasValue)
                   .Select((r, i) => new Measurement
                   {
                       Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                       Value = r.PM10_particulate_matter.Value,
                       Sensor_id = 1,
                       Measurement_type_id = 4
                   }).ToList();

                context.Measurement.AddRange(PM10Measurements);
                await context.SaveChangesAsync();

                var nitrateMeasurements = waterRecords
                   .Where(r => r.Nitrate.HasValue)
                   .Select((r, i) => new Measurement
                   {
                       Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                       Value = r.Nitrate.Value,
                       Sensor_id = 2,
                       Measurement_type_id = 5
                   }).ToList();

                context.Measurement.AddRange(nitrateMeasurements);
                await context.SaveChangesAsync();

                var nitriteMeasurements = waterRecords
                   .Where(r => r.Nitrite.HasValue)
                   .Select((r, i) => new Measurement
                   {
                       Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                       Value = r.Nitrite.Value,
                       Sensor_id = 2,
                       Measurement_type_id = 6
                   }).ToList();

                context.Measurement.AddRange(nitriteMeasurements);
                await context.SaveChangesAsync();

                var phosphateMeasurements = waterRecords
                  .Where(r => r.Phosphate.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                      Value = r.Phosphate.Value,
                      Sensor_id = 2,
                      Measurement_type_id = 7
                  }).ToList();

                context.Measurement.AddRange(phosphateMeasurements);
                await context.SaveChangesAsync();

                var ECMeasurements = waterRecords
                  .Where(r => r.EC.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Date.Date.Add(r.Time.TimeOfDay),
                      Value = r.EC.Value,
                      Sensor_id = 2,
                      Measurement_type_id = 8
                  }).ToList();

                context.Measurement.AddRange(ECMeasurements);
                await context.SaveChangesAsync();

                var temperatureMeasurements = weatherRecords
                  .Where(r => r.Temperature.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Timestamp,
                      Value = r.Temperature.Value,
                      Sensor_id = 3,
                      Measurement_type_id = 9
                  }).ToList();

                context.Measurement.AddRange(temperatureMeasurements);
                await context.SaveChangesAsync();

                var humidityMeasurements = weatherRecords
                  .Where(r => r.Relative_humidity.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Timestamp,
                      Value = r.Relative_humidity.Value,
                      Sensor_id = 3,
                      Measurement_type_id = 10
                  }).ToList();

                context.Measurement.AddRange(humidityMeasurements);
                await context.SaveChangesAsync();

                var windSpeedMeasurements = weatherRecords
                  .Where(r => r.Wind_speed.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Timestamp,
                      Value = r.Wind_speed.Value,
                      Sensor_id = 3,
                      Measurement_type_id = 11
                  }).ToList();

                context.Measurement.AddRange(windSpeedMeasurements);
                await context.SaveChangesAsync();

                var windDirectionMeasurements = weatherRecords
                  .Where(r => r.Wind_direction.HasValue)
                  .Select((r, i) => new Measurement
                  {
                      Timestamp = r.Timestamp,
                      Value = r.Wind_direction.Value,
                      Sensor_id = 3,
                      Measurement_type_id = 12
                  }).ToList();

                context.Measurement.AddRange(windDirectionMeasurements);
                await context.SaveChangesAsync();
            }
        }
    }
}
using SensorApp.Database.Models;

namespace SensorApp.Database.Data.CSVHandling.Helpers;
public static class MeasurementSeederHelper
{
    public static async Task SeedMeasurementsAsync<T>(
        SensorDbContext context,
        IEnumerable<T> records,
        Func<T, bool> predicate,
        Func<T, Measurement> mapFunc)
    {
        var measurements = records
            .Where(predicate)
            .Select(mapFunc)
            .ToList();

        context.Measurement.AddRange(measurements);
        await context.SaveChangesAsync();
    }
}
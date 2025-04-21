using SensorApp.Database.Models;

namespace SensorApp.Database.Data.CSVHandling.Helpers;

/// <summary>
/// Provides a helper method to seed measurement objects into the database asynchronously.
/// This method filters the records based on a predicate and maps them to Measurement objects before adding them to the database.
/// </summary>
public static class MeasurementSeederHelper
{
    /// <summary>
    /// Seeds measurements into the database asynchronously.
    /// Filters the records using a predicate, maps them to <see cref="Measurement"/> objects using a provided mapping function,
    /// and adds the mapped measurements to the database.
    /// </summary>
    /// <typeparam name="T">The type of the records to be seeded.</typeparam>
    /// <param name="dbContext">The database context to interact with the database.</param>
    /// <param name="records">The collection of records to be seeded into the database.</param>
    /// <param name="predicate">A function to filter records that should be processed.</param>
    /// <param name="mapFunc">A function to map the filtered records to <see cref="Measurement"/> objects.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedMeasurementsAsync<T>(
        SensorDbContext dbContext,
        IEnumerable<T> records,
        Func<T, bool> predicate,
        Func<T, Measurement> mapFunc)
    {
        var measurements = records
            .Where(predicate)
            .Select(mapFunc)
            .ToList();

        dbContext.Measurement.AddRange(measurements);
        await dbContext.SaveChangesAsync();
    }
}
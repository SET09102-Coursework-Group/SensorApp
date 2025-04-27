using Microcharts;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Interfaces;

/// <summary>
/// Defines a factory for creating chart objects based on measurement data.
/// </summary>
public interface IChartFactory
{
    /// <summary>
    /// Creates a LineChart from a collection of measurement data.
    /// Each point in the chart represents a measurement at a specific timestamp.
    /// </summary>
    /// <param name="data">The collection of measurement values to plot.</param>
    /// <returns>A configured <see cref="LineChart"/> representing the measurement data.</returns>
    LineChart CreateLineChart(IEnumerable<MeasurementModel> data);
    /// <summary>
    /// Creates a BarChart from a collection of measurement data.
    /// Each bar represents an individual measurement value, grouped by date.
    /// </summary>
    /// <param name="data">The collection of measurement values to display.</param>
    /// <returns>A configured <see cref="BarChart"/> representing the measurement data.</returns>
    BarChart CreateBarChart(IEnumerable<MeasurementModel> data);
}

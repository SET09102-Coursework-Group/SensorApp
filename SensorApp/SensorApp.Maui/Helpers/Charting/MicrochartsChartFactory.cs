using Microcharts;
using SensorApp.Maui.Interfaces;
using SensorApp.Shared.Models;
using SkiaSharp;

namespace SensorApp.Maui.Helpers.Charting;

/// <summary>
/// Factory class responsible for creating Microcharts chart objects
/// from measurement data for visual representation in the application.
/// </summary>
public class MicrochartsChartFactory : IChartFactory
{
    private readonly SKColor _lineColor = SKColors.DeepSkyBlue;
    private readonly SKColor _barColor = SKColors.Purple;

    /// <summary>
    /// Creates a LineChart based on the provided measurement data.
    /// Each point represents a measurement at a specific timestamp.
    /// </summary>
    /// <param name="data">The collection of measurement values to plot.</param>
    /// <returns>A configured <see cref="LineChart"/> representing the data.</returns>
    public LineChart CreateLineChart(IEnumerable<MeasurementModel> data)
    {
        var entries = data.Select(m => new ChartEntry(m.Value)
        {
            Label = m.Timestamp.ToString("dd/MM HH:mm"),
            ValueLabel = m.Value.ToString("0.##"),
            Color = _lineColor
        }).ToList();

        return new LineChart
        {
            Entries = entries,
            LineMode = LineMode.Straight,
            PointMode = PointMode.Circle,
            LineSize = 4,
            PointSize = 8
        };
    }

    /// <summary>
    /// Creates a BarChart based on the provided measurement data.
    /// Each bar represents a measurement value, grouped by date.
    /// </summary>
    /// <param name="data">The collection of measurement values to display.</param>
    /// <returns>A configured <see cref="BarChart"/> representing the data.</returns>
    public BarChart CreateBarChart(IEnumerable<MeasurementModel> data)
    {
        var entries = data.Select(m => new ChartEntry(m.Value)
        {
            Label = m.Timestamp.ToString("dd/MM"),
            ValueLabel = m.Value.ToString("0.##"),
            Color = _barColor
        }).ToList();

        return new BarChart
        {
            Entries = entries,
            BarAreaAlpha = 128,    
            MaxValue = entries.Max(e => float.Parse(e.ValueLabel)),
            MinValue = entries.Min(e => float.Parse(e.ValueLabel))
        };
    }
}
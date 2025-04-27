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
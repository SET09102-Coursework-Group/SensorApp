using Microcharts;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Interfaces;

public interface IChartFactory
{
    LineChart CreateLineChart(IEnumerable<MeasurementModel> data);
    BarChart CreateBarChart(IEnumerable<MeasurementModel> data);
}

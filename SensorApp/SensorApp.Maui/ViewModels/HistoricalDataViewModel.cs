using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SensorApp.Maui.Interfaces;
using SensorApp.Maui.Utils;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel responsible for loading, managing, and displaying historical measurement data,
/// including generating charts and basic statistics.
/// </summary>
public partial class HistoricalDataViewModel : BaseViewModel
{
    readonly IMeasurementService _measurementService;
    readonly ITokenProvider _tokenProvider;
    readonly IMeasurandService _measurandService;
    readonly ISensorApiService _sensorService;
    readonly IChartFactory _chartFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoricalDataViewModel"/> class.
    /// </summary>
    /// <param name="measurementService">Service for retrieving measurement data.</param>
    /// <param name="tokenProvider">Service for retrieving authentication tokens.</param>
    /// <param name="sensorService">Service for retrieving sensor information.</param>
    /// <param name="measurandService">Service for retrieving measurand types.</param>
    /// <param name="chartFactory">Factory for creating charts.</param>
    public HistoricalDataViewModel(IMeasurementService measurementService, ITokenProvider tokenProvider, ISensorApiService sensorService, IMeasurandService measurandService, IChartFactory chartFactory)
    {
        _measurementService = measurementService;
        _tokenProvider = tokenProvider;
        _sensorService = sensorService;
        _measurandService = measurandService;

        From = DateTime.Today.AddDays(-7);
        To = DateTime.Today;
        _chartFactory = chartFactory;

        ChartTypeOptions = new ObservableCollection<ChartType>(Enum.GetValues<ChartType>());
        SelectedChartType = ChartType.Line;
    }

    
    [ObservableProperty]
    DateTime? from;

    [ObservableProperty] 
    DateTime? to;

    [ObservableProperty] 
    SensorModel? selectedSensor;

    [ObservableProperty]
    MeasurandDisplayExtension? selectedMeasurand;

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    float? minValue;

    [ObservableProperty]
    float? maxValue;

    [ObservableProperty]
    float? averageValue;

    [ObservableProperty]
    int count;

    [ObservableProperty] Chart? measurementChart;
    public ObservableCollection<MeasurementModel> MeasurementValues { get; } = [];

    public ObservableCollection<SensorModel> SensorOptions { get; } = [];
    public ObservableCollection<MeasurandDisplayExtension> MeasurandOptions { get; } = [];

    public enum ChartType { Line, Bar }

    [ObservableProperty]
    ChartType selectedChartType;
    public ObservableCollection<ChartType> ChartTypeOptions { get; }



    /// <summary>
    /// Triggered when the selected sensor changes.
    /// Reloads the available measurands for the selected sensor.
    /// </summary>
    partial void OnSelectedSensorChanged(SensorModel? value) => _ = ReloadMeasurandsAsync(value);

    /// <summary>
    /// Reloads measurand options based on the selected sensor.
    /// </summary>
    private async Task ReloadMeasurandsAsync(SensorModel? sensor)
    {
        MeasurandOptions.Clear();
        SelectedMeasurand = null;
        if (sensor == null) return;

        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return;

        var measurandsList = await _measurandService.GetMeasurandsAsync(token, sensor.Id);
        foreach (var measurand in measurandsList)
        {
            MeasurandOptions.Add(new MeasurandDisplayExtension(measurand));
        }
    }

    /// <summary>
    /// Loads measurement data for the selected sensor and measurand.
    /// Updates statistics and chart visualization.
    /// </summary>
    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await EnsureAuthenticatedAsync();
            if (token == null) return;

            var data = await LoadMeasurementsAsync(token);

            if (data.Any())
            {
                CalculateStats(data);
                RefreshChart();
            }
            else
            {
                ClearStatsAndChart();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Could not load data: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Triggered when the selected chart type changes.
    /// Refreshes the chart accordingly.
    /// </summary>
    partial void OnSelectedChartTypeChanged(ChartType value)
    {
        if (MeasurementValues?.Count > 0)
        {
            RefreshChart();
        }
    }

    /// <summary>
    /// Refreshes the measurement chart based on the selected chart type.
    /// </summary>
    void RefreshChart()
    {
        if (MeasurementValues == null || MeasurementValues.Count == 0)
        {
            MeasurementChart = null;
            return;
        }

        MeasurementChart = SelectedChartType switch
        {
            ChartType.Bar => _chartFactory.CreateBarChart(MeasurementValues),
            ChartType.Line => _chartFactory.CreateLineChart(MeasurementValues),
            _ => null
        };
    }

    /// <summary>
    /// Ensures the user is authenticated by retrieving a valid token.
    /// Shows an alert if authentication fails.
    /// </summary>
    private async Task<string?> EnsureAuthenticatedAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            await Shell.Current.DisplayAlert(
                "Not Authenticated",
                "You are not logged in. Please log in to view measurements.",
                "OK");
            return null;
        }
        return token;
    }

    /// <summary>
    /// Loads measurement data from the server based on current filter criteria.
    /// </summary>
    private async Task<IReadOnlyList<MeasurementModel>> LoadMeasurementsAsync(string token)
    {
        var list = await _measurementService.GetMeasurementsAsync(
            sensorId: SelectedSensor?.Id,
            measurandId: SelectedMeasurand?.Model.Id,
            from: From,
            to: To,
            token: token);

        MeasurementValues.Clear();
        foreach (var measurements in list)
            MeasurementValues.Add(measurements);

        return list;
    }

    /// <summary>
    /// Calculates basic statistics (min, max, average, count) from measurement data.
    /// </summary>
    private void CalculateStats(IEnumerable<MeasurementModel> data)
    {
        MinValue = data.Min(m => m.Value);
        MaxValue = data.Max(m => m.Value);
        AverageValue = (float)data.Average(m => m.Value);
        Count = data.Count();
    }

    /// <summary>
    /// Clears calculated statistics and chart when no data is available.
    /// </summary>
    private void ClearStatsAndChart()
    {
        MinValue = null;
        MaxValue = null;
        AverageValue = null;
        Count = 0;
        MeasurementChart = null;
    }

    /// <summary>
    /// Loads available sensor options for the user to select from.
    /// </summary>
    public async Task LoadSensorOptionsAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return;

        SensorOptions.Clear();
        var sensors = await _sensorService.GetSensorsAsync(token);
        foreach (var s in sensors)
            SensorOptions.Add(s);
    }

}

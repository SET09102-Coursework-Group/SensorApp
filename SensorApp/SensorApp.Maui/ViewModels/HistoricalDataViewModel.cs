using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SensorApp.Maui.Interfaces;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class HistoricalDataViewModel : BaseViewModel
{
    readonly IMeasurementService _measurementService;
    readonly ITokenProvider _tokenProvider;
    readonly IMeasurandService _measurandService;
    readonly SensorApiService _sensorService;
    readonly IChartFactory _chartFactory;

    public HistoricalDataViewModel(IMeasurementService measurementService, ITokenProvider tokenProvider, SensorApiService sensorService, IMeasurandService measurandService, IChartFactory chartFactory)
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
    MeasurandModel? selectedMeasurand;

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
    public ObservableCollection<MeasurandModel> MeasurandOptions { get; } = [];

    public enum ChartType { Line, Bar }

    [ObservableProperty]
    ChartType selectedChartType;
    public ObservableCollection<ChartType> ChartTypeOptions { get; }




    partial void OnSelectedSensorChanged(SensorModel? value) => _ = ReloadMeasurandsAsync(value);


    private async Task ReloadMeasurandsAsync(SensorModel? sensor)
    {
        MeasurandOptions.Clear();
        SelectedMeasurand = null;
        if (sensor == null) return;

        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return;

        var measurandsList = await _measurandService.GetMeasurandsAsync(token, sensor.Id);
        foreach (var measurands in measurandsList)
            MeasurandOptions.Add(measurands);
    }


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

    partial void OnSelectedChartTypeChanged(ChartType value)
    {
        if (MeasurementValues?.Count > 0)
        {
            RefreshChart();
        }
    }

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

    private async Task<IReadOnlyList<MeasurementModel>> LoadMeasurementsAsync(string token)
    {
        var list = await _measurementService.GetMeasurementsAsync(
            sensorId: SelectedSensor?.Id,
            measurandId: SelectedMeasurand?.Id,
            from: From,
            to: To,
            token: token);

        MeasurementValues.Clear();
        foreach (var measurements in list)
            MeasurementValues.Add(measurements);

        return list;
    }

    private void CalculateStats(IEnumerable<MeasurementModel> data)
    {
        MinValue = data.Min(m => m.Value);
        MaxValue = data.Max(m => m.Value);
        AverageValue = (float)data.Average(m => m.Value);
        Count = data.Count();
    }

    private void ClearStatsAndChart()
    {
        MinValue = null;
        MaxValue = null;
        AverageValue = null;
        Count = 0;
        MeasurementChart = null;
    }

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

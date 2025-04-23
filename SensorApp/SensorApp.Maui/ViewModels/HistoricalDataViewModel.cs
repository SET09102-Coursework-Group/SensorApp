using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class HistoricalDataViewModel : BaseViewModel
{
    readonly IMeasurementService _measurementService;
    readonly ITokenProvider _tokenProvider;
    readonly IMeasurandService _measurandService;
    readonly SensorApiService _sensorService;

    public HistoricalDataViewModel(IMeasurementService measurementService, ITokenProvider tokenProvider, SensorApiService sensorService, IMeasurandService measurandService)
    {
        _measurementService = measurementService;
        _tokenProvider = tokenProvider;
        _sensorService = sensorService;
        _measurandService = measurandService;

        From = DateTime.Today.AddDays(-7);
        To = DateTime.Today;
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

    public ObservableCollection<MeasurementModel> MeasurementValues { get; } = [];
    public ObservableCollection<SensorModel> SensorOptions { get; } = [];
    public ObservableCollection<MeasurandModel> MeasurandOptions { get; } = [];

    [ObservableProperty] 
    float? minValue;

    [ObservableProperty] 
    float? maxValue;

    [ObservableProperty] 
    float? averageValue;

    [ObservableProperty] 
    int count;

    [ObservableProperty] 
    Chart? measurementChart;

    partial void OnSelectedSensorChanged(SensorModel? value)  => _ = ReloadMeasurandsAsync(value);

    async Task ReloadMeasurandsAsync(SensorModel? sensor)
    {
        MeasurandOptions.Clear();
        SelectedMeasurand = null;
        if (sensor == null)
        {
            return;
        }

        var token = await _tokenProvider.GetTokenAsync();
        var measurandsList = await _measurandService.GetMeasurandsAsync(token!, sensor.Id);
        foreach (var measurands in measurandsList)
        {
            MeasurandOptions.Add(measurands);
        }
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert("Not Authenticated", "You are not logged in. Please log in to view measurements.", "OK");
                return;
            }

            var measurements = await _measurementService.GetMeasurementsAsync(sensorId: SelectedSensor?.Id, measurandId: SelectedMeasurand?.Id, from: From, to: To, token: token);

            MeasurementValues.Clear();

            foreach (var measurement in measurements)
                MeasurementValues.Add(measurement);

            if (measurements.Any())
            {
                MinValue = measurements.Min(m => m.Value);
                MaxValue = measurements.Max(m => m.Value);
                AverageValue = (float)measurements.Average(m => m.Value);
                Count = measurements.Count;

                var entries = measurements.Select(m => new ChartEntry(m.Value)
                {
                    Label = m.Timestamp.ToString("dd/MM HH:mm"),
                    ValueLabel = m.Value.ToString("0.##"),
                    Color = SKColors.DeepSkyBlue
                }).ToList();

                MeasurementChart = new LineChart
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    PointMode = PointMode.Circle,
                    LineSize = 4,
                    PointSize = 8
                };
            }
            else
            {
                MinValue = null;
                MaxValue = null;
                AverageValue = null;
                Count = 0;
                MeasurementChart = null;
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

    public async Task LoadLookupListsAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        SensorOptions.Clear();
        var sensors = await _sensorService.GetSensorsAsync(token);
        foreach (var sensor in sensors)
        {
            SensorOptions.Add(sensor);
        }
    }
}

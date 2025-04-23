using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class HistoricalDataViewModel : BaseViewModel
{
    private readonly IMeasurementService _measurementService;
    private readonly ITokenProvider _tokenProvider;
    private readonly IMeasurandService _measurandService;
    private readonly SensorApiService _sensorService;

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


    partial void OnSelectedSensorChanged(SensorModel? value)
    {
        _ = ReloadMeasurandsAsync(value);
    }

    private async Task ReloadMeasurandsAsync(SensorModel? sensor)
    {
        MeasurandOptions.Clear();
        SelectedMeasurand = null;
        if (sensor == null)
        {
            return;
        }

        var token = await _tokenProvider.GetTokenAsync();
        var mesurandList = await _measurandService.GetMeasurandsAsync(token!, sensor.Id);

        foreach (var mesurand in mesurandList)
        {
            MeasurandOptions.Add(mesurand);
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

            MeasurementValues.Clear();
            var measurements = await _measurementService.GetMeasurementsAsync(sensorId: SelectedSensor?.Id, measurandId: SelectedMeasurand?.Id, from: From, to: To, token: token);

            foreach (var measurement in measurements)
            {
                MeasurementValues.Add(measurement);
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

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Maui.ViewModels;
using SensorApp.Maui.Models;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using SensorApp.Maui.Interfaces;

namespace SensorApp.Maui.Views.Pages;

public partial class SensorMapPage : ContentPage
{
    private readonly SensorMapViewModel mapViewModel;
    public SensorMapPage(SensorApiService sensorService, ISensorPinFactory pinFactory)
    {
        InitializeComponent();
        mapViewModel = new SensorMapViewModel(sensorService, pinFactory);
        mapViewModel.ThresholdBreached += OnThresholdBreached;
        BindingContext = mapViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAndDisplaySensorsAsync();
        mapViewModel.StartRealTimeUpdates();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        mapViewModel.StopRealTimeUpdates();
        mapViewModel.ThresholdBreached -= OnThresholdBreached;
    }
    private async Task LoadAndDisplaySensorsAsync()
    {
        await mapViewModel.LoadSensors();
        DisplaySensorPins(mapViewModel.Sensors);
    }

    private void DisplaySensorPins(IEnumerable<SensorModel> sensors)
    {
        SensorMap.Pins.Clear();

        foreach (var pin in mapViewModel.Pins)
        {
            SensorMap.Pins.Add(pin);
        }

        CenterMapOnFirstValidSensor(sensors);
    }

    private void CenterMapOnFirstValidSensor(IEnumerable<SensorModel> sensors)
    {
        var first = sensors.FirstOrDefault(sensor =>
            !float.IsNaN(sensor.Latitude) &&
            !float.IsNaN(sensor.Longitude));

        if (first != null)
        {
            var location = new Location(first.Latitude, first.Longitude);
            var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
            SensorMap.MoveToRegion(mapSpan);
        }
    }

    private async void OnThresholdBreached(IEnumerable<SensorModel> breachedSensors)
    {
        string message = string.Join("\n\n", breachedSensors.Select(sensor =>
        {
            var breaches = sensor.LatestMeasurementsByType.Values
                .Where(measurement =>
                    measurement.MeasurementType != null && measurement.MeasurementType.Min_safe_threshold.HasValue && measurement.MeasurementType.Max_safe_threshold.HasValue &&
                    (measurement.Value < measurement.MeasurementType.Min_safe_threshold || measurement.Value > measurement.MeasurementType.Max_safe_threshold))
                .Select(measurement => $"- {measurement.MeasurementType.Name}: {measurement.Value} (Safe Range: {measurement.MeasurementType.Min_safe_threshold}–{measurement.MeasurementType.Max_safe_threshold}) at {measurement.Timestamp}");

            string breachesText = string.Join("\n", breaches);

            return $"{sensor.Type} sensor at coordinate location {sensor.Longitude}, {sensor.Latitude} has breached the following thresholds:\n\n{breachesText}";
        }));

        await DisplayAlert("Sensor Alert!!", message, "OK");
    }
};
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Maui.ViewModels;
using SensorApp.Maui.Models;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;

namespace SensorApp.Maui.Views.Pages;

public partial class SensorMapPage : ContentPage
{
    private readonly SensorMapViewModel mapViewModel;
    public SensorMapPage(SensorApiService sensorService)
    {
        InitializeComponent();
        mapViewModel = new SensorMapViewModel(sensorService);
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
        var first = sensors.FirstOrDefault(s =>
            !double.IsNaN(s.Latitude) &&
            !double.IsNaN(s.Longitude) &&
            s.Latitude != 0 &&
            s.Longitude != 0);

        if (first != null)
        {
            var location = new Location(first.Longitude, first.Latitude);
            var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
            SensorMap.MoveToRegion(mapSpan);
        }
    }

    private async void OnThresholdBreached(IEnumerable<SensorModel> breachedSensors)
    {
        string message = string.Join("\n\n", breachedSensors.Select(sensor =>
        {
            var breaches = sensor.LatestMeasurementsByType.Values
                .Where(m =>
                    m.MeasurementType != null && m.MeasurementType.Min_safe_threshold.HasValue && m.MeasurementType.Max_safe_threshold.HasValue &&
                    (m.Value < m.MeasurementType.Min_safe_threshold || m.Value > m.MeasurementType.Max_safe_threshold))
                .Select(m => $"- {m.MeasurementType.Name}: {m.Value} (Safe Range: {m.MeasurementType.Min_safe_threshold}–{m.MeasurementType.Max_safe_threshold}) at {m.Timestamp}");

            string breachesText = string.Join("\n", breaches);

            return $"{sensor.Type} sensor in Zone {sensor.Site_zone} has breached the following thresholds:\n{breachesText}";
        }));

        await DisplayAlert("Sensor Alert ⚠️", message, "OK");
    }
};
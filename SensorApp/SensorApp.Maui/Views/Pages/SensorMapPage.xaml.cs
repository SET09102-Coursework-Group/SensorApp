using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Maui.ViewModels;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page that displays an interactive map of sensors, showing sensor data and handling threshold breach notifications.
/// </summary>
public partial class SensorMapPage : ContentPage
{
    private readonly SensorMapViewModel _mapViewModel;
    private readonly ISensorAnalysisService _sensorAnalysisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorMapPage"/> class.
    /// </summary>
    /// <param name="sensorService">Service for fetching sensor data.</param>
    /// <param name="pinInfoFactory">Factory for creating sensor pin information.</param>
    /// <param name="sensorAnalysisService">Service for analyzing sensor data.</param>
    public SensorMapPage(ISensorApiService _sensorService, ISensorPinInfoFactory pinInfoFactory, ISensorAnalysisService _sensorAnalysisService)
    {
        InitializeComponent();
        this._sensorAnalysisService = _sensorAnalysisService;
        _mapViewModel = new SensorMapViewModel(_sensorService, pinInfoFactory, _sensorAnalysisService);
        _mapViewModel.ThresholdBreached += OnThresholdBreached; // Event subscription for threshold breaches
        BindingContext = _mapViewModel;
    }

    /// <summary>
    /// Loads sensors and starts real-time data pdates when the page appears.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAndDisplaySensorsAsync();
        _mapViewModel.StartRealTimeUpdates();
    }

    /// <summary>
    /// Stops real-time updates and unsubscribes from threshold breach events when the page disappears.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _mapViewModel.StopRealTimeUpdates();
        _mapViewModel.ThresholdBreached -= OnThresholdBreached;
    }

    /// <summary>
    /// Loads sensors asynchronously and displays them on the map.
    /// </summary>
    private async Task LoadAndDisplaySensorsAsync()
    {
        await _mapViewModel.LoadSensors();
        DisplaySensorPins(_mapViewModel.Sensors);
    }

    /// <summary>
    /// Adds map pins for each sensor and centers the map on the first valid sensor.
    /// </summary>
    private void DisplaySensorPins(IEnumerable<SensorModel> sensors)
    {
        SensorMap.Pins.Clear();

        foreach (var pinInfo in _mapViewModel.Pins)
        {
            SensorMap.Pins.Add(ToMapPin(pinInfo));
        }

        CenterMapOnFirstValidSensor(sensors);
    }

    /// <summary>
    /// Centers the map on the first valid sensor with latitude and longitude.
    /// </summary>
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

    /// <summary>
    /// Converts a <see cref="SensorPinInfo"/> to a <see cref="Pin"/> to be used on the map.
    /// </summary>
    private Pin ToMapPin(SensorPinInfo pinInfo)
    {
        return new Pin
        {
            Label = pinInfo.Label,
            Address = pinInfo.Address,
            Location = new Location(pinInfo.Latitude, pinInfo.Longitude),
            Type = PinType.Place
        };
    }

    /// <summary>
    /// Handles the event when at least one sensor exceeds its threshold, displaying an alert with details.
    /// </summary>
    private async void OnThresholdBreached(IEnumerable<SensorModel> breachedSensors)
    {
        string message = string.Join("\n\n", breachedSensors.Select(sensor =>
        {
            var breaches = _sensorAnalysisService.GetLatestMeasurementsByType(sensor).Values
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
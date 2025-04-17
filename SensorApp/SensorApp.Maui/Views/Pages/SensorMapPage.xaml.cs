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
        BindingContext = mapViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAndDisplaySensorsAsync();
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
};
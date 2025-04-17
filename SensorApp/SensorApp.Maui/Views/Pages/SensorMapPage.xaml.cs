using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Maui.ViewModels;
using SensorApp.Maui.Models;
using SensorApp.Shared.Services;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Views.Pages;

public partial class SensorMapPage : ContentPage
{
    public SensorMapPage(SensorApiService sensorService)
    {
        InitializeComponent();
        BindingContext = new SensorMapViewModel(sensorService);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SensorMapViewModel vm)
        {
            await vm.LoadSensors();
            AddSensorPins(vm.Sensors);
        }
    }

    private async void AddSensorPins(IEnumerable<SensorModel> sensors)
    {
        SensorMap.Pins.Clear();

        foreach (var sensor in sensors)
        {
            if (sensor.Latitude == 0 && sensor.Longitude == 0)
                continue;

            var pin = new Microsoft.Maui.Controls.Maps.Pin
            {
                Label = sensor.IsThresholdBreached ? $"⚠️ {sensor.Type} - ALERT" : $"{sensor.Type} - {sensor.Status}",
                Location = new Microsoft.Maui.Devices.Sensors.Location(sensor.Longitude, sensor.Latitude),
                Type = PinType.Place,
                Address = $"Zone: {sensor.Site_zone}\nLatest: {sensor.LatestMeasurement?.Value} @ {sensor.LatestMeasurement?.Timestamp}",
            };

            SensorMap.Pins.Add(pin);
        }

        var first = sensors.FirstOrDefault(s =>
        !double.IsNaN(s.Latitude) &&
        !double.IsNaN(s.Longitude));

        if (first != null)
        {
            SensorMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Location(first.Longitude, first.Latitude),
                    Distance.FromKilometers(5)
                )
            );
        }
    }
};
}
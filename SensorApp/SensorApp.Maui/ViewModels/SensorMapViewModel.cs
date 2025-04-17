using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Helpers.MenuRoles;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Timer = System.Timers.Timer;

namespace SensorApp.Maui.ViewModels;

public partial class SensorMapViewModel : BaseViewModel
{
    private readonly SensorApiService sensorService;
    public ObservableCollection<SensorModel> Sensors { get; } = new();
    public ObservableCollection<Pin> Pins { get; } = new();

    public SensorMapViewModel(SensorApiService sensorService)
    {
        this.sensorService = sensorService;
    }

    public event Action<IEnumerable<SensorModel>>? ThresholdBreached;

    private System.Timers.Timer? updateTimer;

    public async Task LoadSensors()
    {
        var token = await SecureStorage.GetAsync("Token");

        var sensors = await sensorService.GetSensorsAsync(token);
        var breached = new List<SensorModel>();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Pins.Clear();
            Sensors.Clear();

            foreach (var sensor in sensors)
            {
                Sensors.Add(sensor);
                if (sensor.Latitude == 0 && sensor.Longitude == 0)
                    continue;

                if (sensor.IsThresholdBreached)
                    breached.Add(sensor);

                var pin = new Pin
                {
                    Label = sensor.IsThresholdBreached
                        ? $"⚠️ {sensor.Type} - ALERT"
                        : $"{sensor.Type} - {sensor.Status}",
                    Location = new Location(sensor.Longitude, sensor.Latitude),
                    Type = PinType.Place,
                    Address = $"Zone: {sensor.Site_zone}\nLatest: {sensor.LatestMeasurement?.Value} @ {sensor.LatestMeasurement?.Timestamp}"
                };

                Pins.Add(pin);

                if (breached.Any())
                {
                    ThresholdBreached?.Invoke(breached);
                }
            }  
        });
    }
    public void StartRealTimeUpdates(int intervalMs = 10000)
    {
        updateTimer = new Timer(intervalMs);
        updateTimer.Elapsed += async (s, e) => await LoadSensors();
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;
    }

    public void StopRealTimeUpdates()
    {
        if (updateTimer != null)
        {
            updateTimer?.Stop();
            updateTimer?.Dispose();
            updateTimer = null;
        }
    }

}
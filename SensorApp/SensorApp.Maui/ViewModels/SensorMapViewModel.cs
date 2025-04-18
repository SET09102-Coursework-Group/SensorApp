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
using SensorApp.Maui.Interfaces;
using System.Diagnostics;

namespace SensorApp.Maui.ViewModels;

public partial class SensorMapViewModel : BaseViewModel
{
    private readonly SensorApiService sensorService;
    private readonly ISensorPinFactory pinFactory;
    public ObservableCollection<SensorModel> Sensors { get; } = new();
    public ObservableCollection<Pin> Pins { get; } = new();

    public SensorMapViewModel(SensorApiService sensorService, ISensorPinFactory pinFactory)
    {
        this.sensorService = sensorService;
        this.pinFactory = pinFactory;
    }

    public event Action<IEnumerable<SensorModel>>? ThresholdBreached;

    private System.Timers.Timer? updateTimer;

    public async Task LoadSensors()
    {
        try
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

                    Pins.Add(pinFactory.CreatePin(sensor));
                }

                if (breached.Any())
                {
                    ThresholdBreached?.Invoke(breached);
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading sensors: {ex.Message}");
        }
    }
    public void StartRealTimeUpdates(int intervalMs = 30000)
    {
        updateTimer = new Timer(intervalMs);
        updateTimer.Elapsed += async (s, e) => await LoadSensors();
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;
    }

    public void StopRealTimeUpdates()
    {
        updateTimer?.Stop();
        updateTimer?.Dispose();
        updateTimer = null;
    }

}
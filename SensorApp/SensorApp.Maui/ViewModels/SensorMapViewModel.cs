using System.Collections.ObjectModel;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using Microsoft.Maui.Controls.Maps;
using Timer = System.Timers.Timer;
using SensorApp.Shared.Interfaces;
using System.Diagnostics;
using SensorApp.Maui.Interfaces;

namespace SensorApp.Maui.ViewModels;

public partial class SensorMapViewModel : BaseViewModel
{
    private readonly SensorApiService _sensorService;
    private readonly ISensorPinFactory pinFactory;
    private readonly ISensorAnalysisService _sensorAnalysisService;
    public ObservableCollection<SensorModel> Sensors { get; } = new();
    public ObservableCollection<Pin> Pins { get; } = new();
    public event Action<IEnumerable<SensorModel>>? ThresholdBreached;

    private System.Timers.Timer? updateTimer;

    public SensorMapViewModel(SensorApiService _sensorService, ISensorPinFactory pinFactory, ISensorAnalysisService _sensorAnalysisService)
    {
        this._sensorService = _sensorService;
        this.pinFactory = pinFactory;
        this._sensorAnalysisService = _sensorAnalysisService;
    }

    public async Task LoadSensors()
    {
        try
        {
            var token = await SecureStorage.GetAsync("Token");

            var sensors = await _sensorService.GetSensorsAsync(token);
            var breached = new List<SensorModel>();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Pins.Clear();
                Sensors.Clear();

                foreach (var sensor in sensors)
                {
                    Sensors.Add(sensor);

                    if (_sensorAnalysisService.IsThresholdBreached(sensor))
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
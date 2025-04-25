using System.Collections.ObjectModel;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;
using Microsoft.Maui.Controls.Maps;
using Timer = System.Timers.Timer;
using SensorApp.Shared.Interfaces;
using System.Diagnostics;
using SensorApp.Maui.Interfaces;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for managing the interactive sensor map, handling sensor data, map pin creation, and providing real-time updates.
/// </summary>
public partial class SensorMapViewModel : BaseViewModel
{
    private readonly ISensorApiService _sensorService;
    private readonly ISensorPinInfoFactory pinInfoFactory;
    private readonly ISensorAnalysisService _sensorAnalysisService;

    /// <summary>
    /// A collection of sensors to be displayed on the map.
    /// </summary>
    public ObservableCollection<SensorModel> Sensors { get; } = new();
    /// <summary>
    /// A collection of sensor pin info for map pins.
    /// </summary>
    public ObservableCollection<SensorPinInfo> Pins { get; } = new();

    /// <summary>
    /// Event triggered when at least one sensor measurement exceeds its threshold value.
    /// </summary>
    public event Action<IEnumerable<SensorModel>>? ThresholdBreached;

    private System.Timers.Timer? updateTimer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorMapViewModel"/> class with the required services.
    /// </summary>
    /// <param name="sensorService">The service for retrieving sensor data.</param>
    /// <param name="pinInfoFactory">Factory for creating sensor pin information.</param>
    /// <param name="sensorAnalysisService">Service for analyzing sensor data.</param>
    public SensorMapViewModel(ISensorApiService _sensorService, ISensorPinInfoFactory pinInfoFactory, ISensorAnalysisService _sensorAnalysisService)
    {
        this._sensorService = _sensorService;
        this.pinInfoFactory = pinInfoFactory;
        this._sensorAnalysisService = _sensorAnalysisService;
    }

    /// <summary>
    /// Loads the list of sensors from the API and updates the sensor collection and map pins.
    /// Triggers the ThresholdBreached event if any sensor exceeds its threshold.
    /// </summary>
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

                    Pins.Add(pinInfoFactory.CreatePinInfo(sensor));
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

    /// <summary>
    /// Starts real-time updates of sensor data at specified intervals.
    /// </summary>
    /// <param name="intervalMs">The interval in milliseconds for real-time updates.</param>
    public void StartRealTimeUpdates(int intervalMs = 30000)
    {
        updateTimer = new Timer(intervalMs);
        updateTimer.Elapsed += async (s, e) => await LoadSensors();
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;
    }

    /// <summary>
    /// Stops real-time updates of sensor data.
    /// </summary>
    public void StopRealTimeUpdates()
    {
        updateTimer?.Stop();
        updateTimer?.Dispose();
        updateTimer = null;
    }

}
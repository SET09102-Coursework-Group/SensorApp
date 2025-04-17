using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Helpers.MenuRoles;
using SensorApp.Shared.Models;
using SensorApp.Shared.Services;

namespace SensorApp.Maui.ViewModels;

public partial class SensorMapViewModel : BaseViewModel
{
    private readonly SensorApiService sensorService;
    public ObservableCollection<SensorModel> Sensors { get; } = new();

    public SensorMapViewModel(SensorApiService sensorService)
    {
        this.sensorService = sensorService;
    }

    public async Task LoadSensors()
    {
        var token = await SecureStorage.GetAsync("Token");

        var sensors = await sensorService.GetSensorsAsync(token);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Sensors.Clear();
            foreach (var sensor in sensors)
                Sensors.Add(sensor);
        });
    }

}
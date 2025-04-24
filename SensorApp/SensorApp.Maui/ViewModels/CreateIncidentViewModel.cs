using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Services;
using SensorApp.Shared.Dtos;
using System.Collections.ObjectModel;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.ViewModels;

public partial class CreateIncidentViewModel : BaseViewModel
{
    private readonly IIncidentApiService _incidentService;
    private readonly SensorApiService _sensorService;
    private readonly ITokenProvider _tokenProvider;

    [ObservableProperty] private SensorModel selectedSensor;
    [ObservableProperty] private string selectedType;
    [ObservableProperty] private int selectedSensorId;
    [ObservableProperty] private string selectedPriority;

    [ObservableProperty] private string comments;

    public List<string> TypeOptions { get; } = ["Max threshold breach", "Min threshold breach", "Sensor unresponsive"];
    public List<string> PriorityOptions { get; } = ["High", "Medium", "Low"];
    public ObservableCollection<SensorModel> Sensors { get; } = new();

    public CreateIncidentViewModel(IIncidentApiService incidentService, SensorApiService sensorService, ITokenProvider tokenProvider)
    {
        _incidentService = incidentService;
        _sensorService = sensorService;
        _tokenProvider = tokenProvider;

        SelectedType = TypeOptions.First();
        SelectedPriority = PriorityOptions.First();

        _ = LoadSensorsAsync();
    }
    private async Task LoadSensorsAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        var sensors = await _sensorService.GetSensorsAsync(token);
        Sensors.Clear();
        foreach (var s in sensors)
        {
            Sensors.Add(s);
        }

        SelectedSensor = Sensors.FirstOrDefault();
    }
    partial void OnSelectedSensorChanged(SensorModel value)
    {
        if (value != null)
            selectedSensorId = value.Id;
    }

    [RelayCommand]
    public async Task CreateIncidentAsync()
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("No auth token");
            }

            var newIncident = new CreateIncidentDto
            {
                Type = SelectedType,
                Status = "Open",
                SensorId = SelectedSensorId,
                Priority = SelectedPriority,
                Comments = Comments,
            };

            Console.WriteLine(newIncident.SensorId);

            var success = await _incidentService.AddIncidentAsync(token, newIncident);
            if (success)
                await Shell.Current.DisplayAlert("Success", "Incident created!", "OK");
                await Shell.Current.GoToAsync("..");
        }
        catch
        {
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create incident report.", "OK");
            }
        }
    }
}
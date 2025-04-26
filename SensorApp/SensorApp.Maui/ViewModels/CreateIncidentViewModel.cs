using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Dtos;
using System.Collections.ObjectModel;
using SensorApp.Shared.Models;
using SensorApp.Shared.Enums;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for creating a new incident report. 
/// It handles the logic for loading sensors, setting up incident types and priorities, 
/// and creating an incident report via the API service.
/// </summary>
public partial class CreateIncidentViewModel : BaseViewModel
{
    private readonly IIncidentApiService _incidentService;
    private readonly ISensorApiService _sensorService;
    private readonly ITokenProvider _tokenProvider;

    [ObservableProperty] private SensorModel selectedSensor;
    [ObservableProperty] private IncidentType selectedType;
    [ObservableProperty] private int selectedSensorId;
    [ObservableProperty] private IncidentPriority selectedPriority;

    [ObservableProperty] private string comments;

    public List<IncidentType> TypeOptions { get; } = Enum.GetValues<IncidentType>().ToList();
    public List<IncidentPriority> PriorityOptions { get; } = Enum.GetValues<IncidentPriority>().ToList();

    public ObservableCollection<SensorModel> Sensors { get; } = new();

    [ObservableProperty]
    private bool isLoadingSensors;

    /// <summary>
    /// Constructor that initializes services and loads sensor data asynchronously.
    /// </summary>
    /// <param name="incidentService">The service for creating incidents.</param>
    /// <param name="sensorService">The service for fetching sensors.</param>
    /// <param name="tokenProvider">The provider for authentication tokens.</param>
    public CreateIncidentViewModel(IIncidentApiService incidentService, ISensorApiService sensorService, ITokenProvider tokenProvider)
    {
        _incidentService = incidentService;
        _sensorService = sensorService;
        _tokenProvider = tokenProvider;

        SelectedType = TypeOptions.First();
        SelectedPriority = PriorityOptions.First();

        _ = LoadSensorsAsync();
    }

    /// <summary>
    /// Asynchronously loads the list of sensors from the sensor service and updates the sensor list.
    /// Also sets the default selected sensor.
    /// </summary>
    private async Task LoadSensorsAsync()
    {
        isLoadingSensors = true;

        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load sensors: {ex.Message}");
        }
        finally
        {
            IsLoadingSensors = false;
        }

    }

    /// <summary>
    /// Triggered when the selected sensor changes. Updates the stored selectedSensorID based on the selected sensor.
    /// </summary>
    /// <param name="value">The newly selected sensor.</param>
    partial void OnSelectedSensorChanged(SensorModel value)
    {
        if (value != null)
            selectedSensorId = value.Id;
    }

    /// <summary>
    /// Command method that creates a new incident report by calling the incident service.
    /// It also handles error display and navigation after creating an incident.
    /// </summary>
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
                Status = IncidentStatus.Open,
                SensorId = SelectedSensorId,
                Priority = SelectedPriority,
                Comments = Comments,
            };

            var success = await _incidentService.AddIncidentAsync(token, newIncident);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Incident created!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create incident report.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while creating incident: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Failed to create incident report.", "OK");
        }
    }
}
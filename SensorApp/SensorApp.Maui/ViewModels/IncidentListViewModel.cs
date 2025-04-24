using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Services;
using SensorApp.Shared.Interfaces;
using SensorApp.Maui.Views.Pages;

namespace SensorApp.Maui.ViewModels;

public partial class IncidentListViewModel(IIncidentApiService incidentService, ITokenProvider tokenProvider) : BaseViewModel
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IIncidentApiService _incidentService = incidentService;

    [ObservableProperty]
    private ObservableCollection<IncidentDto> incidents = new();

    [RelayCommand]
    public async Task LoadIncidentsAsync()
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                await Shell.Current.DisplayAlert("Error", "You are not logged in or your session has expired. Please log in again.", "OK");
                return;
            }

            var list = await _incidentService.GetAllIncidentsAsync(token);
            incidents.Clear();
            foreach (var incident in list)
            {
                incidents.Add(incident);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Unable to load incidents.", "OK");
        }
    }

    [RelayCommand]
    public async Task GoToCreateIncident()
    {
        await Shell.Current.GoToAsync(nameof(CreateIncidentPage));
    }

    [RelayCommand]
    public async Task GoToIncidentDetailsAsync(IncidentDto selected)
    {
        if (selected == null)
            return;

        var navParam = new Dictionary<string, object>
        {
            { "Incident", selected }
        };

        await Shell.Current.GoToAsync(nameof(IncidentDetailPage), navParam);
    }

}
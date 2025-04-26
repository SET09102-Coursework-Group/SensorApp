using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for managing the detail of a specific incident. 
/// This handles the logic for resolving or deleting an incident and updating the UI accordingly.
/// </summary>
[QueryProperty(nameof(Incident), "Incident")]
public partial class IncidentDetailViewModel(IIncidentApiService incidentService, ITokenProvider tokenProvider) : BaseViewModel
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IIncidentApiService _incidentService = incidentService;

    [ObservableProperty]
    private IncidentDto incident;

    [ObservableProperty]
    private string resolutionComments;

    public bool IsOpen => Incident?.Status != IncidentStatus.Resolved;

    /// <summary>
    /// Method to load the details of a selected incident into the ViewModel.
    /// </summary>
    /// <param name="selectedIncident">The incident to load into the ViewModel.</param>
    public void LoadIncident(IncidentDto selectedIncident)
    {
        Incident = selectedIncident;
    }

    /// <summary>
    /// This partial method is called whenever the incident property changes.
    /// It updates the IsOpen property to reflect the current status of the incident.
    /// </summary>
    /// <param name="value">The updated incident.</param>
    partial void OnIncidentChanged(IncidentDto value)
    {
        OnPropertyChanged(nameof(IsOpen));
    }

    /// <summary>
    /// Command for resolving an incident. It calls the API to resolve the incident
    /// and updates the incident's status to "Resolved".
    /// </summary>
    [RelayCommand]
    public async Task ResolveIncidentAsync()
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            var dto = new IncidentResolutionDto
            {
                ResolutionComments = resolutionComments
            };

            var success = await _incidentService.ResolveIncidentAsync(token, Incident.Id, dto);
            if (success)
            {
                Incident.Status = IncidentStatus.Resolved;
                Incident.Resolution_comments = resolutionComments;
                OnPropertyChanged(nameof(IsOpen));
                OnPropertyChanged(nameof(Incident));
                await Shell.Current.DisplayAlert("Success", "Incident resolved.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to resolve incident.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Something went wrong.", "OK");
        }
    }

    /// <summary>
    /// Command for deleting an incident. It confirms the deletion and calls the API to delete the incident.
    /// </summary>
    [RelayCommand]
    public async Task DeleteIncidentAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this incident?", "Yes", "No");
        if (!confirm)
            return;

        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            var success = await _incidentService.DeleteIncidentAsync(token, Incident.Id);

            if (success)
            {
                await Shell.Current.DisplayAlert("Deleted", "Incident deleted successfully.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to delete incident.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "An unexpected error occurred.", "OK");
        }
    }

}

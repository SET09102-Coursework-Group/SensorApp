using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Incident;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.ViewModels;

[QueryProperty(nameof(Incident), "Incident")]
public partial class IncidentDetailViewModel(IIncidentApiService incidentService, ITokenProvider tokenProvider) : BaseViewModel
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IIncidentApiService _incidentService = incidentService;

    [ObservableProperty]
    private IncidentDto incident;

    [ObservableProperty]
    private string resolutionComments;

    public bool IsOpen => Incident?.Status != "Resolved";

    public void LoadIncident(IncidentDto selectedIncident)
    {
        Incident = selectedIncident;
    }
    partial void OnIncidentChanged(IncidentDto value)
    {
        OnPropertyChanged(nameof(IsOpen));
    }

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
                Incident.Status = "Resolved";
                OnPropertyChanged(nameof(IsOpen));
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

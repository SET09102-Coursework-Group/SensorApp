using CommunityToolkit.Mvvm.ComponentModel;
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Maui.ViewModels;

[QueryProperty(nameof(Incident), "Incident")]
public partial class IncidentDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private IncidentDto incident;

    public void LoadIncident(IncidentDto selectedIncident)
    {
        Incident = selectedIncident;
    }
}

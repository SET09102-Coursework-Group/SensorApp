using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Represents the page where a user can create a new incident.
/// The page is bound to the CreateIncidentViewModel which handles the logic of creating incidents.
/// </summary>
public partial class CreateIncidentPage : ContentPage
{
    /// <summary>
    /// Constructor for initializing the CreateIncidentPage.
    /// The page's BindingContext is set to the provided CreateIncidentViewModel.
    /// </summary>
    /// <param name="vm">The ViewModel that contains the logic for creating incidents.</param>
    public CreateIncidentPage(CreateIncidentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
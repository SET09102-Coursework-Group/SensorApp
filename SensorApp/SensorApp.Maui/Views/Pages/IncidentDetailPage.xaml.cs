using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Represents the page that displays the details of a selected incident.
/// The page is bound to the IncidentDetailViewModel which handles the logic of incident details.
/// </summary>
public partial class IncidentDetailPage : ContentPage
{
    /// <summary>
    /// Constructor for initializing the IncidentDetailPage.
    /// The page's BindingContext is set to the provided IncidentDetailViewModel.
    /// </summary>
    /// <param name="vm">The ViewModel that contains the logic for displaying incident details.</param>
	public IncidentDetailPage(IncidentDetailViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}
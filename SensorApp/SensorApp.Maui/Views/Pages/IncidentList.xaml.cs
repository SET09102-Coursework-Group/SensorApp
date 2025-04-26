using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.ViewModels;
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Represents the page that displays a list of incidents.
/// The page is bound to the IncidentListViewModel which handles the logic of loading and managing incidents.
/// </summary>
public partial class IncidentList : ContentPage
{
    private readonly IncidentListViewModel _viewModel;

    /// <summary>
    /// Constructor for initializing the IncidentList page.
    /// Sets up the BindingContext to the provided IncidentListViewModel.
    /// </summary>
    /// <param name="vm">The ViewModel responsible for managing incidents.</param>
    public IncidentList(IncidentListViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// This method is called when the page appears. It triggers the loading of incidents from the ViewModel.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadIncidentsAsync();
    }   
}
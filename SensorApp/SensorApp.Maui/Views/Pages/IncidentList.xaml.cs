using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.ViewModels;
using SensorApp.Shared.Dtos.Incident;

namespace SensorApp.Maui.Views.Pages;

public partial class IncidentList : ContentPage
{
    private readonly IncidentListViewModel _viewModel;
    public IncidentList(IncidentListViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadIncidentsAsync();
    }   
}
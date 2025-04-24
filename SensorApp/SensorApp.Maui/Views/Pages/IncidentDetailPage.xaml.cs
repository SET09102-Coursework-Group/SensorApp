using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class IncidentDetailPage : ContentPage
{
	public IncidentDetailPage(IncidentDetailViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}
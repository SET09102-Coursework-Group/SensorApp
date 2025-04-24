using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class CreateIncidentPage : ContentPage
{
    public CreateIncidentPage(CreateIncidentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
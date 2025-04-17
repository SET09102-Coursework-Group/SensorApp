using SensorApp.Maui.ViewModels;
using SensorApp.Shared.Services;

namespace SensorApp.Maui.Views.Pages;

public partial class SensorMapPage : ContentPage
{
    public SensorMapPage(SensorApiService sensorService)
    {
        InitializeComponent();
        BindingContext = new SensorMapViewModel(sensorService);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SensorMapViewModel vm)
            await vm.LoadSensors();
    }
}
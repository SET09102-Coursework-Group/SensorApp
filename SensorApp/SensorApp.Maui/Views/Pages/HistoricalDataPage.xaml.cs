using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class HistoricalDataPage : ContentPage
{
    public HistoricalDataPage(HistoricalDataViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HistoricalDataViewModel vm)
            await vm.LoadAsync();
    }
}
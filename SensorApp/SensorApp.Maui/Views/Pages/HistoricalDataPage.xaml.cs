using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page for displaying historical measurement data, charts, and statistics.
/// Binds to <see cref="HistoricalDataViewModel"/> for sensor selection, filtering, and data visualization.
/// </summary>
public partial class HistoricalDataPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HistoricalDataPage"/> class.
    /// </summary>
    /// <param name="vm">The view model responsible for handling historical data operations.</param>
    public HistoricalDataPage(HistoricalDataViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    /// <summary>
    /// Called when the page appears on screen.
    /// Loads available sensor options and measurement data by invoking the view model's methods.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HistoricalDataViewModel vm)
        {
            await vm.LoadSensorOptionsAsync();
            await vm.LoadAsync();
        }
    }
}
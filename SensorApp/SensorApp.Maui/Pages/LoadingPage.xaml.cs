using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Pages;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}
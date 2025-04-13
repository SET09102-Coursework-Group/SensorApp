using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}
using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page displayed while the application checks user authentication status.
/// Binds to <see cref="LoadingPageViewModel"/> which handles token validation and navigation logic.
/// </summary>
public partial class LoadingPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingPage"/> class.
    /// </summary>
    /// <param name="viewModel">The view model responsible for handling authentication checks during the loading phase.</param>
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}
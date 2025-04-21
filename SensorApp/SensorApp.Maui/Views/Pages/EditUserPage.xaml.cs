using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class EditUserPage : ContentPage
{
    public EditUserPage(EditUserViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
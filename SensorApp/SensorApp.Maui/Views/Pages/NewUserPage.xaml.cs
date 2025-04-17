using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class NewUserPage : ContentPage
{
    public NewUserPage(NewUserViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
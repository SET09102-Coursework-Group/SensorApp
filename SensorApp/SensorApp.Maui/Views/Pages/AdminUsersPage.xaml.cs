using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

public partial class AdminUsersPage : ContentPage
{
    private readonly AdminUsersViewModel viewModel;

    public AdminUsersPage(AdminUsersViewModel vm)
    {
        InitializeComponent();
        viewModel = vm;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadUsersAsync();
    }
}
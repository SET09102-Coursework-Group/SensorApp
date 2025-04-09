using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Pages;

public partial class AdminUsersPage : ContentPage
{
    private readonly AdminUsersViewModel viewModel;

    public AdminUsersPage(AdminUsersViewModel vm)
    {
        InitializeComponent();
        this.viewModel = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadUsersAsync();
    }
}
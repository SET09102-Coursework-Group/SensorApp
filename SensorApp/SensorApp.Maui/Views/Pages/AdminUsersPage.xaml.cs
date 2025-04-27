using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page for managing application users from an admin perspective.
/// Displays a list of users, and provides options to create, edit, or delete users.
/// </summary>
public partial class AdminUsersPage : ContentPage
{
    private readonly AdminUsersViewModel viewModel;
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminUsersPage"/> class.
    /// </summary>
    /// <param name="vm">The view model responsible for managing users and actions on this page.</param>
    public AdminUsersPage(AdminUsersViewModel vm)
    {
        InitializeComponent();
        viewModel = vm;
        BindingContext = viewModel;
    }

    /// <summary>
    /// Called automatically when the page appears.
    /// Triggers loading of the user list by calling the view model.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadUsersAsync();
    }
}
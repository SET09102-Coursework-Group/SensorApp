using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page for editing an existing user's details (such as username, email, role, and password).
/// Binds to an instance of <see cref="EditUserViewModel"/> for logic and data operations.
/// </summary>
public partial class EditUserPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditUserPage"/> class.
    /// </summary>
    /// <param name="viewModel">The view model responsible for handling the edit user functionality.</param>
    public EditUserPage(EditUserViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
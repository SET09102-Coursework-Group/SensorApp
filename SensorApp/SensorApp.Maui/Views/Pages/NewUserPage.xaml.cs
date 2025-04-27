using SensorApp.Maui.ViewModels;

namespace SensorApp.Maui.Views.Pages;

/// <summary>
/// Page for creating a new user within the application.
/// Binds to <see cref="NewUserViewModel"/> to handle input validation and user creation logic.
/// </summary>
public partial class NewUserPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NewUserPage"/> class.
    /// </summary>
    /// <param name="vm">The view model responsible for managing new user creation operations.</param>
    public NewUserPage(NewUserViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
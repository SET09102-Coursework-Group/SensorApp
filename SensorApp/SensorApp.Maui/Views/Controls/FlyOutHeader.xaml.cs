namespace SensorApp.Maui.Views.Controls;

/// <summary>
/// FlyOutHeader control that binds and displays
/// the current user's username and role in the navigation menu.
/// </summary>
public partial class FlyOutHeader : StackLayout
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyOutHeader"/> class.
    /// Sets the binding context to the current logged-in user information.
    /// </summary>
    public FlyOutHeader()
    {
        InitializeComponent();

        if (App.UserInfo != null)
        {
            BindingContext = App.UserInfo;
        }
    }
}
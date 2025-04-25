using CommunityToolkit.Mvvm.ComponentModel;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// A base ViewModel class providing common properties for all ViewModels,
/// including title management and loading state tracking.
/// </summary>
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    string title;

    /// <summary>
    /// Indicates whether a loading operation is currently in progress.
    /// Notifies changes to both IsLoading and IsNotLoading properties.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    bool isLoading;


    /// <summary>
    /// Gets a value indicating whether no loading operation is in progress.
    /// Useful for enabling/disabling UI elements based on loading state.
    /// </summary>
    public bool IsNotLoading => !IsLoading;
}
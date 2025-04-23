using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Models;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class HistoricalDataViewModel : BaseViewModel
{
    private readonly IMeasurementService _service;
    private readonly ITokenProvider _tokenProvider;

    public HistoricalDataViewModel(IMeasurementService service, ITokenProvider tokenProvider)
    {
        _service = service;
        _tokenProvider = tokenProvider;

        From = DateTime.Today.AddDays(-7);
        To = DateTime.Today;
    }

    [ObservableProperty]
    DateTime? from;

    [ObservableProperty]
    DateTime? to;

    [ObservableProperty]
    int? selectedSensorId;

    [ObservableProperty]
    int? selectedMeasurandId;

    [ObservableProperty]
    bool isLoading;

    public ObservableCollection<MeasurementModel> Points { get; } = [];

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert("Not Authenticated", "You are not logged in. Please log in to view measurements.", "OK");
                return;
            }

            Points.Clear();
            var list = await _service.GetMeasurementsAsync(sensorId: SelectedSensorId, measurandId: SelectedMeasurandId, from: From, to: To, token: token);

            foreach (var measurement in list)
            {
                Points.Add(measurement);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Could not load data: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
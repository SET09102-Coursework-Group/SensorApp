using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;


public partial class AdminUsersViewModel : BaseViewModel
{
    private readonly AdminService _adminService;
    private readonly ILogger<AdminUsersViewModel> _logger;

    public AdminUsersViewModel(AdminService adminService, ILogger<AdminUsersViewModel> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> users = new();

    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing. Cannot load users.");
                return;
            }

            var list = await _adminService.GetAllUsersAsync(token);
            Users = new ObservableCollection<UserWithRoleDto>(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load users.");
            await Shell.Current.DisplayAlert("Error", "Unable to load users.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task GoToCreateUser()
    {
        await Shell.Current.GoToAsync(nameof(NewUserPage));
    }
}
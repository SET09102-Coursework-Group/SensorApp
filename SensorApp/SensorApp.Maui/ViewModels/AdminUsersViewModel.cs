using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class AdminUsersViewModel(AdminService adminService, ILogger<AdminUsersViewModel> logger) : BaseViewModel
{
    private readonly AdminService _adminService = adminService;
    private readonly ILogger<AdminUsersViewModel> _logger = logger;

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

    [RelayCommand]
    private async Task DeleteUser(UserWithRoleDto user)
    {
        var confirm = await Shell.Current.DisplayAlert("Delete User", $"Are you sure you would like to delete this user '{user.Username}'?", "Yes", "No");

        if (!confirm) return;

        try
        {
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing. Cannot delete user.");
                return;
            }

            var success = await _adminService.DeleteUserAsync(token, user.Id);

            if (success)
            {
                await LoadUsersAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to delete user.", "OK");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user.");
            await Shell.Current.DisplayAlert("Error", "An unexpected error occurred.", "OK");
        }
    }
}
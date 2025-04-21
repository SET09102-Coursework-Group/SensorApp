using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Interfaces;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class AdminUsersViewModel(IAdminService adminService, ILogger<AdminUsersViewModel> logger, ITokenProvider tokenProvider) : BaseViewModel
{
    private readonly IAdminService _adminService = adminService;
    private readonly ILogger<AdminUsersViewModel> _logger = logger;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> users = new();

    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            var token = await _tokenProvider.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing. Cannot load users.");
                await Shell.Current.DisplayAlert("Error", "You are not logged in or your session has expired. Please log in again.", "OK");
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
            var token = await _tokenProvider.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is missing. Cannot delete user.");
                await Shell.Current.DisplayAlert("Error", "You are not logged in or your session has expired. Please log in again.", "OK");
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

    [RelayCommand]
    public async Task EditUser(UserWithRoleDto user)
    {
        await Shell.Current.GoToAsync($"{nameof(EditUserPage)}?UserId={user.Id}");
    }

}
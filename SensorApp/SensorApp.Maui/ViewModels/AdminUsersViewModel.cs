﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Interfaces;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for managing admin users, including loading, creating, editing, and deleting users.
/// </summary>
public partial class AdminUsersViewModel : BaseViewModel
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminUsersViewModel> _logger;
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminUsersViewModel"/> class.
    /// </summary>
    /// <param name="adminService">Service for admin-related operations.</param>
    /// <param name="logger">Logger for recording application events.</param>
    /// <param name="tokenProvider">Provider for retrieving authentication tokens.</param>
    public AdminUsersViewModel(IAdminService adminService, ILogger<AdminUsersViewModel> logger, ITokenProvider tokenProvider)
    {
        _adminService = adminService;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> users = new();

    /// <summary>
    /// Loads all users from the server and populates the Users collection.
    /// Checks for a valid token before making the request.
    /// </summary>
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

    /// <summary>
    /// Navigates to the page for creating a new user.
    /// </summary>
    [RelayCommand]
    public async Task GoToCreateUser()
    {
        await Shell.Current.GoToAsync(nameof(NewUserPage));
    }


    /// <summary>
    /// Deletes a user after confirming with the current admin.
    /// Validates the token before sending the delete request.
    /// Reloads the user list upon successful deletion.
    /// </summary>
    /// <param name="user">The user to delete.</param>
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

    /// <summary>
    /// Navigates to the edit page for the selected user.
    /// Passes the user's ID as a query parameter.
    /// </summary>
    /// <param name="user">The user to edit.</param>

    [RelayCommand]
    public async Task EditUser(UserWithRoleDto user)
    {
        await Shell.Current.GoToAsync($"{nameof(EditUserPage)}?UserId={user.Id}");
    }
}
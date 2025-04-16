using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Shared.Dtos;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// View model for managing the administration of user data.
/// </summary>
public partial class AdminUsersViewModel : BaseViewModel
{
    private readonly AdminService _adminService;
    private readonly ILogger<AdminUsersViewModel> _logger;
    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> _users = new();


    public AdminUsersViewModel(AdminService adminService, ILogger<AdminUsersViewModel> logger)
    {
        this._adminService = adminService;
        this._logger = logger;
        this._users = [];
    }


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

                return;
            }

            var list = await _adminService.GetAllUsersAsync(token);
            Users.Clear();
            foreach (var user in list)
            {
                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading users.");
        }
        finally
        {
            IsLoading = false;
        }
    }

}
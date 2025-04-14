using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SensorApp.Maui.Models;
using SensorApp.Maui.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// View model for managing the administration of user data.
/// </summary>
public partial class AdminUsersViewModel : BaseViewModel
{
    private readonly AdminService adminService;
    private readonly ILogger<AdminUsersViewModel> logger;

    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> users;

    public AdminUsersViewModel(AdminService adminService, ILogger<AdminUsersViewModel> logger)
    {
        this.adminService = adminService;
        this.logger = logger;
        users = new ObservableCollection<UserWithRoleDto>();
    }


    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        if (IsLoading) return;
        IsLoading = true;
        try
        {
            var list = await adminService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in list)
            {
                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while loading users.");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
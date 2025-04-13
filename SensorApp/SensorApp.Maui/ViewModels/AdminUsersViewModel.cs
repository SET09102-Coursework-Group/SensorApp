using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Maui.Models;
using SensorApp.Maui.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class AdminUsersViewModel : BaseViewModel
{
    private readonly AdminService adminService;

    [ObservableProperty]
    private ObservableCollection<UserWithRoleDto> users;

    public AdminUsersViewModel(AdminService adminService)
    {
        this.adminService = adminService;
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
            //TODO add logging - extract on its own class for future use
        }
        finally
        {
            IsLoading = false;
        }
    }
}
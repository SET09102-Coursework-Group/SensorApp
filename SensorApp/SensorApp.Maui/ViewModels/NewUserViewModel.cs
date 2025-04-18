using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Services;
using System.Collections.ObjectModel;

namespace SensorApp.Maui.ViewModels;

public partial class NewUserViewModel : BaseViewModel
{
    private readonly AdminService _adminService;

    public NewUserViewModel(AdminService adminService)
    {
        _adminService = adminService;
        Roles = [.. Enum.GetValues<UserRole>()];
    }

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private UserRole selectedRole;

    public ObservableCollection<UserRole> Roles { get; }

    [RelayCommand]
    public async Task CreateUser()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await SecureStorage.GetAsync("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("No auth token");

            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error while submitting form", "All fields are required.", "OK");
                IsLoading = false;
                return;
            }

            var dto = new CreateUserDto
            {
                Username = Username,
                Email = Email,
                Password = Password,
                Role = SelectedRole
            };

            var success = await _adminService.AddUserAsync(token, dto);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "User created!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create user.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
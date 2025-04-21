using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SensorApp.Maui.ViewModels;

[QueryProperty(nameof(UserId), "UserId")]
public partial class EditUserViewModel : BaseViewModel
{
    private readonly IAdminService _adminService;
    private readonly ITokenProvider _tokenProvider;

    public EditUserViewModel(IAdminService adminService, ITokenProvider tokenProvider)
    {
        _adminService = adminService;
        _tokenProvider = tokenProvider;
        Roles = new ObservableCollection<UserRole>(Enum.GetValues<UserRole>());
    }

    [ObservableProperty]
    string userId;

    [ObservableProperty]
    string username;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    UserRole selectedRole;
  
    public ObservableCollection<UserRole> Roles { get; }

    partial void OnUserIdChanged(string value) => _ = LoadUserAsync();

    private async Task LoadUserAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            await Shell.Current.DisplayAlert("Error", "You are not logged in or your session has expired. Please log in again.", "OK");
            IsLoading = false;
            return;
        }
        var currentUserList = await _adminService.GetAllUsersAsync(token);
        var currentUser = currentUserList.FirstOrDefault(u => u.Username == Username && u.Id == UserId);
        var loggedInUser = currentUserList.FirstOrDefault(u => token.Contains(u.Id)); // crude check – you can decode JWT instead

        if (loggedInUser?.Id == UserId)
        {
            await Shell.Current.DisplayAlert("Error", "You cannot edit your own account.", "OK");
            return;
        }

        var dto = await _adminService.GetUserByIdAsync(token, UserId);
        if (dto != null)
        {
            Username = dto.Username;
            Email = dto.Email;
            SelectedRole = Enum.Parse<UserRole>(dto.Role);
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "There was a problem processing your request", "OK");
            await Shell.Current.GoToAsync("..");
        }

        IsLoading = false;
    }


    [RelayCommand]
    private async Task SaveChanges()
    {
        var token = await _tokenProvider.GetTokenAsync();

        if (IsLoading) return;
        IsLoading = true;

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlert("Error", "Username and Email fields must be populated.", "OK");
            IsLoading = false;
            return;
        }

        try
        {
            _ = new MailAddress(Email);
        }
        catch
        {
            await Shell.Current.DisplayAlert("Invalid email format", "Please enter a valid email address.", "OK");
            IsLoading = false;
            return;
        }

        if (!string.IsNullOrWhiteSpace(Password))
        {
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$";
            if (!Regex.IsMatch(Password, passwordPattern))
            {
                await Shell.Current.DisplayAlert(
                    "Weak Password",
                    "Password must be at least 8 characters long and include uppercase, lowercase, a number, and a special character.",
                    "OK");
                IsLoading = false;
                return;
            }
        }

        var updateDto = new UpdateUserDto
        {
            Username = Username,
            Email = Email,
            Role = SelectedRole,
            Password = string.IsNullOrWhiteSpace(Password) ? null : Password
        };

        var success = await _adminService.UpdateUserAsync(token!, UserId, updateDto);
        if (success)
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to update user.", "OK");
        }

        IsLoading = false;
    }
}
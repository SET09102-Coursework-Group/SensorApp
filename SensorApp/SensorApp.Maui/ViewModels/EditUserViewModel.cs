using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SensorApp.Maui.ViewModels;

/// <summary>
/// ViewModel for editing an existing user's details, including username, email, role, and password.
/// </summary>
[QueryProperty(nameof(UserId), "UserId")]
public partial class EditUserViewModel : BaseViewModel
{
    private readonly IAdminService _adminService;
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditUserViewModel"/> class.
    /// </summary>
    /// <param name="adminService">Service to perform admin operations like user management.</param>
    /// <param name="tokenProvider">Provider to retrieve the current authentication token.</param>
    public EditUserViewModel(IAdminService adminService, ITokenProvider tokenProvider)
    {
        _adminService = adminService;
        _tokenProvider = tokenProvider;
        Roles = [.. Enum.GetValues<UserRole>()];
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
    private string? _currentUserId;


    /// <summary>
    /// Called automatically when <see cref="UserId"/> changes.
    /// Triggers loading of the user's existing details.
    /// </summary>
    /// <param name="value">The new UserId value.</param>
    partial void OnUserIdChanged(string value) => _ = LoadUserAsync();

    /// <summary>
    /// Loads user details for editing.
    /// Prevents editing the currently logged-in user's own account.
    /// </summary>
    private async Task LoadUserAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await CheckValidTokenAsync();
            if (token == null) return;

            var userList = await _adminService.GetAllUsersAsync(token);
            var jwtUser = userList.FirstOrDefault(u => token.Contains(u.Id));
            _currentUserId = jwtUser?.Id;

            if (_currentUserId == UserId)
            {
                await Shell.Current.DisplayAlert("Error", "You cannot edit your own account.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            var dto = await _adminService.GetUserByIdAsync(token, UserId);
            if (dto != null)
            {
                Username = dto.Username;
                Email = dto.Email;
                SelectedRole = dto.Role;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "There was a problem processing your request", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }


    /// <summary>
    /// Saves changes made to the user's details.
    /// Validates required fields and performs basic email/password checks before sending the update.
    /// </summary>
    [RelayCommand]
    private async Task SaveChanges()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await CheckValidTokenAsync();
            if (token == null) return;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email))
            {
                await Shell.Current.DisplayAlert("Error", "Username and Email fields must be populated.", "OK");
                return;
            }

            if (!IsValidUsername(Username))
            {
                await Shell.Current.DisplayAlert("Invalid Username", "Username must start with a letter.", "OK");
                return;
            }

            try
            {
                _ = new MailAddress(Email);
            }
            catch
            {
                await Shell.Current.DisplayAlert("Invalid email format", "Please enter a valid email address.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                var validationPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$";
                var regex = new Regex(validationPattern, RegexOptions.None, TimeSpan.FromSeconds(1));

                if (!regex.IsMatch(Password))
                {
                    await Shell.Current.DisplayAlert(
                        "Weak Password",
                        "Password must be at least 8 characters long and include uppercase, lowercase, a number, and a special character.",
                        "OK");
                    return;
                }
            }

            var updateDto = new UpdateUserDto
            {
                Username = string.IsNullOrWhiteSpace(Username) ? null : Username.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
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
        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.Conflict)
        {
            await Shell.Current.DisplayAlert("Error","Username or email is already in use.","OK");
        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.BadRequest)
        {
            await Shell.Current.DisplayAlert("Invalid request", "Something went wrong while updating the user. Please review the fields and try again.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates and retrieves the current user's authentication token.
    /// Displays an error if the token is missing or invalid.
    /// </summary>
    /// <returns>The valid token string if available; otherwise, null.</returns>
    private async Task<string?> CheckValidTokenAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            await Shell.Current.DisplayAlert("Error", "You are not logged in or your session has expired. Please log in again.", "OK");
            return null;
        }

        return token;
    }

    /// <summary>
    /// Validates that the username starts with a letter.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    private bool IsValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        return char.IsLetter(username[0]);
    }
}
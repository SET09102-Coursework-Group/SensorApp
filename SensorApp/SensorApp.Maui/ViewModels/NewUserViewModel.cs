using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SensorApp.Shared.Dtos.Admin;
using SensorApp.Shared.Enums;
using SensorApp.Shared.Interfaces;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SensorApp.Maui.ViewModels;

public partial class NewUserViewModel : BaseViewModel
{
    private readonly IAdminService _adminService;
    private readonly ITokenProvider _tokenProvider;

    public NewUserViewModel(IAdminService adminService, ITokenProvider tokenProvider)
    {
        _adminService = adminService;
        _tokenProvider = tokenProvider;
        Roles = [.. Enum.GetValues<UserRole>()];
    }

    [ObservableProperty]
    private string? username;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private UserRole selectedRole;

    public ObservableCollection<UserRole> Roles { get; } = [.. Enum.GetValues<UserRole>()];

    /// <summary>
    /// Command to create a new user.
    /// Validates fields, checks email format and password strength,
    /// and submits the creation request to the server.
    /// </summary>
    [RelayCommand]
    public async Task CreateUser()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Authentication token could not be retrieved.");
            }

            if (!AreFieldsFilled())
            {
                await DisplayAlertAsync("Error while submitting form", "All fields are required.");
                return;
            }

            if (!IsValidEmail(Email))
            {
                await DisplayAlertAsync("Invalid email format", "Please enter a valid email address.");
                return;
            }

            if (!IsValidPassword(Password))
            {
                await DisplayAlertAsync(
                    "Weak Password",
                    "Password must be at least 8 characters long and include uppercase, lowercase, a number, and a special character."
                );
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
                await DisplayAlertAsync("Success", "User created!");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlertAsync("Error", "Failed to create user.");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates if required fields are filled.
    /// </summary>
    private bool AreFieldsFilled()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
    }

    /// <summary>
    /// Validates the email format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email format is valid; otherwise, false.</returns>
    private bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates the password strength.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password meets strength requirements; otherwise, false.</returns>
    private bool IsValidPassword(string password)
    {
        const string validationPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$";
        var regex = new Regex(validationPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        return regex.IsMatch(password);
    }

    /// <summary>
    /// Displays an alert message.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message content.</param>
    private async Task DisplayAlertAsync(string title, string message)
    {
        await Shell.Current.DisplayAlert(title, message, "OK");
    }
}
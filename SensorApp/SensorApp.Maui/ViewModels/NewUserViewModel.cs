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
    private string username;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private UserRole selectedRole;

    public ObservableCollection<UserRole> Roles { get; } = [.. Enum.GetValues<UserRole>()];

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
                throw new Exception("No auth token");

            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error while submitting form", "All fields are required.", "OK");
                IsLoading = false;
                return;
            }

            try
            {
                var emailAddress = new MailAddress(Email);
                if (emailAddress.Address != Email)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Invalid email format", "Please enter a valid email address.", "OK");
                IsLoading = false;
                return;
            }

            //This is taken from StackOverflow: https://stackoverflow.com/questions/48635152/regex-for-default-asp-net-core-identity-password
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
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Defines a contract for building the application's navigation menu dynamically based on user role.
/// Typically called after user authentication is validated.
/// </summary>
public interface IMenuBuilder
{
    /// <summary>
    /// Generates and configures the application's navigation menu according to the authenticated user's role.
    /// This method is called after login or token validation in the <see cref="LoginViewModel"/> or <see cref="LoadingPageViewModel"/>.
    /// </summary>
    /// <param name="userInfo">Information about the authenticated user, including role and username.</param>
    void BuildMenu(UserInfo userInfo);
}

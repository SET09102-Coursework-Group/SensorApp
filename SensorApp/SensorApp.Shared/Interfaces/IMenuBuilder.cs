using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface IMenuBuilder
{
    /// <summary>
    /// This is the main entry point for menu generation. Called in LoginViewModel or LoadingPageViewModel after login/token validation.
    /// </summary>
    /// <param name="userInfo">The authenticated user's info, including role and username.</param>
    void BuildMenu(UserInfo userInfo);
}

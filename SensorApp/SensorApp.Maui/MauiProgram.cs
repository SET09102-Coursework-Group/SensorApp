using Microsoft.Extensions.Configuration;
using SensorApp.Maui.Extensions;
using SensorApp.Maui.Helpers.Map;
using SensorApp.Maui.Helpers.MenuRoles;
using SensorApp.Maui.Interfaces;
using SensorApp.Maui.Services;
using SensorApp.Maui.ViewModels;
using SensorApp.Maui.Views.Pages;
using SensorApp.Shared.Factories;
using SensorApp.Shared.Interfaces;
using SensorApp.Shared.Services;

namespace SensorApp.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                             .AddEnvironmentVariables();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiMaps();


        builder.Services.AddHttpClient<IAuthService, AuthService>()
            .ConfigureApiHttpClient()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });
        builder.Services.AddHttpClient<SensorApiService>().ConfigureApiHttpClient();

        builder.Services.AddHttpClient<IAdminService, AdminService>().ConfigureApiHttpClient();
        builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
        builder.Services.AddSingleton<IMenuBuilder, MenuBuilder>();
        builder.Services.AddSingleton<ISensorAnalysisService, SensorAnalysisService>();
        builder.Services.AddSingleton<ISensorPinFactory, SensorPinFactory>();
        builder.Services.AddSingleton<ISensorPinInfoFactory, SensorPinInfoFactory>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LogoutPage>();
        builder.Services.AddTransient<AdminUsersPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddTransient<SensorMapPage>();

        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LogoutViewModel>();
        builder.Services.AddTransient<AdminUsersViewModel>();

        builder.Services.AddTransient<NewUserPage>();
        builder.Services.AddTransient<NewUserViewModel>();
        builder.Services.AddTransient<EditUserPage>();
        builder.Services.AddTransient<EditUserViewModel>();

        return builder.Build();
    }
}

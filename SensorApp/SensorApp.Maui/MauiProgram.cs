using Microsoft.Extensions.Configuration;
using SensorApp.Maui.Extensions;
using SensorApp.Maui.Services;
using SensorApp.Maui.ViewModels;
using SensorApp.Maui.Views.Pages;
using System.Reflection;

namespace SensorApp.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("SensorApp.Maui.appsettings.Development.json");
        var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();

        builder.Configuration.AddConfiguration(config);
        builder.Services.AddSingleton<IConfiguration>(config);

        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient<AuthService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<AdminService>().ConfigureApiHttpClient();

        builder.Services.AddSingleton<TokenService>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LogoutPage>();
        builder.Services.AddTransient<AdminUsersPage>();
        builder.Services.AddSingleton<LoadingPage>();

        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LogoutViewModel>();
        builder.Services.AddTransient<AdminUsersViewModel>();

        return builder.Build();
    }
}

using Microcharts.Maui;
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
            })
            .UseMauiMaps().UseMicrocharts();


        builder.Services.AddHttpClient<IAuthService, AuthService>()
            .ConfigureApiHttpClient()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });

        builder.Services.AddHttpClient<SensorApiService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IAdminService, AdminService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IMeasurandService, MeasurandService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IMeasurementService, MeasurementService>().ConfigureApiHttpClient();

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
        builder.Services.AddTransient<NewUserPage>();
        builder.Services.AddTransient<EditUserPage>();
        builder.Services.AddTransient<HistoricalDataPage>();


        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LogoutViewModel>();
        builder.Services.AddTransient<AdminUsersViewModel>();
        builder.Services.AddTransient<NewUserViewModel>();
        builder.Services.AddTransient<EditUserViewModel>();
        builder.Services.AddTransient<HistoricalDataViewModel>();
        builder.Services.AddSingleton<Utils.NullableIntConverter>();

        return builder.Build();
    }
}

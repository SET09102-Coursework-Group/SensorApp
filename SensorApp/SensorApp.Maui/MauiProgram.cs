using Microcharts.Maui;
using Microsoft.Extensions.Configuration;
using SensorApp.Maui.Extensions;
using SensorApp.Maui.Helpers.Charting;
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

        var asm = Assembly.GetExecutingAssembly();
        var baseStream = asm.GetManifestResourceStream("SensorApp.Maui.appsettings.json") ?? throw new InvalidOperationException("Missing appsettings.json embedded resource");
        builder.Configuration.AddJsonStream(baseStream);


        if (asm.GetManifestResourceNames()
               .Contains("SensorApp.Maui.appsettings.Development.json"))
        {
            using var devStream = asm.GetManifestResourceStream("SensorApp.Maui.appsettings.Development.json")!;
            builder.Configuration.AddJsonStream(devStream);
        }

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiMaps().UseMicrocharts();


        //check this is possible error 
        builder.Services.AddHttpClient<IAuthService, AuthService>().ConfigureApiHttpClient();

        builder.Services.AddHttpClient<SensorApiService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IAdminService, AdminService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IMeasurandService, MeasurandService>().ConfigureApiHttpClient();
        builder.Services.AddHttpClient<IMeasurementService, MeasurementService>().ConfigureApiHttpClient();

        builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
        builder.Services.AddSingleton<IMenuBuilder, MenuBuilder>();
        builder.Services.AddSingleton<ISensorAnalysisService, SensorAnalysisService>();
        builder.Services.AddSingleton<ISensorPinFactory, SensorPinFactory>();
        builder.Services.AddSingleton<ISensorPinInfoFactory, SensorPinInfoFactory>();
        builder.Services.AddSingleton<IChartFactory, MicrochartsChartFactory>();

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

        return builder.Build();
    }
}

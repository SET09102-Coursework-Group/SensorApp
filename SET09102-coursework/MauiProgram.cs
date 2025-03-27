using Microsoft.Extensions.Logging;
using SET09102_coursework.Database.Data;

namespace SET09102_coursework;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddDbContext<CourseworkDbContext>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

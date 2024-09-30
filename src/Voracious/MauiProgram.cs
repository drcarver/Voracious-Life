using System.Reflection;

using CommunityToolkit.Maui;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Voracious;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services
            .AddMauiBlazorWebView();

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

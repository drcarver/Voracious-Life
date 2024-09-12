using CommunityToolkit.Maui;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Syncfusion.Maui.Core.Hosting;

using Voracious.Core;
using Voracious.Database;
using Voracious.Reader.Extensions;

namespace Voracious.Reader;

public static class MauiProgram
{
    static ILogger logger;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Add global exception handlers
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Voracious");
        Directory.CreateDirectory(folder);
        Directory.SetCurrentDirectory(folder);

        // Add the file logger extension
        builder.Logging.AddFileLogger(options =>
        {
            options.LogLevel = LogLevel.Information;
            options.LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Voracious", "Voracious.log");
        });

        var lgf = GetServiceFromBuilder<ILoggerFactory>(builder);
        logger = lgf.CreateLogger<MauiApp>();

        logger.LogInformation("Added global exception handler");
        logger.LogInformation("Created application directory");
        logger.LogInformation("Added file logger extension");

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .UseVoraciousCore()
            .UseVoraciousDatabase()
            .UseVoraciousReader();

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //load base settings
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .Build();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Configuration
            .AddJsonFile($"appsettings.dev.json", optional: true); //load environment settings
#endif

        return builder.Build();
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogUnhandledException((Exception) e.ExceptionObject, nameof(OnUnobservedTaskException));
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogUnhandledException(e.Exception, nameof(OnUnobservedTaskException));
        e.SetObserved(); // Prevents the application from crashing due to an unobserved exception
    }

    // Log exception
    public static void LogUnhandledException(Exception exception, string source)
    {
        if (exception != null)
        {
            if (logger != null)
            {
                // You can log to a file, telemetry, etc. Here we use Console.
                logger.LogError($"Unhandled exception in {source}: {exception.Message}");
                logger.LogError(exception.StackTrace);
            }

            if (logger == null)
            {
                // You can log to a file, telemetry, etc. Here we use Console.
                Console.WriteLine($"Unhandled exception in {source}: {exception.Message}");
                Console.WriteLine(exception.StackTrace);
            }
        }
    }

    /// <summary>
    /// Get a service from the builder.
    /// </summary>
    /// <typeparam name="T">The service to get</typeparam>
    /// <param name="builder">The builder for the application</param>
    /// <returns>The service requested</returns>
    private static T GetServiceFromBuilder<T>(MauiAppBuilder builder)
    {
        // Create a temporary service provider to resolve the service
        var serviceProvider = builder.Services.BuildServiceProvider();

        return serviceProvider.GetRequiredService<T>();
    }
}

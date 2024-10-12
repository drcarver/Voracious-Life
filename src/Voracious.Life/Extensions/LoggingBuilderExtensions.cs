using Microsoft.Extensions.Logging;

namespace Voracious.Life.Extensions;

// Extension method to add a file logger
public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
    {
        var options = new FileLoggerOptions();
        configure(options);

        builder.AddProvider(new FileLoggerProvider(options));
        return builder;
    }
}

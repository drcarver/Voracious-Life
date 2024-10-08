using Microsoft.Extensions.Logging;

namespace Voracious.Life.Extensions;

// Logger implementation
public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly FileLoggerOptions _options;

    public FileLogger(string categoryName, FileLoggerOptions options)
    {
        _categoryName = categoryName;
        _options = options;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.LogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = $"{DateTime.Now}: [{logLevel}] {_categoryName} - {formatter(state, exception)}{Environment.NewLine}";
        File.AppendAllText(_options.LogFilePath, message);
    }
}

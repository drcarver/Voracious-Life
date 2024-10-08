using Microsoft.Extensions.Logging;

namespace Voracious.Life.Extensions;

// Logger Provider
public class FileLoggerProvider : ILoggerProvider
{
    private readonly FileLoggerOptions _options;

    public FileLoggerProvider(FileLoggerOptions options)
    {
        _options = options;
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _options);

    public void Dispose() { }
}

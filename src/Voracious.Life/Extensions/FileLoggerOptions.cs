﻿using Microsoft.Extensions.Logging;

namespace Voracious.Life.Extensions;

// Logger Options
public class FileLoggerOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public string LogFilePath { get; set; } = "log.txt";
}

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Il2CppStudio.Core.Diagnostics;

public sealed record LoggingOptions
{
    /// <summary>Minimum level written to the console (stderr, so stdout stays clean for JSON output).</summary>
    public LogEventLevel ConsoleLevel { get; init; } = LogEventLevel.Information;

    /// <summary>Minimum level written to the structured log file.</summary>
    public LogEventLevel FileLevel { get; init; } = LogEventLevel.Verbose;

    /// <summary>Directory for rolling CLEF (compact JSON) log files. Null disables file logging.</summary>
    public string? LogDirectory { get; init; } = DefaultLogDirectory;

    public bool EnableConsole { get; init; } = true;

    public static string DefaultLogDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Il2CppStudio", "logs");
}

public static class LoggingSetup
{
    /// <summary>
    /// Builds the process-wide Serilog logger. Console output goes to stderr; the file sink writes
    /// Compact Log Event Format (one JSON object per line), filterable with tools like `clef-tool` or Seq.
    /// </summary>
    public static Logger CreateLogger(LoggingOptions options, ILogEventSink? extraSink = null)
    {
        var cfg = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("App", "Il2CppStudio")
            .Enrich.WithProperty("ProcessId", Environment.ProcessId);

        if (options.EnableConsole)
        {
            cfg = cfg.WriteTo.Console(
                restrictedToMinimumLevel: options.ConsoleLevel,
                standardErrorFromLevel: LogEventLevel.Verbose,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        }

        if (options.LogDirectory is { } dir)
        {
            Directory.CreateDirectory(dir);
            cfg = cfg.WriteTo.File(
                new CompactJsonFormatter(),
                Path.Combine(dir, "il2cppstudio-.clef"),
                restrictedToMinimumLevel: options.FileLevel,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                shared: true);
        }

        if (extraSink is not null)
            cfg = cfg.WriteTo.Sink(extraSink);

        return cfg.CreateLogger();
    }
}

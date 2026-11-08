using System.Collections.Concurrent;
using Serilog;
using Serilog.Events;

namespace Il2CppStudio.Core.Diagnostics;

/// <summary>Anything that can receive diagnostics.</summary>
public interface IDiagnosticSink
{
    void Report(Diagnostic diagnostic);
}

/// <summary>
/// Thread-safe collector that keeps every diagnostic in memory (for the GUI dock and JSON reports)
/// and forwards it to Serilog with full structured context. Nothing is ever dropped silently.
/// </summary>
public sealed class DiagnosticCollector : IDiagnosticSink
{
    private readonly ConcurrentQueue<Diagnostic> _items = new();
    private readonly ILogger _logger;
    private readonly int[] _counts = new int[Enum.GetValues<DiagnosticSeverity>().Length];

    public DiagnosticCollector(ILogger? logger = null)
    {
        _logger = (logger ?? Log.Logger).ForContext("SourceContext", "Il2CppStudio");
    }

    /// <summary>Raised for every diagnostic (on the reporting thread). GUI marshals to UI thread.</summary>
    public event Action<Diagnostic>? DiagnosticReported;

    public int Count => _items.Count;

    public int CountOf(DiagnosticSeverity severity) => Volatile.Read(ref _counts[(int)severity]);

    public bool HasErrors => CountOf(DiagnosticSeverity.Error) > 0 || CountOf(DiagnosticSeverity.Fatal) > 0;

    public IReadOnlyList<Diagnostic> Snapshot() => [.. _items];

    public IReadOnlyList<Diagnostic> Query(DiagnosticSeverity minimum, DiagnosticCategory? category = null) =>
        [.. _items.Where(d => d.Severity >= minimum && (category is null || d.Category == category))];

    public void Report(Diagnostic diagnostic)
    {
        _items.Enqueue(diagnostic);
        Interlocked.Increment(ref _counts[(int)diagnostic.Severity]);

        var log = _logger
            .ForContext("Category", diagnostic.Category)
            .ForContext("Code", diagnostic.Code);
        if (diagnostic.Location is { } loc)
            log = log.ForContext("Location", loc, destructureObjects: true);
        if (diagnostic.ExceptionText is { } exText)
            log = log.ForContext("ExceptionType", diagnostic.ExceptionType).ForContext("ExceptionText", exText);

        // The message is passed as a property, never as a template, so user/game strings containing
        // braces can never break or inject into Serilog's message template parser.
        log.Write(ToSerilog(diagnostic.Severity), "{Code} [{Category}] {DiagnosticMessage}",
            diagnostic.Code, diagnostic.Category, diagnostic.Message);

        DiagnosticReported?.Invoke(diagnostic);
    }

    internal static LogEventLevel ToSerilog(DiagnosticSeverity s) => s switch
    {
        DiagnosticSeverity.Verbose => LogEventLevel.Verbose,
        DiagnosticSeverity.Debug => LogEventLevel.Debug,
        DiagnosticSeverity.Information => LogEventLevel.Information,
        DiagnosticSeverity.Warning => LogEventLevel.Warning,
        DiagnosticSeverity.Error => LogEventLevel.Error,
        _ => LogEventLevel.Fatal,
    };
}

/// <summary>Ergonomic helpers so call-sites stay one line long.</summary>
public static class DiagnosticSinkExtensions
{
    public static void Report(this IDiagnosticSink sink, DiagnosticSeverity severity, DiagnosticCategory category,
        string code, string message, DiagnosticLocation? location = null, Exception? exception = null) =>
        sink.Report(new Diagnostic(DateTimeOffset.UtcNow, severity, category, code, message, location,
            exception?.GetType().FullName, exception?.ToString()));

    public static void Verbose(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null) =>
        s.Report(DiagnosticSeverity.Verbose, c, code, msg, l);

    public static void Debug(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null) =>
        s.Report(DiagnosticSeverity.Debug, c, code, msg, l);

    public static void Info(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null) =>
        s.Report(DiagnosticSeverity.Information, c, code, msg, l);

    public static void Warn(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null, Exception? ex = null) =>
        s.Report(DiagnosticSeverity.Warning, c, code, msg, l, ex);

    public static void Error(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null, Exception? ex = null) =>
        s.Report(DiagnosticSeverity.Error, c, code, msg, l, ex);

    public static void Fatal(this IDiagnosticSink s, DiagnosticCategory c, string code, string msg, DiagnosticLocation? l = null, Exception? ex = null) =>
        s.Report(DiagnosticSeverity.Fatal, c, code, msg, l, ex);
}

/// <summary>A sink that discards everything (for unit tests / probing code paths).</summary>
public sealed class NullDiagnosticSink : IDiagnosticSink
{
    public static readonly NullDiagnosticSink Instance = new();
    public void Report(Diagnostic diagnostic) { }
}

using System.Globalization;
using System.Text;

namespace Il2CppStudio.Core.Diagnostics;

public enum RuntimeLogLevel
{
    Warning,
    Error,
    FirstChance,
}

public enum RuntimeLogFilter
{
    All,
    Errors,
    Warnings,
    FirstChance,
}

public sealed class RuntimeLogEntry
{
    public DateTimeOffset FirstSeen { get; init; }
    public DateTimeOffset LastSeen { get; set; }
    public RuntimeLogLevel Level { get; init; }
    public string Source { get; init; } = "";
    public string ExceptionType { get; init; } = "";
    public string TargetSite { get; init; } = "";
    public string Message { get; set; } = "";
    public string StackSummary { get; set; } = "";
    public int HitCount { get; set; } = 1;

    public string SummaryLine
    {
        get
        {
            var time = LastSeen.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            var tag = HitCount > 1 ? $"{Level} ×{HitCount}" : Level.ToString();
            if (!string.IsNullOrEmpty(ExceptionType))
            {
                var site = string.IsNullOrEmpty(TargetSite) ? Source : TargetSite;
                return string.IsNullOrEmpty(site)
                    ? $"[{time}] [{tag}] {ExceptionType}: {Message}"
                    : $"[{time}] [{tag}] {ExceptionType} @ {site}: {Message}";
            }
            return string.IsNullOrEmpty(Source)
                ? $"[{time}] [{tag}] {Message}"
                : $"[{time}] [{tag}] [{Source}] {Message}";
        }
    }

    public override string ToString() => SummaryLine;
}

/// <summary>
/// Thread-safe bounded ring buffer and optional daily rotating file logger for internal warnings,
/// errors, unhandled exceptions, unobserved task exceptions, and toggleable deduplicated first-chance exceptions.
/// </summary>
public sealed class RuntimeDiagnosticsLog : IDisposable
{
    public const int DefaultCapacity = 500;

    [ThreadStatic]
    private static bool _inFirstChance;

    private readonly int _capacity;
    private readonly Lock _gate = new();
    private readonly LinkedList<RuntimeLogEntry> _entries = [];
    private readonly Dictionary<(string ExceptionType, string TargetSite), LinkedListNode<RuntimeLogEntry>> _firstChanceIndex = [];
    private readonly object _fileGate = new();

    private StreamWriter? _fileWriter;
    private DateTime _currentLogDate;
    private bool _isFileLoggingEnabled;
    private string _logDirectory;

    private long _warningCount;
    private long _errorCount;
    private long _firstChanceCount;
    private bool _globalHooksInstalled;
    private bool _captureFirstChance;
    private bool _disposed;

    public RuntimeDiagnosticsLog(int capacity = DefaultCapacity, string? logDirectory = null)
    {
        _capacity = Math.Max(16, capacity);
        _logDirectory = string.IsNullOrWhiteSpace(logDirectory) ? DefaultLogDirectory : logDirectory;
        if (Environment.GetEnvironmentVariable("IL2CPPSTUDIO_CAPTURE_FIRST_CHANCE") == "1")
            CaptureFirstChanceExceptions = true;
    }

    public static string DefaultLogDirectory
    {
        get
        {
            var baseDir = OperatingSystem.IsAndroid()
                ? TryGetFolder(Environment.SpecialFolder.LocalApplicationData)
                  ?? TryGetFolder(Environment.SpecialFolder.ApplicationData)
                : TryGetFolder(Environment.SpecialFolder.ApplicationData)
                  ?? TryGetFolder(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrWhiteSpace(baseDir))
                baseDir = Path.GetTempPath();
            return Path.Combine(baseDir, "Il2CppStudio", "perf-logs");
        }
    }

    private static string? TryGetFolder(Environment.SpecialFolder folder)
    {
        try
        {
            var path = Environment.GetFolderPath(folder);
            return string.IsNullOrWhiteSpace(path) ? null : path;
        }
        catch
        {
            return null;
        }
    }

    public long WarningCount => Volatile.Read(ref _warningCount);
    public long ErrorCount => Volatile.Read(ref _errorCount);
    public long FirstChanceExceptionCount => Volatile.Read(ref _firstChanceCount);
    public bool IsFileLoggingEnabled => Volatile.Read(ref _isFileLoggingEnabled);
    public string LogDirectory => _logDirectory;

    public event Action? Changed;

    public bool CaptureFirstChanceExceptions
    {
        get => Volatile.Read(ref _captureFirstChance);
        set
        {
            lock (_gate)
            {
                if (_disposed || _captureFirstChance == value) return;
                _captureFirstChance = value;
                if (value)
                    AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
                else
                    AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;
            }
        }
    }

    public void InstallGlobalHooks()
    {
        lock (_gate)
        {
            if (_disposed || _globalHooksInstalled) return;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            _globalHooksInstalled = true;
        }
    }

    public void UninstallGlobalHooks()
    {
        lock (_gate)
        {
            if (!_globalHooksInstalled) return;
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
            _globalHooksInstalled = false;
        }
    }

    public void StartFileLogging(string? directory = null)
    {
        lock (_fileGate)
        {
            if (_disposed) return;
            if (!string.IsNullOrWhiteSpace(directory))
                _logDirectory = directory;
            Directory.CreateDirectory(_logDirectory);
            _isFileLoggingEnabled = true;
            EnsureWriterOpenLocked(DateTime.UtcNow.Date);
        }
    }

    public void StopFileLogging()
    {
        lock (_fileGate)
        {
            _isFileLoggingEnabled = false;
            CloseWriterLocked();
        }
    }

    public void Flush()
    {
        lock (_fileGate)
        {
            _fileWriter?.Flush();
        }
    }

    public void RecordWarning(string source, string message, Exception? ex = null)
    {
        Interlocked.Increment(ref _warningCount);
        AddEntry(RuntimeLogLevel.Warning, source, ex?.GetType().FullName ?? "", FormatTargetSite(ex), message, SummarizeStack(ex));
    }

    public void RecordError(string source, string message, Exception? ex = null)
    {
        Interlocked.Increment(ref _errorCount);
        AddEntry(RuntimeLogLevel.Error, source, ex?.GetType().FullName ?? "", FormatTargetSite(ex), message, SummarizeStack(ex));
    }

    public void RecordFromDiagnostic(Diagnostic d)
    {
        if (d.Severity == DiagnosticSeverity.Warning)
        {
            Interlocked.Increment(ref _warningCount);
            AddEntry(RuntimeLogLevel.Warning, d.Category.ToString(), d.ExceptionType ?? "", d.Code, d.Message, Truncate(d.ExceptionText, 600));
        }
        else if (d.Severity >= DiagnosticSeverity.Error)
        {
            Interlocked.Increment(ref _errorCount);
            AddEntry(RuntimeLogLevel.Error, d.Category.ToString(), d.ExceptionType ?? "", d.Code, d.Message, Truncate(d.ExceptionText, 600));
        }
    }

    public IReadOnlyList<RuntimeLogEntry> Snapshot(RuntimeLogFilter filter = RuntimeLogFilter.All)
    {
        lock (_gate)
        {
            var list = new List<RuntimeLogEntry>(_entries.Count);
            foreach (var e in _entries)
            {
                if (!MatchesFilter(e.Level, filter)) continue;
                list.Add(new RuntimeLogEntry
                {
                    FirstSeen = e.FirstSeen,
                    LastSeen = e.LastSeen,
                    Level = e.Level,
                    Source = e.Source,
                    ExceptionType = e.ExceptionType,
                    TargetSite = e.TargetSite,
                    Message = e.Message,
                    StackSummary = e.StackSummary,
                    HitCount = e.HitCount,
                });
            }
            return list;
        }
    }

    public IReadOnlyList<(string ExceptionType, string TargetSite, int HitCount, string SampleMessage)> GetTopFirstChanceHotspots(int maxCount = 10)
    {
        lock (_gate)
        {
            return _entries
                .Where(e => e.Level == RuntimeLogLevel.FirstChance)
                .OrderByDescending(e => e.HitCount)
                .Take(Math.Max(1, maxCount))
                .Select(e => (e.ExceptionType, e.TargetSite, e.HitCount, e.Message))
                .ToList();
        }
    }

    public string FormatText(RuntimeLogFilter filter = RuntimeLogFilter.All)
    {
        var items = Snapshot(filter);
        var sb = new StringBuilder();
        sb.AppendLine($"Il2CppStudio Runtime Log (Warnings={WarningCount}, Errors={ErrorCount}, FirstChance={FirstChanceExceptionCount})");
        foreach (var item in items)
        {
            sb.AppendLine(item.SummaryLine);
            if (!string.IsNullOrWhiteSpace(item.StackSummary))
                sb.AppendLine("  " + item.StackSummary.Replace("\n", "\n  ", StringComparison.Ordinal));
        }
        return sb.ToString();
    }

    public void Clear()
    {
        lock (_gate)
        {
            _entries.Clear();
            _firstChanceIndex.Clear();
            Interlocked.Exchange(ref _warningCount, 0);
            Interlocked.Exchange(ref _errorCount, 0);
            Interlocked.Exchange(ref _firstChanceCount, 0);
        }
        Changed?.Invoke();
    }

    private static bool MatchesFilter(RuntimeLogLevel level, RuntimeLogFilter filter) => filter switch
    {
        RuntimeLogFilter.Errors => level == RuntimeLogLevel.Error,
        RuntimeLogFilter.Warnings => level == RuntimeLogLevel.Warning,
        RuntimeLogFilter.FirstChance => level == RuntimeLogLevel.FirstChance,
        _ => true,
    };

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        RecordError("AppDomain.UnhandledException", ex?.Message ?? e.ExceptionObject?.ToString() ?? "Unknown unhandled exception", ex);
        Flush();
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        RecordError("TaskScheduler.UnobservedTaskException", e.Exception.GetBaseException().Message, e.Exception);
    }

    private void OnFirstChanceException(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
        if (_inFirstChance || _disposed) return;
        _inFirstChance = true;
        try
        {
            var ex = e.Exception;
            var exType = ex.GetType().FullName ?? ex.GetType().Name;
            var site = FormatTargetSite(ex);
            var message = ex.Message;
            var now = DateTimeOffset.UtcNow;

            Interlocked.Increment(ref _firstChanceCount);

            RuntimeLogEntry? created = null;
            lock (_gate)
            {
                if (_disposed) return;
                var key = (exType, site);
                if (_firstChanceIndex.TryGetValue(key, out var existingNode))
                {
                    var entry = existingNode.Value;
                    entry.HitCount++;
                    entry.LastSeen = now;
                    entry.Message = message;
                    _entries.Remove(existingNode);
                    _entries.AddLast(existingNode);
                }
                else
                {
                    created = new RuntimeLogEntry
                    {
                        FirstSeen = now,
                        LastSeen = now,
                        Level = RuntimeLogLevel.FirstChance,
                        Source = "CLR.FirstChance",
                        ExceptionType = exType,
                        TargetSite = site,
                        Message = message,
                        StackSummary = SummarizeStack(ex),
                        HitCount = 1,
                    };
                    var node = _entries.AddLast(created);
                    _firstChanceIndex[key] = node;
                    EnforceCapacityLocked();
                }
            }

            if (created is not null)
                WriteFileLine(created);
            Changed?.Invoke();
        }
        catch
        {
            // Never allow exception telemetry to throw.
        }
        finally
        {
            _inFirstChance = false;
        }
    }

    private void AddEntry(RuntimeLogLevel level, string source, string exType, string targetSite, string message, string stackSummary)
    {
        if (_disposed) return;
        var now = DateTimeOffset.UtcNow;
        RuntimeLogEntry entryToWrite;

        lock (_gate)
        {
            if (_disposed) return;
            if (_entries.Last?.Value is { } last
                && last.Level == level
                && string.Equals(last.Source, source, StringComparison.Ordinal)
                && string.Equals(last.Message, message, StringComparison.Ordinal))
            {
                last.HitCount++;
                last.LastSeen = now;
                entryToWrite = last;
            }
            else
            {
                entryToWrite = new RuntimeLogEntry
                {
                    FirstSeen = now,
                    LastSeen = now,
                    Level = level,
                    Source = source,
                    ExceptionType = exType,
                    TargetSite = targetSite,
                    Message = message,
                    StackSummary = stackSummary,
                    HitCount = 1,
                };
                _entries.AddLast(entryToWrite);
                EnforceCapacityLocked();
            }
        }

        WriteFileLine(entryToWrite);
        Changed?.Invoke();
    }

    private void EnforceCapacityLocked()
    {
        while (_entries.Count > _capacity && _entries.First is { } oldest)
        {
            _entries.RemoveFirst();
            if (oldest.Value.Level == RuntimeLogLevel.FirstChance)
            {
                var key = (oldest.Value.ExceptionType, oldest.Value.TargetSite);
                if (_firstChanceIndex.TryGetValue(key, out var mapped) && ReferenceEquals(mapped, oldest))
                    _firstChanceIndex.Remove(key);
            }
        }
    }

    private void WriteFileLine(RuntimeLogEntry entry)
    {
        if (!Volatile.Read(ref _isFileLoggingEnabled)) return;
        lock (_fileGate)
        {
            if (!_isFileLoggingEnabled || _disposed) return;
            try
            {
                var today = entry.LastSeen.UtcDateTime.Date;
                EnsureWriterOpenLocked(today);
                _fileWriter?.WriteLine(entry.SummaryLine);
                if (!string.IsNullOrWhiteSpace(entry.StackSummary))
                    _fileWriter?.WriteLine("  " + entry.StackSummary.Replace("\n", " | ", StringComparison.Ordinal));
                _fileWriter?.Flush();
            }
            catch
            {
                // Ignore log write failures to avoid cascading errors.
            }
        }
    }

    private void EnsureWriterOpenLocked(DateTime dateUtc)
    {
        if (_fileWriter is not null && _currentLogDate == dateUtc) return;
        CloseWriterLocked();
        Directory.CreateDirectory(_logDirectory);
        var path = Path.Combine(_logDirectory, $"diag-{dateUtc:yyyy-MM-dd}.log");
        var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        _fileWriter = new StreamWriter(fs, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)) { AutoFlush = false };
        _currentLogDate = dateUtc;
    }

    private void CloseWriterLocked()
    {
        try { _fileWriter?.Flush(); } catch { }
        try { _fileWriter?.Dispose(); } catch { }
        _fileWriter = null;
    }

    private static string FormatTargetSite(Exception? ex)
    {
        if (ex is null) return "";
        var site = ex.TargetSite;
        if (site is not null)
        {
            var decl = site.DeclaringType?.Name;
            return string.IsNullOrEmpty(decl) ? site.Name : $"{decl}.{site.Name}";
        }
        var st = ex.StackTrace;
        if (string.IsNullOrWhiteSpace(st)) return "";
        var line = st.AsSpan().TrimStart();
        var nl = line.IndexOf('\n');
        if (nl >= 0) line = line[..nl].TrimEnd('\r');
        return line.Length > 120 ? line[..120].ToString() : line.ToString();
    }

    private static string SummarizeStack(Exception? ex)
    {
        if (ex is null) return "";
        var st = ex.StackTrace;
        if (string.IsNullOrWhiteSpace(st)) return "";
        var lines = st.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var take = Math.Min(4, lines.Length);
        return string.Join('\n', lines, 0, take);
    }

    private static string Truncate(string? value, int maxLen)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";
        return value.Length <= maxLen ? value : value[..maxLen] + "...";
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (_disposed) return;
            _disposed = true;
            if (_captureFirstChance)
            {
                AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;
                _captureFirstChance = false;
            }
            if (_globalHooksInstalled)
            {
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
                TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
                _globalHooksInstalled = false;
            }
        }
        StopFileLogging();
    }
}

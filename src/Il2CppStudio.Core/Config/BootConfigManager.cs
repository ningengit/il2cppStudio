using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Config;

/// <summary>One line of boot.config. Comments / blank / malformed lines are kept verbatim (Key is null).</summary>
public sealed record BootConfigLine(string? Key, string? Value, string Raw);

public sealed record BootConfigChange(string Key, string? OldValue, string NewValue);

/// <summary>
/// Plain-text reader/writer for &lt;Game&gt;_Data/boot.config (key=value per line). The file is edited
/// in place with line order, unknown keys and comments preserved, and a one-time backup
/// (boot.config.il2cppstudio.bak) is written before the first modification so the original can be restored.
/// </summary>
public sealed class BootConfigManager
{
    public const string BackupSuffix = ".il2cppstudio.bak";

    /// <summary>The diagnostic flag set applied by Tools → Configure Diagnostic Boot Flags / `enable-debug`.</summary>
    public static readonly IReadOnlyDictionary<string, string> DiagnosticFlags = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["player-connection-debug"] = "1",
        ["wait-for-native-debugger"] = "0",
        ["scripting-runtime-version"] = "latest",
        ["enable-native-profile-agent"] = "1",
    };

    private readonly List<BootConfigLine> _lines;
    private readonly IDiagnosticSink _sink;

    public string Path { get; }
    public bool Exists { get; }
    public IReadOnlyList<BootConfigLine> Lines => _lines;

    private BootConfigManager(string path, bool exists, List<BootConfigLine> lines, IDiagnosticSink sink)
    {
        Path = path;
        Exists = exists;
        _lines = lines;
        _sink = sink;
    }

    /// <summary>Opens boot.config from a game root, a *_Data folder or a direct file path.</summary>
    public static BootConfigManager Open(string path, IDiagnosticSink sink)
    {
        var file = ResolvePath(path);
        if (!File.Exists(file))
        {
            sink.Warn(DiagnosticCategory.BootConfig, DiagnosticCodes.BootConfigMissing,
                $"boot.config not found at '{file}'; it will be created on save.", new() { File = file });
            return new BootConfigManager(file, false, [], sink);
        }
        var lines = File.ReadAllLines(file, Encoding.UTF8).Select(Parse).ToList();
        return new BootConfigManager(file, true, lines, sink);
    }

    public static string ResolvePath(string path)
    {
        path = System.IO.Path.GetFullPath(path);
        if (File.Exists(path)) return path;
        if (Directory.Exists(path))
        {
            var direct = System.IO.Path.Combine(path, "boot.config");
            if (File.Exists(direct) || path.EndsWith("_Data", StringComparison.OrdinalIgnoreCase)) return direct;
            var data = Directory.EnumerateDirectories(path, "*_Data").FirstOrDefault();
            if (data is not null) return System.IO.Path.Combine(data, "boot.config");
        }
        return System.IO.Path.Combine(path, "boot.config");
    }

    private static BootConfigLine Parse(string raw)
    {
        var trimmed = raw.Trim();
        if (trimmed.Length == 0 || trimmed.StartsWith('#') || trimmed.StartsWith(';')) return new(null, null, raw);
        var eq = raw.IndexOf('=');
        return eq <= 0 ? new(null, null, raw) : new(raw[..eq].Trim(), raw[(eq + 1)..].Trim(), raw);
    }

    public string? Get(string key) => _lines.LastOrDefault(l => l.Key == key)?.Value;

    public IReadOnlyDictionary<string, string> ToDictionary()
    {
        var d = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var l in _lines) if (l.Key is not null) d[l.Key] = l.Value ?? "";
        return d;
    }

    /// <summary>Sets a key (replacing every existing occurrence) and returns the change, or null if unchanged.</summary>
    public BootConfigChange? Set(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (key.Contains('=') || key.Contains('\n') || value.Contains('\n'))
            throw new ArgumentException("Keys may not contain '=' and neither part may contain newlines.");

        var old = Get(key);
        if (old == value) return null;
        var idx = _lines.FindIndex(l => l.Key == key);
        var line = new BootConfigLine(key, value, $"{key}={value}");
        if (idx < 0) _lines.Add(line);
        else
        {
            _lines[idx] = line;
            _lines.RemoveAll(l => l.Key == key && !ReferenceEquals(l, line));
        }
        return new BootConfigChange(key, old, value);
    }

    public bool Remove(string key) => _lines.RemoveAll(l => l.Key == key) > 0;

    /// <summary>Applies <see cref="DiagnosticFlags"/>; returns only the keys that actually changed.</summary>
    public IReadOnlyList<BootConfigChange> ApplyDiagnosticFlags(IReadOnlyDictionary<string, string>? overrides = null)
    {
        var changes = new List<BootConfigChange>();
        foreach (var (k, v) in DiagnosticFlags)
            if (Set(k, overrides?.GetValueOrDefault(k) ?? v) is { } c) changes.Add(c);
        return changes;
    }

    /// <summary>Writes the file (UTF-8, no BOM, LF endings as Unity writes them) after creating a one-time backup.</summary>
    public void Save()
    {
        var loc = new DiagnosticLocation { File = Path };
        if (Exists && !File.Exists(Path + BackupSuffix))
        {
            File.Copy(Path, Path + BackupSuffix);
            _sink.Info(DiagnosticCategory.BootConfig, DiagnosticCodes.BootConfigBackupCreated, $"Backup written: {Path + BackupSuffix}", loc);
        }
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path)!);
        var text = string.Join('\n', _lines.Select(l => l.Raw)) + "\n";
        File.WriteAllText(Path, text, new UTF8Encoding(false));
        _sink.Info(DiagnosticCategory.BootConfig, DiagnosticCodes.BootConfigPatched, $"boot.config saved ({_lines.Count(l => l.Key is not null)} keys).", loc);
    }

    /// <summary>Restores the backup created by <see cref="Save"/>. Returns false when no backup exists.</summary>
    public static bool RestoreBackup(string path, IDiagnosticSink sink)
    {
        var file = ResolvePath(path);
        if (!File.Exists(file + BackupSuffix)) return false;
        File.Copy(file + BackupSuffix, file, overwrite: true);
        File.Delete(file + BackupSuffix);
        sink.Info(DiagnosticCategory.BootConfig, DiagnosticCodes.BootConfigRestored, "boot.config restored from backup.", new() { File = file });
        return true;
    }
}

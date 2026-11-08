using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Fingerprinting;

public sealed record Estimate(string Value, string Confidence, IReadOnlyList<string> Evidence);

public sealed record GameEnvironment
{
    public string? UnityVersion { get; init; }
    public string? UnityVersionSource { get; init; }
    public string? CompanyName { get; init; }
    public string? ProductName { get; init; }
    public string? PlayerLogPath { get; init; }
    public string ScriptingBackend { get; init; } = "Unknown";
    public IReadOnlyList<string> ScriptingAssemblies { get; init; } = [];
    public string? MetadataVersion { get; init; }
    public Estimate? ApiCompatibility { get; init; }
    public Estimate? StrippingLevel { get; init; }
}

/// <summary>
/// Detects the Unity environment of a game folder from the files it ships with. Nothing is hardcoded to a
/// specific game; every estimate lists the evidence it is based on.
/// </summary>
public static partial class EnvironmentDetector
{
    [GeneratedRegex(@"(?<v>(?:[2-9]\d{3}|[56])\.\d{1,2}\.\d{1,3}[abfpxc]\d{1,3})")]
    private static partial Regex UnityVersionRegex();

    public static GameEnvironment Detect(string gameRoot, string? dataFolder, string? binaryPath, MetadataModel? model, IDiagnosticSink sink)
    {
        string? version = null, source = null;

        if (dataFolder is not null)
        {
            foreach (var name in new[] { "globalgamemanagers", "data.unity3d", "mainData" })
            {
                var p = Path.Combine(dataFolder, name);
                if (File.Exists(p) && ReadVersionFromSerializedFile(p) is { } v) { version = v; source = name; break; }
            }
        }

        if (version is null && OperatingSystem.IsWindows())
        {
            var player = Path.Combine(gameRoot, "UnityPlayer.dll");
            if (File.Exists(player))
            {
                var fv = FileVersionInfo.GetVersionInfo(player);
                if (fv.FileMajorPart > 0) { version = $"{fv.FileMajorPart}.{fv.FileMinorPart}.{fv.FileBuildPart} (build {fv.FilePrivatePart})"; source = "UnityPlayer.dll version resource"; }
            }
        }

        if (version is not null)
            sink.Info(DiagnosticCategory.Fingerprint, DiagnosticCodes.UnityVersionDetected, $"Unity {version} (from {source}).", new() { File = gameRoot });
        else
            sink.Warn(DiagnosticCategory.Fingerprint, DiagnosticCodes.UnityVersionUnknown, "Unity version could not be determined.", new() { File = gameRoot });

        string? company = null, product = null;
        if (dataFolder is not null && File.Exists(Path.Combine(dataFolder, "app.info")))
        {
            var lines = File.ReadAllLines(Path.Combine(dataFolder, "app.info"));
            company = lines.ElementAtOrDefault(0)?.Trim();
            product = lines.ElementAtOrDefault(1)?.Trim();
        }

        string? playerLog = null;
        if (company is not null && product is not null && OperatingSystem.IsWindows())
        {
            var localLow = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "AppData", "LocalLow");
            playerLog = Path.Combine(localLow, company, product, "Player.log");
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.PlayerLogLocation, $"Player.log location: {playerLog}", new() { File = playerLog });
        }

        var backend = binaryPath is not null ? "IL2CPP"
            : dataFolder is not null && File.Exists(Path.Combine(dataFolder, "Managed", "Assembly-CSharp.dll")) ? "Mono"
            : "Unknown";
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ScriptingBackendDetected, $"Scripting backend: {backend}.", new() { File = gameRoot });

        var scripting = ReadScriptingAssemblies(dataFolder, sink);

        return new GameEnvironment
        {
            UnityVersion = version,
            UnityVersionSource = source,
            CompanyName = company,
            ProductName = product,
            PlayerLogPath = playerLog,
            ScriptingBackend = backend,
            ScriptingAssemblies = scripting,
            MetadataVersion = model?.Statistics.Version,
            ApiCompatibility = model is null ? null : EstimateApiCompatibility(model, sink),
            StrippingLevel = model is null ? null : EstimateStripping(model, sink),
        };
    }

    /// <summary>
    /// Unity serialized files store the editor version as a NUL-terminated ASCII string near the start of
    /// the header (offset varies by format version: ~0x14 for old formats, ~0x30 for format 22+). Scanning
    /// the first 1 KiB with a strict version pattern works across all formats.
    /// </summary>
    public static string? ReadVersionFromSerializedFile(string path)
    {
        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var buf = new byte[Math.Min(1024, fs.Length)];
            fs.ReadExactly(buf);
            var text = Encoding.ASCII.GetString(buf.Select(b => b is >= 0x20 and < 0x7F ? b : (byte)' ').ToArray());
            var m = UnityVersionRegex().Match(text);
            return m.Success ? m.Groups["v"].Value : null;
        }
        catch { return null; }
    }

    private static IReadOnlyList<string> ReadScriptingAssemblies(string? dataFolder, IDiagnosticSink sink)
    {
        var p = dataFolder is null ? null : Path.Combine(dataFolder, "ScriptingAssemblies.json");
        if (p is null || !File.Exists(p)) return [];
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(p));
            return doc.RootElement.TryGetProperty("names", out var names)
                ? [.. names.EnumerateArray().Select(e => e.GetString()).OfType<string>()]
                : [];
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Fingerprint, DiagnosticCodes.FileAccessFailed, $"ScriptingAssemblies.json unreadable: {ex.Message}", new() { File = p }, ex);
            return [];
        }
    }

    /// <summary>
    /// .NET Framework profile ships mscorlib with full-framework assemblies (System.Data, System.Drawing,
    /// System.Configuration, ...) referenced directly; the .NET Standard 2.1 profile routes through netstandard.dll.
    /// </summary>
    private static Estimate EstimateApiCompatibility(MetadataModel model, IDiagnosticSink sink)
    {
        var names = model.Assemblies.Select(a => a.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var frameworkOnly = new[] { "System.Data", "System.Drawing", "System.Configuration", "System.Web", "System.Transactions", "System.EnterpriseServices", "Microsoft.CSharp" }
            .Where(names.Contains).ToList();
        var evidence = new List<string>();
        if (names.Contains("netstandard")) evidence.Add("netstandard facade present");
        if (frameworkOnly.Count > 0) evidence.Add("full-framework assemblies present: " + string.Join(", ", frameworkOnly));

        var gameRefsNetStandard = model.Assemblies
            .Where(a => a.Name.StartsWith("Assembly-CSharp", StringComparison.OrdinalIgnoreCase))
            .Any(a => a.References.Contains("netstandard", StringComparer.OrdinalIgnoreCase));
        evidence.Add(gameRefsNetStandard ? "Assembly-CSharp references netstandard" : "Assembly-CSharp references mscorlib directly");

        var value = !gameRefsNetStandard && frameworkOnly.Count > 0 ? ".NET Framework" : gameRefsNetStandard ? ".NET Standard 2.1" : ".NET Framework (likely)";
        var est = new Estimate(value, frameworkOnly.Count > 0 || gameRefsNetStandard ? "Medium" : "Low", evidence);
        sink.Info(DiagnosticCategory.Fingerprint, DiagnosticCodes.ApiCompatibilityEstimate, $"API compatibility: {est.Value} ({est.Confidence}) - {string.Join("; ", evidence)}");
        return est;
    }

    /// <summary>
    /// Managed stripping removes unused BCL types. mscorlib keeps roughly 1500+ types with Minimal stripping
    /// and far fewer with Medium/High. This is a coarse heuristic and is reported as such.
    /// </summary>
    private static Estimate EstimateStripping(MetadataModel model, IDiagnosticSink sink)
    {
        var mscorlib = model.Assemblies.FirstOrDefault(a => a.Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase));
        var count = mscorlib?.TypeCount ?? 0;
        var value = count switch
        {
            0 => "Unknown",
            >= 1500 => "Minimal (or Disabled)",
            >= 900 => "Low",
            >= 500 => "Medium",
            _ => "High",
        };
        var est = new Estimate(value, "Low", [$"mscorlib type count = {count}"]);
        sink.Info(DiagnosticCategory.Fingerprint, DiagnosticCodes.StrippingEstimate, $"Managed stripping estimate: {est.Value} (mscorlib has {count} types; heuristic).");
        return est;
    }
}

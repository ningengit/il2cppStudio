using System.Diagnostics;
using System.Text.RegularExpressions;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Backup;

/// <summary>
/// Public symbols of a matching GameAssembly.pdb (Enhanced mode), read through LLVM's <c>llvm-pdbutil dump -publics</c>.
/// Names are C symbols (<c>Mathf_RoundToInt_mXXXX</c>) or MSVC-decorated (<c>?Name@@YA…</c>); the plain function
/// name is extracted. Used to name out-of-line <c>_inline</c> copies and to cross-check helper classification.
/// </summary>
public static partial class PdbPublicSymbols
{
    [GeneratedRegex(@"S_PUB32 \[size = \d+\] `(?<name>[^`]+)`\s+flags = (?<flags>[^,]*), addr = (?<seg>\d+):(?<off>\d+)")]
    private static partial Regex PublicRegex();

    public static string? FindTool() =>
        new[] { Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PDBUTIL"), @"C:\Program Files\LLVM\bin\llvm-pdbutil.exe" }
            .FirstOrDefault(p => !string.IsNullOrEmpty(p) && File.Exists(p));

    /// <summary>RVA → undecorated function name for every function public symbol, or empty when unavailable.</summary>
    public static IReadOnlyDictionary<ulong, string> Load(string pdbPath, IReadOnlyList<BinarySection> sections, IDiagnosticSink sink)
    {
        var tool = FindTool();
        if (tool is null)
        {
            sink.Info(DiagnosticCategory.Pdb, DiagnosticCodes.PdbParseFailed, "llvm-pdbutil not found (set IL2CPPSTUDIO_PDBUTIL); PDB symbol names are not used.");
            return new Dictionary<ulong, string>();
        }
        var sw = Stopwatch.StartNew();
        string text;
        try
        {
            using var p = Process.Start(new ProcessStartInfo(tool) { ArgumentList = { "dump", "-publics", pdbPath }, RedirectStandardOutput = true, UseShellExecute = false })!;
            text = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Pdb, DiagnosticCodes.PdbParseFailed, $"llvm-pdbutil failed: {ex.Message}", new() { File = pdbPath }, ex);
            return new Dictionary<ulong, string>();
        }

        var result = new Dictionary<ulong, string>();
        foreach (Match m in PublicRegex().Matches(text))
        {
            if (!m.Groups["flags"].Value.Contains("function")) continue;
            var seg = int.Parse(m.Groups["seg"].Value);
            if (seg < 1 || seg > sections.Count) continue;
            var rva = sections[seg - 1].Rva + ulong.Parse(m.Groups["off"].Value);
            result.TryAdd(rva, Undecorate(m.Groups["name"].Value));
        }
        sink.Info(DiagnosticCategory.Pdb, DiagnosticCodes.PdbSymbolsLoaded, $"{result.Count:N0} function symbols from {Path.GetFileName(pdbPath)} in {sw.Elapsed.TotalSeconds:F1}s.", new() { File = pdbPath });
        return result;
    }

    /// <summary>"?Name@@YAHM…" → "Name"; plain C names are returned unchanged.</summary>
    public static string Undecorate(string symbol)
    {
        if (!symbol.StartsWith('?')) return symbol;
        var end = symbol.IndexOf("@@", StringComparison.Ordinal);
        var core = end > 1 ? symbol[1..end] : symbol[1..];
        var at = core.IndexOf('@');   // nested scopes (static locals) are not functions we map
        return at >= 0 ? core[..at] : core;
    }
}

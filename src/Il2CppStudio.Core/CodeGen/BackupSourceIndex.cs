using System.Collections.Concurrent;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.CodeGen;

/// <summary>
/// Index of function definitions in the build backup's il2cppOutput/*.cpp files (Enhanced mode only).
/// Unity emits every method as <c>IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ret Symbol (args)</c> followed by a
/// brace-balanced body; the index maps Symbol → (file, offset) so the original generated C++ can be shown verbatim.
/// Indexing happens once, lazily, and streams files line by line.
/// </summary>
public sealed class BackupSourceIndex(string il2cppOutputFolder, IDiagnosticSink sink)
{
    private readonly Lazy<ConcurrentDictionary<string, (string File, long Offset)>> _index = new(() => Build(il2cppOutputFolder, sink));

    public int Count => _index.Value.Count;

    private static ConcurrentDictionary<string, (string, long)> Build(string folder, IDiagnosticSink sink)
    {
        var map = new ConcurrentDictionary<string, (string, long)>(StringComparer.Ordinal);
        var files = Directory.EnumerateFiles(folder, "*.cpp", SearchOption.TopDirectoryOnly).ToList();
        Parallel.ForEach(files, file =>
        {
            try
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1 << 16);
                var probe = new byte[Math.Min(65536, fs.Length)];
                fs.ReadExactly(probe);
                var newline = probe.AsSpan().IndexOf("\r\n"u8) >= 0 ? 2 : 1;
                var bom = probe.AsSpan().StartsWith((ReadOnlySpan<byte>)[0xEF, 0xBB, 0xBF]) ? 3 : 0;
                fs.Position = 0;
                using var reader = new StreamReader(fs, Encoding.UTF8);
                long offset = bom;
                string? line;
                while ((line = reader.ReadLine()) is not null)
                {
                    var lineStart = offset;
                    offset += Encoding.UTF8.GetByteCount(line) + newline;
                    if (!line.Contains("IL2CPP_METHOD_ATTR", StringComparison.Ordinal) || line.TrimEnd().EndsWith(';')) continue;
                    var paren = line.IndexOf(" (", StringComparison.Ordinal);
                    if (paren < 0) continue;
                    var before = line[..paren];
                    var space = before.LastIndexOf(' ');
                    var symbol = before[(space + 1)..].TrimStart('*');
                    if (symbol.Length > 0) map.TryAdd(symbol, (file, lineStart));
                }
            }
            catch (Exception ex)
            {
                sink.Warn(DiagnosticCategory.BuildBackup, DiagnosticCodes.FileAccessFailed, $"Cannot index '{file}': {ex.Message}", new() { File = file }, ex);
            }
        });
        sink.Info(DiagnosticCategory.BuildBackup, DiagnosticCodes.CodeGenModuleParsed, $"Indexed {map.Count:N0} generated C++ function definitions in {files.Count} files.");
        return map;
    }

    /// <summary>Returns the full definition (signature + body) of a generated function, or null.</summary>
    public string? GetDefinition(string symbol)
    {
        if (!_index.Value.TryGetValue(symbol, out var loc)) return null;
        var text = ReadFrom(loc.File, loc.Offset, symbol);
        return text;
    }

    private static string? ReadFrom(string file, long offset, string symbol)
    {
        using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        fs.Position = offset;
        using var reader = new StreamReader(fs, Encoding.UTF8, detectEncodingFromByteOrderMarks: false);
        var sb = new StringBuilder();
        string? line;
        var found = false;
        var depth = 0;
        var sawBrace = false;
        var lines = 0;
        while ((line = reader.ReadLine()) is not null)
        {
            if (!found)
            {
                if (!line.Contains(symbol + " (", StringComparison.Ordinal)) return null;   // index out of sync with file
                found = true;
            }
            sb.AppendLine(line);
            foreach (var c in line)
            {
                if (c == '{') { depth++; sawBrace = true; }
                else if (c == '}') depth--;
            }
            if (sawBrace && depth <= 0) break;
            if (++lines > 20_000) break;
        }
        return found ? sb.ToString() : null;
    }
}

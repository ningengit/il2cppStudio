using System.Collections.Concurrent;
using System.Text;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Per-assembly error log (<c>&lt;Assembly&gt;_Error_Logs.txt</c>). Collects decompiler warnings removed from the C#
/// output (e.g. <c>IL_007e: Expected I, but got I8</c>), lifter conversion/fallback notes and lift/decompile
/// failures with stack traces, each with the method, RVA and the fallback action taken. The C# stays clean.
/// </summary>
public sealed class DecompilationErrorLog
{
    public sealed record Entry(DateTimeOffset Time, string Kind, string Member, string? Rva, string Message, string? Action);

    private readonly ConcurrentDictionary<string, ConcurrentQueue<Entry>> _byAssembly = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, byte> _seen = new();

    public int Count => _byAssembly.Values.Sum(q => q.Count);

    public void Add(string assembly, string kind, string member, string? rva, string message, string? action = null)
    {
        // Re-rendering the same member must not duplicate entries.
        if (!_seen.TryAdd($"{assembly}|{kind}|{member}|{message}", 0)) return;
        _byAssembly.GetOrAdd(assembly, _ => new()).Enqueue(new Entry(DateTimeOffset.Now, kind, member, rva, message, action));
    }

    public IReadOnlyList<Entry> EntriesFor(string assembly) => _byAssembly.TryGetValue(assembly, out var q) ? [.. q] : [];

    public static string FileNameFor(string assembly) =>
        (assembly.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? assembly[..^4] : assembly) + "_Error_Logs.txt";

    /// <summary>Writes one file per assembly that has entries; returns the written paths.</summary>
    public IReadOnlyList<string> WriteAll(string directory)
    {
        Directory.CreateDirectory(directory);
        var written = new List<string>();
        foreach (var (asm, queue) in _byAssembly)
        {
            var path = Path.Combine(directory, FileNameFor(asm));
            File.WriteAllText(path, Format(asm, [.. queue]));
            written.Add(path);
        }
        return written;
    }

    public static string Format(string assembly, IReadOnlyList<Entry> entries)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Il2CppStudio error log for {assembly} - {entries.Count} entries, generated {DateTimeOffset.Now:u}");
        foreach (var g in entries.GroupBy(e => e.Kind).OrderBy(g => g.Key))
            sb.AppendLine($"  {g.Key}: {g.Count()}");
        sb.AppendLine(new string('=', 100));
        foreach (var e in entries.OrderBy(e => e.Member, StringComparer.Ordinal).ThenBy(e => e.Time))
        {
            sb.AppendLine($"[{e.Kind}] {e.Member}{(e.Rva is null ? "" : $"  (RVA {e.Rva})")}");
            foreach (var line in e.Message.Split('\n')) sb.AppendLine("    " + line.TrimEnd('\r'));
            if (e.Action is not null) sb.AppendLine($"    -> action: {e.Action}");
        }
        return sb.ToString();
    }
}

using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Plugins;

/// <summary>
/// Pluggable binary loader contract for console and non-standard executable formats (e.g. Nintendo Switch NSO).
/// </summary>
public interface IConsoleBinaryPlugin
{
    string Name { get; }
    BinaryFormat Format { get; }
    bool CanLoad(ReadOnlySpan<byte> headerBytes, string filePath);
    IBinaryImage Load(string filePath, ReadOnlyMemory<byte> rawBytes, IDiagnosticSink sink);
}

/// <summary>Discovers and dispatches registered <see cref="IConsoleBinaryPlugin"/> implementations.</summary>
public static class ConsoleBinaryPluginRegistry
{
    private static readonly List<IConsoleBinaryPlugin> Plugins = [new SwitchNsoPlugin()];

    public static IReadOnlyList<IConsoleBinaryPlugin> Registered => Plugins;

    public static void Register(IConsoleBinaryPlugin plugin)
    {
        if (!Plugins.Contains(plugin)) Plugins.Add(plugin);
    }

    public static bool CanOpen(string filePath)
    {
        if (!File.Exists(filePath)) return false;
        Span<byte> head = stackalloc byte[16];
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var read = fs.Read(head);
        ReadOnlySpan<byte> slice = head[..read];
        foreach (var plugin in Plugins)
            if (plugin.CanLoad(slice, filePath)) return true;
        return false;
    }

    public static IBinaryImage? TryOpen(string filePath, IDiagnosticSink sink)
    {
        if (!File.Exists(filePath)) return null;
        var bytes = File.ReadAllBytes(filePath);
        ReadOnlySpan<byte> head = bytes.AsSpan(0, Math.Min(16, bytes.Length));
        foreach (var plugin in Plugins)
        {
            if (plugin.CanLoad(head, filePath))
                return plugin.Load(filePath, bytes, sink);
        }
        return null;
    }
}

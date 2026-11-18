using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Plugins;

/// <summary>Nintendo Switch NSO0 executable plugin (LZ4-decompressed AArch64 segments + MOD0 relocations).</summary>
public sealed class SwitchNsoPlugin : IConsoleBinaryPlugin
{
    public string Name => "Nintendo Switch NSO0";
    public BinaryFormat Format => BinaryFormat.Nso;

    public bool CanLoad(ReadOnlySpan<byte> headerBytes, string filePath) =>
        headerBytes.Length >= 4 && headerBytes[0] == (byte)'N' && headerBytes[1] == (byte)'S' && headerBytes[2] == (byte)'O' && headerBytes[3] == (byte)'0';

    public IBinaryImage Load(string filePath, ReadOnlyMemory<byte> rawBytes, IDiagnosticSink sink) =>
        NsoBinaryImage.Load(filePath, rawBytes, sink);
}

using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Plugins;

/// <summary>PlayStation 4 / 5 SELF (FSELF / PRX) plugin: reconstructs ELF64 x86-64 in memory and applies SCE relocations.</summary>
public sealed class Ps4SelfPlugin : IConsoleBinaryPlugin
{
    public string Name => "PlayStation 4/5 SELF (FSELF/PRX)";
    public BinaryFormat Format => BinaryFormat.Elf;

    public bool CanLoad(ReadOnlySpan<byte> headerBytes, string filePath) =>
        Ps4SelfUnpacker.IsSelf(headerBytes);

    public IBinaryImage Load(string filePath, ReadOnlyMemory<byte> rawBytes, IDiagnosticSink sink)
    {
        var elfBytes = Ps4SelfUnpacker.UnpackToElf(rawBytes.Span);
        return ElfBinaryImage.Load(filePath, elfBytes, sink, formatLabel: "PS4 SELF\u2192ELF64");
    }
}

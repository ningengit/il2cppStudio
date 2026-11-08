using AsmArm64;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// ARM64 (AArch64 v8/v9) text disassembly with the vendored AsmArm64 library (managed, zero-allocation decoder).
/// Branch/call targets resolve to symbols (IL2CPP methods, helpers) through <see cref="SymbolResolver"/>.
/// </summary>
public sealed class Arm64TextDisassembler : ITextDisassembler
{
    public string Name => "ARM64 (AsmArm64)";
    public Func<ulong, string?>? SymbolResolver { get; set; }

    public string Disassemble(ReadOnlySpan<byte> code, ulong va)
    {
        var options = new Arm64DisassemblerOptions
        {
            BaseAddress = (long)va,
            PrintAddress = true,
            InvalidDataMode = Arm64InvalidDataMode.EmitBytes,
            TryFormatLabel = FormatLabel,
        };
        var length = code.Length & ~3;   // A64 instructions are 4 bytes
        return new Arm64Disassembler(options).Disassemble(code[..length]);
    }

    private bool FormatLabel(long address, Span<char> destination, out int written)
    {
        written = 0;
        if (SymbolResolver?.Invoke((ulong)address) is not { } name || name.Length > destination.Length) return false;
        name.AsSpan().CopyTo(destination);
        written = name.Length;
        return true;
    }
}

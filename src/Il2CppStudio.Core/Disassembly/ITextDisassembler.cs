namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// Architecture-neutral text disassembly of one function range, used for the "native" view on every platform
/// (x86, x64, ARMv7, ARM64). Lifting is separate (x86-64 only); this only renders instructions. Call/branch targets
/// that resolve to a known symbol are annotated through <see cref="SymbolResolver"/>.
/// </summary>
public interface ITextDisassembler
{
    string Name { get; }

    /// <summary>Disassembles [va, end) of <paramref name="code"/> (code starts at <paramref name="va"/>).</summary>
    string Disassemble(ReadOnlySpan<byte> code, ulong va);

    Func<ulong, string?>? SymbolResolver { get; set; }
}

public static class TextDisassemblerFactory
{
    /// <summary>The disassembler for an architecture, or null when none is available.</summary>
    public static ITextDisassembler? For(Binary.CpuArchitecture arch) => arch switch
    {
        Binary.CpuArchitecture.X64 => new IcedTextDisassembler(64),
        Binary.CpuArchitecture.X86 => new IcedTextDisassembler(32),
        Binary.CpuArchitecture.Arm64 => new Arm64TextDisassembler(),
        Binary.CpuArchitecture.Arm => CapstoneArmDisassembler.TryCreate(),
        Binary.CpuArchitecture.Wasm32 => new WasmTextDisassembler(),
        _ => null,
    };
}

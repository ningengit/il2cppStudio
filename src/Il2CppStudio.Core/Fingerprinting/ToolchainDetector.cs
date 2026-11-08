using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Binary;

namespace Il2CppStudio.Core.Fingerprinting;

public enum NativeCompiler { Unknown, Msvc, ClangCl, Clang, Gcc, Emscripten }

/// <summary>Which toolchain produced the game binary, its target architecture and the evidence for the verdict.</summary>
public sealed record ToolchainInfo(NativeCompiler Compiler, string Architecture, string Format, string? Version, IReadOnlyList<string> Evidence)
{
    public string Display => $"{Compiler}{(Version is null ? "" : " " + Version)} ({Format}, {Architecture})";
}

/// <summary>
/// Detects MSVC / clang-cl / Clang / GCC / Emscripten from the container and code shape, with no user input:
/// PE Rich header (link.exe) vs. none (lld-link); ELF/Mach-O <c>.comment</c> producer strings; WebAssembly magic;
/// and, as a tie-breaker for PE, prologue statistics (MSVC homes non-volatiles with <c>mov [rsp+8], rbx</c>, Clang pushes).
/// </summary>
public static class ToolchainDetector
{
    public static ToolchainInfo Detect(ReadOnlySpan<byte> file, IEnumerable<int>? functionFileOffsets = null)
    {
        if (file.Length >= 4 && file[..4].SequenceEqual("\0asm"u8))
            return new(NativeCompiler.Emscripten, "wasm32", "WebAssembly", null, ["'\\0asm' magic: Unity WebGL builds use Emscripten (Clang/LLVM)."]);
        if (file.Length >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(file) is 0xFEEDFACF or 0xFEEDFACE or 0xBEBAFECA)
            return new(NativeCompiler.Clang, MachOArch(file), "Mach-O", null, ["Mach-O container: Apple toolchains are Clang-only (iOS/macOS)."]);
        if (file.Length >= 0x40 && file[0] == 0x7F && file[1] == 'E' && file[2] == 'L' && file[3] == 'F')
            return ElfToolchain.Detect(file);
        if (file.Length >= 0x40 && file[0] == 'M' && file[1] == 'Z')
            return Pe(file, functionFileOffsets);
        return new(NativeCompiler.Unknown, "unknown", "unknown", null, ["Unrecognized container format."]);
    }

    private static ToolchainInfo Pe(ReadOnlySpan<byte> file, IEnumerable<int>? functions)
    {
        var evidence = new List<string>();
        var peOffset = BinaryPrimitives.ReadInt32LittleEndian(file[0x3C..]);
        var machine = peOffset + 6 < file.Length ? BinaryPrimitives.ReadUInt16LittleEndian(file[(peOffset + 4)..]) : (ushort)0;
        var arch = machine switch { 0x8664 => "x86-64", 0x14C => "x86", 0xAA64 => "ARM64", 0x1C4 => "ARM", _ => $"machine 0x{machine:X}" };

        var rich = RichHeader.TryParse(file);
        var (msvcLike, clangLike) = PrologueStats.Count(file, functions);
        if (functions is not null) evidence.Add($"Prologues: {msvcLike} MSVC-style (mov [rsp+8], reg), {clangLike} Clang-style (push sequences).");
        if (rich is not null)
        {
            var newest = rich.Entries.Where(e => e.ProductId != 0).OrderByDescending(e => e.BuildNumber).FirstOrDefault();
            evidence.Add($"Rich header present ({rich.Entries.Count} @comp.id entries): linked by MSVC link.exe.");
            return new(NativeCompiler.Msvc, arch, "PE", newest?.ToolsetGuess, evidence);
        }
        evidence.Add("No Rich header: not linked by link.exe (lld-link / clang-cl or a stripped header).");
        var compiler = clangLike > msvcLike ? NativeCompiler.ClangCl : msvcLike > 0 ? NativeCompiler.Msvc : NativeCompiler.ClangCl;
        return new(compiler, arch, "PE", null, evidence);
    }

    private static string MachOArch(ReadOnlySpan<byte> f) =>
        f.Length >= 8 && BinaryPrimitives.ReadUInt32LittleEndian(f[4..]) is var cpu ? cpu switch
        {
            0x0100000C => "ARM64", 0x01000007 => "x86-64", 0x0C => "ARM", _ => "fat/universal",
        } : "unknown";

    internal static string Ascii(ReadOnlySpan<byte> s) => Encoding.ASCII.GetString(s);
}

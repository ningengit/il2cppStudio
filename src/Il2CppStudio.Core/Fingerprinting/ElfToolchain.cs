using System.Buffers.Binary;

namespace Il2CppStudio.Core.Fingerprinting;

/// <summary>
/// ELF (Android <c>libil2cpp.so</c>, Linux <c>GameAssembly.so</c>): reads <c>e_machine</c> and the <c>.comment</c>
/// section, where compilers record themselves ("Android (…) clang version 14", "GCC: (Ubuntu 9.4.0)").
/// Minimal section-header walk, 32- and 64-bit little-endian only (all Unity targets).
/// </summary>
public static class ElfToolchain
{
    public static ToolchainInfo Detect(ReadOnlySpan<byte> f)
    {
        var is64 = f[4] == 2;
        var machine = BinaryPrimitives.ReadUInt16LittleEndian(f[0x12..]);
        var arch = machine switch { 0xB7 => "ARM64", 0x28 => "ARMv7", 0x3E => "x86-64", 0x03 => "x86", _ => $"machine 0x{machine:X}" };
        var comment = ReadComment(f, is64);
        var evidence = new List<string> { $"ELF{(is64 ? "64" : "32")} e_machine 0x{machine:X} ({arch})." };
        if (comment is null)
        {
            evidence.Add("No .comment section (stripped); Unity's Android/Linux IL2CPP toolchains are Clang.");
            return new(NativeCompiler.Clang, arch, "ELF", null, evidence);
        }
        evidence.Add($".comment: {comment}");
        var clang = comment.IndexOf("clang version", StringComparison.OrdinalIgnoreCase);
        if (clang >= 0) return new(NativeCompiler.Clang, arch, "ELF", Word(comment, clang + "clang version".Length), evidence);
        var gcc = comment.IndexOf("GCC:", StringComparison.Ordinal);
        if (gcc >= 0) return new(NativeCompiler.Gcc, arch, "ELF", comment[(gcc + 4)..].Trim(), evidence);
        return new(NativeCompiler.Unknown, arch, "ELF", null, evidence);
    }

    private static string? Word(string s, int at)
    {
        var rest = s[at..].TrimStart();
        var end = rest.IndexOfAny([' ', '(', '\0']);
        return end < 0 ? rest : rest[..end];
    }

    private static (long Off, long Size, uint Name) Section(ReadOnlySpan<byte> f, bool is64, long shoff, int shentsize, int i)
    {
        var h = f.Slice((int)(shoff + (long)i * shentsize), shentsize);
        return is64
            ? ((long)BinaryPrimitives.ReadUInt64LittleEndian(h[0x18..]), (long)BinaryPrimitives.ReadUInt64LittleEndian(h[0x20..]), BinaryPrimitives.ReadUInt32LittleEndian(h))
            : (BinaryPrimitives.ReadUInt32LittleEndian(h[0x10..]), BinaryPrimitives.ReadUInt32LittleEndian(h[0x14..]), BinaryPrimitives.ReadUInt32LittleEndian(h));
    }

    private static string? ReadComment(ReadOnlySpan<byte> f, bool is64)
    {
        long shoff = is64 ? (long)BinaryPrimitives.ReadUInt64LittleEndian(f[0x28..]) : BinaryPrimitives.ReadUInt32LittleEndian(f[0x20..]);
        int shentsize = BinaryPrimitives.ReadUInt16LittleEndian(f[(is64 ? 0x3A : 0x2E)..]);
        int shnum = BinaryPrimitives.ReadUInt16LittleEndian(f[(is64 ? 0x3C : 0x30)..]);
        int shstrndx = BinaryPrimitives.ReadUInt16LittleEndian(f[(is64 ? 0x3E : 0x32)..]);
        if (shoff <= 0 || shnum == 0 || shoff + (long)shnum * shentsize > f.Length || shstrndx >= shnum) return null;

        var strtab = Section(f, is64, shoff, shentsize, shstrndx);
        for (var i = 0; i < shnum; i++)
        {
            var s = Section(f, is64, shoff, shentsize, i);
            var nameAt = strtab.Off + s.Name;
            if (nameAt + 8 > f.Length) continue;
            if (!f.Slice((int)nameAt, 9).SequenceEqual(".comment\0"u8)) continue;
            if (s.Off + s.Size > f.Length) return null;
            var text = ToolchainDetector.Ascii(f.Slice((int)s.Off, (int)Math.Min(s.Size, 512)));
            return string.Join(" | ", text.Split('\0', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct());
        }
        return null;
    }
}

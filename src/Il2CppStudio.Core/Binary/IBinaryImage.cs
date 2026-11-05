namespace Il2CppStudio.Core.Binary;

public enum BinaryFormat { Unknown, PE, Elf, MachO }

public enum CpuArchitecture { Unknown, X86, X64, Arm, Arm64 }

public sealed record BinarySection(
    string Name,
    ulong Rva,
    ulong VirtualSize,
    long FileOffset,
    long RawSize,
    bool IsExecutable,
    bool IsWritable,
    bool IsReadable)
{
    public bool ContainsRva(ulong rva) => rva >= Rva && rva < Rva + Math.Max(VirtualSize, (ulong)RawSize);
    public bool ContainsFileOffset(long offset) => offset >= FileOffset && offset < FileOffset + RawSize;
}

/// <summary>
/// Format-neutral view of a native IL2CPP binary (GameAssembly.dll / libil2cpp.so / GameAssembly.so).
/// Addresses: RVA = offset from image base, VA = ImageBase + RVA, FileOffset = position in the file.
/// </summary>
public interface IBinaryImage : IDisposable
{
    string Path { get; }
    BinaryFormat Format { get; }
    CpuArchitecture Architecture { get; }
    bool Is64Bit { get; }
    ulong ImageBase { get; }
    IReadOnlyList<BinarySection> Sections { get; }

    /// <summary>The entire raw file, memory mapped.</summary>
    ReadOnlyMemory<byte> Data { get; }

    bool TryRvaToFileOffset(ulong rva, out long fileOffset);
    bool TryFileOffsetToRva(long fileOffset, out ulong rva);

    ulong RvaToVa(ulong rva) => ImageBase + rva;
    ulong VaToRva(ulong va) => va - ImageBase;

    bool TryVaToFileOffset(ulong va, out long fileOffset)
    {
        if (va < ImageBase) { fileOffset = -1; return false; }
        return TryRvaToFileOffset(va - ImageBase, out fileOffset);
    }

    BinarySection? FindSectionByRva(ulong rva) => Sections.FirstOrDefault(s => s.ContainsRva(rva));
}

/// <summary>Shared RVA/offset translation over a section table.</summary>
internal static class SectionMath
{
    public static bool TryRvaToOffset(IReadOnlyList<BinarySection> sections, ulong rva, out long offset)
    {
        foreach (var s in sections)
        {
            if (rva >= s.Rva && rva < s.Rva + (ulong)s.RawSize)
            {
                offset = s.FileOffset + (long)(rva - s.Rva);
                return true;
            }
        }
        offset = -1;
        return false;
    }

    public static bool TryOffsetToRva(IReadOnlyList<BinarySection> sections, long offset, out ulong rva)
    {
        foreach (var s in sections)
        {
            if (s.RawSize > 0 && s.ContainsFileOffset(offset))
            {
                rva = s.Rva + (ulong)(offset - s.FileOffset);
                return true;
            }
        }
        rva = 0;
        return false;
    }
}

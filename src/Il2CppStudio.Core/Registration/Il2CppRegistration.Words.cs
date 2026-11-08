using System.Runtime.InteropServices;

namespace Il2CppStudio.Core.Registration;

/// <summary>
/// Pointer-size abstraction: every registration structure is an array of pointer-sized words (8 bytes on x64/ARM64,
/// 4 on x86/ARMv7). Scans run over a widened <c>ulong[]</c> view so one code path serves both widths.
/// </summary>
public sealed partial class Il2CppRegistration
{
    /// <summary>Pointer size of the image (4 or 8).</summary>
    public int Ptr => Reader.PointerSize;

    private ulong P(ulong va) => Reader.ReadPointer(va);

    /// <summary>Code pointers: ARMv7 Thumb functions are referenced with bit 0 set; the instruction address clears it.</summary>
    private ulong[] CodePointers(ulong[] pointers)
    {
        if (Reader.Image.Architecture != Binary.CpuArchitecture.Arm) return pointers;
        for (var i = 0; i < pointers.Length; i++) pointers[i] &= ~1UL;
        return pointers;
    }

    private ulong At(ulong va, int index) => Reader.ReadPointer(va + (ulong)(index * Ptr));

    /// <summary>The pointer-aligned words of a data range, widened to 64-bit.</summary>
    private ulong[] Words(ulong start, ulong end)
    {
        var span = Reader.Slice(start, (int)(end - start));
        if (Ptr == 8) return MemoryMarshal.Cast<byte, ulong>(span[..(span.Length & ~7)]).ToArray();
        var narrow = MemoryMarshal.Cast<byte, uint>(span[..(span.Length & ~3)]);
        var wide = new ulong[narrow.Length];
        for (var i = 0; i < narrow.Length; i++) wide[i] = narrow[i];
        return wide;
    }
}

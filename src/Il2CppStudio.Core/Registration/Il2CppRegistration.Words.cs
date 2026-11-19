using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Il2CppStudio.Core.Registration;

/// <summary>
/// Pointer-size abstraction: every registration structure is an array of pointer-sized words (8 bytes on x64/ARM64,
/// 4 on x86/ARMv7). Scanning operates directly on zero-copy <see cref="ReadOnlySpan{T}"/> views over the mapped binary.
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

    private ReadOnlySpan<ulong> Words64(ulong start, ulong end)
    {
        var span = Reader.Slice(start, (int)(end - start));
        return MemoryMarshal.Cast<byte, ulong>(span[..(span.Length & ~7)]);
    }

    private ReadOnlySpan<uint> Words32(ulong start, ulong end)
    {
        var span = Reader.Slice(start, (int)(end - start));
        return MemoryMarshal.Cast<byte, uint>(span[..(span.Length & ~3)]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong WordToU64<TWord>(TWord word) where TWord : unmanaged =>
        Unsafe.SizeOf<TWord>() == 8 ? Unsafe.BitCast<TWord, ulong>(word) : Unsafe.BitCast<TWord, uint>(word);
}

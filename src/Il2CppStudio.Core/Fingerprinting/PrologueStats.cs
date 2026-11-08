namespace Il2CppStudio.Core.Fingerprinting;

/// <summary>
/// Counts function-entry idioms on x86-64. MSVC saves non-volatile registers into the caller's home space
/// (<c>48 89 5C 24 08</c> = <c>mov [rsp+8], rbx</c>, also rsi/rdi/rbp variants); Clang and GCC push them
/// (<c>55</c>, <c>41 57</c>, <c>41 56</c> …) and rarely use home-space stores.
/// </summary>
public static class PrologueStats
{
    public static (int Msvc, int Clang) Count(ReadOnlySpan<byte> file, IEnumerable<int>? functionOffsets, int sample = 4000)
    {
        if (functionOffsets is null) return (0, 0);
        int msvc = 0, clang = 0, n = 0;
        foreach (var off in functionOffsets)
        {
            if (++n > sample) break;
            if (off < 0 || off + 8 > file.Length) continue;
            var p = file.Slice(off, 8);
            // mov [rsp+8|10h|18h|20h], rbx/rbp/rsi/rdi  (48 89 xx 24 disp8 with ModRM 0x5C/0x6C/0x74/0x7C)
            if (p[0] == 0x48 && p[1] == 0x89 && p[2] is 0x5C or 0x6C or 0x74 or 0x7C && p[3] == 0x24 && p[4] is 0x08 or 0x10 or 0x18 or 0x20) msvc++;
            // push rbp / push r15/r14 at entry, the Clang/GCC register-save idiom
            else if (p[0] == 0x55 || p[0] == 0x41 && p[1] is 0x57 or 0x56 or 0x55 or 0x54) clang++;
        }
        return (msvc, clang);
    }
}

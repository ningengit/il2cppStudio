using System.Runtime.InteropServices;
using System.Text;

namespace Il2CppStudio.Core.Disassembly;

/// <summary>
/// ARMv7 (Thumb-2) text disassembly through the native Capstone library (vendored capstone.dll, P/Invoke only:
/// cs_open / cs_disasm / cs_free / cs_close). The <c>cs_insn</c> layout differs between Capstone 4 (16 instruction
/// bytes) and 5 (24 bytes); the major version from <c>cs_version</c> selects it at runtime.
/// IL2CPP ARMv7 code is Thumb-2 (method pointers carry bit 0), so Thumb mode is used.
/// </summary>
public sealed partial class CapstoneArmDisassembler : ITextDisassembler, IDisposable
{
    private const int ArchArm = 0, ModeArm = 0, ModeThumb = 1 << 4;
    private readonly nint _handle;
    private readonly nint _armHandle;
    private readonly int _bytesField;

    public string Name => $"ARMv7 (Capstone {_major}, {_lastMode})";
    public Func<ulong, string?>? SymbolResolver { get; set; }
    private readonly int _major;
    private string _lastMode = "Thumb-2";

    private CapstoneArmDisassembler(nint thumb, nint arm, int major) { _handle = thumb; _armHandle = arm; _major = major; _bytesField = major >= 5 ? 24 : 16; }

    /// <summary>Null when capstone.dll is missing or cannot open an ARM handle.</summary>
    public static CapstoneArmDisassembler? TryCreate()
    {
        try
        {
            int major = 0, minor = 0;
            _ = cs_version(ref major, ref minor);
            if (cs_open(ArchArm, ModeThumb, out var thumb) != 0) return null;
            return cs_open(ArchArm, ModeArm, out var arm) == 0 ? new CapstoneArmDisassembler(thumb, arm, major) : null;
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException) { return null; }
    }

    /// <summary>
    /// The instruction set is not recorded once the Thumb bit is cleared from the method pointer, so both decoders run
    /// and the one that decodes further (Thumb-2 or A32) wins.
    /// </summary>
    public string Disassemble(ReadOnlySpan<byte> code, ulong va)
    {
        var thumb = Decode(_handle, code, va, out var thumbBytes);
        var arm = Decode(_armHandle, code, va, out var armBytes);
        _lastMode = armBytes > thumbBytes ? "A32" : "Thumb-2";
        return armBytes > thumbBytes ? arm : thumb;
    }

    private string Decode(nint handle, ReadOnlySpan<byte> code, ulong va, out int decodedBytes)
    {
        var bytes = code.ToArray();
        var count = (int)cs_disasm(handle, bytes, (nuint)bytes.Length, va, 0, out var insns);
        var sb = new StringBuilder();
        // cs_insn: uint id; [pad]; ulong address @8; ushort size @16; byte bytes[N] @18; char mnemonic[32]; char op_str[160]; [pad]; cs_detail*.
        var mnemonicOff = 18 + _bytesField;
        var opOff = mnemonicOff + 32;
        var stride = Align(opOff + 160, 8) + 8;
        for (var i = 0; i < count; i++)
        {
            var p = insns + i * stride;
            var address = (ulong)Marshal.ReadInt64(p, 8);
            var mnemonic = Marshal.PtrToStringAnsi(p + mnemonicOff) ?? "";
            var ops = Marshal.PtrToStringAnsi(p + opOff) ?? "";
            var line = $"{address:X8}  {mnemonic,-10}{ops}";
            if (mnemonic.StartsWith("bl", StringComparison.Ordinal) && ops.StartsWith('#')
                && ulong.TryParse(ops.AsSpan(3), System.Globalization.NumberStyles.HexNumber, null, out var target) && SymbolResolver?.Invoke(target) is { } name)
                line += $"    ; {name}";
            sb.AppendLine(line);
        }
        var decoded = count == 0 ? 0 : (int)((ulong)Marshal.ReadInt64(insns + (count - 1) * stride, 8) - va);
        if (count > 0) cs_free(insns, (nuint)count);
        decodedBytes = decoded;
        if (count == 0 || decoded + 2 < bytes.Length - 4) sb.AppendLine($"; decoding stopped after {count} instruction(s) (data or unsupported encoding follows)");
        return sb.ToString();
    }

    private static int Align(int v, int a) => (v + a - 1) / a * a;

    public void Dispose() { var h = _handle; _ = cs_close(ref h); var a = _armHandle; _ = cs_close(ref a); }

    [LibraryImport("capstone")] private static partial int cs_version(ref int major, ref int minor);
    [LibraryImport("capstone")] private static partial int cs_open(int arch, int mode, out nint handle);
    [LibraryImport("capstone")] private static partial int cs_close(ref nint handle);
    [LibraryImport("capstone")] private static partial nuint cs_disasm(nint handle, [In] byte[] code, nuint size, ulong address, nuint count, out nint insn);
    [LibraryImport("capstone")] private static partial void cs_free(nint insn, nuint count);
}

using AsmResolver.DotNet.Signatures;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Native calling conventions understood by the lifters.</summary>
public enum NativeAbi { Win64, SysV, Wasm32, Win32 }

/// <summary>
/// Argument locations for the active calling convention (<see cref="Abi"/>, per lifting thread).
/// <list type="bullet">
/// <item>Win64: position n uses RCX/RDX/R8/R9, or XMM0-3 for float/double (positions shared); n ≥ 4 is at
/// [rsp + 0x20 + 8·(n−4)] at the call site (shadow space).</item>
/// <item>Win32 (cdecl): every argument is pushed on the stack from right to left; at call entry argument word w
/// sits at [esp + 4 + 4·w] and at call sites at [esp + 4·w].</item>
/// <item>System V AMD64: integers use RDI, RSI, RDX, RCX, R8, R9 and floats XMM0-7, counted independently; the rest
/// go to the stack in order from [rsp] at the call site (no shadow space). The per-signature float layout
/// (<see cref="Layout"/>) decides which counter a position advances.</item>
/// </list>
/// </summary>
public static class ArgumentLocations
{
    private static readonly Register[] IntRegs = [Register.RCX, Register.RDX, Register.R8, Register.R9];
    private static readonly Register[] FloatRegs = [Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3];
    private static readonly Register[] SysVInt = [Register.RDI, Register.RSI, Register.RDX, Register.RCX, Register.R8, Register.R9];
    private static readonly Register[] SysVFloat = [Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, Register.XMM5, Register.XMM6, Register.XMM7];

    public readonly record struct ArgSlot(Register Reg, int StackIndex, bool IsFloat);

    [ThreadStatic] private static NativeAbi _abi;
    [ThreadStatic] private static bool[]? _layout;
    [ThreadStatic] private static MethodShape.SysVArgSpec[]? _sysVLayout;
    [ThreadStatic] private static int[]? _win32Words;

    /// <summary>Calling convention of the method being lifted on this thread.</summary>
    public static NativeAbi Abi { get => _abi; set => _abi = value; }

    /// <summary>Pointer size in bytes for the active calling convention (4 on Win32/Wasm32, 8 on 64-bit).</summary>
    public static int PointerSize => _abi is NativeAbi.Win32 or NativeAbi.Wasm32 ? 4 : 8;

    /// <summary>Float/int kind of each argument position of the signature being bound (plus SysV/Win32 layouts).</summary>
    public static void Layout(bool[] isFloatByPosition, MethodShape.SysVArgSpec[]? sysV = null, int[]? win32Words = null)
    {
        _layout = isFloatByPosition;
        _sysVLayout = sysV;
        _win32Words = win32Words;
    }

    /// <summary>Integer-class register used by the hidden return buffer / `this` / pointer arguments of the first position.</summary>
    public static Register FirstIntRegister => _abi switch { NativeAbi.SysV => Register.RDI, NativeAbi.Win32 => Register.None, _ => Register.RCX };

    /// <summary>The n-th integer argument register (n &lt; 4) of a helper call taking only integer/pointer arguments.</summary>
    public static Register IntArg(int n, NativeAbi abi) => abi switch { NativeAbi.SysV => SysVInt[n], NativeAbi.Win32 => Register.None, _ => IntRegs[n] };

    public static Register IntArg(int n) => IntArg(n, _abi);

    /// <summary>Every argument register of the convention (liveness at unknown call targets).</summary>
    public static Register[] AllArgRegisters(NativeAbi abi) => abi switch { NativeAbi.SysV => [.. SysVInt, .. SysVFloat], NativeAbi.Win32 => [], _ => [.. IntRegs, .. FloatRegs] };

    public static Register IntRegister(int position) => Locate(position, false) is (var r, _) ? r : Register.None;

    public static Register FloatRegister(int position) => Locate(position, true) is (var r, _) ? r : Register.None;

    /// <summary>Reads helper argument <paramref name="index"/> (from register on 64-bit, or [esp+4*index] on Win32).</summary>
    public static Expr ReadHelperArg(RegisterTracker regs, int index) => _abi == NativeAbi.Win32
        ? regs.GetSlot(regs.SlotKey(4L * index)) ?? new NativeExpr($"arg{index}")
        : regs.Get(IntArg(index));

    public static bool TryReadHelperArg(RegisterTracker regs, int index, out Expr value)
    {
        if (_abi == NativeAbi.Win32)
        {
            if (regs.GetSlot(regs.SlotKey(4L * index)) is { } e) { value = e; return true; }
            value = null!;
            return false;
        }
        return regs.TryGet(IntArg(index), out value);
    }

    /// <summary>All register or stack slots used by argument <paramref name="position"/> (1 for scalars, 1-N eightbytes for SysV structs).</summary>
    public static IReadOnlyList<ArgSlot> LocateAll(int position, MethodShape.SysVArgSpec? specOverride = null)
    {
        if (_abi != NativeAbi.SysV)
        {
            var isFloat = specOverride is { Classes.Length: > 0 } so ? so.Classes[0] : _layout is { } l && position < l.Length && l[position];
            var (reg, si) = Locate(position, isFloat);
            return [new ArgSlot(reg, si, isFloat)];
        }
        int ints = 0, floats = 0, stack = 0;
        for (var p = 0; p <= position; p++)
        {
            MethodShape.SysVArgSpec spec = p == position && specOverride is { } ov ? ov
                : _sysVLayout is { } sl && p < sl.Length ? sl[p]
                : new(false, [_layout is { } fl && p < fl.Length && fl[p]]);
            var needF = spec.Classes.Count(c => c);
            var needI = spec.Classes.Length - needF;
            var useRegs = !spec.StackOnly && floats + needF <= SysVFloat.Length && ints + needI <= SysVInt.Length;
            var assigned = new ArgSlot[spec.Classes.Length];
            for (var c = 0; c < spec.Classes.Length; c++)
            {
                var f = spec.Classes[c];
                assigned[c] = useRegs
                    ? new ArgSlot(f ? SysVFloat[floats++] : SysVInt[ints++], -1, f)
                    : new ArgSlot(Register.None, stack++, f);
            }
            if (p == position) return assigned;
        }
        return [];
    }

    /// <summary>Register, or (None, stack index) for stack-passed arguments.</summary>
    private static (Register Reg, int StackIndex) Locate(int position, bool isFloat)
    {
        if (_abi == NativeAbi.Win32)
            return (Register.None, _win32Words is { } wo && position < wo.Length ? wo[position] : position);
        if (_abi == NativeAbi.Win64)
            return position < 4 ? ((isFloat ? FloatRegs : IntRegs)[position], -1) : (Register.None, position - 4);
        var slots = LocateAll(position, new MethodShape.SysVArgSpec(false, [isFloat]));
        return slots.Count > 0 ? (slots[0].Reg, slots[0].StackIndex) : (Register.None, 0);
    }

    /// <summary>Reads the value of argument <paramref name="position"/> at a call site.</summary>
    public static Expr Read(RegisterTracker regs, int position, bool isFloat)
    {
        var (reg, stackIndex) = Locate(position, isFloat);
        return ReadSlot(regs, reg, stackIndex, position);
    }

    public static Expr ReadSlot(RegisterTracker regs, ArgSlot slot, int position) => ReadSlot(regs, slot.Reg, slot.StackIndex, position);

    private static Expr ReadSlot(RegisterTracker regs, Register reg, int stackIndex, int position)
    {
        if (reg != Register.None) return regs.Get(reg);
        if (_abi == NativeAbi.Win32)
        {
            var wkey = regs.SlotKey(4L * stackIndex);
            return regs.GetSlot(wkey) ?? new NativeExpr($"stack_arg{position}");
        }
        var key = regs.SlotKey((_abi == NativeAbi.Win64 ? 0x20 : 0) + 8L * stackIndex);
        return regs.GetSlot(key) ?? new NativeExpr($"stack_arg{position}");
    }

    /// <summary>Binds incoming argument <paramref name="position"/> at function entry (register or stack slot).</summary>
    public static void Assign(RegisterTracker regs, int position, Expr value, bool isFloat)
    {
        var (reg, stackIndex) = Locate(position, isFloat);
        AssignSlot(regs, new ArgSlot(reg, stackIndex, isFloat), value);
    }

    public static void AssignSlot(RegisterTracker regs, ArgSlot slot, Expr value)
    {
        if (slot.Reg != Register.None) regs.Set(slot.Reg, value);
        else regs.SetSlot(IncomingStackKey(slot.StackIndex), value);
    }

    /// <summary>Incoming stack argument slot at function entry (before any push): return address, then arguments.</summary>
    public static long IncomingStackKey(int stackIndex) => _abi switch
    {
        NativeAbi.Win64 => 8 + 8L * (stackIndex + 4),
        NativeAbi.Win32 => 4 + 4L * stackIndex,
        _ => 8 + 8L * stackIndex,
    };

    public static bool IsFloat(TypeSignature t) => MethodShape.IsFloat(t);
}

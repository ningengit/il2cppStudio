using AsmResolver.DotNet.Signatures;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>x86-64 calling conventions understood by the lifter.</summary>
public enum NativeAbi { Win64, SysV }

/// <summary>
/// Argument locations for the active calling convention (<see cref="Abi"/>, per lifting thread).
/// <list type="bullet">
/// <item>Win64: position n uses RCX/RDX/R8/R9, or XMM0-3 for float/double (positions shared); n ≥ 4 is at
/// [rsp + 0x20 + 8·(n−4)] at the call site (shadow space).</item>
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

    [ThreadStatic] private static NativeAbi _abi;
    [ThreadStatic] private static bool[]? _layout;

    /// <summary>Calling convention of the method being lifted on this thread.</summary>
    public static NativeAbi Abi { get => _abi; set => _abi = value; }

    /// <summary>Float/int kind of each argument position of the signature being bound (SysV register assignment).</summary>
    public static void Layout(bool[] isFloatByPosition) => _layout = isFloatByPosition;

    /// <summary>Integer-class register used by the hidden return buffer / `this` / pointer arguments of the first position.</summary>
    public static Register FirstIntRegister => _abi == NativeAbi.SysV ? Register.RDI : Register.RCX;

    /// <summary>The n-th integer argument register (n &lt; 4) of a helper call taking only integer/pointer arguments.</summary>
    public static Register IntArg(int n, NativeAbi abi) => abi == NativeAbi.SysV ? SysVInt[n] : IntRegs[n];

    public static Register IntArg(int n) => IntArg(n, _abi);

    /// <summary>Every argument register of the convention (liveness at unknown call targets).</summary>
    public static Register[] AllArgRegisters(NativeAbi abi) => abi == NativeAbi.SysV ? [.. SysVInt, .. SysVFloat] : [.. IntRegs, .. FloatRegs];

    public static Register IntRegister(int position) => Locate(position, false) is (var r, _) ? r : Register.None;

    public static Register FloatRegister(int position) => Locate(position, true) is (var r, _) ? r : Register.None;

    /// <summary>Register, or (None, stack index) for stack-passed arguments.</summary>
    private static (Register Reg, int StackIndex) Locate(int position, bool isFloat)
    {
        if (_abi == NativeAbi.Win64)
            return position < 4 ? ((isFloat ? FloatRegs : IntRegs)[position], -1) : (Register.None, position - 4);
        int ints = 0, floats = 0, stack = 0;
        for (var p = 0; p <= position; p++)
        {
            var f = p == position ? isFloat : _layout is { } l && p < l.Length && l[p];
            Register reg = Register.None;
            if (f && floats < SysVFloat.Length) reg = SysVFloat[floats++];
            else if (!f && ints < SysVInt.Length) reg = SysVInt[ints++];
            if (p == position) return (reg, reg == Register.None ? stack : -1);
            if (reg == Register.None) stack++;
        }
        return (Register.None, stack);
    }

    /// <summary>Reads the value of argument <paramref name="position"/> at a call site.</summary>
    public static Expr Read(RegisterTracker regs, int position, bool isFloat)
    {
        var (reg, stackIndex) = Locate(position, isFloat);
        if (reg != Register.None) return regs.Get(reg);
        var key = regs.SlotKey((_abi == NativeAbi.Win64 ? 0x20 : 0) + 8L * stackIndex);
        return regs.GetSlot(key) ?? new NativeExpr($"stack_arg{position}");
    }

    /// <summary>Binds incoming argument <paramref name="position"/> at function entry (register or stack slot).</summary>
    public static void Assign(RegisterTracker regs, int position, Expr value, bool isFloat)
    {
        var (reg, stackIndex) = Locate(position, isFloat);
        if (reg != Register.None) regs.Set(reg, value);
        else regs.SetSlot(IncomingStackKey(stackIndex), value);
    }

    /// <summary>Incoming stack argument slot at function entry (before any push): return address, then Win64 home area.</summary>
    public static long IncomingStackKey(int stackIndex) => _abi == NativeAbi.Win64 ? 8 + 8L * (stackIndex + 4) : 8 + 8L * stackIndex;

    public static bool IsFloat(TypeSignature t) => MethodShape.IsFloat(t);
}

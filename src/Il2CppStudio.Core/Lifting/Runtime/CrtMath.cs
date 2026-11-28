using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// C runtime math functions that IL2CPP-generated code calls directly (Mathf inlines to sqrtf/sin/…; MSVC emits a
/// call to the CRT for slow paths). Mapped to System.Math with arguments taken from XMM0/XMM1. Names come from
/// exports or the PDB (Enhanced mode).
/// </summary>
public static class CrtMath
{
    private static readonly Dictionary<string, (string Math, int Args, bool Single)> Functions = new(StringComparer.Ordinal)
    {
        ["sqrt"] = ("Sqrt", 1, false), ["sqrtf"] = ("Sqrt", 1, true),
        ["sin"] = ("Sin", 1, false), ["sinf"] = ("Sin", 1, true), ["cos"] = ("Cos", 1, false), ["cosf"] = ("Cos", 1, true),
        ["tan"] = ("Tan", 1, false), ["tanf"] = ("Tan", 1, true), ["asin"] = ("Asin", 1, false), ["asinf"] = ("Asin", 1, true),
        ["acos"] = ("Acos", 1, false), ["acosf"] = ("Acos", 1, true), ["atan"] = ("Atan", 1, false), ["atanf"] = ("Atan", 1, true),
        ["atan2"] = ("Atan2", 2, false), ["atan2f"] = ("Atan2", 2, true), ["pow"] = ("Pow", 2, false), ["powf"] = ("Pow", 2, true),
        ["exp"] = ("Exp", 1, false), ["expf"] = ("Exp", 1, true), ["log"] = ("Log", 1, false), ["logf"] = ("Log", 1, true),
        ["log10"] = ("Log10", 1, false), ["log10f"] = ("Log10", 1, true), ["floor"] = ("Floor", 1, false), ["floorf"] = ("Floor", 1, true),
        ["ceil"] = ("Ceiling", 1, false), ["ceilf"] = ("Ceiling", 1, true), ["fabs"] = ("Abs", 1, false), ["fabsf"] = ("Abs", 1, true),
        ["fmod"] = ("IEEERemainder", 2, false),
    };

    public static bool IsKnown(string name) => Functions.ContainsKey(name) || name is "modf" or "modff";

    public static int ArgCount(string name) => Functions.TryGetValue(name, out var fn) ? fn.Args : name is "modf" or "modff" ? 2 : 0;

    /// <summary>Emits <c>modf(x, double* intpart)</c> → stores <c>Math.Truncate(x)</c> into intpart and leaves fractional part in XMM0.</summary>
    public static bool TryEmitModF(string name, EmitContext ctx, RegisterTracker regs, MemoryResolver mem, StructSlotTable slots, CorLibTypeFactory f)
    {
        if (name is not ("modf" or "modff")) return false;
        var single = name == "modff";
        var targetType = single ? f.Single : f.Double;
        var x = regs.Get(Register.XMM0);
        var xDbl = single || x.Kind != ValueKind.R8 ? new ConvExpr(x, f.Double) : x;
        var outPtr = ArgumentLocations.ReadHelperArg(regs, 1);

        Expr truncExpr = new MathCallExpr("Truncate", f.Double, xDbl);
        if (single) truncExpr = new ConvExpr(truncExpr, f.Single);
        var truncLocal = regs.NewLocal(targetType);
        Expr.EmitAs(ctx, truncExpr, targetType);
        ctx.IL.Add(CilOpCodes.Stloc, truncLocal);

        var truncVal = new LocalExpr(truncLocal);
        if (outPtr is StackAddrPlaceholder sp)
        {
            slots.Forget(sp.Key);
            for (var inner = sp.Key + 1; inner < sp.Key + (single ? 4 : 8); inner++) { regs.ClearSlot(inner); slots.Forget(inner); }
            regs.SetSlot(sp.Key, truncVal);
        }
        else if (outPtr is AddressExpr or { Type: ByReferenceTypeSignature })
        {
            mem.Store(ctx, outPtr, null, 1, 0, single ? 4 : 8, truncVal);
        }

        var xVal = regs.NewLocal(targetType);
        Expr.EmitAs(ctx, x, targetType);
        ctx.IL.Add(CilOpCodes.Stloc, xVal);

        var fracLocal = regs.NewLocal(targetType);
        var diff = new BinaryExpr(CilOpCodes.Sub, new LocalExpr(xVal), truncVal, single ? ValueKind.R4 : ValueKind.R8);
        diff.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Stloc, fracLocal);

        regs.ClobberVolatiles(CallEmitter.Volatile);
        regs.Set(Register.XMM0, new LocalExpr(fracLocal));
        return true;
    }

    /// <summary>Builds the System.Math call for a CRT function, or null when the name is not a known math function.</summary>
    public static Expr? Build(string name, RegisterTracker regs, CorLibTypeFactory f)
    {
        if (!Functions.TryGetValue(name, out var fn)) return null;
        // System.Math works on double; float CRT variants are wrapped in conversions.
        var args = new Expr[fn.Args];
        for (var i = 0; i < fn.Args; i++)
        {
            var x = regs.Get(i == 0 ? Register.XMM0 : Register.XMM1);
            args[i] = fn.Single ? new ConvExpr(x, f.Double) : x;
        }
        Expr call = new MathCallExpr(fn.Math, f.Double, args);
        return fn.Single ? new ConvExpr(call, f.Single) : call;
    }
}

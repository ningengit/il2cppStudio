using AsmResolver.DotNet.Signatures;
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

    public static bool IsKnown(string name) => Functions.ContainsKey(name);

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

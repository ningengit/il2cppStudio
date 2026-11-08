using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Flag state produced by cmp / test / comiss / flag-setting arithmetic.</summary>
public enum FlagSource { None, Compare, Test, FloatCompare, Result }

public readonly record struct FlagState(FlagSource Source, Expr? Left, Expr? Right);

/// <summary>
/// Maps x86 condition codes to <see cref="CompareExpr"/> conditions given the current flag state.
/// Returns null for parity conditions (NaN checks after ucomiss), which are treated as not taken.
/// </summary>
public static class ConditionMapper
{
    public static CompareExpr? Map(FlagState flags, ConditionCode cc)
    {
        var zero32 = new ConstExpr(0, ValueKind.I4);
        switch (flags.Source)
        {
            case FlagSource.Compare when flags.Left is { } a && flags.Right is { } b:
                return Cmp(cc, a, b, floating: false);
            case FlagSource.FloatCompare when flags.Left is { } a && flags.Right is { } b:
                return Cmp(cc, a, b, floating: true);
            case FlagSource.Test when flags.Left is { } a && flags.Right is { } b:
            {
                var value = ReferenceEquals(a, b) || SameRegisterValue(a, b) ? a : new BinaryExpr(AsmResolver.PE.DotNet.Cil.CilOpCodes.And, a, b, a.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);
                return ZeroTest(cc, value, zero32);
            }
            case FlagSource.Result when flags.Left is { } r:
                return ZeroTest(cc, r, zero32);
            default:
                return null;
        }
    }

    private static bool SameRegisterValue(Expr a, Expr b) =>
        a is LocalExpr la && b is LocalExpr lb && la.Local == lb.Local;

    private static CompareExpr? ZeroTest(ConditionCode cc, Expr value, Expr zero) => cc switch
    {
        ConditionCode.e => new CompareExpr(Condition.Eq, value, zero),
        ConditionCode.ne => new CompareExpr(Condition.Ne, value, zero),
        ConditionCode.s => new CompareExpr(Condition.Lt, value, zero),
        ConditionCode.ns => new CompareExpr(Condition.Ge, value, zero),
        ConditionCode.le => new CompareExpr(Condition.Le, value, zero),
        ConditionCode.g => new CompareExpr(Condition.Gt, value, zero),
        _ => null,
    };

    /// <summary>Signed codes (l/ge/le/g) and unsigned codes (b/ae/be/a). comiss reports through the unsigned codes.</summary>
    private static CompareExpr? Cmp(ConditionCode cc, Expr a, Expr b, bool floating) => cc switch
    {
        ConditionCode.e => new CompareExpr(Condition.Eq, a, b),
        ConditionCode.ne => new CompareExpr(Condition.Ne, a, b),
        ConditionCode.l => new CompareExpr(Condition.Lt, a, b),
        ConditionCode.ge => new CompareExpr(Condition.Ge, a, b),
        ConditionCode.le => new CompareExpr(Condition.Le, a, b),
        ConditionCode.g => new CompareExpr(Condition.Gt, a, b),
        ConditionCode.b => new CompareExpr(floating ? Condition.Lt : Condition.LtUn, a, b),
        ConditionCode.ae => new CompareExpr(floating ? Condition.Ge : Condition.GeUn, a, b),
        ConditionCode.be => new CompareExpr(floating ? Condition.Le : Condition.LeUn, a, b),
        ConditionCode.a => new CompareExpr(floating ? Condition.Gt : Condition.GtUn, a, b),
        _ => null,
    };

    /// <summary>True when a condition reads IL2CPP runtime state (class-init flags, cctor status) rather than program data.</summary>
    public static bool IsRuntimeCheck(CompareExpr c) => Involves(c.Left) || Involves(c.Right);

    private static bool Involves(Expr e) => e switch
    {
        MarkerExpr => true,
        NativeExpr n => n.Text.Contains("TypeInfo", StringComparison.Ordinal) || n.Text.Contains("static_fields", StringComparison.Ordinal)
                        || n.Text.Contains("->klass", StringComparison.Ordinal),
        // Bit tests of class flags (2018: `test byte [klass+127h], 2` = has_cctor) wrap the read in and/conv.
        BinaryExpr b => Involves(b.Left) || Involves(b.Right),
        ConvExpr cv => Involves(cv.Operand),
        _ => false,
    };
}

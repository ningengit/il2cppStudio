using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Evaluation-stack category of a lifted value.</summary>
public enum ValueKind { I4, I8, R4, R8, Ref, Struct, Native, Void }

/// <summary>Emission context handed to expressions.</summary>
public sealed class EmitContext(CilInstructionCollection il, ModuleDefinition module, NativeIntrinsics intrinsics)
{
    /// <summary>Receives type-conversion / fallback notes for the per-assembly error log (never written into C#).</summary>
    public Action<string>? Log { get; init; }

    public CilInstructionCollection IL => il;
    public ModuleDefinition Module => module;
    public NativeIntrinsics Intrinsics => intrinsics;

    public void Native(string text)
    {
        il.Add(CilOpCodes.Ldstr, text);
        il.Add(CilOpCodes.Call, intrinsics.Asm);
    }
}

/// <summary>A symbolic value produced while lifting a basic block (expression tree over registers/memory).</summary>
public abstract class Expr
{
    public abstract ValueKind Kind { get; }
    public virtual TypeSignature? Type => null;
    public abstract void Emit(EmitContext ctx);
    /// <summary>True for values that are only meaningful as addresses (TypeInfo, static base, ...), never as CIL values.</summary>
    public virtual bool IsMarker => false;
    public virtual string Describe() => GetType().Name;

    public static ValueKind KindOf(TypeSignature? t)
    {
        if (t is null) return ValueKind.I8;
        if (EnumRegistry.TryGetUnderlying(t, out var underlying)) return underlying;
        return t.ElementType switch
        {
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Boolean or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Char
                or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I1 or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U1
                or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I2 or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U2
                or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I4 or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U4 => ValueKind.I4,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I8 or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U8 => ValueKind.I8,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.R4 => ValueKind.R4,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.R8 => ValueKind.R8,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U
                or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Ptr => ValueKind.Native,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Void => ValueKind.Void,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.ValueType => ValueKind.Struct,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.GenericInst => t.IsValueType ? ValueKind.Struct : ValueKind.Ref,
            AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Var or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.MVar => ValueKind.Ref,
            _ => ValueKind.Ref,
        };
    }

    /// <summary>Emits <paramref name="e"/> converted to <paramref name="target"/> (numeric conversions or reinterpretation).</summary>
    /// <param name="numeric">True only for explicit conversions (cvt*): int?float is then a value conversion, not a bit move.</param>
    public static void EmitAs(EmitContext ctx, Expr e, TypeSignature target, bool numeric = false)
    {
        var tk = KindOf(target);
        var sk = e.Kind;
        if (e is ConstExpr { Value: 0 } && tk == ValueKind.Ref) { ctx.IL.Add(CilOpCodes.Ldnull); return; }
        if (!numeric && IsBitMove(sk, tk))
        {
            // x86 moves floats through GPRs/immediates as raw bits (mov [mem], 41F00000h == 30f); only cvt* converts.
            switch (e)
            {
                case ConstExpr c when tk == ValueKind.R4: ctx.IL.Add(CilOpCodes.Ldc_R4, BitConverter.Int32BitsToSingle(unchecked((int)c.Value))); return;
                case ConstExpr c when tk == ValueKind.R8: ctx.IL.Add(CilOpCodes.Ldc_R8, BitConverter.Int64BitsToDouble(c.Value)); return;
                case FloatExpr f when tk == ValueKind.I4: ctx.IL.Add(CilOpCodes.Ldc_I4, BitConverter.SingleToInt32Bits((float)f.Value)); return;
                case FloatExpr f when tk == ValueKind.I8: ctx.IL.Add(CilOpCodes.Ldc_I8, BitConverter.DoubleToInt64Bits(f.Value)); return;
            }
            e.Emit(ctx);
            ctx.IL.Add(CilOpCodes.Box, BoxType(ctx.Module, sk));
            ctx.IL.Add(CilOpCodes.Call, new MethodSpecification(ctx.Intrinsics.Reinterpret, new GenericInstanceMethodSignature([target])));
            ConversionLog.Record(ctx, $"bit reinterpretation {sk} -> {tk} of {e.Describe()} (non-constant raw-bit move)");
            return;
        }
        e.Emit(ctx);
        Convert(ctx, sk, tk, target);
    }

    /// <summary>Integer ↔ floating-point flows that are register/memory bit moves rather than numeric conversions.</summary>
    public static bool IsBitMove(ValueKind from, ValueKind to) =>
        (from is ValueKind.I4 or ValueKind.I8 && to is ValueKind.R4 or ValueKind.R8) || (from is ValueKind.R4 or ValueKind.R8 && to is ValueKind.I4 or ValueKind.I8);

    public static void Convert(EmitContext ctx, ValueKind from, ValueKind to, TypeSignature target)
    {
        if (from == to) return;
        var il = ctx.IL;
        switch (to)
        {
            case ValueKind.I4 when from is ValueKind.I8 or ValueKind.Native or ValueKind.R4 or ValueKind.R8:
                il.Add(target.ElementType == AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U4 ? CilOpCodes.Conv_U4 : CilOpCodes.Conv_I4); return;
            case ValueKind.I8 when from is ValueKind.I4 or ValueKind.Native or ValueKind.R4 or ValueKind.R8:
                il.Add(CilOpCodes.Conv_I8); return;
            case ValueKind.R4 when from is ValueKind.I4 or ValueKind.I8 or ValueKind.R8 or ValueKind.Native:
                il.Add(CilOpCodes.Conv_R4); return;
            case ValueKind.R8 when from is ValueKind.I4 or ValueKind.I8 or ValueKind.R4 or ValueKind.Native:
                il.Add(CilOpCodes.Conv_R8); return;
            case ValueKind.Native when from is ValueKind.I4 or ValueKind.I8:
                il.Add(CilOpCodes.Conv_I); return;
            case ValueKind.Ref or ValueKind.Struct when from is not (ValueKind.Ref or ValueKind.Struct):
            case ValueKind.Struct when from == ValueKind.Ref:
            case ValueKind.Ref when from == ValueKind.Struct:
                if (from is ValueKind.I4 or ValueKind.I8 or ValueKind.R4 or ValueKind.R8 or ValueKind.Native or ValueKind.Struct)
                    il.Add(CilOpCodes.Box, BoxType(ctx.Module, from));
                il.Add(CilOpCodes.Call, new MethodSpecification(ctx.Intrinsics.Reinterpret, new GenericInstanceMethodSignature([target])));
                return;
            case ValueKind.I4 or ValueKind.I8 or ValueKind.R4 or ValueKind.R8 or ValueKind.Native when from is ValueKind.Ref or ValueKind.Struct:
                if (from == ValueKind.Struct) il.Add(CilOpCodes.Box, ctx.Module.CorLibTypeFactory.Object.Type);
                il.Add(CilOpCodes.Call, new MethodSpecification(ctx.Intrinsics.Reinterpret, new GenericInstanceMethodSignature([target])));
                return;
        }
    }

    private static ITypeDefOrRef BoxType(ModuleDefinition m, ValueKind k) => k switch
    {
        ValueKind.I4 => m.CorLibTypeFactory.Int32.Type,
        ValueKind.R4 => m.CorLibTypeFactory.Single.Type,
        ValueKind.R8 => m.CorLibTypeFactory.Double.Type,
        ValueKind.Native => m.CorLibTypeFactory.IntPtr.Type,
        _ => m.CorLibTypeFactory.Int64.Type,
    };
}

public sealed class ConstExpr(long value, ValueKind kind, bool fromFloatBits = false) : Expr
{
    public long Value => value;
    public bool FromFloatBits => fromFloatBits;
    public override ValueKind Kind => kind;
    public override void Emit(EmitContext ctx)
    {
        if (kind == ValueKind.I8) ctx.IL.Add(CilOpCodes.Ldc_I8, value);
        else ctx.IL.Add(CilOpCodes.Ldc_I4, unchecked((int)value));
    }
    public override string Describe() => $"0x{value:X}";
}

public sealed class FloatExpr(double value, bool single) : Expr
{
    public double Value => value;
    public override ValueKind Kind => single ? ValueKind.R4 : ValueKind.R8;
    public override void Emit(EmitContext ctx)
    {
        if (single) ctx.IL.Add(CilOpCodes.Ldc_R4, (float)value);
        else ctx.IL.Add(CilOpCodes.Ldc_R8, value);
    }
}

public sealed class ArgExpr(AsmResolver.DotNet.Collections.Parameter parameter, TypeSignature type) : Expr
{
    public AsmResolver.DotNet.Collections.Parameter Parameter => parameter;
    public override TypeSignature Type => type;
    public override ValueKind Kind => KindOf(type);
    public override void Emit(EmitContext ctx) => ctx.IL.Add(CilOpCodes.Ldarg, parameter);
    public override string Describe() => parameter.Name ?? "this";
}

public sealed class LocalExpr(CilLocalVariable local) : Expr
{
    public CilLocalVariable Local => local;
    public override TypeSignature Type => local.VariableType;
    public override ValueKind Kind => KindOf(local.VariableType);
    public override void Emit(EmitContext ctx) => ctx.IL.Add(CilOpCodes.Ldloc, local);
}

/// <summary>Address of a stack slot holding a struct (hidden return buffers, by-ref struct arguments).</summary>
public sealed class StackAddrExpr(int offset, CilLocalVariable? local) : Expr
{
    public int Offset => offset;
    public CilLocalVariable? Local => local;
    public override ValueKind Kind => ValueKind.Native;
    public override void Emit(EmitContext ctx)
    {
        if (local is not null) ctx.IL.Add(CilOpCodes.Ldloc, local);
        else ctx.Native($"&stack[0x{offset:X}]");
    }
}

public sealed class FieldExpr(Expr instance, IFieldDescriptor fieldRef, TypeSignature type) : Expr
{
    public Expr Instance => instance;
    public IFieldDescriptor Field => fieldRef;
    public override TypeSignature Type => type;
    public override ValueKind Kind => KindOf(type);
    public override void Emit(EmitContext ctx)
    {
        if (instance is LocalExpr { Kind: ValueKind.Struct } l) ctx.IL.Add(CilOpCodes.Ldloca, l.Local);
        else instance.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Ldfld, fieldRef);
    }
}

public sealed class StaticFieldExpr(IFieldDescriptor fieldRef, TypeSignature type) : Expr
{
    public IFieldDescriptor Field => fieldRef;
    public override TypeSignature Type => type;
    public override ValueKind Kind => KindOf(type);
    public override void Emit(EmitContext ctx) => ctx.IL.Add(CilOpCodes.Ldsfld, fieldRef);
}

public sealed class StringExpr(string value, TypeSignature stringType) : Expr
{
    public override TypeSignature Type => stringType;
    public override ValueKind Kind => ValueKind.Ref;
    public override void Emit(EmitContext ctx) => ctx.IL.Add(CilOpCodes.Ldstr, value);
}

public sealed class ArrayLengthExpr(Expr array) : Expr
{
    public Expr Array => array;
    public override ValueKind Kind => ValueKind.I4;
    public override void Emit(EmitContext ctx)
    {
        array.Emit(ctx);
        ctx.IL.Add(CilOpCodes.Ldlen);
        ctx.IL.Add(CilOpCodes.Conv_I4);
    }
}

public sealed class ArrayElementExpr(Expr array, Expr index, TypeSignature elementType) : Expr
{
    public Expr Array => array;
    public Expr Index => index;
    public override TypeSignature Type => elementType;
    public override ValueKind Kind => KindOf(elementType);
    public override void Emit(EmitContext ctx)
    {
        array.Emit(ctx);
        EmitAs(ctx, index, ctx.Module.CorLibTypeFactory.Int32);
        ctx.IL.Add(CilOpCodes.Ldelem, elementType.ToTypeDefOrRef());
    }
}

public sealed class BinaryExpr(CilOpCode op, Expr left, Expr right, ValueKind kind) : Expr
{
    public Expr Left => left;
    public Expr Right => right;
    public CilOpCode Op => op;
    public override ValueKind Kind => kind;
    public override void Emit(EmitContext ctx)
    {
        EmitOperand(ctx, left);
        EmitOperand(ctx, right);
        ctx.IL.Add(op);
    }
    private void EmitOperand(EmitContext ctx, Expr e)
    {
        e.Emit(ctx);
        if (e.Kind != kind)
            Convert(ctx, e.Kind, kind, kind switch
            {
                ValueKind.I4 => ctx.Module.CorLibTypeFactory.Int32,
                ValueKind.R4 => ctx.Module.CorLibTypeFactory.Single,
                ValueKind.R8 => ctx.Module.CorLibTypeFactory.Double,
                _ => ctx.Module.CorLibTypeFactory.Int64,
            });
    }
}

public sealed class UnaryExpr(CilOpCode op, Expr operand) : Expr
{
    public Expr Operand => operand;
    public override ValueKind Kind => operand.Kind;
    public override void Emit(EmitContext ctx) { operand.Emit(ctx); ctx.IL.Add(op); }
}

public sealed class ConvExpr(Expr operand, TypeSignature target) : Expr
{
    public override TypeSignature Type => target;
    public override ValueKind Kind => KindOf(target);
    public Expr Operand => operand;
    public override void Emit(EmitContext ctx) => EmitAs(ctx, operand, target, numeric: true);
}

/// <summary>A boolean comparison (result of cmp/test + condition code).</summary>
public sealed class CompareExpr(Condition cond, Expr left, Expr right) : Expr
{
    public Condition Condition => cond;
    public Expr Left => left;
    public Expr Right => right;
    public override ValueKind Kind => ValueKind.I4;
    public override void Emit(EmitContext ctx)
    {
        var k = OperandKind(left, right);
        EmitOperand(ctx, left, k);
        EmitOperand(ctx, right, k);
        var il = ctx.IL;
        switch (cond)
        {
            case Condition.Eq: il.Add(CilOpCodes.Ceq); break;
            case Condition.Ne: il.Add(CilOpCodes.Ceq); il.Add(CilOpCodes.Ldc_I4_0); il.Add(CilOpCodes.Ceq); break;
            case Condition.Lt: il.Add(CilOpCodes.Clt); break;
            case Condition.LtUn: il.Add(CilOpCodes.Clt_Un); break;
            case Condition.Gt: il.Add(CilOpCodes.Cgt); break;
            case Condition.GtUn: il.Add(CilOpCodes.Cgt_Un); break;
            case Condition.Le: il.Add(CilOpCodes.Cgt); il.Add(CilOpCodes.Ldc_I4_0); il.Add(CilOpCodes.Ceq); break;
            case Condition.LeUn: il.Add(CilOpCodes.Cgt_Un); il.Add(CilOpCodes.Ldc_I4_0); il.Add(CilOpCodes.Ceq); break;
            case Condition.Ge: il.Add(CilOpCodes.Clt); il.Add(CilOpCodes.Ldc_I4_0); il.Add(CilOpCodes.Ceq); break;
            case Condition.GeUn: il.Add(CilOpCodes.Clt_Un); il.Add(CilOpCodes.Ldc_I4_0); il.Add(CilOpCodes.Ceq); break;
        }
    }

    public static ValueKind OperandKind(Expr a, Expr b)
    {
        if (a.Kind == ValueKind.Ref || b.Kind == ValueKind.Ref) return ValueKind.Ref;
        if (a.Kind is ValueKind.R4 or ValueKind.R8 || b.Kind is ValueKind.R4 or ValueKind.R8)
            return a.Kind == ValueKind.R8 || b.Kind == ValueKind.R8 ? ValueKind.R8 : ValueKind.R4;
        if (a.Kind == ValueKind.I8 || b.Kind == ValueKind.I8 || a.Kind == ValueKind.Native || b.Kind == ValueKind.Native) return ValueKind.I8;
        return ValueKind.I4;
    }

    public static void EmitOperand(EmitContext ctx, Expr e, ValueKind k)
    {
        if (k == ValueKind.Ref)
        {
            if (e is ConstExpr { Value: 0 }) { ctx.IL.Add(CilOpCodes.Ldnull); return; }
            e.Emit(ctx);
            if (e.Kind != ValueKind.Ref) EmitAsObject(ctx, e.Kind);
            return;
        }
        var f = ctx.Module.CorLibTypeFactory;
        EmitAs(ctx, e, k switch { ValueKind.I4 => f.Int32, ValueKind.R4 => f.Single, ValueKind.R8 => f.Double, _ => f.Int64 });
    }

    private static void EmitAsObject(EmitContext ctx, ValueKind from) =>
        Convert(ctx, from, ValueKind.Ref, ctx.Module.CorLibTypeFactory.Object);
}

public enum Condition { Eq, Ne, Lt, LtUn, Gt, GtUn, Le, LeUn, Ge, GeUn }

/// <summary>Value with no CIL equivalent; emitted as NativeIntrinsics.Asm("...") so the output stays honest.</summary>
public sealed class NativeExpr(string text, ValueKind kind = ValueKind.I8) : Expr
{
    public string Text => text;
    public override ValueKind Kind => kind == ValueKind.I8 ? ValueKind.I8 : kind;
    public override void Emit(EmitContext ctx)
    {
        ctx.Native(text);
        if (kind is ValueKind.I4) ctx.IL.Add(CilOpCodes.Conv_I4);
        else if (kind is ValueKind.R4) ctx.IL.Add(CilOpCodes.Conv_R4);
        else if (kind is ValueKind.R8) ctx.IL.Add(CilOpCodes.Conv_R8);
    }
    public override string Describe() => text;
}

/// <summary>Address-only runtime values (Il2CppClass*, static field base, MethodInfo*, vtable slot) used for pattern matching.</summary>
/// <remarks><paramref name="usage"/> is the originating runtime-metadata global (exact Il2CppType / method spec), when known.</remarks>
public sealed class MarkerExpr(MarkerKind marker, int index, string text, Expr? baseExpr = null, Registration.MetadataUsage? usage = null) : Expr
{
    public MarkerKind Marker => marker;
    public int Index => index;
    public Expr? Base => baseExpr;
    public Registration.MetadataUsage? Usage => usage;
    public override bool IsMarker => true;
    public override ValueKind Kind => ValueKind.Native;
    public override void Emit(EmitContext ctx) => ctx.Native(text);
    public override string Describe() => text;
}

public enum MarkerKind { TypeInfo, StaticFields, MethodInfo, Klass, VTableEntry, FieldInfo, Il2CppType }

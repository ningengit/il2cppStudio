using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Struct values that travel by pointer (by-ref struct arguments, hidden return buffers). When a call needs a
/// struct at <c>&amp;stack[k]</c>, the slot becomes a typed local and the field values previously stored
/// piecewise into <c>[rsp+k+offset]</c> are materialized as <c>stfld</c>s into that local.
/// </summary>
public sealed class StructBinder(RegisterTracker regs, StructSlotTable slots, MemoryResolver mem)
{
    /// <summary>The struct value an argument pointer refers to, or a reinterpretation fallback.</summary>
    public Expr BindValue(EmitContext ctx, Expr pointer, TypeSignature structType)
    {
        switch (pointer)
        {
            case AddressExpr a:
                return a.Target;
            case StackAddrPlaceholder p:
                // Whole-struct copy (e.g. movups/movdqa of a previous result): pass the source value directly.
                if (regs.GetSlot(p.Key) is StructChunkExpr { Offset: 0 } whole && whole.Size >= mem.ValueSize(structType)
                    && whole.Source.Type is { } st && SignatureComparer.Default.Equals(st, structType))
                    return whole.Source;
                if (WholeCopySource(p.Key, structType) is { } source) return source;
                return new LocalExpr(Materialize(ctx, p.Key, structType));
            case StructChunkExpr c when c.Offset == 0 && c.Size >= mem.ValueSize(structType):
                return c.Source;
            default:
                return pointer;
        }
    }

    /// <summary>
    /// When the slot was filled piecewise (8-byte chunks / single fields) entirely from one struct value of the same
    /// type at the same offsets — the native form of a struct copy — returns that source value.
    /// </summary>
    private Expr? WholeCopySource(long key, TypeSignature structType)
    {
        var td = mem.TypeDefOf(structType);
        if (td < 0) return null;
        Expr? source = null;
        foreach (var f in mem.FieldsOf(td).Where(f => !f.IsStatic))
        {
            Expr? from = null;
            for (var o = f.Offset; o >= 0 && o > f.Offset - 16 && from is null; o--)
            {
                switch (regs.GetSlot(key + o))
                {
                    case StructChunkExpr c when c.Offset == o && f.Offset < o + c.Size: from = c.Source; break;
                    case FieldExpr fe when o == f.Offset && fe.Field.Name == mem.FieldRef(f)?.Name: from = fe.Instance; break;
                    case not null when o == f.Offset: return null;   // some other value
                }
            }
            if (from is null || from.Type is not { } ft || !SignatureComparer.Default.Equals(ft, structType)) return null;
            if (source is null) source = from;
            else if (!SameValue(source, from)) return null;
        }
        return source;
    }

    private static bool SameValue(Expr a, Expr b) =>
        ReferenceEquals(a, b) || a is LocalExpr la && b is LocalExpr lb && la.Local == lb.Local;

    /// <summary>The address an argument pointer denotes, as an lvalue address expression when possible.</summary>
    public Expr BindAddress(EmitContext ctx, Expr pointer, TypeSignature structType) => pointer switch
    {
        StackAddrPlaceholder p => new AddressExpr(new LocalExpr(Materialize(ctx, p.Key, structType))),
        _ => pointer,
    };

    /// <summary>Creates (or reuses) the struct local for a stack slot and copies pending scalar stores into it.</summary>
    public AsmResolver.DotNet.Code.Cil.CilLocalVariable Materialize(EmitContext ctx, long key, TypeSignature structType)
    {
        // A struct local still bound to the slot (no store hit it since) is the slot's current value: reuse it, so an
        // `out` argument and later reads of the same stack struct refer to one variable.
        if (slots.TryFind(key, out var existing, out var at) && at == 0 && SignatureComparer.Default.Equals(existing.VariableType, structType))
            return existing;
        var local = slots.GetOrCreate(key, structType, mem.ValueSize(structType));
        var td = mem.TypeDefOf(structType);
        if (td < 0) return local;
        var target = new LocalExpr(local);
        foreach (var f in mem.FieldsOf(td).Where(f => !f.IsStatic))
        {
            var pending = regs.GetSlot(key + f.Offset);
            if (pending is null) continue;
            if (pending is StructChunkExpr chunk)
            {
                mem.Store(ctx, new AddressExpr(target), null, 1, f.Offset, chunk.Size, chunk);
                continue;
            }
            if (mem.StructField(target, td, f.Offset) is not FieldExpr fe) continue;
            new AddressExpr(fe.Instance).Emit(ctx);
            Expr.EmitAs(ctx, pending, fe.Type!);
            ctx.IL.Add(CilOpCodes.Stfld, fe.Field);
        }
        return local;
    }
}

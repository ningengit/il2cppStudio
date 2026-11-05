using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Address of an lvalue (struct local / argument / field), i.e. ldloca / ldarga / ldflda / ldsflda.</summary>
public sealed class AddressExpr(Expr target) : Expr
{
    public Expr Target => target;
    public override ValueKind Kind => ValueKind.Native;
    public override void Emit(EmitContext ctx)
    {
        switch (target)
        {
            case LocalExpr l: ctx.IL.Add(CilOpCodes.Ldloca, l.Local); break;
            case ArgExpr a: ctx.IL.Add(CilOpCodes.Ldarga, a.Parameter); break;
            case FieldExpr f:
                if (f.Instance is LocalExpr { Kind: ValueKind.Struct } sl) ctx.IL.Add(CilOpCodes.Ldloca, sl.Local);
                else f.Instance.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Ldflda, f.Field);
                break;
            case StaticFieldExpr s: ctx.IL.Add(CilOpCodes.Ldsflda, s.Field); break;
            default: ctx.Native("&" + target.Describe()); break;
        }
    }
}

/// <summary>An 8/16-byte chunk of a struct moved through a GPR/XMM register (struct copy idiom).</summary>
public sealed class StructChunkExpr(Expr source, int offset, int size) : Expr
{
    public Expr Source => source;
    public int Offset => offset;
    public int Size => size;
    public override ValueKind Kind => ValueKind.I8;
    public override void Emit(EmitContext ctx) => ctx.Native($"struct_chunk({source.Describe()}+0x{offset:X}, {size})");
}

/// <summary>A field matched by native offset, with its declaring type definition index.</summary>
public readonly record struct ResolvedField(int FieldIndex, int DeclaringTypeDef, int Offset, int TypeIndex, bool IsStatic);

/// <summary>
/// Resolves native memory operands against IL2CPP object layouts: instance fields (fieldOffsets, with the
/// 16-byte header removed for unboxed value types), static fields (Il2CppClass::static_fields + offset),
/// arrays (max_length at 0x18, data at 0x20), strings (length at 0x10), the klass pointer at [obj+0] and the
/// vtable (VirtualInvokeData[] at klass+0x138).
/// </summary>
public sealed class MemoryResolver
{
    public const int StaticFieldsOffset = 0xB8;
    public const int VTableOffset = 0x138;
    public const int ArrayLengthOffset = 0x18;
    public const int ArrayDataOffset = 0x20;
    public const int StringLengthOffset = 0x10;

    private readonly GlobalMetadata _md;
    private readonly MetadataModel _model;
    private readonly Il2CppRegistration _reg;
    private readonly TypeNameService _names;
    private readonly StubAssemblyBuilder _stubs;
    private readonly ModuleDefinition _module;
    private readonly Dictionary<string, int> _typeDefByName;
    private readonly Dictionary<int, List<ResolvedField>> _fieldsCache = [];

    public MemoryResolver(GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, TypeNameService names, StubAssemblyBuilder stubs,
        ModuleDefinition module, Dictionary<string, int> typeDefByName)
    {
        _md = md; _model = model; _reg = reg; _names = names; _stubs = stubs; _module = module; _typeDefByName = typeDefByName;
    }

    public static Dictionary<string, int> BuildTypeIndex(MetadataModel model)
    {
        var d = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var t in model.Types) d.TryAdd(t.FullName, t.Index);
        return d;
    }

    // ------------------------------------------------------------------ type helpers

    /// <summary>Typedef index of a signature's underlying type (generic instances → definition), or -1.</summary>
    public int TypeDefOf(TypeSignature? sig)
    {
        if (sig is null) return -1;
        if (sig is GenericInstanceTypeSignature g) sig = g.GenericType.ToTypeSignature(g.IsValueType);
        if (sig is not TypeDefOrRefSignature and not CorLibTypeSignature) return -1;
        var name = sig.FullName.Replace('/', '+');
        return _typeDefByName.GetValueOrDefault(name, -1);
    }

    public TypeSignature FieldType(ResolvedField f, TypeSignature? ownerSig)
    {
        var t = _stubs.ToSig(_module, f.TypeIndex);
        // Substitute class generic parameters when the owner is a closed generic instance.
        if (t is GenericParameterSignature { ParameterType: GenericParameterType.Type } gp && ownerSig is GenericInstanceTypeSignature gi
            && gp.Index < gi.TypeArguments.Count)
            return gi.TypeArguments[gp.Index];
        return t;
    }

    public IFieldDescriptor? FieldRef(ResolvedField f) => _stubs.FieldRefFor(_module, f.FieldIndex, f.DeclaringTypeDef);

    /// <summary>All fields of a type and its base types with native offsets (instance) or static-data offsets.</summary>
    public IReadOnlyList<ResolvedField> FieldsOf(int typeDef)
    {
        if (_fieldsCache.TryGetValue(typeDef, out var cached)) return cached;
        var list = new List<ResolvedField>();
        var seen = new HashSet<int>();
        var current = typeDef;
        while ((uint)current < (uint)_md.TypeDefinitions.Length && seen.Add(current))
        {
            var def = _md.TypeDefinitions[current];
            for (var k = 0; k < def.FieldCount; k++)
            {
                var fi = def.FieldStart + k;
                var fd = _md.Fields[fi];
                var isStatic = _reg.TryGetType(fd.TypeIndex, out var ft) && (ft.Attrs & 0x10) != 0;
                var isLiteral = (ft.Attrs & 0x40) != 0;
                if (isLiteral) continue;
                var off = _reg.GetFieldOffset(current, k);
                if (off < 0) continue;
                if (!isStatic && def.IsValueType) off -= 16;
                list.Add(new ResolvedField(fi, current, off, fd.TypeIndex, isStatic));
            }
            if (def.ParentIndex < 0 || !_reg.TryGetType(def.ParentIndex, out var parent)) break;
            current = _names.ResolveTypeDefIndex(parent);
        }
        _fieldsCache[typeDef] = list;
        return list;
    }

    public ResolvedField? FindInstanceField(int typeDef, long offset) =>
        FieldsOf(typeDef).Where(f => !f.IsStatic && f.Offset == offset).Cast<ResolvedField?>().FirstOrDefault();

    public ResolvedField? FindStaticField(int typeDef, long offset)
    {
        if ((uint)typeDef >= (uint)_md.TypeDefinitions.Length) return null;
        // Statics are per declaring type only (no inheritance).
        return FieldsOf(typeDef).Where(f => f.IsStatic && f.DeclaringTypeDef == typeDef && f.Offset == offset).Cast<ResolvedField?>().FirstOrDefault();
    }

    /// <summary>Field of a value type that starts at <paramref name="offset"/>, descending into nested structs.</summary>
    public Expr? StructField(Expr structValue, int typeDef, long offset, int depth = 0)
    {
        if (depth > 6) return null;
        var fields = FieldsOf(typeDef).Where(f => !f.IsStatic).OrderBy(f => f.Offset).ToList();
        foreach (var f in fields)
        {
            var ft = FieldType(f, structValue.Type);
            var size = ValueSize(ft);
            if (f.Offset == offset && FieldRef(f) is { } fr)
            {
                var fe = new FieldExpr(structValue, fr, ft);
                // Offset 0 of a nested struct: prefer the innermost scalar.
                var inner = TypeDefOf(ft);
                return inner >= 0 && _md.TypeDefinitions[inner].IsValueType && Expr.KindOf(ft) == ValueKind.Struct
                    ? StructField(fe, inner, 0, depth + 1) ?? fe
                    : fe;
            }
            if (offset > f.Offset && offset < f.Offset + size && Expr.KindOf(ft) == ValueKind.Struct && FieldRef(f) is { } nr)
            {
                var inner = TypeDefOf(ft);
                if (inner >= 0) return StructField(new FieldExpr(structValue, nr, ft), inner, offset - f.Offset, depth + 1);
            }
        }
        return null;
    }

    /// <summary>Byte size of a value of the given type as laid out natively.</summary>
    public int ValueSize(TypeSignature t)
    {
        switch (Expr.KindOf(t))
        {
            case ValueKind.I4:
                return t.ElementType switch
                {
                    AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Boolean or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I1
                        or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U1 => 1,
                    AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Char or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I2
                        or AsmResolver.PE.DotNet.Metadata.Tables.ElementType.U2 => 2,
                    _ => 4,
                };
            case ValueKind.R4: return 4;
            case ValueKind.Struct:
                var td = TypeDefOf(t);
                return td >= 0 ? Math.Max(1, _reg.GetValueTypeSize(td)) : 8;
            default: return 8;
        }
    }

    // ------------------------------------------------------------------ loads

    /// <summary>
    /// Value loaded from [base + index*scale + disp] with the given access size, or null when unrecognized.
    /// </summary>
    public Expr? Load(Expr baseExpr, Expr? index, int scale, long disp, int size)
    {
        // Address of a struct lvalue → its field (or a chunk for wide copies).
        if (baseExpr is AddressExpr addr && TypeDefOf(addr.Target.Type) is var std and >= 0)
        {
            if (index is not null) return null;
            var field = StructField(addr.Target, std, disp);
            if (field is not null && (size <= ValueSize(field.Type!) || size <= 4)) return field;
            return new StructChunkExpr(addr.Target, (int)disp, size);
        }

        if (baseExpr is MarkerExpr m)
        {
            switch (m.Marker)
            {
                case MarkerKind.TypeInfo when disp == StaticFieldsOffset && index is null:
                    return new MarkerExpr(MarkerKind.StaticFields, m.Index, $"{_names.TypeDefName(m.Index, true)}.static_fields");
                case MarkerKind.StaticFields when index is null:
                    if (FindStaticField(m.Index, disp) is { } sf && FieldRef(sf) is { } sfr)
                    {
                        var ft = FieldType(sf, null);
                        var se = new StaticFieldExpr(sfr, ft);
                        return size < ValueSize(ft) && Expr.KindOf(ft) == ValueKind.Struct ? StructFieldOrChunk(se, ft, 0, size) : se;
                    }
                    if (FindContainingStatic(m.Index, disp) is { } outer && FieldRef(outer) is { } ofr)
                    {
                        var oft = FieldType(outer, null);
                        return StructFieldOrChunk(new StaticFieldExpr(ofr, oft), oft, (int)(disp - outer.Offset), size);
                    }
                    return null;
                case MarkerKind.Klass when index is null && disp >= VTableOffset && (disp - VTableOffset) % 16 == 0:
                    return new MarkerExpr(MarkerKind.VTableEntry, (int)((disp - VTableOffset) / 16), $"vtable[{(disp - VTableOffset) / 16}]", m.Base);
                case MarkerKind.Klass when index is null && disp >= VTableOffset && (disp - VTableOffset) % 16 == 8:
                    return new MarkerExpr(MarkerKind.MethodInfo, (int)((disp - VTableOffset) / 16), $"vtable[{(disp - VTableOffset) / 16}].method", m.Base);
            }
            return null;
        }

        var kind = baseExpr.Kind;
        var type = baseExpr.Type;
        if (kind != ValueKind.Ref || type is null) return null;

        if (disp == 0 && index is null && size == 8)
            return new MarkerExpr(MarkerKind.Klass, TypeDefOf(type), $"{baseExpr.Describe()}->klass", baseExpr);

        if (type is SzArrayTypeSignature arr)
        {
            if (index is null && disp == ArrayLengthOffset) return new ArrayLengthExpr(baseExpr);
            var elemSize = ValueSize(arr.BaseType);
            if (index is not null && disp >= ArrayDataOffset && scale == elemSize && (disp - ArrayDataOffset) == 0)
                return new ArrayElementExpr(baseExpr, index, arr.BaseType);
            if (index is null && disp >= ArrayDataOffset && (disp - ArrayDataOffset) % elemSize == 0)
                return new ArrayElementExpr(baseExpr, new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4), arr.BaseType);
            if (index is not null && scale == elemSize && Expr.KindOf(arr.BaseType) == ValueKind.Struct && TypeDefOf(arr.BaseType) is var etd and >= 0)
            {
                var elem = new ArrayElementExpr(baseExpr, index, arr.BaseType);
                return StructField(elem, etd, disp - ArrayDataOffset);
            }
            return null;
        }

        if (type.ElementType == AsmResolver.PE.DotNet.Metadata.Tables.ElementType.String && index is null && disp == StringLengthOffset)
            return new StringLengthExpr(baseExpr);

        var td = TypeDefOf(type);
        if (td < 0 || index is not null) return null;
        if (FindInstanceField(td, disp) is { } f && FieldRef(f) is { } fr)
        {
            var ft = FieldType(f, type);
            var fe = new FieldExpr(baseExpr, fr, ft);
            return size < ValueSize(ft) && Expr.KindOf(ft) == ValueKind.Struct ? StructFieldOrChunk(fe, ft, 0, size) : fe;
        }
        if (FindContainingInstance(td, disp) is { } of && FieldRef(of) is { } ofr2)
        {
            var oft = FieldType(of, type);
            return StructFieldOrChunk(new FieldExpr(baseExpr, ofr2, oft), oft, (int)(disp - of.Offset), size);
        }
        return null;
    }

    private Expr StructFieldOrChunk(Expr structValue, TypeSignature structType, int offset, int size)
    {
        var td = TypeDefOf(structType);
        if (td >= 0 && StructField(structValue, td, offset) is { } f && size <= Math.Max(4, ValueSize(f.Type!))) return f;
        return new StructChunkExpr(structValue, offset, size);
    }

    private ResolvedField? FindContainingInstance(int td, long disp) =>
        FieldsOf(td).Where(f => !f.IsStatic && disp > f.Offset).Select(f => (f, t: _stubs.ToSig(_module, f.TypeIndex)))
            .Where(x => Expr.KindOf(x.t) == ValueKind.Struct && disp < x.f.Offset + ValueSize(x.t)).Select(x => (ResolvedField?)x.f).FirstOrDefault();

    private ResolvedField? FindContainingStatic(int td, long disp) =>
        FieldsOf(td).Where(f => f.IsStatic && f.DeclaringTypeDef == td && disp > f.Offset).Select(f => (f, t: _stubs.ToSig(_module, f.TypeIndex)))
            .Where(x => Expr.KindOf(x.t) == ValueKind.Struct && disp < x.f.Offset + ValueSize(x.t)).Select(x => (ResolvedField?)x.f).FirstOrDefault();

    // ------------------------------------------------------------------ stores

    /// <summary>
    /// Emits a store of <paramref name="value"/> to [base + index*scale + disp]. Returns false when the target
    /// cannot be expressed in CIL (the caller then emits a native fallback statement).
    /// </summary>
    public bool Store(EmitContext ctx, Expr baseExpr, Expr? index, int scale, long disp, int size, Expr value)
    {
        var il = ctx.IL;
        if (value is StructChunkExpr chunk && baseExpr is AddressExpr dst && TypeDefOf(dst.Target.Type) is var dtd and >= 0)
            return CopyChunk(ctx, chunk, dst.Target, dtd, (int)disp, size);

        if (baseExpr is AddressExpr a && TypeDefOf(a.Target.Type) is var std and >= 0 && index is null)
        {
            if (StructField(a.Target, std, disp) is FieldExpr fe)
            {
                new AddressExpr(fe.Instance).Emit(ctx);
                Expr.EmitAs(ctx, value, fe.Type!);
                il.Add(CilOpCodes.Stfld, fe.Field);
                return true;
            }
            return false;
        }

        if (baseExpr is MarkerExpr { Marker: MarkerKind.StaticFields } sm && index is null)
        {
            if (FindStaticField(sm.Index, disp) is { } sf && FieldRef(sf) is { } sfr)
            {
                Expr.EmitAs(ctx, value, FieldType(sf, null));
                il.Add(CilOpCodes.Stsfld, sfr);
                return true;
            }
            if (FindContainingStatic(sm.Index, disp) is { } outer && FieldRef(outer) is { } ofr)
                return Store(ctx, new AddressExpr(new StaticFieldExpr(ofr, FieldType(outer, null))), null, 1, disp - outer.Offset, size, value);
            return false;
        }

        if (baseExpr.Kind != ValueKind.Ref || baseExpr.Type is null) return false;

        if (baseExpr.Type is SzArrayTypeSignature arr)
        {
            var elemSize = ValueSize(arr.BaseType);
            Expr? idx = index is not null && scale == elemSize && disp == ArrayDataOffset ? index
                : index is null && disp >= ArrayDataOffset && (disp - ArrayDataOffset) % elemSize == 0 ? new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4)
                : null;
            if (idx is null) return false;
            baseExpr.Emit(ctx);
            Expr.EmitAs(ctx, idx, _module.CorLibTypeFactory.Int32);
            Expr.EmitAs(ctx, value, arr.BaseType);
            il.Add(CilOpCodes.Stelem, arr.BaseType.ToTypeDefOrRef());
            return true;
        }

        var td = TypeDefOf(baseExpr.Type);
        if (td < 0 || index is not null) return false;
        if (FindInstanceField(td, disp) is { } f && FieldRef(f) is { } fr)
        {
            var ft = FieldType(f, baseExpr.Type);
            if (Expr.KindOf(ft) == ValueKind.Struct && value is StructChunkExpr)
                return Store(ctx, new AddressExpr(new FieldExpr(baseExpr, fr, ft)), null, 1, 0, size, value);
            baseExpr.Emit(ctx);
            Expr.EmitAs(ctx, value, ft);
            il.Add(CilOpCodes.Stfld, fr);
            return true;
        }
        if (FindContainingInstance(td, disp) is { } of && FieldRef(of) is { } ofr2)
            return Store(ctx, new AddressExpr(new FieldExpr(baseExpr, ofr2, FieldType(of, baseExpr.Type))), null, 1, disp - of.Offset, size, value);
        return false;
    }

    /// <summary>Struct copy through registers: copies every field overlapping [offset, offset+size) from source to target.</summary>
    private bool CopyChunk(EmitContext ctx, StructChunkExpr chunk, Expr target, int targetTypeDef, int offset, int size)
    {
        var srcTd = TypeDefOf(chunk.Source.Type);
        if (srcTd < 0) return false;
        var copied = false;
        foreach (var f in FieldsOf(targetTypeDef).Where(f => !f.IsStatic && f.Offset >= offset && f.Offset < offset + size))
        {
            var srcField = StructField(chunk.Source, srcTd, chunk.Offset + (f.Offset - offset));
            var dstField = StructField(target, targetTypeDef, f.Offset);
            if (srcField is null || dstField is not FieldExpr dfe) continue;
            new AddressExpr(dfe.Instance).Emit(ctx);
            Expr.EmitAs(ctx, srcField, dfe.Type!);
            ctx.IL.Add(CilOpCodes.Stfld, dfe.Field);
            copied = true;
        }
        return copied;
    }
}

/// <summary>string.Length via String::get_Length.</summary>
public sealed class StringLengthExpr(Expr str) : Expr
{
    public Expr String => str;
    public override ValueKind Kind => ValueKind.I4;
    public override void Emit(EmitContext ctx)
    {
        str.Emit(ctx);
        var f = ctx.Module.CorLibTypeFactory;
        ctx.IL.Add(CilOpCodes.Callvirt, new MemberReference((IMemberRefParent)f.String.Type, "get_Length", MethodSignature.CreateInstance(f.Int32)));
    }
}

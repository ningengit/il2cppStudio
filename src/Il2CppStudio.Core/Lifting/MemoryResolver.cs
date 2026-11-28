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
            case LocalExpr l when l.Type is ByReferenceTypeSignature: ctx.IL.Add(CilOpCodes.Ldloc, l.Local); break;
            case LocalExpr l: ctx.IL.Add(CilOpCodes.Ldloca, l.Local); break;
            case ArgExpr a when a.Type is ByReferenceTypeSignature: ctx.IL.Add(CilOpCodes.Ldarg, a.Parameter); break;
            case ArgExpr a: ctx.IL.Add(CilOpCodes.Ldarga, a.Parameter); break;
            case ThisStructExpr: ctx.IL.Add(CilOpCodes.Ldarg_0); break;   // value-type `this` is already a managed pointer
            case IndirectStructExpr i: i.Address.Emit(ctx); break;
            case FieldExpr f:
                if (f.Instance.Kind == ValueKind.Struct && f.Instance is LocalExpr or ArgExpr or ThisStructExpr or IndirectStructExpr or FieldExpr or StaticFieldExpr or ArrayElementExpr)
                    new AddressExpr(f.Instance).Emit(ctx);
                else
                    f.Instance.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Ldflda, f.Field);
                break;
            case StaticFieldExpr s: ctx.IL.Add(CilOpCodes.Ldsflda, s.Field); break;
            case ArrayElementExpr ae:
                ae.Array.Emit(ctx);
                Expr.EmitAs(ctx, ae.Index, ctx.Module.CorLibTypeFactory.Int32);
                ctx.IL.Add(CilOpCodes.Ldelema, ae.Type.ToTypeDefOrRef());
                break;
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
    public const int StaticFieldsOffset64 = 0xB8;
    public const int StaticFieldsOffset64V108 = 0x98;
    public const int VTableOffset64 = 0x138;
    public const int VTableOffset64V108 = 0x130;

    /// <summary>Il2CppClass::element_class (after image, gc_desc, name, namespaze, byval_arg, this_arg) on 64-bit v24+.</summary>
    public const int ElementClassOffset64 = 0x40;
    public const int ArrayLengthOffset64 = 0x18;
    public const int ArrayDataOffset64 = 0x20;
    public const int StringLengthOffset64 = 0x10;

    private readonly int _ptr;

    /// <summary>Pointer width of the image being lifted (8 for x64/ARM64, 4 for wasm32/ARMv7/x86).</summary>
    public int PointerSize => _ptr;

    // Il2CppClass/Il2CppObject/Il2CppArray/Il2CppString layouts differ with pointer width (and v108 reordered Il2CppClass).
    public int StaticFieldsOffset => _ptr == 8
        ? (_md.Version.Major >= 108 ? StaticFieldsOffset64V108 : StaticFieldsOffset64)
        : (_md.Version.Major >= 108 ? 0x4C : 0x5C);
    public int VTableOffset => _ptr == 8
        ? (_md.Version.Major >= 108 ? VTableOffset64V108 : VTableOffset64)
        : 0xB8;
    public int VTableEntrySize => _ptr * 2;
    public int ElementClassOffset => _ptr == 8 ? ElementClassOffset64 : 0x20;
    public int ArrayLengthOffset => _ptr == 8 ? ArrayLengthOffset64 : 0x0C;
    public int ArrayDataOffset => _ptr == 8 ? ArrayDataOffset64 : 0x10;
    public int StringLengthOffset => _ptr == 8 ? StringLengthOffset64 : 0x08;
    public int ObjectHeaderSize => _ptr * 2;

    private readonly GlobalMetadata _md;
    private readonly MetadataModel _model;
    private readonly Il2CppRegistration _reg;
    private readonly TypeNameService _names;
    private readonly StubAssemblyBuilder _stubs;
    private readonly ModuleDefinition _module;
    private readonly Dictionary<string, int> _typeDefByName;
    private readonly Dictionary<int, List<ResolvedField>> _fieldsCache = [];
    private readonly Dictionary<(int TypeDef, string OwnerFullName), List<ResolvedField>> _genericFieldsCache = [];
    public Action<EmitContext, Expr, Expr>? BeforeFieldStored { get; set; }
    public Action<Expr, Expr>? OnFieldStored { get; set; }

    public MemoryResolver(GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, TypeNameService names, StubAssemblyBuilder stubs,
        ModuleDefinition module, Dictionary<string, int> typeDefByName, int? pointerSize = null)
    {
        _md = md; _model = model; _reg = reg; _names = names; _stubs = stubs; _module = module; _typeDefByName = typeDefByName;
        _ptr = pointerSize ?? reg.Ptr;
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
        if (sig is ByReferenceTypeSignature br) sig = br.BaseType;
        if (sig is GenericInstanceTypeSignature g) sig = g.GenericType.ToTypeSignature(g.IsValueType);
        if (sig is not TypeDefOrRefSignature and not CorLibTypeSignature) return -1;
        var name = sig.FullName.Replace('/', '+');
        return _typeDefByName.GetValueOrDefault(name, -1);
    }

    public TypeSignature FieldType(ResolvedField f, TypeSignature? ownerSig)
    {
        var t = _stubs.ToSig(_module, f.TypeIndex);
        // Substitute class generic parameters when the owner is a closed generic instance.
        if (ownerSig is GenericInstanceTypeSignature gi)
            return SubstituteGenericType(t, gi);
        return t;
    }

    private static TypeSignature SubstituteGenericType(TypeSignature t, GenericInstanceTypeSignature gi) => t switch
    {
        GenericParameterSignature { ParameterType: GenericParameterType.Type } gp when gp.Index < gi.TypeArguments.Count =>
            gi.TypeArguments[gp.Index],
        GenericInstanceTypeSignature n when n.TypeArguments.Any(ContainsGenericTypeParam) =>
            new GenericInstanceTypeSignature(n.GenericType, n.IsValueType, n.TypeArguments.Select(a => SubstituteGenericType(a, gi)).ToArray()),
        SzArrayTypeSignature sz when ContainsGenericTypeParam(sz.BaseType) =>
            new SzArrayTypeSignature(SubstituteGenericType(sz.BaseType, gi)),
        ByReferenceTypeSignature br when ContainsGenericTypeParam(br.BaseType) =>
            new ByReferenceTypeSignature(SubstituteGenericType(br.BaseType, gi)),
        PointerTypeSignature ptr when ContainsGenericTypeParam(ptr.BaseType) =>
            new PointerTypeSignature(SubstituteGenericType(ptr.BaseType, gi)),
        _ => t,
    };

    private static bool ContainsGenericTypeParam(TypeSignature t) => t switch
    {
        GenericParameterSignature { ParameterType: GenericParameterType.Type } => true,
        GenericInstanceTypeSignature g => g.TypeArguments.Any(ContainsGenericTypeParam),
        SzArrayTypeSignature sz => ContainsGenericTypeParam(sz.BaseType),
        ByReferenceTypeSignature br => ContainsGenericTypeParam(br.BaseType),
        PointerTypeSignature ptr => ContainsGenericTypeParam(ptr.BaseType),
        _ => false,
    };

    public IFieldDescriptor? FieldRef(ResolvedField f) => _stubs.FieldRefFor(_module, f.FieldIndex, f.DeclaringTypeDef);

    public IFieldDescriptor? FieldRef(ResolvedField f, TypeSignature? ownerSig)
    {
        var fr = FieldRef(f);
        if (fr is null) return null;
        if (ownerSig is GenericInstanceTypeSignature gi && f.DeclaringTypeDef == TypeDefOf(gi)
            && fr.Signature is FieldSignature fs)
            return new MemberReference(new TypeSpecification(gi), fr.Name, fs);
        return fr;
    }

    public IFieldDescriptor? ResolveFieldRefDescriptor(int fieldRefIndex)
    {
        if ((uint)fieldRefIndex >= (uint)_md.FieldRefs.Length) return null;
        var fr = _md.FieldRefs[fieldRefIndex];
        if (!_reg.TryGetType(fr.TypeIndex, out var t)) return null;
        var declTd = _names.ResolveTypeDefIndex(t);
        if ((uint)declTd >= (uint)_md.TypeDefinitions.Length) return null;
        var absIdx = _md.TypeDefinitions[declTd].FieldStart + fr.FieldIndex;
        return (uint)absIdx < (uint)_md.Fields.Length ? _stubs.FieldRefFor(_module, absIdx, declTd) : null;
    }

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
                if (!isStatic && def.IsValueType) off -= ObjectHeaderSize;
                list.Add(new ResolvedField(fi, current, off, fd.TypeIndex, isStatic));
            }
            if (def.ParentIndex < 0 || !_reg.TryGetType(def.ParentIndex, out var parent)) break;
            current = _names.ResolveTypeDefIndex(parent);
        }
        _fieldsCache[typeDef] = list;
        return list;
    }

    /// <summary>Fields of a type, computing natural-alignment layout for closed generic instantiations whose open definition has unknown offsets.</summary>
    public IReadOnlyList<ResolvedField> FieldsOf(int typeDef, TypeSignature? ownerSig)
    {
        var baseFields = FieldsOf(typeDef);
        if (ownerSig is not GenericInstanceTypeSignature)
            return baseFields;
        var totalInstanceFields = 0;
        var countSeen = new HashSet<int>();
        var countCur = typeDef;
        while ((uint)countCur < (uint)_md.TypeDefinitions.Length && countSeen.Add(countCur))
        {
            var cdef = _md.TypeDefinitions[countCur];
            for (var k = 0; k < cdef.FieldCount; k++)
            {
                var fd = _md.Fields[cdef.FieldStart + k];
                var isStaticOrLiteral = _reg.TryGetType(fd.TypeIndex, out var ft) && (ft.Attrs & 0x50) != 0;
                if (!isStaticOrLiteral) totalInstanceFields++;
            }
            if (cdef.ParentIndex < 0 || !_reg.TryGetType(cdef.ParentIndex, out var cparent)) break;
            countCur = _names.ResolveTypeDefIndex(cparent);
        }
        if (baseFields.Where(f => !f.IsStatic).Select(f => f.Offset).Distinct().Count() >= totalInstanceFields)
            return baseFields;
        var cacheKey = (typeDef, ownerSig.FullName);
        if (_genericFieldsCache.TryGetValue(cacheKey, out var cached)) return cached;

        var list = new List<ResolvedField>();
        var seen = new HashSet<int>();
        var current = typeDef;
        var cursor = 0;
        var initializedCursor = false;
        while ((uint)current < (uint)_md.TypeDefinitions.Length && seen.Add(current))
        {
            var def = _md.TypeDefinitions[current];
            if (!initializedCursor)
            {
                cursor = def.IsValueType ? 0 : ObjectHeaderSize;
                initializedCursor = true;
            }
            for (var k = 0; k < def.FieldCount; k++)
            {
                var fi = def.FieldStart + k;
                var fd = _md.Fields[fi];
                var isStatic = _reg.TryGetType(fd.TypeIndex, out var ft) && (ft.Attrs & 0x10) != 0;
                var isLiteral = (ft.Attrs & 0x40) != 0;
                if (isLiteral || isStatic) continue;
                var fType = FieldType(new ResolvedField(fi, current, 0, fd.TypeIndex, false), ownerSig);
                var off = _reg.GetFieldOffset(current, k);
                var normOff = off >= 0 ? (def.IsValueType ? off - ObjectHeaderSize : off) : -1;
                if (normOff >= 0 && (list.Count == 0 || normOff > 0))
                {
                    off = normOff;
                }
                else
                {
                    var align = FieldAlignment(fType);
                    cursor = (cursor + align - 1) / align * align;
                    off = cursor;
                }
                var fsz = Math.Max(1, ValueSize(fType));
                cursor = Math.Max(cursor, off + fsz);
                list.Add(new ResolvedField(fi, current, off, fd.TypeIndex, false));
            }
            if (def.ParentIndex < 0 || !_reg.TryGetType(def.ParentIndex, out var parent)) break;
            current = _names.ResolveTypeDefIndex(parent);
        }
        _genericFieldsCache[cacheKey] = list;
        return list;
    }

    private int FieldAlignment(TypeSignature t, int depth = 0)
    {
        if (depth >= 8) return _ptr;
        switch (Expr.KindOf(t))
        {
            case ValueKind.I4:
                return Math.Min(_ptr, ValueSize(t, depth));
            case ValueKind.R4:
                return Math.Min(_ptr, 4);
            case ValueKind.Struct:
                var td = TypeDefOf(t);
                if (td < 0) return _ptr;
                var maxAlign = 1;
                foreach (var f in FieldsOf(td, t))
                {
                    if (f.IsStatic) continue;
                    var fa = FieldAlignment(FieldType(f, t), depth + 1);
                    if (fa > maxAlign) maxAlign = fa;
                }
                return Math.Min(_ptr, maxAlign);
            default:
                return _ptr;
        }
    }

    private bool IsEnumType(TypeSignature sig) =>
        TypeDefOf(sig) is var td and >= 0 && (uint)td < (uint)_md.TypeDefinitions.Length && _md.TypeDefinitions[td].IsEnum;

    public ResolvedField? FindInstanceField(int typeDef, long offset)
    {
        if ((uint)typeDef < (uint)_md.TypeDefinitions.Length && _md.TypeDefinitions[typeDef].IsEnum) return null;
        return FieldsOf(typeDef).Where(f => !f.IsStatic && f.Offset == offset).Cast<ResolvedField?>().FirstOrDefault();
    }

    public ResolvedField? FindStaticField(int typeDef, long offset)
    {
        if ((uint)typeDef >= (uint)_md.TypeDefinitions.Length) return null;
        // Statics are per declaring type only (no inheritance).
        return FieldsOf(typeDef).Where(f => f.IsStatic && f.DeclaringTypeDef == typeDef && f.Offset == offset).Cast<ResolvedField?>().FirstOrDefault();
    }

    /// <summary>Field of a value type that starts at <paramref name="offset"/>, descending into nested structs.</summary>
    public Expr? StructField(Expr structValue, int typeDef, long offset, int depth = 0, int desiredSize = 0)
    {
        if (depth > 6) return null;
        if ((uint)typeDef < (uint)_md.TypeDefinitions.Length && _md.TypeDefinitions[typeDef].IsEnum) return null;
        var fields = FieldsOf(typeDef, structValue.Type).Where(f => !f.IsStatic).OrderBy(f => f.Offset).ToList();
        foreach (var f in fields)
        {
            var ft = FieldType(f, structValue.Type);
            var size = ValueSize(ft);
            if (f.Offset == offset && FieldRef(f, structValue.Type) is { } fr)
            {
                var fe = new FieldExpr(structValue, fr, ft);
                // Offset 0 of a nested struct: prefer the innermost scalar unless caller asked for the whole nested struct size.
                var inner = TypeDefOf(ft);
                return inner >= 0 && _md.TypeDefinitions[inner].IsValueType && Expr.KindOf(ft) == ValueKind.Struct
                    && (desiredSize == 0 || desiredSize < size)
                    ? StructField(fe, inner, 0, depth + 1, desiredSize) ?? fe
                    : fe;
            }
            if (offset > f.Offset && offset < f.Offset + size && FieldRef(f, structValue.Type) is { } nr)
            {
                if (Expr.KindOf(ft) == ValueKind.Struct)
                {
                    var inner = TypeDefOf(ft);
                    if (inner >= 0) return StructField(new FieldExpr(structValue, nr, ft), inner, offset - f.Offset, depth + 1, desiredSize);
                }
                if (Expr.KindOf(ft) is ValueKind.I4 or ValueKind.I8)
                {
                    var k = Expr.KindOf(ft);
                    return new BinaryExpr(CilOpCodes.Shr_Un, new FieldExpr(structValue, nr, ft), new ConstExpr((int)((offset - f.Offset) * 8), ValueKind.I4), k);
                }
            }
        }
        return null;
    }

    /// <summary>Byte size of a value of the given type as laid out natively.</summary>
    public int ValueSize(TypeSignature t, int depth = 0)
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
                if (td < 0) return _ptr;
                if (t is not GenericInstanceTypeSignature)
                {
                    var regSize = _reg.GetValueTypeSize(td);
                    if (regSize > 0) return regSize;
                }
                if (depth >= 8) return _ptr;
                var computed = 0;
                var maxAlign = 1;
                foreach (var f in FieldsOf(td, t))
                {
                    if (f.IsStatic) continue;
                    var fType = FieldType(f, t);
                    var fsz = Math.Max(1, ValueSize(fType, depth + 1));
                    var align = FieldAlignment(fType, depth + 1);
                    if (align > maxAlign) maxAlign = align;
                    computed = (computed + align - 1) / align * align;
                    if (f.Offset > computed) computed = f.Offset;
                    computed += fsz;
                }
                var structAlign = Math.Min(_ptr, maxAlign);
                return computed > 0 ? (computed + structAlign - 1) / structAlign * structAlign : _ptr;
            default: return _ptr;
        }
    }

    /// <summary>Address of an instance field at <c>baseExpr + disp</c> when <paramref name="baseExpr"/> is an object reference.</summary>
    public AddressExpr? ResolveFieldAddress(Expr baseExpr, long disp)
    {
        if (TryRebaseFieldAddress(baseExpr, disp, out var rb, out var rd))
        {
            baseExpr = rb;
            disp = rd;
        }
        if (baseExpr.Kind != ValueKind.Ref || baseExpr.Type is null) return null;
        var td = TypeDefOf(baseExpr.Type);
        if (td < 0) return null;
        if (FindInstanceField(td, disp) is { } f && FieldRef(f) is { } fr)
            return new AddressExpr(new FieldExpr(baseExpr, fr, FieldType(f, baseExpr.Type)));
        if (FindContainingInstance(td, disp) is { } of && FieldRef(of) is { } ofr)
        {
            var oft = FieldType(of, baseExpr.Type);
            var outer = new FieldExpr(baseExpr, ofr, oft);
            var otd = TypeDefOf(oft);
            if (otd >= 0 && StructField(outer, otd, disp - of.Offset) is { } inner) return new AddressExpr(inner);
        }
        return null;
    }

    private bool TryRebaseFieldAddress(Expr baseExpr, long disp, out Expr rebasedBase, out long rebasedDisp)
    {
        if (disp != 0 && baseExpr is AddressExpr { Target: FieldExpr fe } && fe.Instance.Type is not null)
        {
            var td = TypeDefOf(fe.Instance.Type);
            if (td >= 0)
            {
                foreach (var f in FieldsOf(td))
                {
                    if (!f.IsStatic && FieldRef(f) is { } fr && string.Equals(fr.Name, fe.Field.Name, StringComparison.Ordinal)
                        && SignatureComparer.Default.Equals(fr.DeclaringType, fe.Field.DeclaringType))
                    {
                        var feSize = fe.Type is not null ? ValueSize(fe.Type) : ValueSize(FieldType(f, fe.Instance.Type));
                        if (disp > 0 && (feSize <= 0 || disp < feSize))
                        {
                            rebasedBase = baseExpr;
                            rebasedDisp = disp;
                            return false;
                        }
                        rebasedBase = Expr.KindOf(fe.Instance.Type) == ValueKind.Ref ? fe.Instance : new AddressExpr(fe.Instance);
                        rebasedDisp = f.Offset + disp;
                        return true;
                    }
                }
            }
        }
        if (baseExpr is BinaryExpr { Op.Code: CilCode.Add, Left: var inner, Right: ConstExpr c } && inner.Kind == ValueKind.Ref)
        {
            rebasedBase = inner;
            rebasedDisp = disp + c.Value;
            return true;
        }
        rebasedBase = baseExpr;
        rebasedDisp = disp;
        return false;
    }

    // ------------------------------------------------------------------ loads

    /// <summary>
    /// Value loaded from [base + index*scale + disp] with the given access size, or null when unrecognized.
    /// </summary>
    public Expr? Load(Expr baseExpr, Expr? index, int scale, long disp, int size)
    {
        if (index is null && disp != 0 && TryRebaseFieldAddress(baseExpr, disp, out var rb, out var rd)
            && (!ReferenceEquals(rb, baseExpr) || rd != disp))
            return Load(rb, null, scale, rd, size);
        // Address of a struct lvalue → its field (or a chunk for wide copies).
        if (baseExpr is AddressExpr addr && index is null && disp == 0 && addr.Target.Type is { } att)
        {
            var elem = att is ByReferenceTypeSignature br ? br.BaseType : att;
            var elemTd = TypeDefOf(elem);
            if (size <= Math.Max(4, ValueSize(elem)) && (Expr.KindOf(elem) != ValueKind.Struct || (elemTd >= 0 && _md.TypeDefinitions[elemTd].IsEnum)))
                return att is ByReferenceTypeSignature ? new IndirectExpr(addr, elem) : addr.Target;
        }
        if (baseExpr is AddressExpr { Target: var addrTarget } && TypeDefOf(addrTarget.Type) is var std and >= 0)
        {
            if (index is not null) return null;
            var field = StructField(addrTarget, std, disp, desiredSize: size);
            if (field is not null && (size <= ValueSize(field.Type!) || size <= 4))
            {
                if (field.Type is not null && ValueSize(field.Type) > size && Expr.KindOf(field.Type) is ValueKind.I4 or ValueKind.I8)
                {
                    var mask = size == 1 ? 0xFFL : size == 2 ? 0xFFFFL : 0xFFFFFFFFL;
                    return new BinaryExpr(CilOpCodes.And, field, new ConstExpr(mask, ValueKind.I4), ValueKind.I4);
                }
                return field;
            }
            return new StructChunkExpr(addrTarget, (int)disp, size);
        }
        if (baseExpr.Type is ByReferenceTypeSignature brDirect && index is null)
        {
            var elem = brDirect.BaseType;
            if (disp == 0 && (Expr.KindOf(elem) != ValueKind.Struct || IsEnumType(elem)))
            {
                if (size <= Math.Max(4, ValueSize(elem)))
                    return new IndirectExpr(baseExpr, elem);
            }
            if (TypeDefOf(elem) is var btd and >= 0 && Expr.KindOf(elem) == ValueKind.Struct && !IsEnumType(elem))
            {
                var indStruct = new IndirectStructExpr(elem, baseExpr);
                var field = StructField(indStruct, btd, disp, desiredSize: size);
                if (field is not null && (size <= ValueSize(field.Type!) || size <= 4)) return field;
                return new StructChunkExpr(indStruct, (int)disp, size);
            }
        }
        if (baseExpr is IndirectStructExpr indirect && index is null && TypeDefOf(indirect.Type) is var itd and >= 0)
        {
            var field = StructField(indirect, itd, disp, desiredSize: size);
            if (field is not null && (size <= ValueSize(field.Type!) || size <= 4)) return field;
            return new StructChunkExpr(indirect, (int)disp, size);
        }

        if (baseExpr is MarkerExpr m)
        {
            switch (m.Marker)
            {
                case MarkerKind.TypeInfo when disp == 0 && index is null && size == _ptr:
                    // The TypeInfo runtime-metadata global holds Il2CppClass* itself; [TypeInfo] is the klass pointer.
                    return new MarkerExpr(MarkerKind.Klass, m.Index, $"{_names.TypeDefName(m.Index, true)}->klass", m, m.Usage);
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
                case MarkerKind.Klass when index is null && disp == ElementClassOffset && size == _ptr:
                    // Il2CppClass::element_class - read by the array-store covariance check before stelem.ref.
                    return new MarkerExpr(MarkerKind.ElementClass, m.Index, $"{m.Describe()}.element_class", m.Base);
                case MarkerKind.Klass when index is null && disp >= VTableOffset && (disp - VTableOffset) % VTableEntrySize == 0:
                    return new MarkerExpr(MarkerKind.VTableEntry, (int)((disp - VTableOffset) / VTableEntrySize), $"vtable[{(disp - VTableOffset) / VTableEntrySize}]", m.Base);
                case MarkerKind.Klass when index is null && disp >= VTableOffset && (disp - VTableOffset) % VTableEntrySize == _ptr:
                    return new MarkerExpr(MarkerKind.MethodInfo, (int)((disp - VTableOffset) / VTableEntrySize), $"vtable[{(disp - VTableOffset) / VTableEntrySize}].method", m.Base);
                case MarkerKind.VTableEntry when index is null && disp == _ptr:
                    return new MarkerExpr(MarkerKind.MethodInfo, m.Index, $"{m.Describe()}.method", m.Base, m.Usage);
            }
            return null;
        }

        var kind = baseExpr.Kind;
        var type = baseExpr.Type;
        if (type is not SzArrayTypeSignature && baseExpr is BinaryExpr { Op.Code: CilCode.Add, Left: var bLeft, Right: var bRight } && index is null)
        {
            if (bLeft.Type is SzArrayTypeSignature) { baseExpr = bLeft; index = bRight; scale = 1; type = baseExpr.Type; kind = baseExpr.Kind; }
            else if (bRight.Type is SzArrayTypeSignature) { baseExpr = bRight; index = bLeft; scale = 1; type = baseExpr.Type; kind = baseExpr.Kind; }
        }
        else if (index is not null && baseExpr is BinaryExpr { Op.Code: CilCode.Add, Left: var arrL, Right: ConstExpr arrC } && arrL.Type is SzArrayTypeSignature)
        {
            baseExpr = arrL;
            disp += arrC.Value;
            type = baseExpr.Type;
            kind = baseExpr.Kind;
        }
        if (kind != ValueKind.Ref || type is null) return null;

        if (disp == 0 && index is null && size == _ptr)
            return new MarkerExpr(MarkerKind.Klass, TypeDefOf(type), $"{baseExpr.Describe()}->klass", baseExpr);

        if (type is SzArrayTypeSignature arr)
        {
            PeelScale(ref index, ref scale);
            if (index is BinaryExpr { Op.Code: CilCode.Add, Left: var innerIdx, Right: ConstExpr { Value: var c } })
            {
                index = innerIdx;
                disp += c * scale;
            }
            if (index is null && disp == ArrayLengthOffset) return new ArrayLengthExpr(baseExpr);
            if (ByteOffsetIndex(arr, index, scale, disp) is { } byteIdx) return new ArrayElementExpr(baseExpr, byteIdx, arr.BaseType);
            int elemSize = ValueSize(arr.BaseType);
            if (index is not null && scale == elemSize && disp >= 0 && (disp - ArrayDataOffset) % elemSize == 0)
                return new ArrayElementExpr(baseExpr, OffsetIndex(index, (disp - ArrayDataOffset) / elemSize), arr.BaseType);
            if (index is null && disp >= ArrayDataOffset && (disp - ArrayDataOffset) % elemSize == 0)
                return new ArrayElementExpr(baseExpr, new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4), arr.BaseType);
            if (index is null && disp >= ArrayDataOffset && Expr.KindOf(arr.BaseType) == ValueKind.Struct && TypeDefOf(arr.BaseType) is >= 0)
            {
                var elem = new ArrayElementExpr(baseExpr, new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4), arr.BaseType);
                var fieldOff = (disp - ArrayDataOffset) % elemSize;
                return fieldOff == 0 && size >= elemSize ? elem : StructFieldOrChunk(elem, arr.BaseType, (int)fieldOff, size);
            }
            if (index is not null && scale > 0 && elemSize % scale == 0 && disp >= ArrayDataOffset && Expr.KindOf(arr.BaseType) == ValueKind.Struct && TypeDefOf(arr.BaseType) is var etd and >= 0)
            {
                var elemIdx = OffsetIndex(UnscaleIndex(index, elemSize / scale), (disp - ArrayDataOffset) / elemSize);
                var elem = new ArrayElementExpr(baseExpr, elemIdx, arr.BaseType);
                var fieldOff = (disp - ArrayDataOffset) % elemSize;
                return fieldOff == 0 && size >= elemSize ? elem : StructFieldOrChunk(elem, arr.BaseType, (int)fieldOff, size);
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
        if (td >= 0 && StructField(structValue, td, offset, desiredSize: size) is { } f && size <= Math.Max(4, ValueSize(f.Type!))) return f;
        return new StructChunkExpr(structValue, offset, size);
    }

    public PackedFloatsExpr? TryLoadFloatPair(Expr baseExpr, long disp)
    {
        if (Load(baseExpr, null, 1, disp, 4) is { Kind: ValueKind.R4 } lo
            && Load(baseExpr, null, 1, disp + 4, 4) is { Kind: ValueKind.R4 } hi)
            return new PackedFloatsExpr(lo, hi, _module.CorLibTypeFactory.Single);
        return null;
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
        if (value is PackedFloatsExpr packed && size == 8)
            return Store(ctx, baseExpr, index, scale, disp, 4, packed.Lane0)
                && Store(ctx, baseExpr, index, scale, disp + 4, 4, packed.Lane1);
        if (value is VectorConstExpr vc && size == 16)
        {
            for (var lane = 0; lane < 4; lane++)
                if (!Store(ctx, baseExpr, index, scale, disp + lane * 4, 4, vc.Lane(lane)))
                    return false;
            return true;
        }
        if (index is null && disp != 0 && TryRebaseFieldAddress(baseExpr, disp, out var rb, out var rd)
            && (!ReferenceEquals(rb, baseExpr) || rd != disp))
            return Store(ctx, rb, null, scale, rd, size, value);
        var il = ctx.IL;
        if (value is StructChunkExpr chunk && baseExpr is AddressExpr dst && TypeDefOf(dst.Target.Type) is var dtd and >= 0)
            return CopyChunk(ctx, chunk, dst.Target, dtd, (int)disp, size);

        if (baseExpr is AddressExpr scalarAddr && index is null && disp == 0 && scalarAddr.Target.Type is { } satt)
        {
            var elem = satt is ByReferenceTypeSignature sbr ? sbr.BaseType : satt;
            var elemTd = TypeDefOf(elem);
            if (Expr.KindOf(elem) != ValueKind.Struct || (elemTd >= 0 && _md.TypeDefinitions[elemTd].IsEnum))
            {
                if (scalarAddr.Target is FieldExpr or StaticFieldExpr)
                    BeforeFieldStored?.Invoke(ctx, value, scalarAddr.Target);
                if (scalarAddr.Target is LocalExpr le && satt is not ByReferenceTypeSignature)
                {
                    Expr.EmitAs(ctx, value, elem);
                    il.Add(CilOpCodes.Stloc, le.Local);
                    return true;
                }
                scalarAddr.Emit(ctx);
                Expr.EmitAs(ctx, value, elem);
                IndirectExpr.EmitStore(ctx, elem);
                if (scalarAddr.Target is FieldExpr or StaticFieldExpr)
                    NotifyStored(value, scalarAddr.Target);
                return true;
            }
        }
        if (baseExpr.Type is ByReferenceTypeSignature sbrDirect && index is null)
        {
            var elem = sbrDirect.BaseType;
            if (disp == 0 && (Expr.KindOf(elem) != ValueKind.Struct || IsEnumType(elem)))
            {
                baseExpr.Emit(ctx);
                Expr.EmitAs(ctx, value, elem);
                IndirectExpr.EmitStore(ctx, elem);
                return true;
            }
            if (TypeDefOf(elem) is var btd and >= 0 && Expr.KindOf(elem) == ValueKind.Struct && !IsEnumType(elem))
                return Store(ctx, new IndirectStructExpr(elem, baseExpr), null, 1, disp, size, value);
        }

        if (baseExpr is AddressExpr a && TypeDefOf(a.Target.Type) is var std and >= 0 && index is null)
        {
            if (size == 16 && value is Const128Expr c128
                && StructField(a.Target, std, disp) is FieldExpr
                && StructField(a.Target, std, disp + 8) is FieldExpr)
            {
                return Store(ctx, baseExpr, null, 1, disp, 8, new ConstExpr(unchecked((long)c128.Lo), ValueKind.I8))
                    && Store(ctx, baseExpr, null, 1, disp + 8, 8, new ConstExpr(unchecked((long)c128.Hi), ValueKind.I8));
            }
            if (size is 2 or 4 or 8 && TryStorePackedScalarFields(ctx, a.Target, isStructAddr: true, std, a.Target.Type!, disp, size, value))
                return true;
            if (StructField(a.Target, std, disp) is FieldExpr zf && size > ValueSize(zf.Type!) && IsZero(value))
                return StoreZeros(ctx, a.Target, std, disp, size);   // `mov qword [x], 0`: zeroes two adjacent fields at once
            if (StructField(a.Target, std, disp) is FieldExpr fe)
            {
                BeforeFieldStored?.Invoke(ctx, value, fe);
                new AddressExpr(fe.Instance).Emit(ctx);
                Expr.EmitAs(ctx, value, fe.Type!);
                il.Add(CilOpCodes.Stfld, fe.Field);
                NotifyStored(value, fe);
                return true;
            }
            return false;
        }

        if (baseExpr is IndirectStructExpr ind && TypeDefOf(ind.Type) is var itd2 and >= 0 && index is null)
        {
            if (StructField(ind, itd2, disp) is FieldExpr fe2)
            {
                BeforeFieldStored?.Invoke(ctx, value, fe2);
                new AddressExpr(fe2.Instance).Emit(ctx);
                Expr.EmitAs(ctx, value, fe2.Type!);
                il.Add(CilOpCodes.Stfld, fe2.Field);
                NotifyStored(value, fe2);
                return true;
            }
            return false;
        }

        if (baseExpr is MarkerExpr { Marker: MarkerKind.StaticFields } sm && index is null)
        {
            if (FindStaticField(sm.Index, disp) is { } sf && FieldRef(sf) is { } sfr)
            {
                var st = FieldType(sf, null);
                var se = new StaticFieldExpr(sfr, st);
                BeforeFieldStored?.Invoke(ctx, value, se);
                Expr.EmitAs(ctx, value, st);
                il.Add(CilOpCodes.Stsfld, sfr);
                NotifyStored(value, se);
                return true;
            }
            if (FindContainingStatic(sm.Index, disp) is { } outer && FieldRef(outer) is { } ofr)
                return Store(ctx, new AddressExpr(new StaticFieldExpr(ofr, FieldType(outer, null))), null, 1, disp - outer.Offset, size, value);
            return false;
        }

        if (baseExpr.Type is not SzArrayTypeSignature && baseExpr is BinaryExpr { Op.Code: CilCode.Add, Left: var bLeft, Right: var bRight } && index is null)
        {
            if (bLeft.Type is SzArrayTypeSignature) { baseExpr = bLeft; index = bRight; scale = 1; }
            else if (bRight.Type is SzArrayTypeSignature) { baseExpr = bRight; index = bLeft; scale = 1; }
        }
        else if (index is not null && baseExpr is BinaryExpr { Op.Code: CilCode.Add, Left: var arrL, Right: ConstExpr arrC } && arrL.Type is SzArrayTypeSignature)
        {
            baseExpr = arrL;
            disp += arrC.Value;
        }
        if (baseExpr.Kind != ValueKind.Ref || baseExpr.Type is null) return false;

        if (baseExpr.Type is SzArrayTypeSignature arr)
        {
            int elemSize = ValueSize(arr.BaseType);
            PeelScale(ref index, ref scale);
            if (index is BinaryExpr { Op.Code: CilCode.Add, Left: var innerIdx, Right: ConstExpr { Value: var c } })
            {
                index = innerIdx;
                disp += c * scale;
            }
            Expr? idx = index is not null && scale == elemSize && disp >= 0 && (disp - ArrayDataOffset) % elemSize == 0 ? OffsetIndex(index, (disp - ArrayDataOffset) / elemSize)
                : index is null && disp >= ArrayDataOffset && (disp - ArrayDataOffset) % elemSize == 0 ? new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4)
                : null;
            idx ??= ByteOffsetIndex(arr, index, scale, disp);
            if (idx is null)
            {
                if (Expr.KindOf(arr.BaseType) == ValueKind.Struct && disp >= ArrayDataOffset)
                {
                    Expr? elemIdx = index is not null && scale > 0 && elemSize % scale == 0
                        ? OffsetIndex(UnscaleIndex(index, elemSize / scale), (disp - ArrayDataOffset) / elemSize)
                        : index is null ? new ConstExpr((disp - ArrayDataOffset) / elemSize, ValueKind.I4) : null;
                    if (elemIdx is not null)
                        return Store(ctx, new AddressExpr(new ArrayElementExpr(baseExpr, elemIdx, arr.BaseType)), null, 1, (disp - ArrayDataOffset) % elemSize, size, value);
                }
                return false;
            }
            if (Expr.KindOf(arr.BaseType) == ValueKind.Struct && (value is StructChunkExpr || size < elemSize && value.Kind != ValueKind.Struct))
                return Store(ctx, new AddressExpr(new ArrayElementExpr(baseExpr, idx, arr.BaseType)), null, 1, 0, size, value);
            baseExpr.Emit(ctx);
            Expr.EmitAs(ctx, idx, _module.CorLibTypeFactory.Int32);
            Expr.EmitAs(ctx, value, arr.BaseType);
            il.Add(CilOpCodes.Stelem, arr.BaseType.ToTypeDefOrRef());
            return true;
        }

        var td = TypeDefOf(baseExpr.Type);
        if (td < 0 || index is not null) return false;
        if (size == 16 && value is Const128Expr c128Obj
            && FindInstanceField(td, disp) is not null && FindInstanceField(td, disp + 8) is not null)
        {
            return Store(ctx, baseExpr, null, 1, disp, 8, new ConstExpr(unchecked((long)c128Obj.Lo), ValueKind.I8))
                && Store(ctx, baseExpr, null, 1, disp + 8, 8, new ConstExpr(unchecked((long)c128Obj.Hi), ValueKind.I8));
        }
        if (FindInstanceField(td, disp) is { } f && FieldRef(f) is { } fr)
        {
            var ft = FieldType(f, baseExpr.Type);
            if (size is 2 or 4 or 8 && ValueSize(ft) < size
                && TryStorePackedScalarFields(ctx, baseExpr, isStructAddr: false, td, baseExpr.Type, disp, size, value))
                return true;
            if (Expr.KindOf(ft) == ValueKind.Struct && (value is StructChunkExpr || size < ValueSize(ft) && value.Kind != ValueKind.Struct))
                return Store(ctx, new AddressExpr(new FieldExpr(baseExpr, fr, ft)), null, 1, 0, size, value);
            var feObj = new FieldExpr(baseExpr, fr, ft);
            BeforeFieldStored?.Invoke(ctx, value, feObj);
            baseExpr.Emit(ctx);
            Expr.EmitAs(ctx, value, ft);
            il.Add(CilOpCodes.Stfld, fr);
            NotifyStored(value, feObj);
            return true;
        }
        if (FindContainingInstance(td, disp) is { } of && FieldRef(of) is { } ofr2)
            return Store(ctx, new AddressExpr(new FieldExpr(baseExpr, ofr2, FieldType(of, baseExpr.Type))), null, 1, disp - of.Offset, size, value);
        return false;
    }

    private bool TryStorePackedScalarFields(
        EmitContext ctx, Expr owner, bool isStructAddr, int td, TypeSignature ownerType, long disp, int size, Expr value)
    {
        var hasConst = TryExtract64BitConstant(value, out var rawBits);
        if (!hasConst && value.Kind is not (ValueKind.I4 or ValueKind.I8))
            return false;
        var subFields = FieldsOf(td)
            .Where(f => !f.IsStatic && f.Offset >= disp && f.Offset < disp + size)
            .OrderBy(f => f.Offset)
            .ToList();
        if (subFields.Count < (hasConst ? 2 : 1))
            return false;

        var parts = new List<(IFieldDescriptor Field, TypeSignature Type, Expr PartExpr, long Offset)>(subFields.Count);
        var prevEnd = disp;
        foreach (var sf in subFields)
        {
            if (sf.Offset < prevEnd || FieldRef(sf) is not { } sfr)
                return false;
            var sft = FieldType(sf, ownerType);
            var sk = Expr.KindOf(sft);
            if (sk is not (ValueKind.I4 or ValueKind.R4 or ValueKind.I8 or ValueKind.R8))
                return false;
            var fsz = ValueSize(sft);
            if (fsz <= 0 || sf.Offset + fsz > disp + size)
                return false;
            prevEnd = sf.Offset + fsz;
            var bitOff = (int)((sf.Offset - disp) * 8);
            var mask = fsz >= 8 ? ulong.MaxValue : ((1UL << (fsz * 8)) - 1UL);
            Expr partExpr;
            if (hasConst)
            {
                var slice = (rawBits >> bitOff) & mask;
                partExpr = PackedSliceExpr(slice, sft, sk);
            }
            else
            {
                if (sk is not (ValueKind.I4 or ValueKind.I8))
                    return false;
                if (value.Kind == ValueKind.I4 && bitOff >= 32)
                {
                    partExpr = new ConstExpr(0, ValueKind.I4);
                }
                else
                {
                    Expr shifted = bitOff == 0
                        ? value
                        : new BinaryExpr(CilOpCodes.Shr_Un, value, new ConstExpr(bitOff, ValueKind.I4), value.Kind);
                    partExpr = fsz >= 4 || value.Kind == ValueKind.I8 && fsz >= 8
                        ? (shifted.Kind == ValueKind.I8 && fsz <= 4 ? new ConvExpr(shifted, _module.CorLibTypeFactory.Int32) : shifted)
                        : new BinaryExpr(CilOpCodes.And, shifted.Kind == ValueKind.I8 ? new ConvExpr(shifted, _module.CorLibTypeFactory.Int32) : shifted,
                            new ConstExpr(unchecked((int)mask), ValueKind.I4), ValueKind.I4);
                }
            }
            parts.Add((sfr, sft, partExpr, sf.Offset));
        }

        foreach (var (fr, sft, partExpr, offset) in parts)
        {
            var fe = isStructAddr
                ? (StructField(owner, td, offset) as FieldExpr ?? new FieldExpr(owner, fr, sft))
                : new FieldExpr(owner, fr, sft);
            BeforeFieldStored?.Invoke(ctx, partExpr, fe);
            if (isStructAddr) new AddressExpr(fe.Instance).Emit(ctx);
            else owner.Emit(ctx);
            Expr.EmitAs(ctx, partExpr, sft);
            ctx.IL.Add(CilOpCodes.Stfld, fr);
            NotifyStored(partExpr, fe);
        }
        return true;
    }

    private static Expr PackedSliceExpr(ulong slice, TypeSignature targetType, ValueKind k) => k switch
    {
        ValueKind.R4 => new FloatExpr(BitConverter.UInt32BitsToSingle(unchecked((uint)slice)), true),
        ValueKind.R8 => new FloatExpr(BitConverter.UInt64BitsToDouble(slice), false),
        ValueKind.I8 => new ConstExpr(unchecked((long)slice), ValueKind.I8),
        _ => new ConstExpr(unchecked((int)slice), ValueKind.I4),
    };

    private static bool TryExtract64BitConstant(Expr value, out ulong bits)
    {
        switch (value)
        {
            case ConstExpr c:
                bits = unchecked((ulong)c.Value);
                return true;
            case FloatExpr { Kind: ValueKind.R8 } fd:
                bits = unchecked((ulong)BitConverter.DoubleToInt64Bits(fd.Value));
                return true;
            default:
                bits = 0;
                return false;
        }
    }

    private void NotifyStored(Expr value, Expr fieldExpr)
    {
        if (value is (BinaryExpr or UnaryExpr or MathCallExpr) && value.Kind == fieldExpr.Kind)
            OnFieldStored?.Invoke(value, fieldExpr);
    }

    /// <summary>
    /// Strength-reduced indexing (Clang): <c>[arr + off]</c> with a byte-offset induction register <c>off = 0x20 + size·k</c>.
    /// The element index is <c>(off + disp - 0x20) / size</c>: exact, if not pretty, instead of a native store.
    /// </summary>
    private Expr? ByteOffsetIndex(SzArrayTypeSignature arr, Expr? index, int scale, long disp)
    {
        if (index is null || scale != 1 || index.Kind is not (ValueKind.I4 or ValueKind.I8) || Expr.KindOf(arr.BaseType) == ValueKind.Struct) return null;
        var size = ValueSize(arr.BaseType);
        if (size <= 1) return null;
        return UnscaleIndex(OffsetIndex(index, disp - ArrayDataOffset), size);
    }

    private static void PeelScale(ref Expr? index, ref int scale)
    {
        if (index is null || scale != 1) return;
        var inner = UnwrapIntConv(index);
        if (inner is BinaryExpr { Op.Code: CilCode.Mul, Left: var mulIdx, Right: ConstExpr { Value: > 1 and <= 65536 and var mulScale } })
        {
            index = UnwrapIntConv(mulIdx);
            scale = (int)mulScale;
        }
        else if (inner is BinaryExpr { Op.Code: CilCode.Shl, Left: var shIdx, Right: ConstExpr { Value: >= 1 and <= 5 and var sh } })
        {
            index = UnwrapIntConv(shIdx);
            scale = 1 << (int)sh;
        }
    }

    private static Expr UnwrapIntConv(Expr e)
    {
        while (e is ConvExpr c && c.Operand.Kind is ValueKind.I4 or ValueKind.I8 && c.Kind is ValueKind.I4 or ValueKind.I8)
            e = c.Operand;
        return e;
    }

    private static Expr OffsetIndex(Expr index, long offset) => offset == 0 ? index
        : new BinaryExpr(offset > 0 ? CilOpCodes.Add : CilOpCodes.Sub, index, new ConstExpr(Math.Abs(offset), index.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4), index.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);

    private static Expr UnscaleIndex(Expr index, int ratio)
    {
        if (ratio <= 1) return index;
        if (index is ConvExpr cv) return new ConvExpr(UnscaleIndex(cv.Operand, ratio), cv.Type);
        if (index is BinaryExpr { Op.Code: CilCode.Mul, Left: var l, Right: ConstExpr { Value: var m } } && m == ratio) return l;
        if (index is BinaryExpr { Op.Code: CilCode.Shl, Left: var sl, Right: ConstExpr { Value: >= 1 and <= 30 and var sh } })
        {
            var factor = 1L << (int)sh;
            if (factor == ratio) return sl;
            if (factor > ratio && factor % ratio == 0)
            {
                var remShift = (int)sh - System.Numerics.BitOperations.TrailingZeroCount((uint)ratio);
                var sk = sl.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4;
                return new BinaryExpr(CilOpCodes.Shl, sl, new ConstExpr(remShift, ValueKind.I4), sk);
            }
        }
        if (index is BinaryExpr { Op.Code: CilCode.Add, Left: var a, Right: BinaryExpr { Op.Code: CilCode.Mul, Left: var b, Right: ConstExpr { Value: var k } } }
            && k + 1 == ratio && SameValue(a, b)) return a;
        if (index is BinaryExpr { Op.Code: CilCode.Add, Left: var a2, Right: BinaryExpr { Op.Code: CilCode.Shl, Left: var b2, Right: ConstExpr { Value: var sh2 } } }
            && (1L << (int)sh2) + 1 == ratio && SameValue(a2, b2)) return a2;
        if (index is BinaryExpr { Op.Code: CilCode.Add, Left: var x, Right: ConstExpr { Value: var c } } && c % ratio == 0)
            return OffsetIndex(UnscaleIndex(x, ratio), c / ratio);
        if (index is BinaryExpr { Op.Code: CilCode.Sub, Left: var sx, Right: ConstExpr { Value: var sc } } && sc % ratio == 0)
            return OffsetIndex(UnscaleIndex(sx, ratio), -(sc / ratio));
        var kind = index.Kind == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4;
        return new BinaryExpr(CilOpCodes.Div, index, new ConstExpr(ratio, kind), kind);
    }

    private static bool SameValue(Expr a, Expr b)
    {
        while (a is ConvExpr ca) a = ca.Operand;
        while (b is ConvExpr cb) b = cb.Operand;
        return ReferenceEquals(a, b) || a is LocalExpr la && b is LocalExpr lb && la.Local == lb.Local
               || a is ArgExpr aa && b is ArgExpr ab && aa.Parameter == ab.Parameter;
    }

    private static bool IsZero(Expr e) => e is ConstExpr { Value: 0 } or FloatExpr { Value: 0 };

    private bool StoreZeros(EmitContext ctx, Expr target, int td, long disp, int size)
    {
        var any = false;
        foreach (var f in FieldsOf(td, target.Type).Where(f => !f.IsStatic && f.Offset >= disp && f.Offset < disp + size))
        {
            if (StructField(target, td, f.Offset) is not FieldExpr part || part.Type is not { } pt) continue;
            var k = Expr.KindOf(pt);
            Expr zero = k is ValueKind.R4 or ValueKind.R8 ? new FloatExpr(0, k == ValueKind.R4) : new ConstExpr(0, k == ValueKind.I8 ? ValueKind.I8 : ValueKind.I4);
            new AddressExpr(part.Instance).Emit(ctx);
            Expr.EmitAs(ctx, zero, pt);
            ctx.IL.Add(CilOpCodes.Stfld, part.Field);
            any = true;
        }
        return any;
    }

    /// <summary>Struct copy through registers: copies every field overlapping [offset, offset+size) from source to target.</summary>
    private bool CopyChunk(EmitContext ctx, StructChunkExpr chunk, Expr target, int targetTypeDef, int offset, int size)
    {
        var srcTd = TypeDefOf(chunk.Source.Type);
        if (srcTd < 0) return false;
        var copied = false;
        foreach (var f in FieldsOf(targetTypeDef, target.Type).Where(f => !f.IsStatic && f.Offset >= offset && f.Offset < offset + size))
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

    /// <summary>True when <paramref name="derivedTd"/> strictly inherits from <paramref name="baseTd"/>.</summary>
    public bool IsStrictDerivedFrom(int derivedTd, int baseTd)
    {
        if (derivedTd < 0 || baseTd < 0 || derivedTd == baseTd || _reg is null) return false;
        var cur = derivedTd;
        for (var d = 0; d < 32 && (uint)cur < (uint)_md.TypeDefinitions.Length; d++)
        {
            var parentIdx = _md.TypeDefinitions[cur].ParentIndex;
            if (parentIdx < 0 || !_reg.TryGetType(parentIdx, out var pt)) return false;
            var ptd = _names.ResolveTypeDefIndex(pt);
            if (ptd == baseTd) return true;
            if (ptd < 0 || ptd == cur) return false;
            cur = ptd;
        }
        return false;
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

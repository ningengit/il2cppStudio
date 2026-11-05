using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Builder;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Per-image .NET module synthesized from metadata, plus helper intrinsics used by lifted bodies.</summary>
public sealed class StubModule
{
    public required int ImageIndex { get; init; }
    public required string AssemblyName { get; init; }
    public required ModuleDefinition Module { get; init; }
    public required NativeIntrinsics Intrinsics { get; init; }
    public HashSet<int> LiftedMethods { get; } = [];
}

/// <summary>Static helper methods emitted into each stub module; lifted code calls them for native-only operations.</summary>
public sealed class NativeIntrinsics
{
    public required TypeDefinition Type { get; init; }
    /// <summary>long Asm(string) — value produced by an instruction that has no CIL equivalent.</summary>
    public required MethodDefinition Asm { get; init; }
    /// <summary>void Stmt(string) — side-effecting native instruction (store to unknown memory, ...).</summary>
    public required MethodDefinition Stmt { get; init; }
    /// <summary>long CallNative(string, long, long, long, long) — call to a non-metadata function (runtime helper / thunk).</summary>
    public required MethodDefinition CallNative { get; init; }
    /// <summary>T Reinterpret&lt;T&gt;(object) — reinterpretation of a native value as a managed type.</summary>
    public required MethodDefinition Reinterpret { get; init; }
}

/// <summary>
/// Synthesizes one AsmResolver <see cref="ModuleDefinition"/> per IL2CPP image (like Il2CppDumper's DummyDll),
/// with exact type/member signatures resolved from Il2CppType records. Cross-assembly references are emitted
/// as TypeReferences into AssemblyReferences carrying the original names, versions and public key tokens, so
/// they bind to either another stub or a pristine BCL reference assembly at decompilation time.
/// </summary>
public sealed class StubAssemblyBuilder
{
    private readonly GlobalMetadata _md;
    private readonly MetadataModel _model;
    private readonly Il2CppRegistration? _reg;
    private readonly TypeNameService _names;
    private readonly IDiagnosticSink _sink;

    private readonly int[] _imageOfType;
    private readonly TypeDefinition?[] _typeDefs;
    private readonly MethodDefinition?[] _methodDefs;
    private readonly FieldDefinition?[] _fieldDefs;
    private readonly StubModule?[] _modules;
    private readonly Dictionary<(ModuleDefinition, int), ITypeDefOrRef> _typeRefCache = [];
    private readonly Dictionary<(ModuleDefinition, int), AssemblyReference> _asmRefCache = [];
    private readonly Lock _gate = new();

    public StubAssemblyBuilder(GlobalMetadata md, MetadataModel model, Il2CppRegistration? reg, TypeNameService names, IDiagnosticSink sink)
    {
        _md = md; _model = model; _reg = reg; _names = names; _sink = sink;
        _imageOfType = new int[md.TypeDefinitions.Length];
        Array.Fill(_imageOfType, -1);
        for (var i = 0; i < md.Images.Length; i++)
            for (var t = md.Images[i].TypeStart; t < md.Images[i].TypeStart + md.Images[i].TypeCount && t < _imageOfType.Length; t++)
                _imageOfType[t] = i;
        _typeDefs = new TypeDefinition?[md.TypeDefinitions.Length];
        _methodDefs = new MethodDefinition?[md.Methods.Length];
        _fieldDefs = new FieldDefinition?[md.Fields.Length];
        _modules = new StubModule?[md.Images.Length];
        RegisterEnums();
    }

    private DefaultValueReader? _defaults;

    /// <summary>Metadata constant of a literal field (enum member / const), as an AsmResolver Constant.</summary>
    private Constant? Constant(int fieldIndex)
    {
        if (_reg is null) return null;
        _defaults ??= new DefaultValueReader(_md, _reg);
        if (!_defaults.TryGetFieldDefault(fieldIndex, out var v)) return null;
        return v switch
        {
            bool b => AsmResolver.DotNet.Constant.FromValue(b), char c => AsmResolver.DotNet.Constant.FromValue(c),
            byte x => AsmResolver.DotNet.Constant.FromValue(x), sbyte x => AsmResolver.DotNet.Constant.FromValue(x),
            ushort x => AsmResolver.DotNet.Constant.FromValue(x), short x => AsmResolver.DotNet.Constant.FromValue(x),
            uint x => AsmResolver.DotNet.Constant.FromValue(x), int x => AsmResolver.DotNet.Constant.FromValue(x),
            ulong x => AsmResolver.DotNet.Constant.FromValue(x), long x => AsmResolver.DotNet.Constant.FromValue(x),
            float x => AsmResolver.DotNet.Constant.FromValue(x), double x => AsmResolver.DotNet.Constant.FromValue(x),
            string s => AsmResolver.DotNet.Constant.FromValue(s),
            _ => null,
        };
    }

    /// <summary>Registers every enum (signature full name → underlying stack kind) so lifted code treats them as integers.</summary>
    private void RegisterEnums()
    {
        foreach (var t in _model.Types.Where(t => t.Raw.IsEnum))
        {
            var valueField = t.Fields.FirstOrDefault(f => f.Name == "value__");
            var wide = valueField is not null && _reg is not null && _reg.TryGetType(valueField.TypeIndex, out var vt)
                       && vt.Type is Il2CppTypeEnum.I8 or Il2CppTypeEnum.U8;
            EnumRegistry.Register(t.FullName.Replace('+', '/'), wide ? ValueKind.I8 : ValueKind.I4);
        }
    }

    public int ImageOfType(int typeDefIndex) => (uint)typeDefIndex < (uint)_imageOfType.Length ? _imageOfType[typeDefIndex] : -1;

    public string ImageName(int imageIndex) => _md.GetString(_md.Images[imageIndex].NameIndex);

    public int FindImage(string assemblyName)
    {
        var n = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? assemblyName : assemblyName + ".dll";
        for (var i = 0; i < _md.Images.Length; i++)
            if (ImageName(i).Equals(n, StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }

    public MethodDefinition? GetMethodDef(int methodIndex) => (uint)methodIndex < (uint)_methodDefs.Length ? _methodDefs[methodIndex] : null;
    public TypeDefinition? GetTypeDef(int typeIndex) => (uint)typeIndex < (uint)_typeDefs.Length ? _typeDefs[typeIndex] : null;
    public FieldDefinition? GetFieldDef(int fieldIndex) => (uint)fieldIndex < (uint)_fieldDefs.Length ? _fieldDefs[fieldIndex] : null;

    /// <summary>Gets (building on first use) the stub module of an image.</summary>
    public StubModule GetModule(int imageIndex)
    {
        lock (_gate)
        {
            return _modules[imageIndex] ??= BuildModule(imageIndex);
        }
    }

    // ------------------------------------------------------------------ building

    private StubModule BuildModule(int imageIndex)
    {
        var img = _md.Images[imageIndex];
        var asmDef = _md.Assemblies.FirstOrDefault(a => a.ImageIndex == imageIndex);
        var asmName = _md.GetString(asmDef.NameIndex);
        var corlib = MakeAssemblyRef(_md.Assemblies.FirstOrDefault(a => _md.GetString(a.NameIndex) == "mscorlib"), "mscorlib");
        var module = new ModuleDefinition(ImageName(imageIndex), corlib);
        var assembly = new AssemblyDefinition(asmName, new Version(asmDef.Major, asmDef.Minor, asmDef.Build, asmDef.Revision));
        assembly.Modules.Add(module);

        var start = img.TypeStart;
        var end = img.TypeStart + (int)img.TypeCount;

        // Pass 1: type shells (so every intra-module reference resolves to a TypeDefinition).
        for (var t = start; t < end; t++)
        {
            var def = _md.TypeDefinitions[t];
            var tm = _model.Types[t];
            var td = new TypeDefinition(tm.DeclaringType is null ? tm.Namespace : "", tm.Name, (TypeAttributes)def.Flags);
            _typeDefs[t] = td;
            AddGenericParameters(td.GenericParameters, def.GenericContainerIndex);
        }
        for (var t = start; t < end; t++)
        {
            var tm = _model.Types[t];
            if (tm.DeclaringType is { } parent && _typeDefs[parent.Index] is { } ptd) ptd.NestedTypes.Add(_typeDefs[t]!);
            else module.TopLevelTypes.Add(_typeDefs[t]!);
        }

        // Pass 2: base types, interfaces, members.
        for (var t = start; t < end; t++) PopulateType(module, t);

        var intrinsics = CreateIntrinsics(module);
        var stub = new StubModule { ImageIndex = imageIndex, AssemblyName = asmName, Module = module, Intrinsics = intrinsics };
        return stub;
    }

    private void PopulateType(ModuleDefinition module, int t)
    {
        var def = _md.TypeDefinitions[t];
        var td = _typeDefs[t]!;
        try
        {
            if (def.ParentIndex >= 0) td.BaseType = ToTypeDefOrRef(module, def.ParentIndex);
            for (var i = 0; i < def.InterfacesCount; i++)
            {
                var ti = _md.Interfaces[def.InterfacesStart + i];
                if (ToTypeDefOrRef(module, ti) is { } iface) td.Interfaces.Add(new InterfaceImplementation(iface));
            }
        }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.SignatureParsing, DiagnosticCodes.SignatureMismatch, $"Base/interface of {_model.Types[t].FullName}: {ex.Message}", new() { TypeName = _model.Types[t].FullName });
        }

        for (var f = 0; f < def.FieldCount; f++)
        {
            var fi = def.FieldStart + f;
            var fd = _md.Fields[fi];
            var attrs = _reg is not null && _reg.TryGetType(fd.TypeIndex, out var ft) ? (FieldAttributes)ft.Attrs : FieldAttributes.Public;
            var field = new FieldDefinition(_md.GetString(fd.NameIndex), attrs & ~FieldAttributes.HasFieldRva, ToSig(module, fd.TypeIndex));
            if ((attrs & FieldAttributes.HasDefault) != 0 && Constant(fi) is { } constant) field.Constant = constant;
            else field.Attributes &= ~FieldAttributes.HasDefault;
            _fieldDefs[fi] = field;
            td.Fields.Add(field);
        }

        for (var m = 0; m < def.MethodCount; m++)
        {
            var mi = def.MethodStart + m;
            var method = BuildMethod(module, mi);
            _methodDefs[mi] = method;
            td.Methods.Add(method);
        }

        for (var p = 0; p < def.PropertyCount; p++)
        {
            var pd = _md.Properties[def.PropertyStart + p];
            var getter = pd.Get >= 0 ? _methodDefs[def.MethodStart + pd.Get] : null;
            var setter = pd.Set >= 0 ? _methodDefs[def.MethodStart + pd.Set] : null;
            var accessor = getter ?? setter;
            if (accessor is null) continue;
            var propType = getter?.Signature!.ReturnType ?? setter!.Signature!.ParameterTypes[^1];
            var indexParams = getter is not null ? getter.Signature!.ParameterTypes.ToList() : setter!.Signature!.ParameterTypes.Take(setter.Signature.ParameterTypes.Count - 1).ToList();
            var sig = accessor.IsStatic ? PropertySignature.CreateStatic(propType, indexParams) : PropertySignature.CreateInstance(propType, indexParams);
            var prop = new PropertyDefinition(_md.GetString(pd.NameIndex), (PropertyAttributes)pd.Attrs, sig);
            if (getter is not null) prop.Semantics.Add(new MethodSemantics(getter, MethodSemanticsAttributes.Getter));
            if (setter is not null) prop.Semantics.Add(new MethodSemantics(setter, MethodSemanticsAttributes.Setter));
            td.Properties.Add(prop);
        }

        for (var e = 0; e < def.EventCount; e++)
        {
            var ed = _md.Events[def.EventStart + e];
            if (ToTypeDefOrRef(module, ed.TypeIndex) is not { } evType) continue;
            var ev = new EventDefinition(_md.GetString(ed.NameIndex), 0, evType);
            if (ed.Add >= 0 && _methodDefs[def.MethodStart + ed.Add] is { } add) ev.Semantics.Add(new MethodSemantics(add, MethodSemanticsAttributes.AddOn));
            if (ed.Remove >= 0 && _methodDefs[def.MethodStart + ed.Remove] is { } rem) ev.Semantics.Add(new MethodSemantics(rem, MethodSemanticsAttributes.RemoveOn));
            if (ev.Semantics.Count > 0) td.Events.Add(ev);
        }
    }

    private MethodDefinition BuildMethod(ModuleDefinition module, int mi)
    {
        var md = _md.Methods[mi];
        var attrs = (MethodAttributes)md.Flags & ~MethodAttributes.PInvokeImpl;
        var isStatic = (attrs & MethodAttributes.Static) != 0;
        var ret = ToSig(module, md.ReturnType);
        var paramTypes = new List<TypeSignature>(md.ParameterCount);
        for (var p = 0; p < md.ParameterCount; p++) paramTypes.Add(ToSig(module, _md.Parameters[md.ParameterStart + p].TypeIndex));
        var genericCount = (uint)md.GenericContainerIndex < (uint)_md.GenericContainers.Length ? _md.GenericContainers[md.GenericContainerIndex].TypeArgc : 0;
        var sig = isStatic ? MethodSignature.CreateStatic(ret, genericCount, paramTypes) : MethodSignature.CreateInstance(ret, genericCount, paramTypes);
        var method = new MethodDefinition(_md.GetString(md.NameIndex), attrs, sig)
        {
            ImplAttributes = (MethodImplAttributes)md.IFlags & ~(MethodImplAttributes.InternalCall | MethodImplAttributes.Runtime),
        };
        AddGenericParameters(method.GenericParameters, md.GenericContainerIndex);
        for (var p = 0; p < md.ParameterCount; p++)
        {
            var pd = _md.Parameters[md.ParameterStart + p];
            var pattrs = _reg is not null && _reg.TryGetType(pd.TypeIndex, out var pt) ? (ParameterAttributes)pt.Attrs : 0;
            method.ParameterDefinitions.Add(new ParameterDefinition((ushort)(p + 1), _md.GetString(pd.NameIndex), pattrs));
        }
        if (!method.IsAbstract) SetPlaceholderBody(method);
        return method;
    }

    /// <summary>Default body for methods that have not been lifted (or have no native code): <c>throw null;</c>.</summary>
    public static void SetPlaceholderBody(MethodDefinition method)
    {
        var body = new CilMethodBody();
        body.Instructions.Add(CilOpCodes.Ldnull);
        body.Instructions.Add(CilOpCodes.Throw);
        method.CilMethodBody = body;
    }

    private void AddGenericParameters(IList<GenericParameter> target, int containerIndex)
    {
        if ((uint)containerIndex >= (uint)_md.GenericContainers.Length) return;
        var c = _md.GenericContainers[containerIndex];
        for (var i = 0; i < c.TypeArgc; i++)
        {
            var gp = _md.GenericParameters[c.GenericParameterStart + i];
            target.Add(new GenericParameter(_md.GetString(gp.NameIndex), (GenericParameterAttributes)gp.Flags));
        }
    }

    private static NativeIntrinsics CreateIntrinsics(ModuleDefinition module)
    {
        var f = module.CorLibTypeFactory;
        var type = new TypeDefinition("Il2CppStudio.Native", "NativeIntrinsics",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.BeforeFieldInit, f.Object.Type);
        MethodDefinition Make(string name, TypeSignature ret, params TypeSignature[] ps)
        {
            var m = new MethodDefinition(name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, MethodSignature.CreateStatic(ret, ps));
            for (var i = 0; i < ps.Length; i++) m.ParameterDefinitions.Add(new ParameterDefinition((ushort)(i + 1), i == 0 ? "native" : $"arg{i}", 0));
            SetPlaceholderBody(m);
            type.Methods.Add(m);
            return m;
        }
        var asm = Make("Asm", f.Int64, f.String);
        var stmt = Make("Stmt", f.Void, f.String);
        var call = Make("CallNative", f.Int64, f.String, f.Int64, f.Int64, f.Int64, f.Int64);
        var reinterpret = new MethodDefinition("Reinterpret", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig,
            MethodSignature.CreateStatic(new GenericParameterSignature(GenericParameterType.Method, 0), 1, [f.Object]));
        reinterpret.GenericParameters.Add(new GenericParameter("T"));
        reinterpret.ParameterDefinitions.Add(new ParameterDefinition(1, "value", 0));
        SetPlaceholderBody(reinterpret);
        type.Methods.Add(reinterpret);
        module.TopLevelTypes.Add(type);
        return new NativeIntrinsics { Type = type, Asm = asm, Stmt = stmt, CallNative = call, Reinterpret = reinterpret };
    }

    // ------------------------------------------------------------------ signatures

    public TypeSignature ToSig(ModuleDefinition ctx, int typeIndex)
    {
        if (_reg is null || !_reg.TryGetType(typeIndex, out var t)) return ctx.CorLibTypeFactory.Object;
        return ToSig(ctx, t, 0);
    }

    public TypeSignature ToSig(ModuleDefinition ctx, Il2CppTypeRecord t, int depth = 0)
    {
        var core = ToSigCore(ctx, t, depth);
        return t.ByRef ? core.MakeByReferenceType() : core;
    }

    private TypeSignature ToSigCore(ModuleDefinition ctx, Il2CppTypeRecord t, int depth)
    {
        var f = ctx.CorLibTypeFactory;
        if (depth > 24 || _reg is null) return f.Object;
        switch (t.Type)
        {
            case Il2CppTypeEnum.Void: return f.Void;
            case Il2CppTypeEnum.Boolean: return f.Boolean;
            case Il2CppTypeEnum.Char: return f.Char;
            case Il2CppTypeEnum.I1: return f.SByte;
            case Il2CppTypeEnum.U1: return f.Byte;
            case Il2CppTypeEnum.I2: return f.Int16;
            case Il2CppTypeEnum.U2: return f.UInt16;
            case Il2CppTypeEnum.I4: return f.Int32;
            case Il2CppTypeEnum.U4: return f.UInt32;
            case Il2CppTypeEnum.I8: return f.Int64;
            case Il2CppTypeEnum.U8: return f.UInt64;
            case Il2CppTypeEnum.R4: return f.Single;
            case Il2CppTypeEnum.R8: return f.Double;
            case Il2CppTypeEnum.String: return f.String;
            case Il2CppTypeEnum.I: return f.IntPtr;
            case Il2CppTypeEnum.U: return f.UIntPtr;
            case Il2CppTypeEnum.Object: return f.Object;
            case Il2CppTypeEnum.TypedByRef: return f.TypedReference;
            case Il2CppTypeEnum.Class or Il2CppTypeEnum.ValueType:
            {
                var r = TypeRefFor(ctx, t.KlassIndex);
                return r is null ? f.Object : new TypeDefOrRefSignature(r, IsValueType(t.KlassIndex));
            }
            case Il2CppTypeEnum.Var or Il2CppTypeEnum.MVar:
            {
                var num = (uint)t.GenericParameterIndex < (uint)_md.GenericParameters.Length ? _md.GenericParameters[t.GenericParameterIndex].Num : 0;
                return new GenericParameterSignature(t.Type == Il2CppTypeEnum.Var ? GenericParameterType.Type : GenericParameterType.Method, num);
            }
            case Il2CppTypeEnum.SzArray:
                return ToSigCore(ctx, _reg.GetTypeAt(t.Data), depth + 1).MakeSzArrayType();
            case Il2CppTypeEnum.Ptr:
                return ToSigCore(ctx, _reg.GetTypeAt(t.Data), depth + 1).MakePointerType();
            case Il2CppTypeEnum.Array:
            {
                var a = _reg.ReadArrayType(t.Data);
                return new ArrayTypeSignature(ToSigCore(ctx, _reg.GetTypeAt(a.ElementTypeVa), depth + 1), Math.Max(1, (int)a.Rank));
            }
            case Il2CppTypeEnum.GenericInst:
            {
                var gc = _reg.ReadGenericClass(t.Data);
                var defType = _reg.GetTypeAt(gc.TypeVa);
                var defIndex = _names.ResolveTypeDefIndex(defType);
                var defRef = defIndex >= 0 ? TypeRefFor(ctx, defIndex) : null;
                if (defRef is null) return f.Object;
                var inst = _reg.ReadGenericInst(gc.ClassInstVa);
                var args = inst.TypeArgVas.Select(v => ToSig(ctx, _reg.GetTypeAt(v), depth + 1)).ToList();
                return new GenericInstanceTypeSignature(defRef, IsValueType(defIndex), args);
            }
            default:
                return f.Object;
        }
    }

    public bool IsValueType(int typeDefIndex) =>
        (uint)typeDefIndex < (uint)_md.TypeDefinitions.Length && _md.TypeDefinitions[typeDefIndex].IsValueType;

    public ITypeDefOrRef? ToTypeDefOrRef(ModuleDefinition ctx, int typeIndex)
    {
        if (_reg is null || !_reg.TryGetType(typeIndex, out var t)) return null;
        if (t.Type is Il2CppTypeEnum.Class or Il2CppTypeEnum.ValueType) return TypeRefFor(ctx, t.KlassIndex);
        var sig = ToSig(ctx, t);
        return sig is CorLibTypeSignature c ? c.Type : new TypeSpecification(sig);
    }

    /// <summary>A TypeDefinition when the type lives in <paramref name="ctx"/>, otherwise a cached TypeReference.</summary>
    public ITypeDefOrRef? TypeRefFor(ModuleDefinition ctx, int typeDefIndex)
    {
        if ((uint)typeDefIndex >= (uint)_typeDefs.Length) return null;
        if (_typeDefs[typeDefIndex] is { } td && td.DeclaringModule == ctx) return td;
        if (_typeRefCache.TryGetValue((ctx, typeDefIndex), out var cached)) return cached;

        var tm = _model.Types[typeDefIndex];
        IResolutionScope scope;
        if (tm.DeclaringType is { } parent)
        {
            if (TypeRefFor(ctx, parent.Index) is not TypeReference parentRef)
                return _typeDefs[typeDefIndex] is { } local && local.DeclaringModule == ctx ? local : null;
            scope = parentRef;
        }
        else
        {
            var image = ImageOfType(typeDefIndex);
            if (image < 0) return null;
            scope = AssemblyRefFor(ctx, image);
        }
        var tr = new TypeReference(ctx, scope, tm.DeclaringType is null ? tm.Namespace : null, tm.Name);
        _typeRefCache[(ctx, typeDefIndex)] = tr;
        return tr;
    }

    private AssemblyReference AssemblyRefFor(ModuleDefinition ctx, int imageIndex)
    {
        if (_asmRefCache.TryGetValue((ctx, imageIndex), out var r)) return r;
        var asm = _md.Assemblies.FirstOrDefault(a => a.ImageIndex == imageIndex);
        var name = _md.GetString(asm.NameIndex);
        r = name == "mscorlib" && ctx.CorLibTypeFactory.CorLibScope is AssemblyReference corlib
            ? corlib
            : ctx.AssemblyReferences.FirstOrDefault(a => a.Name == name) ?? MakeAssemblyRef(asm, name);
        if (!ctx.AssemblyReferences.Contains(r)) ctx.AssemblyReferences.Add(r);
        _asmRefCache[(ctx, imageIndex)] = r;
        return r;
    }

    private static AssemblyReference MakeAssemblyRef(Il2CppAssemblyDefinition asm, string name)
    {
        var version = new Version(Math.Max(0, asm.Major), Math.Max(0, asm.Minor), Math.Max(0, asm.Build), Math.Max(0, asm.Revision));
        return asm.PublicKeyToken == 0
            ? new AssemblyReference(name, version)
            : new AssemblyReference(name, version, false, BitConverter.GetBytes(asm.PublicKeyToken));
    }

    // ------------------------------------------------------------------ member references

    private readonly Dictionary<(ModuleDefinition, int, int), IMethodDescriptor> _methodRefCache = [];
    private readonly Dictionary<(ModuleDefinition, int), IFieldDescriptor> _fieldRefCache = [];

    /// <summary>
    /// A method usable from <paramref name="ctx"/>: the definition itself, a MemberReference into another
    /// stub, and for generic instantiations (<paramref name="specIndex"/> ≥ 0) a TypeSpec parent and/or MethodSpec.
    /// </summary>
    public IMethodDescriptor? MethodRefFor(ModuleDefinition ctx, int methodIndex, int specIndex = -1)
    {
        lock (_gate)
        {
            if (_methodRefCache.TryGetValue((ctx, methodIndex, specIndex), out var cached)) return cached;
            var md = _md.Methods[methodIndex];
            var image = ImageOfType(md.DeclaringType);
            if (image < 0) return null;
            var defModule = _modules[image]?.Module ?? BuildModuleUnlocked(image);
            var def = _methodDefs[methodIndex];
            if (def is null) return null;

            Il2CppGenericInstRecord? classInst = null, methodInst = null;
            if (specIndex >= 0 && _reg is not null && (uint)specIndex < (uint)_reg.MethodSpecs.Count)
            {
                var spec = _reg.MethodSpecs[specIndex];
                if (spec.ClassInstIndex >= 0 && spec.ClassInstIndex < _reg.GenericInstVas.Count) classInst = _reg.ReadGenericInst(_reg.GenericInstVas[spec.ClassInstIndex]);
                if (spec.MethodInstIndex >= 0 && spec.MethodInstIndex < _reg.GenericInstVas.Count) methodInst = _reg.ReadGenericInst(_reg.GenericInstVas[spec.MethodInstIndex]);
            }

            IMethodDefOrRef baseMethod;
            if (defModule == ctx && classInst is null) baseMethod = def;
            else
            {
                IMemberRefParent parent;
                var typeRef = TypeRefFor(ctx, md.DeclaringType)!;
                if (classInst is not null)
                    parent = new TypeSpecification(new GenericInstanceTypeSignature(typeRef, IsValueType(md.DeclaringType),
                        classInst.TypeArgVas.Select(v => ToSig(ctx, _reg!.GetTypeAt(v)))));
                else parent = (IMemberRefParent)typeRef;
                baseMethod = new MemberReference(parent, def.Name, RebuildSignature(ctx, methodIndex));
            }

            IMethodDescriptor result = methodInst is not null
                ? new MethodSpecification(baseMethod, new GenericInstanceMethodSignature(methodInst.TypeArgVas.Select(v => ToSig(ctx, _reg!.GetTypeAt(v)))))
                : baseMethod;
            _methodRefCache[(ctx, methodIndex, specIndex)] = result;
            return result;
        }
    }

    private ModuleDefinition BuildModuleUnlocked(int image) => (_modules[image] = BuildModule(image)).Module;

    /// <summary>Method signature (with generic parameters unsubstituted) expressed in <paramref name="ctx"/>.</summary>
    public MethodSignature RebuildSignature(ModuleDefinition ctx, int methodIndex)
    {
        var md = _md.Methods[methodIndex];
        var ret = ToSig(ctx, md.ReturnType);
        var ps = Enumerable.Range(0, md.ParameterCount).Select(p => ToSig(ctx, _md.Parameters[md.ParameterStart + p].TypeIndex)).ToList();
        var gc = (uint)md.GenericContainerIndex < (uint)_md.GenericContainers.Length ? _md.GenericContainers[md.GenericContainerIndex].TypeArgc : 0;
        return (md.Flags & 0x10) != 0 ? MethodSignature.CreateStatic(ret, gc, ps) : MethodSignature.CreateInstance(ret, gc, ps);
    }

    public IFieldDescriptor? FieldRefFor(ModuleDefinition ctx, int fieldIndex, int declaringTypeDef)
    {
        lock (_gate)
        {
            if (_fieldRefCache.TryGetValue((ctx, fieldIndex), out var cached)) return cached;
            var image = ImageOfType(declaringTypeDef);
            if (image < 0) return null;
            if (_modules[image] is null) BuildModuleUnlocked(image);
            var def = _fieldDefs[fieldIndex];
            if (def is null) return null;
            IFieldDescriptor result = def.DeclaringModule == ctx
                ? def
                : new MemberReference((IMemberRefParent)TypeRefFor(ctx, declaringTypeDef)!, def.Name,
                    def.IsStatic ? FieldSignature.CreateStatic(ToSig(ctx, _md.Fields[fieldIndex].TypeIndex)) : FieldSignature.CreateInstance(ToSig(ctx, _md.Fields[fieldIndex].TypeIndex)));
            _fieldRefCache[(ctx, fieldIndex)] = result;
            return result;
        }
    }

    // ------------------------------------------------------------------ writing

    /// <summary>Serializes a stub module to a PE image in memory. Errors are reported, never thrown.</summary>
    public byte[]? Write(StubModule stub)
    {
        lock (_gate)
        {
            var bag = new DiagnosticBag();
            try
            {
                using var ms = new MemoryStream();
                stub.Module.Write(ms, new ManagedPEImageBuilder(new DotNetDirectoryFactory(MetadataBuilderFlags.NoMemberSignatureVerification), bag));
                foreach (var ex in bag.Exceptions.Take(20))
                    _sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"{stub.AssemblyName}: {ex.Message}", new() { AssemblyName = stub.AssemblyName });
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _sink.Error(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"Writing stub '{stub.AssemblyName}' failed: {ex.Message}", new() { AssemblyName = stub.AssemblyName }, ex);
                return null;
            }
        }
    }
}

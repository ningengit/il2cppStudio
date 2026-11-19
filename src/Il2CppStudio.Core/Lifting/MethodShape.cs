using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Calling-convention shape of a metadata method as IL2CPP compiles it on Win64:
/// <c>[ret buffer*] [this] params… [const MethodInfo*]</c>. Types are instantiated for generic method specs.
/// </summary>
public sealed record MethodShape(
    int MethodIndex, bool IsStatic, bool DeclaringIsValueType, bool HasReturnBuffer,
    TypeSignature ReturnType, IReadOnlyList<TypeSignature> Parameters)
{
    /// <summary>Position of the first metadata parameter in the native argument list.</summary>
    public int FirstParameterPosition => (HasReturnBuffer ? 1 : 0) + (IsStatic ? 0 : 1);

    public int ThisPosition => HasReturnBuffer ? 1 : 0;

    public static MethodShape Create(GlobalMetadata md, Il2CppRegistration reg, StubAssemblyBuilder stubs, MemoryResolver mem,
        ModuleDefinition ctx, int methodIndex, int specIndex, bool forCallSite = false,
        IReadOnlyList<TypeSignature>? classArgsOverride = null)
    {
        var def = md.Methods[methodIndex];
        var classArgs = new List<TypeSignature>();
        var methodArgs = new List<TypeSignature>();
        if (classArgsOverride is not null)
            classArgs.AddRange(classArgsOverride);
        if (specIndex >= 0 && (uint)specIndex < (uint)reg.MethodSpecs.Count)
        {
            var spec = reg.MethodSpecs[specIndex];
            if (classArgsOverride is null && spec.ClassInstIndex >= 0 && spec.ClassInstIndex < reg.GenericInstVas.Count)
                classArgs.AddRange(reg.ReadGenericInst(reg.GenericInstVas[spec.ClassInstIndex]).TypeArgVas.Select(v => stubs.ToSig(ctx, reg.GetTypeAt(v))));
            if (spec.MethodInstIndex >= 0 && spec.MethodInstIndex < reg.GenericInstVas.Count)
                methodArgs.AddRange(reg.ReadGenericInst(reg.GenericInstVas[spec.MethodInstIndex]).TypeArgVas.Select(v => stubs.ToSig(ctx, reg.GetTypeAt(v))));
        }

        TypeSignature Inst(TypeSignature t) => t switch
        {
            GenericParameterSignature { ParameterType: GenericParameterType.Type } g =>
                (uint)g.Index < (uint)classArgs.Count ? classArgs[g.Index] : (forCallSite ? ctx.CorLibTypeFactory.Object : t),
            GenericParameterSignature { ParameterType: GenericParameterType.Method } g =>
                (uint)g.Index < (uint)methodArgs.Count ? methodArgs[g.Index] : (forCallSite ? ctx.CorLibTypeFactory.Object : t),
            SzArrayTypeSignature sz => Inst(sz.BaseType).MakeSzArrayType(),
            ByReferenceTypeSignature br => Inst(br.BaseType).MakeByReferenceType(),
            PointerTypeSignature ptr => Inst(ptr.BaseType).MakePointerType(),
            ArrayTypeSignature arr => new ArrayTypeSignature(Inst(arr.BaseType), arr.Rank),
            GenericInstanceTypeSignature gi => new GenericInstanceTypeSignature(gi.GenericType, gi.IsValueType, gi.TypeArguments.Select(Inst).ToArray()),
            _ => t,
        };

        var ret = Inst(stubs.ToSig(ctx, def.ReturnType));
        var ps = Enumerable.Range(0, def.ParameterCount).Select(p => Inst(stubs.ToSig(ctx, md.Parameters[def.ParameterStart + p].TypeIndex))).ToList();
        var isValueDecl = stubs.IsValueType(def.DeclaringType);
        return new MethodShape(methodIndex, (def.Flags & 0x10) != 0, isValueDecl, NeedsReturnBuffer(ret, mem), ret, ps);
    }

    /// <summary>
    /// Struct returned through a hidden pointer. Win64: size not 1, 2, 4 or 8. System V: larger than 16 bytes
    /// (MEMORY class); smaller structs come back in RAX/RDX/XMM0/XMM1, see <see cref="EnsureAbiSupported"/>.
    /// Wasm32: every struct goes through the hidden first pointer (all structs are memory values there).
    /// </summary>
    public static bool NeedsReturnBuffer(TypeSignature t, MemoryResolver mem) =>
        Expr.KindOf(t) == ValueKind.Struct && ArgumentLocations.Abi switch
        {
            NativeAbi.SysV => mem.ValueSize(t) > 16,
            NativeAbi.Wasm32 => true,
            _ => mem.ValueSize(t) is not (1 or 2 or 4 or 8),
        };

    /// <summary>Win64/Wasm32: structs that do not fit a register are passed by pointer (Wasm32: all of them).</summary>
    public static bool PassedByPointer(TypeSignature t, MemoryResolver mem) =>
        ArgumentLocations.Abi is NativeAbi.Win64 or NativeAbi.Wasm32 && NeedsReturnBuffer(t, mem);

    public static bool IsFloat(TypeSignature t) => Expr.KindOf(t) is ValueKind.R4 or ValueKind.R8;

    /// <summary>Float/int class of every native argument position: [ret buffer] [this] params… [MethodInfo*].</summary>
    public bool[] FloatLayout(MemoryResolver mem)
    {
        var layout = new List<bool>();
        if (HasReturnBuffer) layout.Add(false);
        if (!IsStatic) layout.Add(false);
        foreach (var p in Parameters) layout.Add(p is not ByReferenceTypeSignature && !PassedByPointer(p, mem) && IsFloat(p));
        layout.Add(false);
        return [.. layout];
    }

    public readonly record struct SysVArgSpec(bool StackOnly, bool[] Classes);

    /// <summary>System V eightbyte classification for parameter or struct types.</summary>
    public static SysVArgSpec ClassifySysVEightbytes(TypeSignature t, MemoryResolver mem)
    {
        if (t is ByReferenceTypeSignature || PassedByPointer(t, mem) || Expr.KindOf(t) != ValueKind.Struct)
            return new(false, [t is not ByReferenceTypeSignature && IsFloat(t)]);
        var size = mem.ValueSize(t);
        if (size <= 0) return new(false, [false]);
        if (size > 16) return new(true, Enumerable.Repeat(false, (size + 7) / 8).ToArray());
        var count = size > 8 ? 2 : 1;
        var td = mem.TypeDefOf(t);
        var fields = td >= 0 ? FlattenInstanceFields(mem, td, 0) : [];
        var classes = new bool[count];
        for (var c = 0; c < count; c++)
        {
            var lo = c * 8;
            var hi = lo + 8;
            var inChunk = fields.Where(x => x.Offset >= lo && x.Offset < hi).ToList();
            classes[c] = inChunk.Count > 0 && inChunk.All(x => x.Kind is ValueKind.R4 or ValueKind.R8);
        }
        return new(false, classes);
    }

    private static List<(int Offset, ValueKind Kind)> FlattenInstanceFields(MemoryResolver mem, int td, int baseOff)
    {
        var list = new List<(int, ValueKind)>();
        foreach (var f in mem.FieldsOf(td))
        {
            if (f.IsStatic || mem.FieldRef(f)?.Signature?.FieldType is not { } ft) continue;
            var k = Expr.KindOf(ft);
            if (k == ValueKind.Struct && mem.TypeDefOf(ft) is var subTd and >= 0)
                list.AddRange(FlattenInstanceFields(mem, subTd, baseOff + f.Offset));
            else
                list.Add((baseOff + f.Offset, k));
        }
        return list;
    }

    /// <summary>System V eightbyte layout of every native argument position: [ret buffer] [this] params… [MethodInfo*].</summary>
    public SysVArgSpec[] SysVLayout(MemoryResolver mem)
    {
        var list = new List<SysVArgSpec>();
        if (HasReturnBuffer) list.Add(new(false, [false]));
        if (!IsStatic) list.Add(new(false, [false]));
        foreach (var p in Parameters) list.Add(ClassifySysVEightbytes(p, mem));
        list.Add(new(false, [false]));
        return [.. list];
    }

    /// <summary>Registers carrying the return value on System V AMD64 (empty for void or hidden return buffer).</summary>
    public static Iced.Intel.Register[] SysVReturnRegs(TypeSignature ret, MemoryResolver mem)
    {
        var k = Expr.KindOf(ret);
        if (k == ValueKind.Void || NeedsReturnBuffer(ret, mem)) return [];
        if (k != ValueKind.Struct) return [IsFloat(ret) ? Iced.Intel.Register.XMM0 : Iced.Intel.Register.RAX];
        var classes = ClassifySysVEightbytes(ret, mem).Classes;
        var regs = new Iced.Intel.Register[classes.Length];
        int f = 0, g = 0;
        for (var i = 0; i < classes.Length; i++)
            regs[i] = classes[i] ? (f++ == 0 ? Iced.Intel.Register.XMM0 : Iced.Intel.Register.XMM1) : (g++ == 0 ? Iced.Intel.Register.RAX : Iced.Intel.Register.RDX);
        return regs;
    }

    /// <summary>Expression representing eightbyte chunk <paramref name="c"/> of a SysV register-passed/returned struct.</summary>
    public static Expr SysVChunkExpr(Expr source, TypeSignature structType, int c, bool isFloat, MemoryResolver mem, CorLibTypeFactory f)
    {
        var size = mem.ValueSize(structType);
        var off = c * 8;
        var cSize = Math.Min(8, Math.Max(1, size - off));
        var td = mem.TypeDefOf(structType);
        if (td >= 0 && isFloat && mem.StructField(source, td, off) is { } f0 && f0.Kind == ValueKind.R4)
        {
            if (cSize > 4 && mem.StructField(source, td, off + 4) is { } f1 && f1.Kind == ValueKind.R4)
                return new PackedFloatsExpr(f0, f1, f.Single, off == 0 ? source : null);
            if (cSize <= 4)
                return f0;
        }
        return new StructChunkExpr(source, off, cSize);
    }

    /// <summary>Win32 cdecl stack word offsets for each native argument position: [ret buffer] [this] params… [MethodInfo*].</summary>
    public int[] Win32Layout(MemoryResolver mem)
    {
        var list = new List<int>();
        var word = 0;
        if (HasReturnBuffer) { list.Add(word); word += 1; }
        if (!IsStatic) { list.Add(word); word += 1; }
        foreach (var p in Parameters)
        {
            list.Add(word);
            if (p is ByReferenceTypeSignature) { word += 1; continue; }
            var size = Expr.KindOf(p) switch
            {
                ValueKind.I8 or ValueKind.R8 => 8,
                ValueKind.Struct => mem.ValueSize(p),
                _ => 4,
            };
            word += Math.Max(1, (size + 3) / 4);
        }
        list.Add(word);
        return [.. list];
    }

    /// <summary>SysV structs <= 16 bytes and stack > 16 byte structs are modeled.</summary>
    public void EnsureAbiSupported(MemoryResolver mem) { }
}

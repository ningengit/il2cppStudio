using AsmResolver.DotNet.Signatures;
using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Turns decoded operands into expressions: registers (via <see cref="RegisterTracker"/>), immediates, stack
/// slots (scalar or struct-typed), RIP-relative globals (runtime metadata globals, read-only constants) and
/// object memory (via <see cref="MemoryResolver"/>). Unrecognized operands become <see cref="NativeExpr"/>.
/// </summary>
public sealed class OperandReader(RegisterTracker regs, StructSlotTable slots, MemoryResolver mem, NativeSymbolizer sym, CorLibTypeFactory corlib)
{
    public Expr Read(in Instruction ins, int operand) => ins.GetOpKind(operand) switch
    {
        OpKind.Register => regs.Get(ins.GetOpRegister(operand)),
        OpKind.Memory => ReadMemory(ins),
        OpKind.Immediate8 or OpKind.Immediate16 or OpKind.Immediate32 or OpKind.Immediate64
            or OpKind.Immediate8to16 or OpKind.Immediate8to32 or OpKind.Immediate8to64 or OpKind.Immediate32to64 => Immediate(ins, operand),
        _ => new NativeExpr(ins.ToString()),
    };

    private static Expr Immediate(in Instruction ins, int operand)
    {
        var value = unchecked((long)ins.GetImmediate(operand));
        var wide = ins.Op0Kind == OpKind.Register ? ins.Op0Register.IsGPR64() : ins.MemorySize.GetSize() == 8;
        return new ConstExpr(wide ? value : unchecked((int)value), wide ? ValueKind.I8 : ValueKind.I4);
    }

    public Expr ReadMemory(in Instruction ins)
    {
        var size = Math.Max(1, ins.MemorySize.GetSize());
        if (ins.IsIPRelativeMemoryOperand) return ReadGlobal(ins.IPRelativeMemoryAddress, ins.MemorySize);
        var disp = unchecked((long)ins.MemoryDisplacement64);
        if (ins.MemoryBase == Register.RSP && ins.MemoryIndex == Register.None) return ReadStack(regs.SlotKey(disp), size);
        var baseExpr = regs.Get(ins.MemoryBase);
        if (baseExpr is StackAddrPlaceholder p && ins.MemoryIndex == Register.None) return ReadStack(p.Key + disp, size);
        var index = ins.MemoryIndex == Register.None ? null : regs.Get(ins.MemoryIndex);
        if (index is null && disp == 0 && _gotValues.Contains(baseExpr)) return baseExpr;   // [GOT value] = the metadata item
        return mem.Load(baseExpr, index, ins.MemoryIndexScale, disp, size)
               ?? new NativeExpr($"*({baseExpr.Describe()} + 0x{disp:X})", size == 4 ? ValueKind.I4 : ValueKind.I8);
    }

    /// <summary>Scalar slot value, field of a struct-typed slot, or an opaque stack read.</summary>
    public Expr ReadStack(long key, int size)
    {
        if (slots.TryFind(key, out var local, out var offset))
        {
            var holder = new LocalExpr(local);
            if (mem.TypeDefOf(local.VariableType) is var td and >= 0 && mem.StructField(holder, td, offset) is { } field) return field;
            return new StructChunkExpr(holder, (int)offset, size);
        }
        return regs.GetSlot(key) ?? new NativeExpr($"stack[0x{key:X}]", size == 4 ? ValueKind.I4 : ValueKind.I8);
    }

    /// <summary>Address computed by <c>lea</c>.</summary>
    public Expr AddressOf(in Instruction ins)
    {
        if (ins.IsIPRelativeMemoryOperand) return ReadGlobal(ins.IPRelativeMemoryAddress, MemorySize.UInt64);
        var disp = unchecked((long)ins.MemoryDisplacement64);
        if (ins.MemoryBase == Register.RSP && ins.MemoryIndex == Register.None) return new StackAddrPlaceholder(regs.SlotKey(disp));
        // Frame-pointer-relative addresses (rbp = rsp - N, then lea rcx,[rbp+d]) are stack slots too.
        if (ins.MemoryIndex == Register.None && ins.MemoryBase != Register.None && regs.Get(ins.MemoryBase) is StackAddrPlaceholder frame)
            return new StackAddrPlaceholder(frame.Key + disp);
        Expr sum = ins.MemoryBase == Register.None ? new ConstExpr(0, ValueKind.I8) : regs.Get(ins.MemoryBase);
        if (ins.MemoryIndex == Register.None && disp != 0 && mem.ResolveFieldAddress(sum, disp) is { } fieldAddr)
            return fieldAddr;
        if (ins.MemoryIndex != Register.None && sum.Type is SzArrayTypeSignature arr
            && mem.Load(sum, regs.Get(ins.MemoryIndex), ins.MemoryIndexScale, disp, mem.ValueSize(arr.BaseType)) is ArrayElementExpr elem)
            return new AddressExpr(elem);
        if (ins.MemoryIndex != Register.None)
        {
            Expr idx = regs.Get(ins.MemoryIndex);
            if (ins.MemoryIndexScale > 1) idx = new BinaryExpr(AsmResolver.PE.DotNet.Cil.CilOpCodes.Mul, idx, new ConstExpr(ins.MemoryIndexScale, ValueKind.I8), ValueKind.I8);
            sum = new BinaryExpr(AsmResolver.PE.DotNet.Cil.CilOpCodes.Add, sum, idx, ValueKind.I8);
        }
        return disp == 0 ? sum : new BinaryExpr(AsmResolver.PE.DotNet.Cil.CilOpCodes.Add, sum, new ConstExpr(disp, ValueKind.I8), ValueKind.I8);
    }

    public Expr? TryFieldAddress(Expr baseExpr, long disp) => mem.ResolveFieldAddress(baseExpr, disp);

    /// <summary>Runtime metadata global (string literal, TypeInfo, MethodInfo…) or a constant from read-only data.</summary>
    public Expr ReadGlobal(ulong va, MemorySize size)
    {
        var reader = sym.Registration.Reader;
        if (sym.TryGetUsage(va, out var u)) return UsageValue(u);
        // Position-independent code (ELF/Clang): `mov rax, [rip+GOT]; mov rsi, [rax]`. The GOT slot holds the address
        // of the runtime-metadata global; both reads stand for the same metadata item, as `mov rsi, [rip+usage]` does.
        if (size.GetSize() == 8 && reader.IsWritableData(va) && sym.TryGetUsage(reader.ReadPointer(va), out var gu))
        {
            var viaGot = UsageValue(gu);
            _gotValues.Add(viaGot);
            return viaGot;
        }
        if (!reader.IsWritableData(va) && reader.IsMapped(va))
        {
            if (size == MemorySize.Float32) return new FloatExpr(BitConverter.Int32BitsToSingle((int)reader.ReadU32(va)), single: true);
            if (size == MemorySize.Float64) return new FloatExpr(BitConverter.Int64BitsToDouble((long)reader.ReadU64(va)), single: false);
            if (size.GetSize() is 4) return new ConstExpr(reader.ReadI32(va), ValueKind.I4);
            if (size.GetSize() is 16)
                return new VectorConstExpr([reader.ReadU32(va), reader.ReadU32(va + 4), reader.ReadU32(va + 8), reader.ReadU32(va + 12)], corlib.Single);
            if (size.GetSize() is 8) return new ConstExpr((long)reader.ReadU64(va), ValueKind.I8);
        }
        else if (size.GetSize() == 1 && reader.IsWritableData(va))
        {
            // s_Il2CppMethodInitialized-style guard bytes (set to 1 after the metadata-init helper ran): runtime state.
            return new MarkerExpr(MarkerKind.InitFlag, -1, $"init_flag_{va - reader.ImageBase:X}");
        }
        return new NativeExpr($"global_{va - reader.ImageBase:X}", size.GetSize() == 4 ? ValueKind.I4 : ValueKind.I8);
    }

    /// <summary>Values read through a GOT slot; dereferencing them (<c>[reg+0]</c>) yields the same metadata item.</summary>
    private readonly HashSet<Expr> _gotValues = new(ReferenceEqualityComparer.Instance);

    private Expr UsageValue(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.StringLiteral => new StringExpr(sym.Metadata.GetStringLiteral(u.Index), corlib.String),
        MetadataUsageKind.TypeInfo or MetadataUsageKind.Il2CppType => new MarkerExpr(
            u.Kind == MetadataUsageKind.TypeInfo ? MarkerKind.TypeInfo : MarkerKind.Il2CppType,
            sym.Registration.TryGetType(u.Index, out var t) ? sym.Names.ResolveTypeDefIndex(t) : -1, sym.DescribeUsage(u), usage: u),
        MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva => new MarkerExpr(MarkerKind.FieldInfo, u.Index, sym.DescribeUsage(u), usage: u),
        _ => new MarkerExpr(MarkerKind.MethodInfo, u.Index, sym.DescribeUsage(u), usage: u),
    };
}

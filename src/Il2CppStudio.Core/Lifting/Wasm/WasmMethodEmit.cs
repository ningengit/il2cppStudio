using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using WasmDisassembler;

namespace Il2CppStudio.Core.Lifting.Wasm;

/// <summary>
/// Interprets one wasm function body and emits the equivalent CIL. The operand stack is symbolic: a value is an
/// expression tree that is only emitted where a statement consumes it (a store, call, branch condition or return).
/// As a consequence the CIL stack is empty at every wasm control-flow boundary, which makes structured branches,
/// <c>br_table</c> and block results plain labels plus typed locals.
/// </summary>
internal sealed partial class WasmMethodEmit(
    WasmLifter owner, EmitContext ctx, CilMethodBody body, MethodDefinition method, MethodShape shape,
    MemoryResolver mem, List<string> notes)
{
    private const byte TI32 = 0x7F, TI64 = 0x7E, TF32 = 0x7D, TF64 = 0x7C, TVoid = 0x40;

    private readonly CorLibTypeFactory _f = ctx.Module.CorLibTypeFactory;
    private readonly GlobalMetadata _md = owner.Symbolizer.Metadata;
    private readonly Il2CppRegistration _reg = owner.Symbolizer.Registration;
    private readonly ModuleDefinition _module = ctx.Module;
    private readonly StubAssemblyBuilder _stubs = owner.Stubs;
    private readonly MethodShape _shape = shape;
    private readonly MemoryResolver _mem = mem;

    private readonly List<WValue> _stack = [];
    private readonly List<BlockFrame> _frames = [];
    private readonly Dictionary<int, CilLocalVariable> _locals = [];          // wasm local (non-parameter) → CIL local
    private readonly Dictionary<int, Expr> _paramValue = [];                 // wasm local → the value it starts with
    private readonly Dictionary<int, Expr> _subst = [];                      // wasm local → pure substitutable value
    private readonly Dictionary<int, TypeSignature?> _localPointee = [];      // wasm local holding a raw struct pointer
    private readonly Dictionary<int, int> _localDisp = [];                   // wasm local → byte displacement on _subst
    private readonly Dictionary<int, int> _setCount = [];
    private readonly Dictionary<int, FrameSlotState> _slots = [];
    private CilLocalVariable? _retBuf;
    private int _frameBase = -1;
    private int _frameSize;
    private int _spGlobal = -1;
    private int _start;
    private bool _reachable = true;
    private WasmFuncType _sig = new([], []);
    private List<byte> _declaredLocals = [];
    private List<WasmInstruction> _insns = [];
    private int[] _matchEnd = [];
    private int _currentPc;

    public int Unresolved { get; private set; }
    public int Elided { get; private set; }

    // ------------------------------------------------------------------ driver

    public void Run(WasmFuncType sig, List<byte> declaredLocals, List<WasmInstruction> insns)
    {
        _sig = sig;
        _declaredLocals = declaredLocals;
        _insns = insns;
        BuildMatchEnds(insns);
        MapParameters(declaredLocals.Count);
        DetectFrameBase();

        for (var i = _start; i < insns.Count; i++)
        {
            _currentPc = i;
            var ins = insns[i];
            if (!_reachable && ins.Mnemonic is not (WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If
                or WasmMnemonic.Else or WasmMnemonic.End))
                continue;
            Step(ins, ref i);
        }

        if (_frames.Count != 0) throw new WasmUnsupportedException("unbalanced control flow (missing end)");
        if (_reachable) EmitReturn();
    }

    private void BuildMatchEnds(List<WasmInstruction> insns)
    {
        _matchEnd = new int[insns.Count];
        Array.Fill(_matchEnd, insns.Count - 1);
        var stack = new Stack<(int OpenPc, int ElsePc)>();
        for (var k = 0; k < insns.Count; k++)
        {
            var m = insns[k].Mnemonic;
            if (m is WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If)
            {
                stack.Push((k, -1));
            }
            else if (m == WasmMnemonic.Else && stack.Count > 0)
            {
                var top = stack.Pop();
                stack.Push((top.OpenPc, k));
            }
            else if (m == WasmMnemonic.End && stack.Count > 0)
            {
                var (openPc, elsePc) = stack.Pop();
                _matchEnd[openPc] = k;
                if (elsePc >= 0) _matchEnd[elsePc] = k;
            }
        }
    }

    private void MapParameters(int declaredLocalCount)
    {
        var pos = 0;
        if (_shape.HasReturnBuffer)
        {
            _retBuf = NewLocal(_shape.ReturnType);
            _paramValue[pos++] = new AddressExpr(new LocalExpr(_retBuf));
        }
        if (!_shape.IsStatic)
        {
            var declType = _stubs.TypeRefFor(_module, _md.Methods[_shape.MethodIndex].DeclaringType)
                           ?? throw new WasmUnsupportedException("declaring type cannot be referenced");
            var thisType = declType.ToTypeSignature(_shape.DeclaringIsValueType);
            _paramValue[pos++] = _shape.DeclaringIsValueType
                ? new ThisStructExpr(thisType)
                : new ArgExpr(method.Parameters.ThisParameter!, thisType);
        }
        for (var p = 0; p < _shape.Parameters.Count && p < method.Parameters.Count && pos < _sig.ParameterCount; p++, pos++)
        {
            var type = _shape.Parameters[p];
            var parameter = method.Parameters[p];
            if (type is ByReferenceTypeSignature byRef)
            {
                var addr = new ArgExpr(parameter, type);
                _paramValue[pos] = Expr.KindOf(byRef.BaseType) == ValueKind.Struct
                    ? new IndirectStructExpr(byRef.BaseType, addr)
                    : new IndirectExpr(addr, byRef.BaseType);
                _localPointee[pos] = byRef.BaseType;
            }
            else if (Expr.KindOf(type) == ValueKind.Struct)
            {
                _paramValue[pos] = new ArgExpr(parameter, type);   // CIL passes the struct by value; &arg is its address
                _localPointee[pos] = type;
            }
            else
            {
                _paramValue[pos] = new ArgExpr(parameter, type);
            }
        }
        // Trailing const MethodInfo*: consumed by calls, never read by the body.
        if (pos < _sig.ParameterCount) _paramValue[pos] = new ConstExpr(0, ValueKind.I4);

        for (var i = 0; i < _insns.Count; i++)
        {
            if (_insns[i].Mnemonic is not (WasmMnemonic.LocalSet or WasmMnemonic.LocalTee)) continue;
            var local = (int)Convert.ToUInt32(_insns[i].Operands[0]);
            _setCount[local] = _setCount.GetValueOrDefault(local) + 1;
        }
        _ = declaredLocalCount;
    }

    /// <summary>Recognizes IL2CPP/Emscripten stack-frame prologues on upstream clang (5-insn) or fastcomp (6-insn).</summary>
    private void DetectFrameBase()
    {
        _start = 0;
        _spGlobal = owner.Image.IsSplitFunctionTable ? -1 : 0;
        if (_insns.Count >= 5
            && _insns[0].Mnemonic == WasmMnemonic.GlobalGet
            && _insns[1].Mnemonic == WasmMnemonic.I32Const
            && _insns[2].Mnemonic == WasmMnemonic.I32Sub
            && _insns[3].Mnemonic is (WasmMnemonic.LocalTee or WasmMnemonic.LocalSet)
            && _insns[4].Mnemonic == WasmMnemonic.GlobalSet
            && Convert.ToUInt32(_insns[0].Operands[0]) == Convert.ToUInt32(_insns[4].Operands[0]))
        {
            _spGlobal = (int)Convert.ToUInt32(_insns[0].Operands[0]);
            _frameBase = (int)Convert.ToUInt32(_insns[3].Operands[0]);
            _frameSize = (int)Convert.ToInt64(_insns[1].Operands[0]);
            _subst[_frameBase] = new FrameAddrExpr(0);
            _start = 5;
            return;
        }
        if (_insns.Count >= 6
            && _insns[0].Mnemonic == WasmMnemonic.GlobalGet
            && _insns[1].Mnemonic is (WasmMnemonic.LocalTee or WasmMnemonic.LocalSet)
            && _insns[2].Mnemonic == WasmMnemonic.GlobalGet
            && _insns[3].Mnemonic == WasmMnemonic.I32Const
            && _insns[4].Mnemonic is (WasmMnemonic.I32Add or WasmMnemonic.I32Sub)
            && _insns[5].Mnemonic == WasmMnemonic.GlobalSet
            && Convert.ToUInt32(_insns[0].Operands[0]) == Convert.ToUInt32(_insns[2].Operands[0])
            && Convert.ToUInt32(_insns[0].Operands[0]) == Convert.ToUInt32(_insns[5].Operands[0]))
        {
            _spGlobal = (int)Convert.ToUInt32(_insns[0].Operands[0]);
            _frameBase = (int)Convert.ToUInt32(_insns[1].Operands[0]);
            var delta = (int)Convert.ToInt64(_insns[3].Operands[0]);
            _frameSize = _insns[4].Mnemonic == WasmMnemonic.I32Sub ? -delta : delta;
            _subst[_frameBase] = new FrameAddrExpr(0);
            _start = 6;
        }
    }

    // ------------------------------------------------------------------ stack helpers

    private WValue Pop()
    {
        if (_stack.Count == 0) throw new WasmUnsupportedException("wasm operand stack underflow");
        var v = _stack[^1];
        _stack.RemoveAt(_stack.Count - 1);
        return v;
    }

    private WValue Peek(int fromTop)
    {
        if (_stack.Count <= fromTop) throw new WasmUnsupportedException("wasm operand stack underflow");
        return _stack[_stack.Count - 1 - fromTop];
    }

    private void Push(WValue v) => _stack.Add(v);

    private void Push(Expr e, byte wasm) => _stack.Add(new WValue(e, wasm));

    private static byte WasmOf(ValueKind k) => k switch
    {
        ValueKind.I8 => TI64,
        ValueKind.R4 => TF32,
        ValueKind.R8 => TF64,
        _ => TI32,
    };

    private static ValueKind KindOf(byte wasm) => wasm switch
    {
        TI64 => ValueKind.I8,
        TF32 => ValueKind.R4,
        TF64 => ValueKind.R8,
        _ => ValueKind.I4,
    };

    private TypeSignature TypeOf(byte wasm) => wasm switch
    {
        TI64 => _f.Int64,
        TF32 => _f.Single,
        TF64 => _f.Double,
        _ => _f.Int32,
    };

    private Expr As(WValue v, ValueKind kind)
    {
        if (v.Expr.Kind == kind) return v.Expr;
        return new ConvExpr(v.Expr, kind switch
        {
            ValueKind.I4 => _f.Int32,
            ValueKind.I8 => _f.Int64,
            ValueKind.R4 => _f.Single,
            ValueKind.R8 => _f.Double,
            ValueKind.Native => _f.IntPtr,
            _ => TypeOf(v.Wasm),
        });
    }

    private Expr As(WValue v, byte wasm)
    {
        var kind = KindOf(wasm);
        if (v.Expr.Kind == kind) return v.Expr;
        return new ConvExpr(v.Expr, TypeOf(wasm));
    }

    private CilLocalVariable NewLocal(TypeSignature type)
    {
        var local = new CilLocalVariable(type);
        body.LocalVariables.Add(local);
        return local;
    }

    private CilLocalVariable Temp(byte wasm)
    {
        _temps ??= [];
        var key = wasm;
        if (_temps.TryGetValue(key, out var t)) return t;
        t = NewLocal(TypeOf(wasm));
        _temps[key] = t;
        return t;
    }

    private Dictionary<byte, CilLocalVariable>? _temps;

    private void EmitReturn()
    {
        var ret = _shape.ReturnType;
        if (_shape.HasReturnBuffer) { ctx.IL.Add(CilOpCodes.Ldloc, _retBuf!); ctx.IL.Add(CilOpCodes.Ret); return; }
        if (Expr.KindOf(ret) == ValueKind.Void) { ctx.IL.Add(CilOpCodes.Ret); return; }
        if (_stack.Count == 0) { EmitDefault(ret); ctx.IL.Add(CilOpCodes.Ret); return; }
        var v = Pop();
        Expr.EmitAs(ctx, v.Expr, ret);
        ctx.IL.Add(CilOpCodes.Ret);
    }

    private void EmitDefault(TypeSignature ret)
    {
        var kind = Expr.KindOf(ret);
        if (kind is ValueKind.R4 or ValueKind.R8)
            ctx.IL.Add(kind == ValueKind.R4 ? CilOpCodes.Ldc_R4 : CilOpCodes.Ldc_R8, kind == ValueKind.R4 ? 0f : 0d);
        else if (kind == ValueKind.I8) ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
        else if (kind == ValueKind.Ref) ctx.IL.Add(CilOpCodes.Ldnull);
        else ctx.IL.Add(CilOpCodes.Ldc_I4_0);
    }

    // ------------------------------------------------------------------ frame slots

    private sealed class FrameAddrExpr(int offset) : Expr
    {
        public int Offset { get; } = offset;
        public override ValueKind Kind => ValueKind.I4;
        public override void Emit(EmitContext ctx) =>
            throw new WasmUnsupportedException($"stack frame address +0x{Offset:X} escaped without a value access");
    }

    private sealed class FramePairExpr(Expr low, Expr high) : Expr
    {
        public Expr Low { get; } = low;
        public Expr High { get; } = high;
        public override ValueKind Kind => ValueKind.I8;
        public override void Emit(EmitContext ctx) =>
            throw new WasmUnsupportedException("packed 64-bit frame pair escaped without a typed store");
    }

    private FrameSlotState? StructSlotCovering(int offset) =>
        _slots.Values.FirstOrDefault(s => s.StructType is not null && offset >= s.Offset && offset < s.Offset + s.Size);

    private FrameSlotState? ExactSlot(int offset) => _slots.GetValueOrDefault(offset);

    private FrameSlotState ScalarSlot(int offset, byte wasm, int size)
    {
        if (_slots.TryGetValue(offset, out var existing))
        {
            var want = KindOf(wasm);
            if (existing.StructType is null && existing.Local is not null
                && (Expr.KindOf(existing.Local.VariableType) == want || Expr.KindOf(existing.Local.VariableType) == ValueKind.Ref))
                return existing;
        }
        var state = new FrameSlotState(offset) { Size = size, Local = NewLocal(TypeOf(wasm)), ScalarUsed = true };
        _slots[offset] = state;
        return state;
    }

    /// <summary>The struct lvalue stored at <paramref name="offset"/>, creating the slot and its local on first use.</summary>
    private Expr StructLValue(int offset, TypeSignature type, bool preserveExisting = false)
    {
        if (StructSlotCovering(offset) is { } covering)
        {
            var fieldOffset = offset - covering.Offset;
            if (fieldOffset == 0 && SignatureComparer.Default.Equals(covering.StructType, type)) return new LocalExpr(covering.Local!);
            if (fieldOffset > 0 && _mem.StructField(new LocalExpr(covering.Local!), _mem.TypeDefOf(covering.StructType!), fieldOffset) is FieldExpr inner)
                return inner;
        }
        var size = _mem.ValueSize(type);
        var overlapping = _slots.Where(kv => kv.Value.Offset < offset + size && offset < kv.Value.Offset + kv.Value.Size).ToList();
        var preserved = preserveExisting
            ? overlapping.Select(kv => kv.Value).Where(s => s.StructType is null && s.ScalarUsed && s.Local is not null).OrderBy(s => s.Offset).ToList()
            : [];
        foreach (var kv in overlapping) _slots.Remove(kv.Key);
        var state = new FrameSlotState(offset) { Size = size, StructType = type, Local = NewLocal(type) };
        _slots[offset] = state;
        if (preserved.Count > 0)
        {
            var destTd = _mem.TypeDefOf(type);
            var localExpr = new LocalExpr(state.Local);
            foreach (var s in preserved)
            {
                var rel = s.Offset - offset;
                if (rel < 0 || rel >= size) continue;
                var destField = destTd >= 0 ? _mem.StructField(localExpr, destTd, rel) : null;
                Expr srcExpr = s.ConstBits is { } cb && (destField?.Type is { } dt ? Expr.KindOf(dt) is ValueKind.R4 or ValueKind.R8 or ValueKind.I4 : Expr.KindOf(s.Local!.VariableType) is ValueKind.I4 or ValueKind.I8)
                    ? new ConstExpr(cb, ValueKind.I4, true)
                    : new LocalExpr(s.Local!);
                _mem.Store(ctx, new AddressExpr(localExpr), null, 0, rel, s.Size, srcExpr);
            }
        }
        return new LocalExpr(state.Local);
    }

    private Expr FrameLoad(int offset, int size, byte wasm)
    {
        if (StructSlotCovering(offset) is { } covering)
        {
            var rel = offset - covering.Offset;
            var loaded = _mem.Load(new AddressExpr(new LocalExpr(covering.Local!)), null, 0, rel, size);
            if (loaded is not null) return loaded;
            throw new WasmUnsupportedException($"stack +0x{offset:X} (size {size}) is not a field of the {covering.StructType!.Name} at +0x{covering.Offset:X}");
        }
        if (size == 8 && _slots.TryGetValue(offset, out var lo) && lo.StructType is null && lo.Size == 4 && lo.Local is not null
            && _slots.TryGetValue(offset + 4, out var hi) && hi.StructType is null && hi.Size == 4 && hi.Local is not null)
        {
            Expr loExpr = lo.ConstBits is { } lb ? new ConstExpr(lb, ValueKind.I4, true) : new LocalExpr(lo.Local);
            Expr hiExpr = hi.ConstBits is { } hb ? new ConstExpr(hb, ValueKind.I4, true) : new LocalExpr(hi.Local);
            return new FramePairExpr(loExpr, hiExpr);
        }
        if (_slots.TryGetValue(offset, out var existing) && existing.StructType is null && existing.Size == size && existing.Local is not null)
            return existing.ConstBits is { } cb && wasm == TF32
                ? new FloatExpr(BitConverter.Int32BitsToSingle(cb), true)
                : new LocalExpr(existing.Local);
        return new LocalExpr(ScalarSlot(offset, wasm, size).Local!);
    }

    private void FrameStore(int offset, int size, WValue value)
    {
        if (StructSlotCovering(offset) is { } covering)
        {
            if (value.Expr is FramePairExpr pair)
            {
                FrameStore(offset, 4, new WValue(pair.Low, WasmOf(pair.Low.Kind)));
                FrameStore(offset + 4, 4, new WValue(pair.High, WasmOf(pair.High.Kind)));
                return;
            }
            if (!_mem.Store(ctx, new AddressExpr(new LocalExpr(covering.Local!)), null, 0, offset - covering.Offset, size, value.Expr))
                throw new WasmUnsupportedException($"cannot store into stack slot +0x{offset:X} of {covering.StructType!.Name}");
            return;
        }
        if (value.Expr is FramePairExpr fp)
        {
            FrameStore(offset, 4, new WValue(fp.Low, WasmOf(fp.Low.Kind)));
            FrameStore(offset + 4, 4, new WValue(fp.High, WasmOf(fp.High.Kind)));
            return;
        }
        if (value.Expr is StructChunkExpr chunk)
        {
            var srcTd = _mem.TypeDefOf(chunk.Source.Type);
            if (srcTd >= 0 && StoreChunkToScalarSlots(offset, chunk.Source, srcTd, chunk.Offset, chunk.Size, 0, 0))
                return;
        }
        var effectiveWasm = value.Expr.Kind == ValueKind.R4 ? TF32 : value.Wasm;
        if (_slots.TryGetValue(offset, out var cur) && cur.StructType is null
            && (cur.Size != size || Expr.KindOf(cur.Local!.VariableType) != KindOf(effectiveWasm)))
            _slots.Remove(offset);
        var slot = ScalarSlot(offset, effectiveWasm, size);
        slot.ConstBits = value.Expr is ConstExpr c && (size == 4 || c.Value == 0) ? unchecked((int)c.Value) : null;
        Expr.EmitAs(ctx, value.Expr, slot.Local!.VariableType);
        ctx.IL.Add(CilOpCodes.Stloc, slot.Local);
    }

    private bool StoreChunkToScalarSlots(int baseFrameOffset, Expr src, int srcTd, int chunkStart, int chunkSize, int relBase, int depth)
    {
        if (depth > 6) return false;
        var chunkEnd = chunkStart + chunkSize;
        var any = false;
        foreach (var f in _mem.FieldsOf(srcTd))
        {
            if (f.IsStatic || f.DeclaringTypeDef != srcTd) continue;
            var ft = _mem.FieldType(f, src.Type);
            var fSize = _mem.ValueSize(ft);
            var absStart = relBase + f.Offset;
            var absEnd = absStart + fSize;
            if (absEnd <= chunkStart || absStart >= chunkEnd) continue;
            if (_mem.FieldRef(f) is not { } fr) continue;
            var fe = new FieldExpr(src, fr, ft);
            var innerTd = _mem.TypeDefOf(ft);
            if (Expr.KindOf(ft) == ValueKind.Struct && innerTd >= 0 && !_md.TypeDefinitions[innerTd].IsEnum)
            {
                any |= StoreChunkToScalarSlots(baseFrameOffset, fe, innerTd, chunkStart, chunkSize, absStart, depth + 1);
            }
            else if (absStart >= chunkStart)
            {
                FrameStore(baseFrameOffset + (absStart - chunkStart), fSize, new WValue(fe, WasmOf(Expr.KindOf(ft))));
                any = true;
            }
        }
        return any;
    }

    // ------------------------------------------------------------------ memory access

    private static Expr AddDisp(Expr e, int disp) =>
        disp == 0 ? e : new BinaryExpr(CilOpCodes.Add, e, new ConstExpr(disp, ValueKind.I4), ValueKind.I4);

    private Expr? LoadFrom(WValue b, int size, int extra = 0)
    {
        var disp = b.Disp + extra;
        if (b.Expr is FrameAddrExpr fa) return FrameLoad(fa.Offset + disp, size, b.Wasm);
        if (b.Expr is NativeExpr) return b.Expr;
        if (b.Pointee is { } pointee)
        {
            if (Expr.KindOf(pointee) == ValueKind.Struct)
                return _mem.Load(new IndirectStructExpr(pointee, b.Expr), null, 0, disp, size);
            if (disp != 0) return new IndirectExpr(AddDisp(b.Expr, disp), pointee);
            return new IndirectExpr(b.Expr, pointee);
        }
        if (b.Expr is IndirectStructExpr)
            return _mem.Load(b.Expr, null, 0, disp, size);
        if (b.Expr is AddressExpr or MarkerExpr)
            return _mem.Load(b.Expr, null, 0, disp, size);
        if (b.Expr.Type is { } type && Expr.KindOf(type) == ValueKind.Struct)
            return _mem.Load(new AddressExpr(b.Expr), null, 0, disp, size);
        return _mem.Load(b.Expr, null, 0, disp, size);
    }

    private bool StoreTo(WValue b, int size, WValue value)
    {
        var disp = b.Disp;
        if (b.Expr is FrameAddrExpr fa) { FrameStore(fa.Offset + disp, size, value); return true; }
        if (b.Expr is NativeExpr) return false;
        if (size == 8 && value.Expr is FramePairExpr fp)
            return StoreTo(b, 4, new WValue(fp.Low, WasmOf(fp.Low.Kind)))
                && StoreTo(b.WithDisp(4), 4, new WValue(fp.High, WasmOf(fp.High.Kind)));
        if (b.Pointee is { } pointee)
        {
            if (Expr.KindOf(pointee) == ValueKind.Struct)
                return _mem.Store(ctx, new IndirectStructExpr(pointee, b.Expr), null, 0, disp, size, value.Expr);
            if (disp != 0) throw new WasmUnsupportedException($"store through a displaced {pointee.Name} pointer");
            value.Expr.Emit(ctx);
            ctx.IL.Add(CilOpCodes.Stloc, Temp(WasmOf(value.Expr.Kind)));
            var tmp = Temp(WasmOf(value.Expr.Kind));
            var local = tmp;
            // re-emit in [address, value] order
            b.Expr.Emit(ctx);
            ctx.IL.Add(CilOpCodes.Ldloc, local);
            IndirectExpr.EmitStore(ctx, pointee);
            return true;
        }
        if (b.Expr is IndirectStructExpr || b.Expr.Type is { } t && Expr.KindOf(t) == ValueKind.Struct)
            return _mem.Store(ctx, b.Expr is IndirectStructExpr ? b.Expr : new AddressExpr(b.Expr), null, 0, disp, size, value.Expr);
        if (_mem.Store(ctx, b.Expr, null, 0, disp, size, value.Expr)) return true;
        if (b.Expr is AddressExpr addr && disp == 0)
        {
            addr.Emit(ctx);
            value.Expr.Emit(ctx);
            IndirectExpr.EmitStore(ctx, addr.Target.Type ?? _f.Int32);
            return true;
        }
        return false;
    }

    /// <summary>A <c>local.get</c> value: its stored expression, a substituted pure value, or a typed local read.</summary>
    private WValue ReadLocal(int index)
    {
        if (_subst.TryGetValue(index, out var subst)) return new WValue(subst, LocalWasm(index)) { Pointee = _localPointee.GetValueOrDefault(index), Disp = _localDisp.GetValueOrDefault(index) };
        if (_paramValue.TryGetValue(index, out var param))
        {
            var pointee = _localPointee.GetValueOrDefault(index);
            if (param is IndirectStructExpr) return new WValue(param, TI32);
            if (pointee is not null) return new WValue(param is IndirectExpr ie ? ie.Pointer : param, TI32) { Pointee = pointee };
            return new WValue(param, WasmOf(param.Kind));
        }
        var local = LocalFor(index);
        var p = _localPointee.GetValueOrDefault(index);
        if (p is not null) return new WValue(new LocalExpr(local), TI32) { Pointee = p };
        if (Expr.KindOf(local.VariableType) == ValueKind.Struct) return new WValue(new LocalExpr(local), TI32);
        return new WValue(new LocalExpr(local), WasmOf(Expr.KindOf(local.VariableType)));
    }

    private byte LocalWasm(int index)
    {
        if (_paramValue.TryGetValue(index, out var p)) return WasmOf(p.Kind);
        return _locals.TryGetValue(index, out var l) ? WasmOf(Expr.KindOf(l.VariableType)) : TI32;
    }

    private CilLocalVariable LocalFor(int index)
    {
        if (_locals.TryGetValue(index, out var local)) return local;
        var declIdx = index - _sig.ParameterCount;
        var initialType = (uint)declIdx < (uint)_declaredLocals.Count ? TypeOf(_declaredLocals[declIdx]) : _f.Int32;
        local = NewLocal(initialType);
        _locals[index] = local;
        return local;
    }

    /// <summary>Records the value a wasm local now holds; splits into a fresh CLR local when its reference/primitive type changes safely.</summary>
    private void WriteLocal(int index, WValue value)
    {
        if (value.Disp != 0 || value.Expr is FrameAddrExpr or AddressExpr or MarkerExpr
            || _setCount.GetValueOrDefault(index) == 1 && IsSubstitutable(value))
        {
            _subst[index] = value.Expr;
            _localPointee[index] = value.Pointee;
            if (value.Disp != 0) _localDisp[index] = value.Disp; else _localDisp.Remove(index);
            return;
        }
        _subst.Remove(index);
        _localDisp.Remove(index);
        var desired = DesiredLocalType(value);
        if (_paramValue.TryGetValue(index, out var paramExpr) && value.Pointee is null && paramExpr is ArgExpr argExpr)
        {
            if (value.Expr.Kind == paramExpr.Kind && SignatureComparer.Default.Equals(argExpr.Type, desired))
            {
                Expr.EmitAs(ctx, value.Expr, argExpr.Type);
                ctx.IL.Add(CilOpCodes.Starg, argExpr.Parameter);
                return;
            }
            if (CanSplitLocal(index))
            {
                _paramValue.Remove(index);
                _localPointee.Remove(index);
                var split = NewLocal(desired);
                _locals[index] = split;
                Expr.EmitAs(ctx, value.Expr, desired);
                ctx.IL.Add(CilOpCodes.Stloc, split);
                return;
            }
            if (value.Expr.Kind == paramExpr.Kind)
            {
                Expr.EmitAs(ctx, value.Expr, argExpr.Type);
                ctx.IL.Add(CilOpCodes.Starg, argExpr.Parameter);
                return;
            }
        }
        if (value.Pointee is not null && Expr.KindOf(value.Pointee) == ValueKind.Struct)
        {
            CilLocalVariable ptrLocal;
            if (_locals.TryGetValue(index, out var curPtr))
            {
                if (curPtr.VariableType.ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I4 && CanSplitLocal(index))
                {
                    ptrLocal = NewLocal(_f.Int32);
                    _locals[index] = ptrLocal;
                }
                else
                {
                    ptrLocal = curPtr;
                    ptrLocal.VariableType = _f.Int32;
                }
            }
            else
            {
                ptrLocal = NewLocal(_f.Int32);
                _locals[index] = ptrLocal;
            }
            _localPointee[index] = value.Pointee;
            var tmp = Temp(TI32);
            Expr.EmitAs(ctx, value.Expr, _f.Int32);
            ctx.IL.Add(CilOpCodes.Stloc, tmp);
            ctx.IL.Add(CilOpCodes.Ldloc, tmp);
            ctx.IL.Add(CilOpCodes.Stloc, ptrLocal);
            return;
        }
        CilLocalVariable local;
        if (_locals.TryGetValue(index, out var existing))
        {
            if (!SignatureComparer.Default.Equals(existing.VariableType, desired) && CanSplitLocal(index))
            {
                local = NewLocal(desired);
                _locals[index] = local;
            }
            else
            {
                local = existing;
            }
        }
        else
        {
            local = NewLocal(desired);
            _locals[index] = local;
        }
        Expr.EmitAs(ctx, value.Expr, local.VariableType);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        if (value.Expr.Type is { } hint && Expr.KindOf(hint) is ValueKind.Ref or ValueKind.Struct) _localPointee.Remove(index);
    }

    private TypeSignature DesiredLocalType(WValue value)
    {
        if (value.Expr is CompareExpr || value.Expr.Type?.ElementType == AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Boolean)
            return _f.Boolean;
        if (value.Expr.Type is { } t && Expr.KindOf(t) is ValueKind.Ref or ValueKind.R4 or ValueKind.R8 or ValueKind.I8)
            return t;
        return TypeOf(value.Wasm);
    }

    private bool CanSplitLocal(int localIndex)
    {
        foreach (var f in _frames)
        {
            if (f.Kind == WasmMnemonic.Loop)
            {
                for (var k = f.StartPc; k < _currentPc; k++)
                    if (_insns[k].Mnemonic == WasmMnemonic.LocalGet && (int)Convert.ToUInt32(_insns[k].Operands[0]) == localIndex)
                        return false;
            }
            if (!IsLocalDeadAfter(localIndex, f.EndPc)) return false;
        }
        return true;
    }

    private bool IsLocalDeadAfter(int localIndex, int afterPc)
    {
        var depth = 0;
        for (var j = afterPc + 1; j < _insns.Count; j++)
        {
            var m = _insns[j].Mnemonic;
            if (m is WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If)
            {
                depth++;
            }
            else if (m == WasmMnemonic.Else)
            {
                if (depth == 0)
                    j = _matchEnd[j];
            }
            else if (m == WasmMnemonic.End)
            {
                if (depth > 0) depth--;
            }
            else if (m == WasmMnemonic.LocalGet)
            {
                if ((int)Convert.ToUInt32(_insns[j].Operands[0]) == localIndex) return false;
            }
            else if (depth == 0 && (m is WasmMnemonic.Return or WasmMnemonic.Unreachable
                || m is WasmMnemonic.LocalSet or WasmMnemonic.LocalTee && (int)Convert.ToUInt32(_insns[j].Operands[0]) == localIndex))
            {
                return true;
            }
        }
        return true;
    }

    private static bool IsSubstitutable(WValue value) =>
        value.Expr is FrameAddrExpr or ConstExpr or FloatExpr or MarkerExpr or StringExpr or ThisStructExpr
        || value.Expr is LocalExpr or ArgExpr or StaticFieldExpr;

    // ------------------------------------------------------------------ control flow

    private void Step(WasmInstruction ins, ref int i)
    {
        switch (ins.Mnemonic)
        {
            case WasmMnemonic.Nop: return;
            case WasmMnemonic.Block:
                ClearConstBits();
                _frames.Add(new BlockFrame(WasmMnemonic.Block, _stack.Count, BlockSig(ins)) { EntryReachable = _reachable, StartPc = i, EndPc = _matchEnd[i] });
                return;
            case WasmMnemonic.Loop:
            {
                ClearConstBits();
                var frame = new BlockFrame(WasmMnemonic.Loop, _stack.Count, BlockSig(ins)) { EntryReachable = _reachable, StartPc = i, EndPc = _matchEnd[i] };
                _frames.Add(frame);
                if (!_reachable) return;
                if (frame.ParamCount != 0) throw new WasmUnsupportedException("loop with parameters");
                frame.LoopLabel = new CilInstructionLabel();
                frame.LoopLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                return;
            }
            case WasmMnemonic.If: EmitIf(ins, ref i); return;
            case WasmMnemonic.Else: EmitElse(); return;
            case WasmMnemonic.End: EmitEnd(); return;
            case WasmMnemonic.Br: EmitBr(ins); return;
            case WasmMnemonic.BrIf: EmitBrIf(ins); return;
            case WasmMnemonic.BrTable: EmitBrTable(ins); return;
            case WasmMnemonic.Return: EmitReturn(); _reachable = false; return;
            case WasmMnemonic.Unreachable:
                ctx.IL.Add(CilOpCodes.Ldnull);
                ctx.IL.Add(CilOpCodes.Throw);
                _reachable = false;
                return;
            default:
                EmitValueInstruction(ins);
                return;
        }
    }

    private WasmFuncType BlockSig(WasmInstruction ins)
    {
        var raw = Convert.ToInt64(ins.Operands[0]);
        if (raw < 0)
        {
            var b = unchecked((byte)(raw & 0x7F));
            if (b == TVoid) return new WasmFuncType([], []);
            if (b is TI32 or TI64 or TF32 or TF64) return new WasmFuncType([], [b]);
            throw new WasmUnsupportedException($"unsupported block type 0x{b:X2}");
        }
        if (!owner.Image.TryGetType((uint)raw, out var sig)) throw new WasmUnsupportedException($"block type {raw} not found");
        return sig;
    }

    private void ClearConstBits()
    {
        foreach (var s in _slots.Values) s.ConstBits = null;
    }

    private static void ReplaceDict<TK, TV>(Dictionary<TK, TV> dst, Dictionary<TK, TV> src) where TK : notnull
    {
        dst.Clear();
        foreach (var (k, v) in src) dst[k] = v;
    }

    private void EmitIf(WasmInstruction ins, ref int i)
    {
        if (!_reachable)
        {
            _frames.Add(new BlockFrame(WasmMnemonic.If, _stack.Count, BlockSig(ins)) { EntryReachable = false, StartPc = i, EndPc = _matchEnd[i] });
            return;
        }
        ClearConstBits();
        var cond = Pop();
        var sig = BlockSig(ins);
        var frame = new BlockFrame(WasmMnemonic.If, _stack.Count - sig.ParameterCount, sig)
        {
            EntryReachable = true,
            StartPc = i,
            EndPc = _matchEnd[i],
            SavedSubst = new Dictionary<int, Expr>(_subst),
            SavedPointee = new Dictionary<int, TypeSignature?>(_localPointee),
            SavedDisp = new Dictionary<int, int>(_localDisp),
            SavedLocals = new Dictionary<int, CilLocalVariable>(_locals),
            SavedParamValue = new Dictionary<int, Expr>(_paramValue),
        };
        frame.ElseLabel = new CilInstructionLabel();
        frame.EndLabel = new CilInstructionLabel();

        if (IsInitGuard(i, out var endIdx))
        {
            // il2cpp_codegen_initialize_* guard: the call has no managed effect, so the whole if is dropped.
            Elided++;
            i = endIdx;
            return;
        }

        if (sig.ParameterCount > 0)
        {
            var locals = LocalsFor(frame, sig.Parameters, "ifp");
            for (var k = sig.ParameterCount - 1; k >= 0; k--)
            {
                var v = Pop();
                Expr.EmitAs(ctx, v.Expr, locals[k].VariableType);
                ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
            }
            for (var k = 0; k < sig.ParameterCount; k++)
                Push(new LocalExpr(locals[k]), sig.Parameters[k]);
            frame.ParamLocals = locals;
        }

        Expr.EmitAs(ctx, cond.Expr, _f.Int32);
        ctx.IL.Add(CilOpCodes.Brfalse, frame.ElseLabel);
        _frames.Add(frame);
    }

    private void EmitElse()
    {
        if (_frames.Count == 0 || _frames[^1].Kind != WasmMnemonic.If) throw new WasmUnsupportedException("else outside if");
        ClearConstBits();
        var frame = _frames[^1];
        frame.HadElse = true;
        frame.ThenFellThrough = _reachable;
        if (_reachable)
        {
            frame.ThenEndSubst = new Dictionary<int, Expr>(_subst);
            frame.ThenEndPointee = new Dictionary<int, TypeSignature?>(_localPointee);
            frame.ThenEndDisp = new Dictionary<int, int>(_localDisp);
            frame.ThenEndLocals = new Dictionary<int, CilLocalVariable>(_locals);
            frame.ThenEndParamValue = new Dictionary<int, Expr>(_paramValue);
            frame.Branched = true;
            if (frame.ResultCount > 0)
            {
                var locals = LocalsFor(frame, frame.Sig.Results, "ifr");
                frame.ResultLocals = locals;
                for (var k = frame.ResultCount - 1; k >= 0; k--)
                {
                    var v = Pop();
                    Expr.EmitAs(ctx, v.Expr, locals[k].VariableType);
                    ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
                }
                ctx.IL.Add(CilOpCodes.Br, frame.EndLabel!);
            }
            else ctx.IL.Add(CilOpCodes.Br, frame.EndLabel!);
        }
        frame.ElseLabel!.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        _stack.RemoveRange(frame.Base, _stack.Count - frame.Base);
        for (var k = 0; k < frame.ParamCount; k++)
            Push(new LocalExpr(frame.ParamLocals![k]), frame.Sig.Parameters[k]);
        if (frame.SavedSubst is not null) ReplaceDict(_subst, frame.SavedSubst);
        if (frame.SavedPointee is not null) ReplaceDict(_localPointee, frame.SavedPointee);
        if (frame.SavedDisp is not null) ReplaceDict(_localDisp, frame.SavedDisp);
        if (frame.SavedLocals is not null) ReplaceDict(_locals, frame.SavedLocals);
        if (frame.SavedParamValue is not null) ReplaceDict(_paramValue, frame.SavedParamValue);
        _reachable = frame.EntryReachable;
    }

    private void EmitEnd()
    {
        if (_frames.Count == 0) { EmitReturn(); _reachable = false; return; }
        ClearConstBits();
        var frame = _frames[^1];
        _frames.RemoveAt(_frames.Count - 1);

        if (frame.Kind == WasmMnemonic.If)
        {
            var thenReachable = frame.HadElse ? frame.ThenFellThrough : _reachable;
            var elseReachable = frame.HadElse ? _reachable : frame.EntryReachable;

            var results = frame.ResultCount;
            if (_reachable && results > 0)
            {
                var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "ifr");
                for (var k = results - 1; k >= 0; k--)
                {
                    var v = Pop();
                    Expr.EmitAs(ctx, v.Expr, locals[k].VariableType);
                    ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
                }
            }
            else _stack.RemoveRange(frame.Base, _stack.Count - frame.Base);

            if (!frame.HadElse && frame.EntryReachable)
                frame.ElseLabel!.Instruction = ctx.IL.Add(CilOpCodes.Nop);
            if (frame.EndLabel is not null && (frame.Branched || frame.HadElse || results > 0))
                frame.EndLabel.Instruction ??= ctx.IL.Add(CilOpCodes.Nop);
            if (results > 0)
            {
                var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "ifr");
                for (var k = 0; k < results; k++) Push(new LocalExpr(locals[k]), frame.Sig.Results[k]);
            }

            if (frame.SavedSubst is not null)
            {
                if (!thenReachable && elseReachable && !frame.HadElse)
                {
                    ReplaceDict(_subst, frame.SavedSubst);
                    ReplaceDict(_localPointee, frame.SavedPointee!);
                    if (frame.SavedDisp is not null) ReplaceDict(_localDisp, frame.SavedDisp);
                    ReplaceDict(_locals, frame.SavedLocals!);
                    ReplaceDict(_paramValue, frame.SavedParamValue!);
                }
                else if (frame.HadElse && thenReachable && !elseReachable && frame.ThenEndSubst is not null)
                {
                    ReplaceDict(_subst, frame.ThenEndSubst);
                    ReplaceDict(_localPointee, frame.ThenEndPointee!);
                    if (frame.ThenEndDisp is not null) ReplaceDict(_localDisp, frame.ThenEndDisp);
                    ReplaceDict(_locals, frame.ThenEndLocals!);
                    ReplaceDict(_paramValue, frame.ThenEndParamValue!);
                }
                else if (thenReachable && elseReachable)
                {
                    var thenSubst = frame.HadElse ? frame.ThenEndSubst! : new Dictionary<int, Expr>(_subst);
                    var elseSubst = frame.HadElse ? new Dictionary<int, Expr>(_subst) : frame.SavedSubst;
                    _subst.Clear();
                    foreach (var (k, v) in thenSubst)
                        if (elseSubst.TryGetValue(k, out var ev) && SameSubst(v, ev))
                            _subst[k] = v;

                    var thenPointee = frame.HadElse ? frame.ThenEndPointee! : new Dictionary<int, TypeSignature?>(_localPointee);
                    var elsePointee = frame.HadElse ? new Dictionary<int, TypeSignature?>(_localPointee) : frame.SavedPointee!;
                    _localPointee.Clear();
                    foreach (var (k, v) in thenPointee)
                        if (elsePointee.TryGetValue(k, out var ev) && SignatureComparer.Default.Equals(v, ev))
                            _localPointee[k] = v;

                    var thenDisp = frame.HadElse ? (frame.ThenEndDisp ?? []) : new Dictionary<int, int>(_localDisp);
                    var elseDisp = frame.HadElse ? new Dictionary<int, int>(_localDisp) : (frame.SavedDisp ?? []);
                    _localDisp.Clear();
                    foreach (var (k, v) in thenDisp)
                        if (elseDisp.TryGetValue(k, out var ev) && v == ev)
                            _localDisp[k] = v;

                    ReplaceDict(_locals, frame.SavedLocals!);
                    ReplaceDict(_paramValue, frame.SavedParamValue!);
                }
            }

            _reachable = frame.EntryReachable && (_reachable || frame.Branched || !frame.HadElse);
            return;
        }

        static bool SameSubst(Expr a, Expr b) =>
            ReferenceEquals(a, b) || (a is FrameAddrExpr fa && b is FrameAddrExpr fb && fa.Offset == fb.Offset);

        if (frame.Kind == WasmMnemonic.Block)
        {
            var results = frame.ResultCount;
            if (_reachable && results > 0)
            {
                if (frame.Branched)
                {
                    var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "blr");
                    for (var k = results - 1; k >= 0; k--)
                    {
                        var v = Pop();
                        Expr.EmitAs(ctx, v.Expr, locals[k].VariableType);
                        ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
                    }
                    frame.EndLabel!.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                    for (var k = 0; k < results; k++) Push(new LocalExpr(locals[k]), frame.Sig.Results[k]);
                }
            }
            else
            {
                _stack.RemoveRange(frame.Base, _stack.Count - frame.Base);
                if (frame.Branched && results > 0)
                {
                    var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "blr");
                    frame.EndLabel!.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                    for (var k = 0; k < results; k++) Push(new LocalExpr(locals[k]), frame.Sig.Results[k]);
                }
            }
            if (frame.Branched && results == 0 && frame.EndLabel is not null)
                frame.EndLabel.Instruction ??= ctx.IL.Add(CilOpCodes.Nop);
            _reachable = frame.EntryReachable && (_reachable || frame.Branched);
            return;
        }

        // loop: results are the loop's own fallthrough values
        _reachable = frame.EntryReachable && _reachable;
    }

    private CilLocalVariable[] LocalsFor(BlockFrame frame, byte[] types, string prefix)
    {
        var locals = new CilLocalVariable[types.Length];
        for (var k = 0; k < types.Length; k++)
        {
            var hint = k == types.Length - 1 ? HintForResult(frame, k) : null;
            locals[k] = NewLocal(hint ?? TypeOf(types[k]));
        }
        _ = prefix;
        return locals;
    }

    private TypeSignature? HintForResult(BlockFrame frame, int index)
    {
        if (_stack.Count <= index) return null;
        var v = _stack[_stack.Count - 1 - (frame.ResultCount - 1 - index)];
        return v.Expr.Type is { } t && Expr.KindOf(t) is ValueKind.Ref or ValueKind.Struct ? t : null;
    }

    /// <summary>True when the <c>if</c> at <paramref name="ifIndex"/> only guards an il2cpp init helper call.</summary>
    private bool IsInitGuard(int ifIndex, out int endIndex)
    {
        endIndex = -1;
        var depth = 0;
        var callSeen = false;
        for (var i = ifIndex + 1; i < _insns.Count; i++)
        {
            var m = _insns[i].Mnemonic;
            if (m is WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If) depth++;
            else if (m == WasmMnemonic.End)
            {
                if (depth == 0) { endIndex = i; return callSeen; }
                depth--;
            }
            else if (m == WasmMnemonic.Else) { if (depth == 0) return false; }
            else if (m == WasmMnemonic.Call)
            {
                if (!IsInitHelper(Convert.ToUInt32(_insns[i].Operands[0]))) return false;
                callSeen = true;
            }
            else if (m is not (WasmMnemonic.I32Const or WasmMnemonic.LocalGet or WasmMnemonic.LocalSet or WasmMnemonic.LocalTee
                     or WasmMnemonic.GlobalGet or WasmMnemonic.GlobalSet or WasmMnemonic.I32Store or WasmMnemonic.I32Store8
                     or WasmMnemonic.I32Load or WasmMnemonic.I32Load8_U or WasmMnemonic.I32Load8_S or WasmMnemonic.Drop
                     or WasmMnemonic.I32Eqz))
                return false;
        }
        return false;
    }

    private void SkipToEnd(int i)
    {
        var depth = 0;
        for (var k = i + 1; k < _insns.Count; k++)
        {
            switch (_insns[k].Mnemonic)
            {
                case WasmMnemonic.Block or WasmMnemonic.Loop or WasmMnemonic.If: depth++; break;
                case WasmMnemonic.End:
                    if (depth == 0) { Rewind(i + 1, k); return; }
                    depth--;
                    break;
            }
        }
        throw new WasmUnsupportedException("unterminated if");
    }

    private void Rewind(int from, int to) => _insns[from] = _insns[to];

    private void EmitBr(WasmInstruction ins)
    {
        var depth = (int)Convert.ToUInt32(ins.Operands[0]);
        if (depth >= _frames.Count) throw new WasmUnsupportedException("br depth out of range");
        if (!_reachable) return;
        var frame = _frames[_frames.Count - 1 - depth];
        var count = frame.Kind == WasmMnemonic.Loop ? frame.ParamCount : frame.ResultCount;
        if (count > 0)
        {
            if (frame.Kind == WasmMnemonic.Loop) throw new WasmUnsupportedException("br to a loop with parameters");
            var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "brr");
            for (var k = count - 1; k >= 0; k--)
            {
                var v = Pop();
                Expr.EmitAs(ctx, v.Expr, locals[k].VariableType);
                ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
            }
            frame.Branched = true;
            frame.EndLabel ??= new CilInstructionLabel();
            ctx.IL.Add(CilOpCodes.Br, frame.EndLabel);
        }
        else
        {
            var label = frame.Kind == WasmMnemonic.Loop ? frame.LoopLabel! : frame.EndLabel ??= new CilInstructionLabel();
            if (frame.Kind != WasmMnemonic.Loop) frame.Branched = true;
            ctx.IL.Add(CilOpCodes.Br, label);
        }
        _stack.RemoveRange(frame.Base, _stack.Count - frame.Base);
        _reachable = false;
    }

    private void EmitBrIf(WasmInstruction ins)
    {
        var depth = (int)Convert.ToUInt32(ins.Operands[0]);
        if (depth >= _frames.Count) throw new WasmUnsupportedException("br_if depth out of range");
        if (!_reachable) return;
        var frame = _frames[_frames.Count - 1 - depth];
        var count = frame.Kind == WasmMnemonic.Loop ? frame.ParamCount : frame.ResultCount;
        var cond = Pop();
        if (count == 0)
        {
            Expr.EmitAs(ctx, cond.Expr, _f.Int32);
            if (frame.Kind == WasmMnemonic.Loop) ctx.IL.Add(CilOpCodes.Brtrue, frame.LoopLabel!);
            else { frame.Branched = true; ctx.IL.Add(CilOpCodes.Brtrue, frame.EndLabel ??= new CilInstructionLabel()); }
            return;
        }
        if (frame.Kind == WasmMnemonic.Loop) throw new WasmUnsupportedException("br_if to a loop with parameters");
        var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "brr");
        var values = new WValue[count];
        for (var k = count - 1; k >= 0; k--) values[k] = Pop();
        for (var k = 0; k < count; k++)
        {
            Expr.EmitAs(ctx, values[k].Expr, locals[k].VariableType);
            ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
        }
        var cont = new CilInstructionLabel();
        Expr.EmitAs(ctx, cond.Expr, _f.Int32);
        ctx.IL.Add(CilOpCodes.Brfalse, cont);
        frame.Branched = true;
        ctx.IL.Add(CilOpCodes.Br, frame.EndLabel ??= new CilInstructionLabel());
        cont.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        foreach (var v in values) Push(v);
    }

    private void EmitBrTable(WasmInstruction ins)
    {
        var labels = (ulong[])ins.Operands[0];
        var defaultDepth = (int)Convert.ToUInt32(ins.Operands[1]);
        if (!_reachable) return;
        var index = Pop();
        var frame0 = _frames[_frames.Count - 1 - defaultDepth];
        var count = frame0.Kind == WasmMnemonic.Loop ? frame0.ParamCount : frame0.ResultCount;
        if (count > 0 && _frames.Any(f => f.Kind == WasmMnemonic.Loop && f.ParamCount > 0))
            throw new WasmUnsupportedException("br_table to a loop with parameters");

        var temps = new CilLocalVariable[count];
        for (var k = count - 1; k >= 0; k--)
        {
            var v = Pop();
            temps[k] = NewLocal(v.Expr.Type is { } t && Expr.KindOf(t) is ValueKind.Ref ? t : TypeOf(v.Wasm));
            Expr.EmitAs(ctx, v.Expr, temps[k].VariableType);
            ctx.IL.Add(CilOpCodes.Stloc, temps[k]);
        }
        Expr.EmitAs(ctx, index.Expr, _f.Int32);
        var cases = new CilInstructionLabel[labels.Length];
        for (var k = 0; k < cases.Length; k++) cases[k] = new CilInstructionLabel();
        ctx.IL.Add(CilOpCodes.Switch, cases);
        EmitBrTableTarget(defaultDepth, temps);
        for (var k = 0; k < cases.Length; k++)
        {
            cases[k].Instruction = ctx.IL.Add(CilOpCodes.Nop);
            EmitBrTableTarget((int)labels[k], temps);
        }
        _reachable = false;
        _stack.RemoveRange(frame0.Base, _stack.Count - frame0.Base);
    }

    private void EmitBrTableTarget(int depth, CilLocalVariable[] temps)
    {
        var frame = _frames[_frames.Count - 1 - depth];
        if (frame.Kind == WasmMnemonic.Loop)
        {
            if (frame.ParamCount != 0) throw new WasmUnsupportedException("br_table to a loop with parameters");
            ctx.IL.Add(CilOpCodes.Br, frame.LoopLabel!);
            return;
        }
        var locals = frame.ResultLocals ??= LocalsFor(frame, frame.Sig.Results, "brr");
        for (var k = 0; k < temps.Length; k++)
        {
            var converted = temps[k].VariableType;
            var target = locals[k].VariableType;
            if (SignatureComparer.Default.Equals(converted, target))
            {
                ctx.IL.Add(CilOpCodes.Ldloc, temps[k]);
                ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
            }
            else
            {
                ctx.IL.Add(CilOpCodes.Ldloc, temps[k]);
                Expr.Convert(ctx, Expr.KindOf(converted), Expr.KindOf(target), target);
                ctx.IL.Add(CilOpCodes.Stloc, locals[k]);
            }
        }
        frame.Branched = true;
        ctx.IL.Add(CilOpCodes.Br, frame.EndLabel ??= new CilInstructionLabel());
    }
}

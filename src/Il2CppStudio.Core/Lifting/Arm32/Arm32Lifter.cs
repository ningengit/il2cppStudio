using System.Globalization;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Arm32;

/// <summary>
/// AAPCS32 (ARMv7 / Thumb-2) native method lifter using the Capstone ARM decoder, lifting directly into
/// shared Unified IR expressions and CIL method bodies for Android armeabi-v7a.
/// </summary>
public sealed class Arm32Lifter(NativeSymbolizer sym, StubAssemblyBuilder stubs, CapstoneArmDisassembler disasm,
    IDiagnosticSink sink, Runtime.RuntimeHelperMap? helpers = null) : IMethodLifter
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);

    private sealed class Block(int start, ulong va)
    {
        public int Start { get; } = start;
        public int End { get; set; }
        public ulong Va { get; } = va;
        public CilInstructionLabel Label { get; } = new();
        public List<Block> Successors { get; } = [];
        public List<Block> Predecessors { get; } = [];
        public bool IsTrap { get; set; }
    }

    public LiftResult Lift(int methodIndex)
    {
        ArgumentLocations.Abi = NativeAbi.Win32;
        var md = sym.Metadata;
        var image = stubs.ImageOfType(md.Methods[methodIndex].DeclaringType);
        if (image < 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "no image");
        var stub = stubs.GetModule(image);
        var method = stubs.GetMethodDef(methodIndex);
        var va = sym.Map.GetMethodVa(methodIndex);
        if (method is null || va == 0 || method.IsAbstract) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");

        var loc = (ulong ip) => new DiagnosticLocation { Rva = ip - sym.Registration.Reader.ImageBase, MethodName = sym.MethodName(methodIndex) };
        try
        {
            var (start, end) = sym.Map.GetFunctionBounds(va, 0x4000);
            var code = sym.Registration.Reader.Slice(start, (int)Math.Min(end - start, 0x4000UL));
            var insns = disasm.DisassembleInstructions(code, start, out var isThumb);
            if (insns.Count == 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "empty function");

            var blocks = BuildBlocks(insns, sym);
            var elidedInsns = FindElidedNullChecks(insns, sym);
            var body = new CilMethodBody { InitializeLocals = true };
            var notes = new List<string>();
            var ctx = new EmitContext(body.Instructions, stub.Module, stub.Intrinsics) { Log = notes.Add };
            var mem = new MemoryResolver(md, sym.Model, sym.Registration, sym.Names, stubs, stub.Module, _typeIndex, pointerSize: 4);
            var shape = MethodShape.Create(md, sym.Registration, stubs, mem, stub.Module, methodIndex, -1);

            var tr = new Translator(sym, stubs, stub.Module, method, shape, mem, body, helpers, isThumb, elidedInsns, notes, start, insns[^1].Address + 4);
            var order = TopologicalOrder(blocks);
            var emitted = order.ToHashSet();
            for (var bi = 0; bi < order.Count; bi++)
            {
                var b = order[bi];
                b.Label.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                tr.EnterBlock(b, isEntry: b == blocks[0]);
                if (b.IsTrap)
                {
                    ctx.IL.Add(CilOpCodes.Ldnull);
                    ctx.IL.Add(CilOpCodes.Throw);
                    continue;
                }
                tr.TranslateBlock(ctx, insns, b, bi + 1 < order.Count ? order[bi + 1] : null, emitted);
            }

            if (body.Instructions.Count == 0 || body.Instructions[^1].OpCode.Code is not (CilCode.Ret or CilCode.Throw or CilCode.Br))
                tr.EmitDefaultReturn(ctx);

            body.Instructions.OptimizeMacros();
            method.CilMethodBody = body;
            stub.LiftedMethods.Add(methodIndex);
            return new LiftResult(methodIndex, true, insns.Count, body.Instructions.Count, tr.Unsupported, tr.Unresolved, tr.Elided, null) { Notes = notes };
        }
        catch (Exception ex)
        {
            StubAssemblyBuilder.SetPlaceholderBody(method);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"ARMv7 lifting failed: {ex.Message}", loc(va), ex);
            return new(methodIndex, false, 0, 0, 0, 0, 0, ex.Message);
        }
    }

    private static HashSet<int> FindElidedNullChecks(IReadOnlyList<CapstoneArmDisassembler.Arm32Insn> insns, NativeSymbolizer sym)
    {
        var elided = new HashSet<int>();
        for (var i = 0; i + 2 < insns.Count; i++)
        {
            if (TryBranchTarget(insns[i], out var cond, out var target) && cond is not null
                && insns[i + 1].Mnemonic.StartsWith("bl", StringComparison.Ordinal)
                && TryParseHexOrDec(insns[i + 1].Operands, out var blTarget)
                && sym.Map.SymbolAt(blTarget) is null
                && target == insns[i + 2].Address)
            {
                elided.Add(i);
                elided.Add(i + 1);
            }
        }
        return elided;
    }

    private static List<Block> BuildBlocks(IReadOnlyList<CapstoneArmDisassembler.Arm32Insn> insns, NativeSymbolizer sym)
    {
        var idxAt = new Dictionary<ulong, int>(insns.Count);
        for (var i = 0; i < insns.Count; i++) idxAt[insns[i].Address] = i;

        bool IsNullCheckSkip(int i) =>
            i + 2 < insns.Count
            && TryBranchTarget(insns[i], out var c, out var t) && c is not null
            && insns[i + 1].Mnemonic.StartsWith("bl", StringComparison.Ordinal)
            && TryParseHexOrDec(insns[i + 1].Operands, out var blTarget)
            && sym.Map.SymbolAt(blTarget) is null
            && t == insns[i + 2].Address;

        bool IsTrapAt(ulong target)
        {
            if (!idxAt.TryGetValue(target, out var i)) return false;
            var hasBl = false;
            for (var k = i; k < Math.Min(insns.Count, i + 3); k++)
            {
                var mn = insns[k].Mnemonic;
                if (mn == "bx" || (mn == "pop" && insns[k].Operands.Contains("pc"))) return false;
                if (mn.StartsWith("bl", StringComparison.Ordinal))
                {
                    if (TryParseHexOrDec(insns[k].Operands, out var bt) && sym.Map.SymbolAt(bt) is not null) return false;
                    hasBl = true;
                    continue;
                }
                if (hasBl && mn == "b") return false;
            }
            return hasBl && i >= insns.Count - 3;
        }

        var leaders = new SortedSet<int> { 0 };
        for (var i = 0; i < insns.Count; i++)
        {
            var ins = insns[i];
            if (IsReturn(ins)) { if (i + 1 < insns.Count) leaders.Add(i + 1); continue; }
            if (IsNullCheckSkip(i)) { i++; continue; }
            if (TryBranchTarget(ins, out var cond, out var target))
            {
                if (cond is null)
                {
                    if (idxAt.TryGetValue(target, out var ti)) leaders.Add(ti);
                    if (i + 1 < insns.Count) leaders.Add(i + 1);
                }
                else if (!IsTrapAt(target) && idxAt.TryGetValue(target, out var ci))
                {
                    leaders.Add(ci);
                    if (i + 1 < insns.Count) leaders.Add(i + 1);
                }
            }
        }

        var list = leaders.ToList();
        var blocks = new List<Block>(list.Count);
        var byVa = new Dictionary<ulong, Block>(list.Count);
        for (var n = 0; n < list.Count; n++)
        {
            var b = new Block(list[n], insns[list[n]].Address) { End = n + 1 < list.Count ? list[n + 1] : insns.Count };
            blocks.Add(b);
            byVa[b.Va] = b;
        }

        for (var bi = 0; bi < blocks.Count; bi++)
        {
            var b = blocks[bi];
            if (b.End - b.Start <= 2 && IsTrapAt(b.Va) && b != blocks[0]) { b.IsTrap = true; continue; }
            var last = insns[b.End - 1];
            if (IsReturn(last)) continue;
            if (TryBranchTarget(last, out var cond, out var target))
            {
                if (cond is null)
                {
                    if (byVa.TryGetValue(target, out var tb) && !tb.IsTrap) b.Successors.Add(tb);
                    continue;
                }
                if (byVa.TryGetValue(target, out var cb) && !cb.IsTrap && !IsTrapAt(target)) b.Successors.Add(cb);
                if (bi + 1 < blocks.Count && !blocks[bi + 1].IsTrap) b.Successors.Add(blocks[bi + 1]);
                continue;
            }
            if (bi + 1 < blocks.Count && !blocks[bi + 1].IsTrap) b.Successors.Add(blocks[bi + 1]);
        }

        foreach (var b in blocks)
            foreach (var s in b.Successors.Distinct())
                s.Predecessors.Add(b);
        return blocks;
    }

    private static List<Block> TopologicalOrder(List<Block> blocks)
    {
        var seen = new HashSet<Block>();
        var q = new Queue<Block>();
        seen.Add(blocks[0]);
        q.Enqueue(blocks[0]);
        while (q.Count > 0)
        {
            var c = q.Dequeue();
            foreach (var s in c.Successors) if (!s.IsTrap && seen.Add(s)) q.Enqueue(s);
        }
        return [.. blocks.Where(seen.Contains)];
    }

    private static bool IsReturn(in CapstoneArmDisassembler.Arm32Insn ins) =>
        ins.Mnemonic == "bx" && ins.Operands.Trim() == "lr"
        || ins.Mnemonic == "pop" && ins.Operands.Contains("pc");

    private static bool TryBranchTarget(in CapstoneArmDisassembler.Arm32Insn ins, out string? cond, out ulong target)
    {
        cond = null; target = 0;
        var m = ins.Mnemonic;
        if (m is "cbz" or "cbnz")
        {
            cond = m;
            var comma = ins.Operands.LastIndexOf(',');
            return comma > 0 && TryParseHexOrDec(ins.Operands.AsSpan(comma + 1), out target);
        }
        if (m == "b") return TryParseHexOrDec(ins.Operands, out target);
        if (m.Length == 3 && m[0] == 'b' && !m.StartsWith("bl", StringComparison.Ordinal) && m != "bic" && m != "bics" && m != "bfc" && m != "bfi" && m != "bkpt" && m != "bx" && m != "blx")
        {
            cond = m[1..];
            return TryParseHexOrDec(ins.Operands, out target);
        }
        return false;
    }

    private static bool TryParseHexOrDec(ReadOnlySpan<char> span, out ulong value)
    {
        span = span.Trim();
        if (span.StartsWith("#")) span = span[1..].Trim();
        if (span.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return ulong.TryParse(span[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        return ulong.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }

    private sealed class Translator
    {
        private readonly NativeSymbolizer _sym;
        private readonly StubAssemblyBuilder _stubs;
        private readonly ModuleDefinition _module;
        private readonly MethodDefinition _method;
        private readonly MethodShape _shape;
        private readonly MemoryResolver _mem;
        private readonly CilMethodBody _body;
        private readonly CorLibTypeFactory _f;
        private readonly Runtime.RuntimeHelperMap? _helpers;
        private readonly bool _isThumb;
        private readonly HashSet<int> _elided;
        private readonly List<string> _notes;
        private readonly ulong _funcStart, _funcEnd;
        private readonly Dictionary<string, Expr> _regs = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<long, Expr> _slots = [];
        private readonly Dictionary<string, CilLocalVariable> _spill = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Block, Dictionary<string, Expr>> _exit = [];
        private readonly HashSet<Expr> _gotValues = new(ReferenceEqualityComparer.Instance);
        private CilLocalVariable? _retBuf;
        private long _spDelta;
        private bool _flagsFloat;
        private Expr? _flagL, _flagR;

        public int Unsupported { get; private set; }
        public int Unresolved { get; private set; }
        public int Elided { get; private set; }

        public Translator(NativeSymbolizer sym, StubAssemblyBuilder stubs, ModuleDefinition module, MethodDefinition method,
            MethodShape shape, MemoryResolver mem, CilMethodBody body, Runtime.RuntimeHelperMap? helpers, bool isThumb,
            HashSet<int> elided, List<string> notes, ulong funcStart, ulong funcEnd)
        {
            _sym = sym; _stubs = stubs; _module = module; _method = method; _shape = shape;
            _mem = mem; _body = body; _f = module.CorLibTypeFactory; _helpers = helpers; _isThumb = isThumb;
            _elided = elided; _notes = notes; _funcStart = funcStart; _funcEnd = funcEnd;
        }

        private CilLocalVariable NewLocal(TypeSignature t) { var l = new CilLocalVariable(t); _body.LocalVariables.Add(l); return l; }

        public void EnterBlock(Block b, bool isEntry)
        {
            if (isEntry)
            {
                _regs.Clear();
                var rIdx = 0;
                if (_shape.HasReturnBuffer)
                {
                    _retBuf = NewLocal(_shape.ReturnType);
                    _regs[$"r{rIdx++}"] = new AddressExpr(new LocalExpr(_retBuf));
                }
                if (!_shape.IsStatic)
                {
                    var declType = _method.DeclaringType!.ToTypeSignature(_shape.DeclaringIsValueType);
                    _regs[$"r{rIdx++}"] = _shape.DeclaringIsValueType
                        ? new AddressExpr(new ThisStructExpr(declType))
                        : new ArgExpr(_method.Parameters.ThisParameter!, declType);
                }
                for (var p = 0; p < _shape.Parameters.Count && p < _method.Parameters.Count; p++)
                {
                    var pt = _shape.Parameters[p];
                    var arg = new ArgExpr(_method.Parameters[p], pt);
                    var key = rIdx < 4 ? $"r{rIdx}" : $"stack_{rIdx}";
                    _regs[key] = Expr.KindOf(pt) == ValueKind.Struct && _mem.ValueSize(pt) > 4 ? new AddressExpr(arg) : arg;
                    rIdx += Expr.KindOf(pt) is ValueKind.I8 or ValueKind.R8 ? 2 : 1;
                }
                if (rIdx < 4) _regs[$"r{rIdx}"] = new MarkerExpr(MarkerKind.MethodInfo, _shape.MethodIndex, "method");
                return;
            }
            var pred = b.Predecessors.FirstOrDefault(_exit.ContainsKey);
            if (pred is not null)
            {
                _regs.Clear();
                foreach (var kv in _exit[pred]) _regs[kv.Key] = kv.Value;
            }
        }

        public void TranslateBlock(EmitContext ctx, IReadOnlyList<CapstoneArmDisassembler.Arm32Insn> insns, Block b, Block? nextInEmit, HashSet<Block> emitted)
        {
            for (var i = b.Start; i < b.End; i++)
            {
                if (_elided.Contains(i)) { Elided++; continue; }
                var ins = insns[i];
                if (IsReturn(ins))
                {
                    EmitReturn(ctx);
                    return;
                }
                if (TryBranchTarget(ins, out var cond, out var target))
                {
                    if (cond is null)
                    {
                        if (target < _funcStart || target >= _funcEnd) { EmitCallTarget(ctx, target); EmitReturn(ctx); return; }
                        SpillOut(ctx, b);
                        if (b.Successors.Count == 1 && emitted.Contains(b.Successors[0]) && b.Successors[0] != nextInEmit)
                            ctx.IL.Add(CilOpCodes.Br, b.Successors[0].Label);
                        return;
                    }
                    var targetBlock = b.Successors.FirstOrDefault(s => s.Va == target && emitted.Contains(s));
                    var ft = b.Successors.FirstOrDefault(s => s != targetBlock && emitted.Contains(s)) ?? (b.Successors.Count > 1 && emitted.Contains(b.Successors[1]) ? b.Successors[1] : null);
                    if (targetBlock is null)
                    {
                        Elided++;
                        continue;
                    }
                    var cmpExpr = BuildBranchCondition(ins, cond);
                    SpillOut(ctx, b);
                    if (cmpExpr is MarkerExpr { Marker: MarkerKind.InitFlag })
                    {
                        Elided++;
                        if (cond == "ne" && targetBlock != nextInEmit) ctx.IL.Add(CilOpCodes.Br, targetBlock.Label);
                        else if (ft is not null && ft != nextInEmit) ctx.IL.Add(CilOpCodes.Br, ft.Label);
                        return;
                    }
                    cmpExpr.Emit(ctx);
                    ctx.IL.Add(CilOpCodes.Brtrue, targetBlock.Label);
                    if (ft is not null && ft != nextInEmit) ctx.IL.Add(CilOpCodes.Br, ft.Label);
                    return;
                }
                TranslateOne(ctx, ins);
            }
            SpillOut(ctx, b);
            if (b.Successors.Count == 1 && emitted.Contains(b.Successors[0]) && b.Successors[0] != nextInEmit)
                ctx.IL.Add(CilOpCodes.Br, b.Successors[0].Label);
        }

        private void TranslateOne(EmitContext ctx, in CapstoneArmDisassembler.Arm32Insn ins)
        {
            var m = ins.Mnemonic;
            if (m is "push" or "vpush" or "pop" or "vpop" or "nop")
            {
                var delta = Math.Max(4, (ins.Operands.Split(',').Length) * (m.StartsWith('v') ? 8 : 4));
                _spDelta += m.EndsWith("push") ? delta : m.EndsWith("pop") ? -delta : 0;
                return;
            }
            if (m is "bl" or "blx")
            {
                if (TryParseHexOrDec(ins.Operands, out var target)) EmitCallTarget(ctx, target);
                else { Unresolved++; ctx.IL.Add(CilOpCodes.Ldstr, $"arm32_indirect@{ins.Address:X}"); Emit4Args(ctx); ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative); _regs["r0"] = StoreI64ToI32(ctx); }
                return;
            }
            if (m == "vmrs") return;

            // Handle conditional instruction suffixes (e.g. moveq, movne, ldrne, strne)
            var baseMnem = StripConditionSuffix(m, out var condSuffix);
            CilInstructionLabel? skipCond = null;
            if (condSuffix is not null && BuildBranchCondition(ins, condSuffix) is { } condExpr)
            {
                if (baseMnem == "mov")
                {
                    var parts = SplitArgs(ins.Operands);
                    if (parts.Length == 2)
                    {
                        var dst = parts[0];
                        var cur = _regs.TryGetValue(dst, out var existing) ? existing : new ConstExpr(0, ValueKind.I4);
                        var newVal = ReadOperand(ins, parts[1], preferFloat: false);
                        var t = newVal.Type ?? cur.Type ?? _f.Int32;
                        var l = NewLocal(t);
                        var elseLbl = new CilInstructionLabel();
                        var endLbl = new CilInstructionLabel();
                        condExpr.Emit(ctx);
                        ctx.IL.Add(CilOpCodes.Brfalse, elseLbl);
                        Expr.EmitAs(ctx, newVal, t);
                        ctx.IL.Add(CilOpCodes.Stloc, l);
                        ctx.IL.Add(CilOpCodes.Br, endLbl);
                        elseLbl.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                        Expr.EmitAs(ctx, cur, t);
                        ctx.IL.Add(CilOpCodes.Stloc, l);
                        endLbl.Instruction = ctx.IL.Add(CilOpCodes.Nop);
                        _regs[dst] = new LocalExpr(l);
                        return;
                    }
                }
                skipCond = new CilInstructionLabel();
                condExpr.Emit(ctx);
                ctx.IL.Add(CilOpCodes.Brfalse, skipCond);
            }

            var ops = SplitArgs(ins.Operands);
            switch (baseMnem)
            {
                case "mov" or "movw" when ops.Length == 2:
                    _regs[ops[0]] = ReadOperand(ins, ops[1], preferFloat: false);
                    break;
                case "vmov" when ops.Length == 2:
                    _regs[ops[0]] = ReadOperand(ins, ops[1], preferFloat: true);
                    break;
                case "add" when ops.Length == 3:
                {
                    if (ops[0] == "sp" && ops[1] == "sp" && TryParseImm(ops[2], out var adj)) { _spDelta -= adj; break; }
                    if (ops[1] == "pc")
                    {
                        var off = ReadOperand(ins, ops[2], false) is ConstExpr c ? c.Value : 0;
                        var targetVa = unchecked((uint)((long)PcVal(ins.Address) + off));
                        if (_sym.TryGetUsage(targetVa, out var u)) _regs[ops[0]] = UsageValue(u);
                        else _regs[ops[0]] = new ConstExpr(targetVa, ValueKind.I4);
                        break;
                    }
                    if (ops[1] == "sp" && TryParseImm(ops[2], out var spOff))
                    {
                        _regs[ops[0]] = new StackAddrPlaceholder(spOff - _spDelta);
                        break;
                    }
                    var l = ReadOperand(ins, ops[1], false);
                    var r = ReadOperand(ins, ops[2], false);
                    if (r is ConstExpr rc && _mem.ResolveFieldAddress(l, rc.Value) is { } fa) _regs[ops[0]] = fa;
                    else _regs[ops[0]] = new BinaryExpr(CilOpCodes.Add, l, r, ValueKind.I4);
                    break;
                }
                case "sub" when ops.Length == 3:
                {
                    if (ops[0] == "sp" && ops[1] == "sp" && TryParseImm(ops[2], out var adj)) { _spDelta += adj; break; }
                    _regs[ops[0]] = new BinaryExpr(CilOpCodes.Sub, ReadOperand(ins, ops[1], false), ReadOperand(ins, ops[2], false), ValueKind.I4);
                    break;
                }
                case "mul" or "and" or "orr" or "eor" or "lsl" or "lsr" or "asr" when ops.Length == 3:
                {
                    var cil = baseMnem switch
                    {
                        "mul" => CilOpCodes.Mul, "and" => CilOpCodes.And, "orr" => CilOpCodes.Or, "eor" => CilOpCodes.Xor,
                        "lsl" => CilOpCodes.Shl, "asr" => CilOpCodes.Shr, _ => CilOpCodes.Shr_Un,
                    };
                    _regs[ops[0]] = new BinaryExpr(cil, ReadOperand(ins, ops[1], false), ReadOperand(ins, ops[2], false), ValueKind.I4);
                    break;
                }
                case "vadd.f32" or "vsub.f32" or "vmul.f32" or "vdiv.f32" when ops.Length == 3:
                {
                    var cil = baseMnem switch
                    {
                        "vadd.f32" => CilOpCodes.Add, "vsub.f32" => CilOpCodes.Sub,
                        "vmul.f32" => CilOpCodes.Mul, _ => CilOpCodes.Div,
                    };
                    _regs[ops[0]] = new BinaryExpr(cil, ReadOperand(ins, ops[1], true), ReadOperand(ins, ops[2], true), ValueKind.R4);
                    break;
                }
                case "vmla.f32" or "vmls.f32" when ops.Length == 3:
                {
                    var d = ReadOperand(ins, ops[0], true);
                    var n = ReadOperand(ins, ops[1], true);
                    var m2 = ReadOperand(ins, ops[2], true);
                    var prod = new BinaryExpr(CilOpCodes.Mul, n, m2, ValueKind.R4);
                    _regs[ops[0]] = new BinaryExpr(baseMnem == "vmla.f32" ? CilOpCodes.Add : CilOpCodes.Sub, d, prod, ValueKind.R4);
                    break;
                }
                case "cmp" when ops.Length == 2:
                    _flagsFloat = false;
                    _flagL = ReadOperand(ins, ops[0], false);
                    _flagR = ReadOperand(ins, ops[1], false);
                    break;
                case "vcmp.f32" when ops.Length == 2:
                    _flagsFloat = true;
                    _flagL = ReadOperand(ins, ops[0], true);
                    _flagR = ReadOperand(ins, ops[1], true);
                    break;
                case "ldr" or "ldrb" or "ldrh" or "ldrsh" or "ldrsb" or "vldr" when ops.Length >= 2:
                {
                    var dst = ops[0];
                    var size = baseMnem is "ldrb" or "ldrsb" ? 1 : baseMnem is "ldrh" or "ldrsh" ? 2 : 4;
                    var isF = baseMnem == "vldr" || dst.StartsWith('s');
                    var memStr = string.Join(", ", ops[1..]);
                    _regs[dst] = LoadMemory(ins.Address, memStr, size, isF);
                    break;
                }
                case "str" or "strb" or "strh" or "vstr" when ops.Length >= 2:
                {
                    var isF = baseMnem == "vstr" || ops[0].StartsWith('s');
                    var size = baseMnem == "strb" ? 1 : baseMnem == "strh" ? 2 : 4;
                    var memStr = string.Join(", ", ops[1..]);
                    StoreMemory(ctx, ins, memStr, size, ReadOperand(ins, ops[0], isF));
                    break;
                }
                default:
                    Unsupported++;
                    _notes.Add($"unsupported instruction at 0x{ins.Address - _sym.Registration.Reader.ImageBase:X}: {m} {ins.Operands}");
                    break;
            }

            if (skipCond is not null) skipCond.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        }

        private Expr LoadMemory(ulong pc, string memStr, int size, bool isF)
        {
            ParseMem(memStr, out var baseReg, out var idxReg, out var disp);
            if (baseReg == "pc")
            {
                var pcVal = PcVal(pc);
                if (idxReg is not null && ReadReg(idxReg) is ConstExpr cOff) disp = cOff.Value;
                var targetVa = unchecked((uint)((long)pcVal + disp));
                if (_sym.TryGetUsage(targetVa, out var u)) return UsageValue(u);
                var rdr = _sym.Registration.Reader;
                var val = rdr.IsMapped(targetVa) ? rdr.ReadU32(targetVa) : 0;
                if (idxReg is null && _sym.TryGetUsage(val, out var u2)) return UsageValue(u2);
                if (idxReg is not null && rdr.IsWritableData(targetVa) && _sym.TryGetUsage(val, out var gu)) { var ge = UsageValue(gu); _gotValues.Add(ge); return ge; }
                return isF ? new FloatExpr(BitConverter.Int32BitsToSingle((int)val), true) : new ConstExpr(unchecked((int)val), ValueKind.I4);
            }
            if (baseReg == "sp")
            {
                var key = disp - _spDelta;
                return _slots.GetValueOrDefault(key) ?? new NativeExpr($"stack[0x{key:X}]", isF ? ValueKind.R4 : ValueKind.I4);
            }
            var baseExpr = ReadReg(baseReg);
            if (baseExpr is ConstExpr cb)
            {
                var addr = unchecked((uint)(cb.Value + disp));
                if (_sym.TryGetUsage(addr, out var u)) return UsageValue(u);
                if (size == 1 && _sym.Registration.Reader.IsWritableData(addr)) return new MarkerExpr(MarkerKind.InitFlag, -1, $"arm32_init_{addr:X}");
            }
            if (_gotValues.Contains(baseExpr) && disp == 0 && idxReg is null) return baseExpr;
            if (baseExpr is StackAddrPlaceholder sp && idxReg is null)
                return _slots.GetValueOrDefault(sp.Key + disp) ?? new NativeExpr($"stack[0x{sp.Key + disp:X}]", isF ? ValueKind.R4 : ValueKind.I4);
            var idx = idxReg is null ? null : ReadReg(idxReg);
            return _mem.Load(baseExpr, idx, 1, disp, size)
                   ?? new NativeExpr($"*({baseExpr.Describe()}+0x{disp:X})", isF ? ValueKind.R4 : ValueKind.I4);
        }

        private void StoreMemory(EmitContext ctx, in CapstoneArmDisassembler.Arm32Insn ins, string memStr, int size, Expr value)
        {
            ParseMem(memStr, out var baseReg, out var idxReg, out var disp);
            if (baseReg == "sp")
            {
                _slots[disp - _spDelta] = value;
                return;
            }
            var baseExpr = ReadReg(baseReg);
            if (baseExpr is ConstExpr) return;
            if (baseExpr is StackAddrPlaceholder sp && idxReg is null) { _slots[sp.Key + disp] = value; return; }
            var idx = idxReg is null ? null : ReadReg(idxReg);
            if (_mem.Store(ctx, baseExpr, idx, 1, disp, size, value)) return;
            Unsupported++;
            ctx.IL.Add(CilOpCodes.Ldstr, $"{ins.Mnemonic} {ins.Operands} ; value = {value.Describe()}");
            ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.Stmt);
        }

        private void EmitCallTarget(EmitContext ctx, ulong targetVa)
        {
            if (_sym.Map.SymbolAt(targetVa) is not null)
            {
                var r0 = ReadReg("r0");
                var recvType = r0 is NewObjExpr no ? no.Type : r0 is AddressExpr ae ? ae.Target.Type : r0.Type;
                var td = _mem.TypeDefOf(recvType);
                var s = IcfResolver.Choose(_sym.Map.CandidatesAt(targetVa), td, _sym.Metadata, _sym.Registration, _sym.Names, ctx.Log,
                    receiverIsNewObject: r0 is NewObjExpr { IsMaterialized: false },
                    receiverType: recvType, stubs: _stubs, module: _module);
                if (EmitManagedCall(ctx, s.MethodIndex, s.MethodSpecIndex)) return;
            }
            if (ReadReg("r0") is MarkerExpr { Marker: MarkerKind.MethodInfo or MarkerKind.TypeInfo or MarkerKind.Il2CppType })
            {
                Elided++;
                ClobberVolatiles();
                return;
            }
            Unresolved++;
            var name = _helpers?.ExportNames.GetValueOrDefault(targetVa) ?? $"sub_{targetVa - _sym.Registration.Reader.ImageBase:X}";
            ctx.IL.Add(CilOpCodes.Ldstr, name);
            Emit4Args(ctx);
            ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative);
            _regs["r0"] = StoreI64ToI32(ctx);
            ClobberVolatiles();
        }

        private bool EmitManagedCall(EmitContext ctx, int methodIndex, int specIndex)
        {
            if (methodIndex < 0 && specIndex >= 0 && specIndex < _sym.Registration.MethodSpecs.Count)
                methodIndex = _sym.Registration.MethodSpecs[specIndex].MethodDefinitionIndex;
            if ((uint)methodIndex >= (uint)_sym.Metadata.Methods.Length) return false;
            var baseShape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex);
            var rPos = (baseShape.HasReturnBuffer ? 1 : 0) + (!baseShape.IsStatic ? 1 : 0) + baseShape.Parameters.Count;
            if (rPos < 4 && ReadReg($"r{rPos}") is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: MetadataUsageKind.MethodRef } mu }
                && (uint)mu.Index < (uint)_sym.Registration.MethodSpecs.Count && _sym.Registration.MethodSpecs[mu.Index].MethodDefinitionIndex == methodIndex)
                specIndex = mu.Index;

            var target = _stubs.MethodRefFor(_module, methodIndex, specIndex);
            if (target is null) return false;
            var shape = MethodShape.Create(_sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex);
            var rIdx = 0;
            var retPtr = shape.HasReturnBuffer ? ReadReg($"r{rIdx++}") : null;
            var thisExpr = shape.IsStatic ? null : ReadReg($"r{rIdx++}");
            CallEmitter.RebindReceiverGeneric(ref target, ref shape, thisExpr, _sym.Metadata, _sym.Registration, _stubs, _mem, _module, methodIndex, specIndex);

            if (thisExpr is not null)
            {
                var baseTarget = target is MethodSpecification ms ? ms.Method : target;
                var declSig = baseTarget is MemberReference { Parent: TypeSpecification { Signature: { } ts } }
                    ? ts
                    : _stubs.TypeRefFor(_module, _sym.Metadata.Methods[methodIndex].DeclaringType)?.ToTypeSignature(shape.DeclaringIsValueType);
                if (shape.DeclaringIsValueType) new AddressExpr(thisExpr).Emit(ctx);
                else if (thisExpr is ConstExpr { Value: 0 }) ctx.IL.Add(CilOpCodes.Ldnull);
                else if (declSig is not null) Expr.EmitAs(ctx, thisExpr, declSig);
                else thisExpr.Emit(ctx);
            }
            for (var p = 0; p < shape.Parameters.Count; p++)
            {
                var pt = shape.Parameters[p];
                var arg = rIdx < 4 ? ReadReg($"r{rIdx++}") : new ConstExpr(0, ValueKind.I4);
                if (Expr.KindOf(pt) is ValueKind.I8 or ValueKind.R8) rIdx++;
                Expr.EmitAs(ctx, arg, pt);
            }
            ctx.IL.Add(!shape.IsStatic && !shape.DeclaringIsValueType ? CilOpCodes.Callvirt : CilOpCodes.Call, target);
            ClobberVolatiles();

            if (Expr.KindOf(shape.ReturnType) == ValueKind.Void) return true;
            if (shape.HasReturnBuffer && retPtr is StackAddrPlaceholder sp)
            {
                var local = NewLocal(shape.ReturnType);
                ctx.IL.Add(CilOpCodes.Stloc, local);
                var td = _mem.TypeDefOf(shape.ReturnType);
                if (td >= 0)
                {
                    var le = new LocalExpr(local);
                    foreach (var fl in _mem.FieldsOf(td).Where(f => !f.IsStatic))
                        if (_mem.StructField(le, td, fl.Offset) is { } sf) _slots[sp.Key + fl.Offset] = sf;
                }
                _regs["r0"] = new AddressExpr(new LocalExpr(local));
                return true;
            }
            var res = NewLocal(shape.ReturnType);
            ctx.IL.Add(CilOpCodes.Stloc, res);
            _regs["r0"] = new LocalExpr(res);
            return true;
        }

        private void EmitReturn(EmitContext ctx)
        {
            var ret = _shape.ReturnType;
            var k = Expr.KindOf(ret);
            if (k == ValueKind.Void) { ctx.IL.Add(CilOpCodes.Ret); return; }
            if (_shape.HasReturnBuffer && _retBuf is not null) { ctx.IL.Add(CilOpCodes.Ldloc, _retBuf); ctx.IL.Add(CilOpCodes.Ret); return; }
            Expr.EmitAs(ctx, ReadReg(MethodShape.IsFloat(ret) && _regs.ContainsKey("s0") ? "s0" : "r0"), ret);
            ctx.IL.Add(CilOpCodes.Ret);
        }

        public void EmitDefaultReturn(EmitContext ctx)
        {
            var k = Expr.KindOf(_shape.ReturnType);
            if (k == ValueKind.I8) ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
            else if (k == ValueKind.R4) ctx.IL.Add(CilOpCodes.Ldc_R4, 0f);
            else if (k == ValueKind.R8) ctx.IL.Add(CilOpCodes.Ldc_R8, 0d);
            else if (k == ValueKind.Ref) ctx.IL.Add(CilOpCodes.Ldnull);
            else if (k != ValueKind.Void) ctx.IL.Add(CilOpCodes.Ldc_I4_0);
            ctx.IL.Add(CilOpCodes.Ret);
        }

        private void SpillOut(EmitContext ctx, Block b)
        {
            var ex = _exit[b] = [];
            foreach (var (r, e) in _regs)
            {
                if (!RegisterTracker.IsStorable(e) || e is ArgExpr or ConstExpr or FloatExpr or StringExpr) { ex[r] = e; continue; }
                var t = e.Type ?? (e.Kind == ValueKind.R4 ? _f.Single : e.Kind == ValueKind.Ref ? _f.Object : _f.Int32);
                if (!_spill.TryGetValue(r, out var l)) { l = NewLocal(t); _spill[r] = l; }
                if (e is LocalExpr le && le.Local == l) { ex[r] = e; continue; }
                Expr.EmitAs(ctx, e, l.VariableType);
                ctx.IL.Add(CilOpCodes.Stloc, l);
                ex[r] = new LocalExpr(l);
            }
        }

        private Expr BuildBranchCondition(in CapstoneArmDisassembler.Arm32Insn ins, string cond)
        {
            if (cond is "cbz" or "cbnz")
            {
                var rName = ins.Operands.Split(',')[0].Trim();
                var rv = ReadReg(rName);
                if (rv is MarkerExpr { Marker: MarkerKind.InitFlag }) return rv;
                return new CompareExpr(cond == "cbz" ? Condition.Eq : Condition.Ne, rv, new ConstExpr(0, rv.Kind == ValueKind.Ref ? ValueKind.Ref : ValueKind.I4));
            }
            if (_flagL is MarkerExpr { Marker: MarkerKind.InitFlag }) return _flagL;
            var l = _flagL ?? new ConstExpr(0, ValueKind.I4);
            var r = _flagR ?? new ConstExpr(0, l.Kind == ValueKind.R4 ? ValueKind.R4 : ValueKind.I4);
            var c = cond switch
            {
                "eq" => Condition.Eq,
                "ne" => Condition.Ne,
                "lt" or "mi" => Condition.Lt,
                "le" or "ls" => _flagsFloat || cond == "le" ? Condition.Le : Condition.LeUn,
                "gt" => Condition.Gt,
                "ge" or "pl" => Condition.Ge,
                "hi" => _flagsFloat ? Condition.Gt : Condition.GtUn,
                "lo" or "cc" => _flagsFloat ? Condition.Lt : Condition.LtUn,
                "hs" or "cs" => _flagsFloat ? Condition.Ge : Condition.GeUn,
                _ => Condition.Ne,
            };
            return new CompareExpr(c, l, r);
        }

        private Expr ReadOperand(in CapstoneArmDisassembler.Arm32Insn ins, string text, bool preferFloat)
        {
            text = text.Trim();
            if (text.StartsWith('#'))
            {
                var s = text.AsSpan(1).Trim();
                if (preferFloat && !s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) && float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
                    return new FloatExpr(f, true);
                if (TryParseImm(text, out var imm))
                    return preferFloat ? new FloatExpr(imm, true) : new ConstExpr(imm, ValueKind.I4);
            }
            return ReadReg(text);
        }

        private Expr ReadReg(string name)
        {
            name = name.Trim();
            if (_regs.TryGetValue(name, out var e)) return e;
            return new NativeExpr(name, name.StartsWith('s') ? ValueKind.R4 : ValueKind.I4);
        }

        private Expr UsageValue(MetadataUsage u) => u.Kind switch
        {
            MetadataUsageKind.StringLiteral => new StringExpr(_sym.Metadata.GetStringLiteral(u.Index), _f.String),
            MetadataUsageKind.TypeInfo or MetadataUsageKind.Il2CppType => new MarkerExpr(
                u.Kind == MetadataUsageKind.TypeInfo ? MarkerKind.TypeInfo : MarkerKind.Il2CppType,
                _sym.Registration.TryGetType(u.Index, out var t) ? _sym.Names.ResolveTypeDefIndex(t) : -1, _sym.DescribeUsage(u), usage: u),
            _ => new MarkerExpr(MarkerKind.MethodInfo, u.Index, _sym.DescribeUsage(u), usage: u),
        };

        private ulong PcVal(ulong insAddr) => _isThumb ? ((insAddr + 4) & ~3UL) : (insAddr + 8);
        private LocalExpr StoreI64ToI32(EmitContext ctx) { var l = NewLocal(_f.Int64); ctx.IL.Add(CilOpCodes.Stloc, l); return new LocalExpr(l); }
        private void Emit4Args(EmitContext ctx)
        {
            for (var i = 0; i < 4; i++)
            {
                if (_regs.TryGetValue($"r{i}", out var e) && !e.IsMarker) Expr.EmitAs(ctx, e, _f.Int64);
                else ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
            }
        }
        private void ClobberVolatiles() { foreach (var r in new[] { "r0", "r1", "r2", "r3", "r12", "s0", "s1", "s2", "s3" }) _regs.Remove(r); }

        private static string StripConditionSuffix(string m, out string? cond)
        {
            cond = null;
            if (m.Length >= 5 && m is not ("ldrsb" or "ldrsh" or "vsqrt.f32" or "vadd.f32" or "vsub.f32" or "vmul.f32" or "vdiv.f32" or "vmla.f32" or "vmls.f32" or "vcmp.f32"))
            {
                var tail = m[^2..];
                if (tail is "eq" or "ne" or "lt" or "le" or "gt" or "ge" or "hi" or "ls" or "hs" or "cs" or "lo" or "cc" or "mi" or "pl")
                {
                    cond = tail;
                    return m[..^2];
                }
            }
            return m;
        }

        private static string[] SplitArgs(string ops)
        {
            var result = new List<string>();
            var depth = 0;
            var start = 0;
            for (var i = 0; i < ops.Length; i++)
            {
                if (ops[i] is '[' or '{') depth++;
                else if (ops[i] is ']' or '}') depth--;
                else if (ops[i] == ',' && depth == 0) { result.Add(ops[start..i].Trim()); start = i + 1; }
            }
            if (start < ops.Length) result.Add(ops[start..].Trim());
            return [.. result];
        }

        private static void ParseMem(string memStr, out string baseReg, out string? indexReg, out long disp)
        {
            baseReg = "r0"; indexReg = null; disp = 0;
            var l = memStr.IndexOf('[');
            var r = memStr.IndexOf(']');
            if (l < 0 || r <= l) return;
            var inner = memStr[(l + 1)..r].Split(',');
            baseReg = inner[0].Trim();
            if (inner.Length >= 2)
            {
                var p1 = inner[1].Trim();
                if (TryParseImm(p1, out var imm)) disp = imm;
                else indexReg = p1;
            }
        }

        private static bool TryParseImm(string text, out long val)
        {
            val = 0;
            var s = text.AsSpan().Trim();
            if (s.StartsWith("#")) s = s[1..].Trim();
            var neg = s.StartsWith("-");
            if (neg) s = s[1..];
            var ok = s.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? long.TryParse(s[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val)
                : long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out val);
            if (ok && neg) val = -val;
            return ok;
        }
    }
}

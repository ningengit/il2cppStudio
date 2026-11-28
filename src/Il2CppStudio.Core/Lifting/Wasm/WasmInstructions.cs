using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using WasmDisassembler;

namespace Il2CppStudio.Core.Lifting.Wasm;

internal sealed partial class WasmMethodEmit
{
    private bool IsInitHelper(uint functionIndex)
    {
        if (owner.Image.TryGetImportName(functionIndex, out _, out var name)
            && name.Contains("initialize", StringComparison.OrdinalIgnoreCase))
            return true;
        if (owner.SymbolsFor(functionIndex).Count == 0
            && owner.Image.TryGetFunctionType(functionIndex, out var ft)
            && ft.ParameterCount == 1 && ft.Parameters[0] == TI32 && ft.ResultCount == 0)
            return true;
        return false;
    }

    private void EmitValueInstruction(WasmInstruction ins)
    {
        switch (ins.Mnemonic)
        {
            case WasmMnemonic.Drop:
                _ = Pop();
                return;
            case WasmMnemonic.Select:
                EmitSelect();
                return;
            case WasmMnemonic.LocalGet:
                Push(ReadLocal(OpInt(ins)));
                return;
            case WasmMnemonic.LocalSet:
                WriteLocal(OpInt(ins), Pop());
                return;
            case WasmMnemonic.LocalTee:
            {
                var v = Pop();
                WriteLocal(OpInt(ins), v);
                Push(ReadLocal(OpInt(ins)));
                return;
            }
            case WasmMnemonic.GlobalGet:
            {
                var gi = (uint)OpUlong(ins);
                if (gi == (uint)_spGlobal) { Push(new FrameAddrExpr(_frameSize), TI32); return; }
                Push(new ConstExpr(owner.Image.TryGetGlobal(gi, out var g) ? g.Init : 0, TI64 == (owner.Image.TryGetGlobal(gi, out var g2) ? g2.ValueType : TI32) ? ValueKind.I8 : ValueKind.I4),
                    owner.Image.TryGetGlobal(gi, out var g3) ? g3.ValueType : TI32);
                return;
            }
            case WasmMnemonic.GlobalSet:
                _ = Pop();   // global 0 stack pointer restore on exit
                return;
            case WasmMnemonic.MemorySize:
                Push(new ConstExpr(0x100, ValueKind.I4), TI32);
                return;
            case WasmMnemonic.MemoryGrow:
                _ = Pop();
                Push(new ConstExpr(-1, ValueKind.I4), TI32);
                return;

            case WasmMnemonic.I32Const:
                Push(new ConstExpr(unchecked((int)OpLong(ins)), ValueKind.I4), TI32);
                return;
            case WasmMnemonic.I64Const:
                Push(new ConstExpr(OpLong(ins), ValueKind.I8), TI64);
                return;
            case WasmMnemonic.F32Const:
                Push(new FloatExpr(Convert.ToSingle(ins.Operands[0]), single: true), TF32);
                return;
            case WasmMnemonic.F64Const:
                Push(new FloatExpr(Convert.ToDouble(ins.Operands[0]), single: false), TF64);
                return;

            // Loads
            case WasmMnemonic.I32Load: EmitLoad(ins, TI32, 4, signed: true); return;
            case WasmMnemonic.I64Load: EmitLoad(ins, TI64, 8, signed: true); return;
            case WasmMnemonic.F32Load: EmitLoad(ins, TF32, 4, signed: true); return;
            case WasmMnemonic.F64Load: EmitLoad(ins, TF64, 8, signed: true); return;
            case WasmMnemonic.I32Load8_S: EmitLoad(ins, TI32, 1, signed: true); return;
            case WasmMnemonic.I32Load8_U: EmitLoad(ins, TI32, 1, signed: false); return;
            case WasmMnemonic.I32Load16_S: EmitLoad(ins, TI32, 2, signed: true); return;
            case WasmMnemonic.I32Load16_U: EmitLoad(ins, TI32, 2, signed: false); return;
            case WasmMnemonic.I64Load8_S: EmitLoad(ins, TI64, 1, signed: true); return;
            case WasmMnemonic.I64Load8_U: EmitLoad(ins, TI64, 1, signed: false); return;
            case WasmMnemonic.I64Load16_S: EmitLoad(ins, TI64, 2, signed: true); return;
            case WasmMnemonic.I64Load16_U: EmitLoad(ins, TI64, 2, signed: false); return;
            case WasmMnemonic.I64Load32_S: EmitLoad(ins, TI64, 4, signed: true); return;
            case WasmMnemonic.I64Load32_U: EmitLoad(ins, TI64, 4, signed: false); return;

            // Stores
            case WasmMnemonic.I32Store or WasmMnemonic.F32Store or WasmMnemonic.I64Store32: EmitStore(ins, 4); return;
            case WasmMnemonic.I64Store or WasmMnemonic.F64Store: EmitStore(ins, 8); return;
            case WasmMnemonic.I32Store8 or WasmMnemonic.I64Store8: EmitStore(ins, 1); return;
            case WasmMnemonic.I32Store16 or WasmMnemonic.I64Store16: EmitStore(ins, 2); return;

            // Comparisons
            case WasmMnemonic.I32Eqz: Push(new CompareExpr(Condition.Eq, Pop().Expr, new ConstExpr(0, ValueKind.I4)), TI32); return;
            case WasmMnemonic.I64Eqz: Push(new CompareExpr(Condition.Eq, Pop().Expr, new ConstExpr(0, ValueKind.I8)), TI32); return;
            case WasmMnemonic.I32Eq or WasmMnemonic.I64Eq or WasmMnemonic.F32Eq or WasmMnemonic.F64Eq: EmitCompare(Condition.Eq); return;
            case WasmMnemonic.I32Ne or WasmMnemonic.I64Ne or WasmMnemonic.F32Ne or WasmMnemonic.F64Ne: EmitCompare(Condition.Ne); return;
            case WasmMnemonic.I32Lt_S or WasmMnemonic.I64Lt_S or WasmMnemonic.F32Lt or WasmMnemonic.F64Lt: EmitCompare(Condition.Lt); return;
            case WasmMnemonic.I32Lt_U or WasmMnemonic.I64Lt_U: EmitCompare(Condition.LtUn); return;
            case WasmMnemonic.I32Gt_S or WasmMnemonic.I64Gt_S or WasmMnemonic.F32Gt or WasmMnemonic.F64Gt: EmitCompare(Condition.Gt); return;
            case WasmMnemonic.I32Gt_U or WasmMnemonic.I64Gt_U: EmitCompare(Condition.GtUn); return;
            case WasmMnemonic.I32Le_S or WasmMnemonic.I64Le_S or WasmMnemonic.F32Le or WasmMnemonic.F64Le: EmitCompare(Condition.Le); return;
            case WasmMnemonic.I32Le_U or WasmMnemonic.I64Le_U: EmitCompare(Condition.LeUn); return;
            case WasmMnemonic.I32Ge_S or WasmMnemonic.I64Ge_S or WasmMnemonic.F32Ge or WasmMnemonic.F64Ge: EmitCompare(Condition.Ge); return;
            case WasmMnemonic.I32Ge_U or WasmMnemonic.I64Ge_U: EmitCompare(Condition.GeUn); return;

            // Integer binary
            case WasmMnemonic.I32Add: EmitI32Add(); return;
            case WasmMnemonic.I32Sub: EmitI32Sub(); return;
            case WasmMnemonic.I32Mul: EmitBinary(CilOpCodes.Mul, TI32); return;
            case WasmMnemonic.I32Div_S: EmitBinary(CilOpCodes.Div, TI32); return;
            case WasmMnemonic.I32Div_U: EmitBinary(CilOpCodes.Div_Un, TI32); return;
            case WasmMnemonic.I32Rem_S: EmitBinary(CilOpCodes.Rem, TI32); return;
            case WasmMnemonic.I32Rem_U: EmitBinary(CilOpCodes.Rem_Un, TI32); return;
            case WasmMnemonic.I32And: EmitBinary(CilOpCodes.And, TI32); return;
            case WasmMnemonic.I32Or: EmitBinary(CilOpCodes.Or, TI32); return;
            case WasmMnemonic.I32Xor: EmitBinary(CilOpCodes.Xor, TI32); return;
            case WasmMnemonic.I32Shl: EmitBinary(CilOpCodes.Shl, TI32); return;
            case WasmMnemonic.I32Shr_S: EmitBinary(CilOpCodes.Shr, TI32); return;
            case WasmMnemonic.I32Shr_U: EmitBinary(CilOpCodes.Shr_Un, TI32); return;

            case WasmMnemonic.I64Add: EmitBinary(CilOpCodes.Add, TI64); return;
            case WasmMnemonic.I64Sub: EmitBinary(CilOpCodes.Sub, TI64); return;
            case WasmMnemonic.I64Mul: EmitBinary(CilOpCodes.Mul, TI64); return;
            case WasmMnemonic.I64Div_S: EmitBinary(CilOpCodes.Div, TI64); return;
            case WasmMnemonic.I64Div_U: EmitBinary(CilOpCodes.Div_Un, TI64); return;
            case WasmMnemonic.I64Rem_S: EmitBinary(CilOpCodes.Rem, TI64); return;
            case WasmMnemonic.I64Rem_U: EmitBinary(CilOpCodes.Rem_Un, TI64); return;
            case WasmMnemonic.I64And: EmitBinary(CilOpCodes.And, TI64); return;
            case WasmMnemonic.I64Or: EmitBinary(CilOpCodes.Or, TI64); return;
            case WasmMnemonic.I64Xor: EmitBinary(CilOpCodes.Xor, TI64); return;
            case WasmMnemonic.I64Shl: EmitBinary(CilOpCodes.Shl, TI64); return;
            case WasmMnemonic.I64Shr_S: EmitBinary(CilOpCodes.Shr, TI64); return;
            case WasmMnemonic.I64Shr_U: EmitBinary(CilOpCodes.Shr_Un, TI64); return;

            // Float unary & binary
            case WasmMnemonic.F32Add: EmitBinary(CilOpCodes.Add, TF32); return;
            case WasmMnemonic.F32Sub: EmitBinary(CilOpCodes.Sub, TF32); return;
            case WasmMnemonic.F32Mul: EmitBinary(CilOpCodes.Mul, TF32); return;
            case WasmMnemonic.F32Div: EmitBinary(CilOpCodes.Div, TF32); return;
            case WasmMnemonic.F32Neg: Push(new UnaryExpr(CilOpCodes.Neg, As(Pop(), ValueKind.R4)), TF32); return;
            case WasmMnemonic.F32Abs: EmitMath1("Abs", TF32); return;
            case WasmMnemonic.F32Sqrt: EmitMath1("Sqrt", TF32); return;
            case WasmMnemonic.F32Ceil: EmitMath1("Ceiling", TF32); return;
            case WasmMnemonic.F32Floor: EmitMath1("Floor", TF32); return;
            case WasmMnemonic.F32Trunc: EmitMath1("Truncate", TF32); return;
            case WasmMnemonic.F32Nearest: EmitMath1("Round", TF32); return;
            case WasmMnemonic.F32Min: EmitMath2("Min", TF32); return;
            case WasmMnemonic.F32Max: EmitMath2("Max", TF32); return;

            case WasmMnemonic.F64Add: EmitBinary(CilOpCodes.Add, TF64); return;
            case WasmMnemonic.F64Sub: EmitBinary(CilOpCodes.Sub, TF64); return;
            case WasmMnemonic.F64Mul: EmitBinary(CilOpCodes.Mul, TF64); return;
            case WasmMnemonic.F64Div: EmitBinary(CilOpCodes.Div, TF64); return;
            case WasmMnemonic.F64Neg: Push(new UnaryExpr(CilOpCodes.Neg, As(Pop(), ValueKind.R8)), TF64); return;
            case WasmMnemonic.F64Abs: EmitMath1("Abs", TF64); return;
            case WasmMnemonic.F64Sqrt: EmitMath1("Sqrt", TF64); return;
            case WasmMnemonic.F64Ceil: EmitMath1("Ceiling", TF64); return;
            case WasmMnemonic.F64Floor: EmitMath1("Floor", TF64); return;
            case WasmMnemonic.F64Trunc: EmitMath1("Truncate", TF64); return;
            case WasmMnemonic.F64Nearest: EmitMath1("Round", TF64); return;
            case WasmMnemonic.F64Min: EmitMath2("Min", TF64); return;
            case WasmMnemonic.F64Max: EmitMath2("Max", TF64); return;

            // Conversions & bit reinterpretations
            case WasmMnemonic.I32Wrap_I64: Push(new ConvExpr(Pop().Expr, _f.Int32), TI32); return;
            case WasmMnemonic.I64Extend_I32_S: Push(new ConvExpr(Pop().Expr, _f.Int64), TI64); return;
            case WasmMnemonic.I64Extend_I32_U: Push(new ConvExpr(Pop().Expr, _f.UInt64), TI64); return;
            case WasmMnemonic.I32Trunc_F32_S or WasmMnemonic.I32Trunc_F32_U or WasmMnemonic.I32Trunc_F64_S or WasmMnemonic.I32Trunc_F64_U:
                Push(new ConvExpr(Pop().Expr, _f.Int32), TI32); return;
            case WasmMnemonic.I64Trunc_F32_S or WasmMnemonic.I64Trunc_F32_U or WasmMnemonic.I64Trunc_F64_S or WasmMnemonic.I64Trunc_F64_U:
                Push(new ConvExpr(Pop().Expr, _f.Int64), TI64); return;
            case WasmMnemonic.F32Convert_I32_S or WasmMnemonic.F32Convert_I32_U or WasmMnemonic.F32Convert_I64_S
                or WasmMnemonic.F32Convert_I64_U or WasmMnemonic.F32Demote_F64:
                Push(new ConvExpr(Pop().Expr, _f.Single), TF32); return;
            case WasmMnemonic.F64Convert_I32_S or WasmMnemonic.F64Convert_I32_U or WasmMnemonic.F64Convert_I64_S
                or WasmMnemonic.F64Convert_I64_U or WasmMnemonic.F64Promote_F32:
                Push(new ConvExpr(Pop().Expr, _f.Double), TF64); return;
            case WasmMnemonic.F32Reinterpret_I32:
            {
                var v = Pop();
                var re = v.Expr is ConstExpr c ? new FloatExpr(BitConverter.Int32BitsToSingle(unchecked((int)c.Value)), single: true) : v.Expr;
                Push(re, TF32);
                return;
            }
            case WasmMnemonic.F64Reinterpret_I64:
            {
                var v = Pop();
                var re = v.Expr is ConstExpr c ? new FloatExpr(BitConverter.Int64BitsToDouble(c.Value), single: false) : v.Expr;
                Push(re, TF64);
                return;
            }
            case WasmMnemonic.I32Reinterpret_F32:
            {
                var v = Pop();
                var re = v.Expr is FloatExpr fl ? new ConstExpr(BitConverter.SingleToInt32Bits((float)fl.Value), ValueKind.I4, true) : v.Expr;
                Push(re, TI32);
                return;
            }
            case WasmMnemonic.I64Reinterpret_F64:
            {
                var v = Pop();
                var re = v.Expr is FloatExpr fl ? new ConstExpr(BitConverter.DoubleToInt64Bits(fl.Value), ValueKind.I8, true) : v.Expr;
                Push(re, TI64);
                return;
            }

            // Calls
            case WasmMnemonic.Call: EmitCall((uint)OpUlong(ins)); return;
            case WasmMnemonic.CallIndirect: EmitCallIndirect((uint)OpUlong(ins)); return;

            default:
                throw new WasmUnsupportedException($"unimplemented wasm instruction {ins.Mnemonic}");
        }
    }

    private void EmitSelect()
    {
        var c = Pop();
        var v2 = Pop();
        var v1 = Pop();
        var t = v1.Expr.Type ?? TypeOf(v1.Wasm);
        var local = NewLocal(t);
        var elseLabel = new CilInstructionLabel();
        var endLabel = new CilInstructionLabel();
        Expr.EmitAs(ctx, c.Expr, _f.Int32);
        ctx.IL.Add(CilOpCodes.Brfalse, elseLabel);
        Expr.EmitAs(ctx, v1.Expr, t);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        ctx.IL.Add(CilOpCodes.Br, endLabel);
        elseLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        Expr.EmitAs(ctx, v2.Expr, t);
        ctx.IL.Add(CilOpCodes.Stloc, local);
        endLabel.Instruction = ctx.IL.Add(CilOpCodes.Nop);
        Push(new LocalExpr(local), v1.Wasm);
    }

    private void EmitLoad(WasmInstruction ins, byte resultWasm, int size, bool signed)
    {
        var offset = (int)Convert.ToUInt32(ins.Operands[1]);
        var addr = Pop();
        if (addr.Expr is ConstExpr c)
        {
            var targetVa = unchecked((uint)(c.Value + addr.Disp + offset));
            if (owner.Symbolizer.TryGetUsage(targetVa, out var u))
            {
                Push(UsageExpr(u), TI32);
                return;
            }
            var reader = owner.Symbolizer.Registration.Reader;
            if (!reader.IsWritableData(targetVa) && reader.IsMapped(targetVa))
            {
                if (resultWasm == TF32 && size == 4) { Push(new FloatExpr(BitConverter.Int32BitsToSingle(reader.ReadI32(targetVa)), true), TF32); return; }
                if (resultWasm == TF64 && size == 8) { Push(new FloatExpr(BitConverter.Int64BitsToDouble((long)reader.ReadU64(targetVa)), false), TF64); return; }
                if (size == 4) { Push(new ConstExpr(reader.ReadI32(targetVa), ValueKind.I4), resultWasm); return; }
                if (size == 8) { Push(new ConstExpr((long)reader.ReadU64(targetVa), ValueKind.I8), resultWasm); return; }
            }
            if (size == 1 && (reader.IsWritableData(targetVa) || targetVa is >= 0x400u and < 0x1000_0000u && !reader.IsMapped(targetVa)))
            {
                Push(new MarkerExpr(MarkerKind.InitFlag, -1, $"wasm_init_{targetVa:X}"), TI32);
                return;
            }
        }
        var loaded = LoadFrom(addr, size, offset);
        if (loaded is not null)
        {
            var actualWasm = resultWasm;
            if (loaded.Type is { } lt && Expr.KindOf(lt) is ValueKind.R4 or ValueKind.R8 or ValueKind.I8)
                actualWasm = WasmOf(Expr.KindOf(lt));
            Push(loaded, actualWasm);
            return;
        }
        _ = signed;
        Push(new NativeExpr($"*({addr.Expr.Describe()}+0x{addr.Disp + offset:X})", KindOf(resultWasm)), resultWasm);
    }

    private void EmitStore(WasmInstruction ins, int size)
    {
        var offset = (int)Convert.ToUInt32(ins.Operands[1]);
        var val = Pop();
        var addr = Pop();
        if (addr.Expr is ConstExpr) return;
        if (!StoreTo(addr.WithDisp(offset), size, val))
        {
            Unresolved++;
            ctx.IL.Add(CilOpCodes.Ldstr, $"*({addr.Expr.Describe()}+0x{addr.Disp + offset:X}) = {val.Expr.Describe()}");
            ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.Stmt);
        }
    }

    private void EmitCompare(Condition cond)
    {
        var r = Pop();
        var l = Pop();
        Push(new CompareExpr(cond, l.Expr, r.Expr), TI32);
    }

    private void EmitI32Add()
    {
        var r = Pop();
        var l = Pop();
        if (l.Expr is FrameAddrExpr fa && r.Expr is ConstExpr rc)
        {
            Push(new FrameAddrExpr(fa.Offset + unchecked((int)rc.Value)), TI32);
            return;
        }
        if (r.Expr is FrameAddrExpr fa2 && l.Expr is ConstExpr lc)
        {
            Push(new FrameAddrExpr(fa2.Offset + unchecked((int)lc.Value)), TI32);
            return;
        }
        if (IsAddressLike(l) && r.Expr is ConstExpr rc2)
        {
            Push(l.WithDisp(checked((int)rc2.Value)));
            return;
        }
        Push(new BinaryExpr(CilOpCodes.Add, l.Expr, r.Expr, ValueKind.I4), TI32);
    }

    private void EmitI32Sub()
    {
        var r = Pop();
        var l = Pop();
        if (l.Expr is FrameAddrExpr fa && r.Expr is ConstExpr rc)
        {
            Push(new FrameAddrExpr(fa.Offset - unchecked((int)rc.Value)), TI32);
            return;
        }
        if (IsAddressLike(l) && r.Expr is ConstExpr rc2)
        {
            Push(l.WithDisp(checked((int)-rc2.Value)));
            return;
        }
        Push(new BinaryExpr(CilOpCodes.Sub, l.Expr, r.Expr, ValueKind.I4), TI32);
    }

    private static bool IsAddressLike(WValue v) =>
        v.Pointee is not null || v.Expr is FrameAddrExpr or AddressExpr or MarkerExpr
        || v.Expr.Kind is ValueKind.Ref or ValueKind.Native or ValueKind.Struct;

    private void EmitBinary(CilOpCode op, byte wasm)
    {
        var r = Pop();
        var l = Pop();
        var k = KindOf(wasm);
        Push(new BinaryExpr(op, As(l, k), As(r, k), k), wasm);
    }

    private void EmitMath1(string name, byte wasm)
    {
        var v = Pop();
        var isSingle = wasm == TF32;
        Expr arg = new ConvExpr(v.Expr, _f.Double);
        Expr call = new MathCallExpr(name, _f.Double, arg);
        Push(isSingle ? new ConvExpr(call, _f.Single) : call, wasm);
    }

    private void EmitMath2(string name, byte wasm)
    {
        var r = Pop();
        var l = Pop();
        var isSingle = wasm == TF32;
        Expr call = new MathCallExpr(name, isSingle ? _f.Single : _f.Double, As(l, KindOf(wasm)), As(r, KindOf(wasm)));
        Push(call, wasm);
    }

    private void EmitCall(uint funcIdx)
    {
        if (!owner.Image.TryGetFunctionType(funcIdx, out var ft))
            throw new WasmUnsupportedException($"unknown call target function {funcIdx}");
        var args = PopArgs(ft.ParameterCount);

        var symbols = owner.SymbolsFor(funcIdx);
        if (symbols.Count > 0)
        {
            EmitManagedCall(symbols, args, ft);
            return;
        }

        if (owner.Image.TryGetImportName(funcIdx, out _, out var importName))
        {
            var shortName = importName.TrimStart('_');
            if (Runtime.CrtMath.IsKnown(shortName) && args.Length == Runtime.CrtMath.ArgCount(shortName))
            {
                var single = shortName.EndsWith('f');
                var mathName = shortName.TrimEnd('f') switch
                {
                    "sqrt" => "Sqrt", "sin" => "Sin", "cos" => "Cos", "tan" => "Tan",
                    "asin" => "Asin", "acos" => "Acos", "atan" => "Atan", "atan2" => "Atan2",
                    "pow" => "Pow", "exp" => "Exp", "log" => "Log", "log10" => "Log10",
                    "floor" => "Floor", "ceil" => "Ceiling", "fabs" => "Abs", "fmod" => "IEEERemainder",
                    var o => char.ToUpperInvariant(o[0]) + o[1..],
                };
                var dArgs = args.Select(a => (Expr)new ConvExpr(a.Expr, _f.Double)).ToArray();
                Expr call = new MathCallExpr(mathName, _f.Double, dArgs);
                if (ft.ResultCount > 0) Push(single ? new ConvExpr(call, _f.Single) : call, ft.Result(0));
                return;
            }
            if (importName.Contains("raise", StringComparison.OrdinalIgnoreCase) || importName.Contains("__cxa_throw", StringComparison.OrdinalIgnoreCase))
            {
                ctx.IL.Add(CilOpCodes.Ldnull);
                ctx.IL.Add(CilOpCodes.Throw);
                _reachable = false;
                return;
            }
        }

        // Heuristic: non-metadata `(TypeInfo) -> i32` helper is il2cpp_codegen_object_new(klass).
        if (args.Length == 1 && ft.ResultCount == 1 && ft.Result(0) == TI32
            && args[0].Expr is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } u } tm
            && _stubs.ToSig(_module, u.Index) is { } objSig)
        {
            Push(new NewObjExpr(objSig, _stubs.GetTypeDef(tm.Index), new RegisterTracker(body, _f)), TI32);
            return;
        }

        // Heuristic: `(TypeInfo, i32) -> i32` is il2cpp_array_new_specific(klass, len).
        if (args.Length == 2 && ft.ResultCount == 1 && ft.Result(0) == TI32
            && args[0].Expr is MarkerExpr { Marker: MarkerKind.TypeInfo, Usage: { Kind: MetadataUsageKind.TypeInfo } au }
            && _stubs.ToSig(_module, au.Index) is SzArrayTypeSignature arrSig)
        {
            Expr.EmitAs(ctx, args[1].Expr, _f.Int32);
            ctx.IL.Add(CilOpCodes.Newarr, arrSig.BaseType.ToTypeDefOrRef());
            var arrLocal = NewLocal(arrSig);
            ctx.IL.Add(CilOpCodes.Stloc, arrLocal);
            Push(new LocalExpr(arrLocal), TI32);
            return;
        }

        EmitNativeFallback($"wasm_fn_{funcIdx}", args, ft);
    }

    private void EmitCallIndirect(uint typeIdx)
    {
        var target = Pop();
        if (!owner.Image.TryGetType(typeIdx, out var ft))
            throw new WasmUnsupportedException($"unknown call_indirect type {typeIdx}");
        var args = PopArgs(ft.ParameterCount);

        if (UnwrapFastcompIndirectTarget(target.Expr) is MarkerExpr { Marker: MarkerKind.VTableEntry or MarkerKind.MethodInfo, Base: { } recv } m)
        {
            var td = _mem.TypeDefOf(recv.Type);
            if (td >= 0 && VTableResolver.Resolve(_md, td, m.Index) is { } t)
            {
                var spec = ExactInstantiation(t.MethodIndex, t.SpecIndex, args);
                if (TryEmitDirectManaged(t.MethodIndex, spec, args, ft, exactWasmArity: true)) return;
            }
        }
        EmitNativeFallback($"wasm_indirect_{typeIdx}", args, ft);
    }

    private static Expr UnwrapFastcompIndirectTarget(Expr e)
    {
        if (e is BinaryExpr add && (add.Op == CilOpCodes.Add || add.Op == CilOpCodes.Or))
        {
            if (add.Right is ConstExpr) e = add.Left;
            else if (add.Left is ConstExpr) e = add.Right;
        }
        if (e is BinaryExpr and && and.Op == CilOpCodes.And)
        {
            if (and.Right is ConstExpr) e = and.Left;
            else if (and.Left is ConstExpr) e = and.Right;
        }
        return e;
    }

    private void EmitManagedCall(List<NativeSymbol> symbols, WValue[] args, WasmFuncType ft)
    {
        int recvTd = -1;
        TypeSignature? recvType = null;
        bool recvNew = false;
        foreach (var a in args)
        {
            if (a.Expr is FrameAddrExpr || a.Expr is MarkerExpr || a.Expr is ConstExpr) continue;
            if (a.Expr is NewObjExpr no) { recvNew = !no.IsMaterialized; recvType = no.Type; recvTd = _mem.TypeDefOf(no.Type); break; }
            if (a.Expr.Type is { } rt && Expr.KindOf(rt) == ValueKind.Ref) { recvType = rt; recvTd = _mem.TypeDefOf(rt); break; }
        }
        if (symbols.Count > 1 && args.Length > 0 && args[0].Expr is FrameAddrExpr)
        {
            var filtered = new List<NativeSymbol>(symbols.Count);
            foreach (var s in symbols)
            {
                var mIdx = s.MethodIndex < 0 && s.MethodSpecIndex >= 0 && (uint)s.MethodSpecIndex < (uint)_reg.MethodSpecs.Count
                    ? _reg.MethodSpecs[s.MethodSpecIndex].MethodDefinitionIndex
                    : s.MethodIndex;
                if ((uint)mIdx >= (uint)_md.Methods.Length) continue;
                var ms = MethodShape.Create(_md, _reg, _stubs, _mem, _module, mIdx, s.MethodSpecIndex);
                if (ms.HasReturnBuffer || (!ms.IsStatic && ms.DeclaringIsValueType))
                    filtered.Add(s);
            }
            if (filtered.Count > 0) symbols = filtered;
        }
        var sym = symbols.Count == 1
            ? symbols[0]
            : IcfResolver.Choose(symbols, recvTd, _md, _reg, owner.Symbolizer.Names, ctx.InfoLog, receiverIsNewObject: recvNew,
                receiverType: recvType, stubs: _stubs, module: _module);
        var methodIndex = sym.MethodIndex < 0 && sym.MethodSpecIndex >= 0 && (uint)sym.MethodSpecIndex < (uint)_reg.MethodSpecs.Count
            ? _reg.MethodSpecs[sym.MethodSpecIndex].MethodDefinitionIndex
            : sym.MethodIndex;
        if ((uint)methodIndex >= (uint)_md.Methods.Length)
        {
            EmitNativeFallback(sym.DisplayName, args, ft);
            return;
        }
        var specIndex = ExactInstantiation(methodIndex, sym.MethodSpecIndex, args);
        if (!TryEmitDirectManaged(methodIndex, specIndex, args, ft))
            EmitNativeFallback(sym.DisplayName, args, ft);
    }

    private int ExactInstantiation(int methodIndex, int specIndex, WValue[] args)
    {
        if (args.Length > 0 && args[^1].Expr is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { Kind: MetadataUsageKind.MethodRef } u }
            && (uint)u.Index < (uint)_reg.MethodSpecs.Count && _reg.MethodSpecs[u.Index].MethodDefinitionIndex == methodIndex)
            return u.Index;
        return specIndex;
    }

    private bool TryEmitDirectManaged(int methodIndex, int specIndex, WValue[] args, WasmFuncType ft, bool exactWasmArity = false)
    {
        var target = _stubs.MethodRefFor(_module, methodIndex, specIndex);
        if (target is null) return false;
        var callee = MethodShape.Create(_md, _reg, _stubs, _mem, _module, methodIndex, specIndex);
        var argPos = callee.HasReturnBuffer ? 1 : 0;
        var requiredArgs = (callee.HasReturnBuffer ? 1 : 0) + (!callee.IsStatic ? 1 : 0) + callee.Parameters.Count;
        if (args.Length < requiredArgs) return false;
        if (exactWasmArity && args.Length != requiredArgs && args.Length != requiredArgs + 1) return false;
        if (!callee.IsStatic && argPos < args.Length)
            CallEmitter.RebindReceiverGeneric(ref target, ref callee, args[argPos].Expr, _md, _reg, _stubs, _mem, _module, methodIndex, specIndex);
        if (target.Signature is not MethodSignature targetSig
            || targetSig.ParameterTypes.Count != callee.Parameters.Count
            || targetSig.HasThis != !callee.IsStatic)
            return false;

        // Pending object_new followed by .ctor -> newobj
        if (!callee.IsStatic && argPos < args.Length && args[argPos].Expr is NewObjExpr { IsMaterialized: false } pending
            && _md.GetString(_md.Methods[methodIndex].NameIndex) == ".ctor")
        {
            argPos++;
            for (var i = 0; i < callee.Parameters.Count && argPos < args.Length; i++, argPos++)
                EmitWasmArg(callee.Parameters[i], args[argPos]);
            var delegateCtor = DelegateCtorFor(pending, target);
            ctx.IL.Add(CilOpCodes.Newobj, delegateCtor ?? target);
            var objLocal = NewLocal(pending.Type);
            ctx.IL.Add(CilOpCodes.Stloc, objLocal);
            ReplaceWasmValue(pending, new LocalExpr(objLocal));
            if (ft.ResultCount > 0) Push(new LocalExpr(objLocal), ft.Result(0));
            return true;
        }

        if (!callee.IsStatic && argPos < args.Length)
            EmitWasmThis(callee, args[argPos++]);

        var calleeMd = _md.Methods[methodIndex];
        for (var i = 0; i < callee.Parameters.Count && argPos < args.Length; i++, argPos++)
        {
            var isPureOut = i < calleeMd.ParameterCount && _reg.TryGetType(_md.Parameters[calleeMd.ParameterStart + i].TypeIndex, out var pat)
                && (pat.Attrs & 3) == 2;
            EmitWasmArg(callee.Parameters[i], args[argPos], preserveExisting: !isPureOut);
        }

        var useCallvirt = !callee.IsStatic && !callee.DeclaringIsValueType;
        ctx.IL.Add(useCallvirt ? CilOpCodes.Callvirt : CilOpCodes.Call, target);

        if (callee.HasReturnBuffer && args.Length > 0)
        {
            StoreReturnBuffer(args[0].Expr, callee.ReturnType);
            if (ft.ResultCount > 0) Push(args[0]);
            return true;
        }
        var retKind = Expr.KindOf(callee.ReturnType);
        if (retKind != ValueKind.Void)
        {
            var res = NewLocal(callee.ReturnType);
            ctx.IL.Add(CilOpCodes.Stloc, res);
            if (ft.ResultCount > 0) Push(new LocalExpr(res), WasmOf(retKind));
        }
        else if (ft.ResultCount > 0)
        {
            Push(new ConstExpr(0, KindOf(ft.Result(0))), ft.Result(0));
        }
        return true;
    }

    private void EmitWasmThis(MethodShape callee, WValue thisArg)
    {
        if (callee.DeclaringIsValueType)
        {
            if (thisArg.Expr is FrameAddrExpr fa)
            {
                var lv = StructLValue(fa.Offset, _stubs.TypeRefFor(_module, _md.Methods[callee.MethodIndex].DeclaringType)!.ToTypeSignature(true), preserveExisting: true);
                new AddressExpr(lv).Emit(ctx);
                return;
            }
            if (thisArg.Expr is ThisStructExpr) { ctx.IL.Add(CilOpCodes.Ldarg_0); return; }
            if (thisArg.Expr is AddressExpr ae) { ae.Emit(ctx); return; }
            if (thisArg.Expr is IndirectStructExpr ise) { ise.Address.Emit(ctx); return; }
            new AddressExpr(thisArg.Expr).Emit(ctx);
            return;
        }
        if (thisArg.Expr is ConstExpr { Value: 0 }) { ctx.IL.Add(CilOpCodes.Ldnull); return; }
        var decl = _stubs.TypeRefFor(_module, _md.Methods[callee.MethodIndex].DeclaringType)?.ToTypeSignature(false);
        if (decl is not null) Expr.EmitAs(ctx, thisArg.Expr, decl);
        else thisArg.Expr.Emit(ctx);
    }

    private void EmitWasmArg(TypeSignature paramType, WValue arg, bool preserveExisting = true)
    {
        if (paramType is ByReferenceTypeSignature byRef)
        {
            if (arg.Expr is FrameAddrExpr fa)
            {
                if (Expr.KindOf(byRef.BaseType) == ValueKind.Struct)
                    new AddressExpr(StructLValue(fa.Offset, byRef.BaseType, preserveExisting: preserveExisting)).Emit(ctx);
                else
                    ctx.IL.Add(CilOpCodes.Ldloca, ScalarSlot(fa.Offset, WasmOf(Expr.KindOf(byRef.BaseType)), _mem.ValueSize(byRef.BaseType)).Local!);
                return;
            }
            if (arg.Expr is AddressExpr or ArgExpr) { arg.Expr.Emit(ctx); return; }
            new AddressExpr(arg.Expr).Emit(ctx);
            return;
        }
        if (Expr.KindOf(paramType) == ValueKind.Struct)
        {
            if (arg.Expr is FrameAddrExpr fa) { StructLValue(fa.Offset, paramType, preserveExisting: true).Emit(ctx); return; }
            if (arg.Expr is AddressExpr ae) { ae.Target.Emit(ctx); return; }
            Expr.EmitAs(ctx, arg.Expr, paramType);
            return;
        }
        if (Expr.KindOf(paramType) == ValueKind.Native && arg.Expr is MarkerExpr { Marker: MarkerKind.MethodInfo, Usage: { } u } && FunctionPointer(u) is { } fn)
        {
            ctx.IL.Add(CilOpCodes.Ldftn, fn);
            return;
        }
        if (paramType.FullName == "System.RuntimeTypeHandle" && arg.Expr is MarkerExpr { Marker: MarkerKind.Il2CppType, Usage: { Kind: MetadataUsageKind.Il2CppType } tu }
            && _stubs.ToSig(_module, tu.Index) is { } tsig)
        {
            ctx.IL.Add(CilOpCodes.Ldtoken, tsig.ToTypeDefOrRef());
            return;
        }
        Expr.EmitAs(ctx, arg.Expr, paramType);
    }

    private void StoreReturnBuffer(Expr retPtr, TypeSignature retType)
    {
        if (retPtr is FrameAddrExpr fa && StructLValue(fa.Offset, retType) is LocalExpr le)
        {
            ctx.IL.Add(CilOpCodes.Stloc, le.Local);
            return;
        }
        if (retPtr is AddressExpr { Target: LocalExpr tl })
        {
            ctx.IL.Add(CilOpCodes.Stloc, tl.Local);
            return;
        }
        var tmp = NewLocal(retType);
        ctx.IL.Add(CilOpCodes.Stloc, tmp);
    }

    private void ReplaceWasmValue(Expr old, Expr replacement)
    {
        foreach (var v in _stack)
            if (ReferenceEquals(v.Expr, old)) v.Expr = replacement;
        foreach (var k in _subst.Where(p => ReferenceEquals(p.Value, old)).Select(p => p.Key).ToList())
            _subst[k] = replacement;
    }

    private IMethodDescriptor? DelegateCtorFor(NewObjExpr obj, IMethodDescriptor resolved)
    {
        if (resolved.Signature is not MethodSignature { ParameterTypes.Count: 2 } sig
            || sig.ParameterTypes[0].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.Object
            || sig.ParameterTypes[1].ElementType != AsmResolver.PE.DotNet.Metadata.Tables.ElementType.I) return null;
        if (resolved.DeclaringType is { } dt && dt.FullName == obj.Type.FullName) return null;
        return new MemberReference(obj.Type.ToTypeDefOrRef(), ".ctor", sig);
    }

    private IMethodDescriptor? FunctionPointer(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.MethodDef => _stubs.MethodRefFor(_module, u.Index),
        MetadataUsageKind.MethodRef when (uint)u.Index < (uint)_reg.MethodSpecs.Count =>
            _stubs.MethodRefFor(_module, _reg.MethodSpecs[u.Index].MethodDefinitionIndex, u.Index),
        _ => null,
    };

    private void EmitNativeFallback(string name, WValue[] args, WasmFuncType ft)
    {
        Unresolved++;
        ctx.IL.Add(CilOpCodes.Ldstr, name);
        for (var i = 0; i < 4; i++)
        {
            if (i < args.Length && !args[i].Expr.IsMarker && args[i].Expr is not FrameAddrExpr)
                Expr.EmitAs(ctx, args[i].Expr, _f.Int64);
            else
                ctx.IL.Add(CilOpCodes.Ldc_I8, 0L);
        }
        ctx.IL.Add(CilOpCodes.Call, ctx.Intrinsics.CallNative);
        if (ft.ResultCount > 0)
        {
            var res = NewLocal(_f.Int64);
            ctx.IL.Add(CilOpCodes.Stloc, res);
            var rw = ft.Result(0);
            var localExpr = new LocalExpr(res);
            Push(rw == TI64 ? localExpr : new ConvExpr(localExpr, TypeOf(rw)), rw);
        }
        else
        {
            ctx.IL.Add(CilOpCodes.Pop);
        }
    }

    private Expr UsageExpr(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.StringLiteral => new StringExpr(_md.GetStringLiteral(u.Index), _f.String),
        MetadataUsageKind.TypeInfo or MetadataUsageKind.Il2CppType => new MarkerExpr(
            u.Kind == MetadataUsageKind.TypeInfo ? MarkerKind.TypeInfo : MarkerKind.Il2CppType,
            _reg.TryGetType(u.Index, out var t) ? owner.Symbolizer.Names.ResolveTypeDefIndex(t) : -1,
            owner.Symbolizer.DescribeUsage(u), usage: u),
        MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva => new MarkerExpr(MarkerKind.FieldInfo, u.Index, owner.Symbolizer.DescribeUsage(u), usage: u),
        _ => new MarkerExpr(MarkerKind.MethodInfo, u.Index, owner.Symbolizer.DescribeUsage(u), usage: u),
    };

    private WValue[] PopArgs(int count)
    {
        var args = new WValue[count];
        for (var i = count - 1; i >= 0; i--) args[i] = Pop();
        return args;
    }

    private static int OpInt(in WasmInstruction ins) => Convert.ToInt32(ins.Operands[0]);
    private static long OpLong(in WasmInstruction ins) => Convert.ToInt64(ins.Operands[0]);
    private static ulong OpUlong(in WasmInstruction ins) => Convert.ToUInt64(ins.Operands[0]);
}

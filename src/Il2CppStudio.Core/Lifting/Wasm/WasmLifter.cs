using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using WasmDisassembler;

namespace Il2CppStudio.Core.Lifting.Wasm;

/// <summary>Thrown when this method uses a wasm construct the lifter does not model; the method fails cleanly.</summary>
public sealed class WasmUnsupportedException(string message) : Exception(message);

/// <summary>
/// Lifts IL2CPP WebAssembly function bodies into CIL. IL2CPP compiles every method with the wasm32 ABI
/// <c>[ret buffer*] [this] params… [const MethodInfo*]</c>: structs are always passed and returned through
/// pointers, primitives travel as i32/i64/f32/f64, and the trailing MethodInfo pointer is unused by the callee.
/// The interpreter evaluates the stack machine symbolically (expressions are emitted where they are consumed), so
/// struct field accesses, array indexing and static fields become real C# instead of raw linear-memory reads.
/// </summary>
public sealed class WasmLifter(NativeSymbolizer sym, StubAssemblyBuilder stubs, WasmBinaryImage wasm, IDiagnosticSink sink) : IMethodLifter
{
    private readonly Dictionary<string, int> _typeIndex = MemoryResolver.BuildTypeIndex(sym.Model);

    /// <summary>Function index → the function-table slots (IL2CPP method pointers) that reference it.</summary>
    private Dictionary<uint, List<ulong>>? _slotsByFunction;

    public bool SupportsParallelLifting => true;

    public LiftResult Lift(int methodIndex)
    {
        ArgumentLocations.Abi = NativeAbi.Wasm32;
        var md = sym.Metadata;
        var image = stubs.ImageOfType(md.Methods[methodIndex].DeclaringType);
        if (image < 0) return new(methodIndex, false, 0, 0, 0, 0, 0, "no image");
        var stub = stubs.GetModule(image);
        var method = stubs.GetMethodDef(methodIndex);
        var slot = sym.Map.GetMethodVa(methodIndex);
        if (method is null || slot == 0 || method.IsAbstract) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");
        if (!wasm.TryGetFunction(slot, out var function, out var funcType)) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");
        var bodyBytes = wasm.FunctionBody(function);
        if (bodyBytes.IsEmpty) return new(methodIndex, false, 0, 0, 0, 0, 0, "no native code");
        if (!WasmBinaryImage.TryReadLocals(bodyBytes, out var locals, out var codeStart))
            return new(methodIndex, false, 0, 0, 0, 0, 0, "malformed local declarations");
        List<WasmInstruction> insns;
        try
        {
            insns = Disassembler.Disassemble(bodyBytes.ToArray()[codeStart..], (uint)codeStart);
        }
        catch (Exception ex)
        {
            return new(methodIndex, false, 0, 0, 0, 0, 0, $"wasm decode failed: {ex.Message}");
        }

        var loc = (ulong ip) => new DiagnosticLocation { Rva = ip, MethodName = sym.MethodName(methodIndex) };
        var notes = new List<string>();
        try
        {
            var body = new CilMethodBody { InitializeLocals = true };
            var ctx = new EmitContext(body.Instructions, stub.Module, stub.Intrinsics) { Log = notes.Add };
            var mem = new MemoryResolver(md, sym.Model, sym.Registration, sym.Names, stubs, stub.Module, _typeIndex, pointerSize: 4);
            var shape = MethodShape.Create(md, sym.Registration, stubs, mem, stub.Module, methodIndex, -1);
            shape.EnsureAbiSupported(mem);
            var emit = new WasmMethodEmit(this, ctx, body, method, shape, mem, notes);
            emit.Run(funcType, locals, insns);
            body.Instructions.OptimizeMacros();
            method.CilMethodBody = body;
            stub.MarkLifted(methodIndex);
            var result = new LiftResult(methodIndex, true, insns.Count, body.Instructions.Count, emit.Unresolved, 0, emit.Elided, null)
            {
                Notes = notes,
            };
            sink.Verbose(DiagnosticCategory.Lifting, DiagnosticCodes.LiftCompleted,
                $"{result.NativeInstructions} wasm -> {result.CilInstructions} CIL, {result.UnresolvedCalls} unresolved calls, " +
                $"{result.ElidedRuntimeChecks} runtime checks elided.", loc(insns.Count > 0 ? insns[0].Ip : 0));
            return result;
        }
        catch (WasmUnsupportedException ex)
        {
            StubAssemblyBuilder.SetPlaceholderBody(method);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"Lifting failed: {ex.Message}", loc(0));
            return new(methodIndex, false, 0, 0, 0, 0, 0, ex.Message);
        }
        catch (Exception ex)
        {
            StubAssemblyBuilder.SetPlaceholderBody(method);
            sink.Warn(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed, $"Lifting failed: {ex.Message}", loc(0), ex);
            return new(methodIndex, false, 0, 0, 0, 0, 0, ex.Message);
        }
    }

    /// <summary>All metadata methods whose native pointer is a table slot naming <paramref name="function"/>.</summary>
    internal List<NativeSymbol> SymbolsFor(uint function)
    {
        var map = Volatile.Read(ref _slotsByFunction);
        if (map is null)
        {
            var built = new Dictionary<uint, List<ulong>>();
            foreach (var symbol in sym.Map.Symbols.Values)
            {
                if (wasm.FunctionBody(symbol.Va, out var fi).IsEmpty) continue;
                if (!built.TryGetValue(fi, out var list)) built[fi] = list = [];
                if (!list.Contains(symbol.Va)) list.Add(symbol.Va);
            }
            map = Interlocked.CompareExchange(ref _slotsByFunction, built, null) ?? built;
        }
        var symbols = new List<NativeSymbol>();
        if (!map.TryGetValue(function, out var slots)) return symbols;
        foreach (var slot in slots)
            foreach (var candidate in sym.Map.CandidatesAt(slot))
                if (!symbols.Contains(candidate)) symbols.Add(candidate);
        return symbols;
    }

    internal NativeSymbolizer Symbolizer => sym;
    internal StubAssemblyBuilder Stubs => stubs;
    internal WasmBinaryImage Image => wasm;
    internal Dictionary<string, int> TypeIndex => _typeIndex;
}

/// <summary>A value on the symbolic wasm operand stack.</summary>
internal sealed class WValue(Expr expr, byte wasmType)
{
    /// <summary>The CIL value (never emitted until a consuming statement needs it).</summary>
    public Expr Expr { get; set; } = expr;

    /// <summary>Wasm value type: 0x7F i32, 0x7E i64, 0x7D f32, 0x7C f64.</summary>
    public byte Wasm { get; set; } = wasmType;

    /// <summary>For i32 values that are raw pointers: the struct/primitive they point to.</summary>
    public TypeSignature? Pointee { get; init; }

    /// <summary>Constant byte displacement already folded into an address-like value (object reference, marker).</summary>
    public int Disp { get; init; }

    public WValue WithDisp(int extra) => new(Expr, Wasm) { Pointee = Pointee, Disp = Disp + extra };
}

/// <summary>A frame slot the program materializes in wasm linear memory (<c>local.get SP; i32.const off; i32.add</c>).</summary>
internal sealed class FrameSlotState(int offset)
{
    public int Offset { get; } = offset;
    public CilLocalVariable? Local { get; set; }
    public TypeSignature? StructType { get; set; }
    public int Size { get; set; }
    public bool ScalarUsed { get; set; }
    public int? ConstBits { get; set; }
}

/// <summary>An open wasm structured-control frame.</summary>
internal sealed class BlockFrame(WasmMnemonic kind, int baseHeight, WasmFuncType sig)
{
    public WasmMnemonic Kind { get; } = kind;
    public int Base { get; } = baseHeight;
    public WasmFuncType Sig { get; } = sig;
    public int StartPc { get; set; }
    public int EndPc { get; set; }
    public CilInstructionLabel? EndLabel { get; set; }
    public CilInstructionLabel? ElseLabel { get; set; }
    public CilInstructionLabel? LoopLabel { get; set; }
    public CilLocalVariable[]? ResultLocals { get; set; }
    public CilLocalVariable[]? ParamLocals { get; set; }
    public bool Branched { get; set; }
    public bool HadElse { get; set; }
    public bool EntryReachable { get; set; }
    public bool ThenFellThrough { get; set; }
    public Dictionary<int, Expr>? SavedSubst { get; set; }
    public Dictionary<int, TypeSignature?>? SavedPointee { get; set; }
    public Dictionary<int, CilLocalVariable>? SavedLocals { get; set; }
    public Dictionary<int, Expr>? SavedParamValue { get; set; }
    public Dictionary<int, Expr>? ThenEndSubst { get; set; }
    public Dictionary<int, TypeSignature?>? ThenEndPointee { get; set; }
    public Dictionary<int, CilLocalVariable>? ThenEndLocals { get; set; }
    public Dictionary<int, Expr>? ThenEndParamValue { get; set; }
    public int ResultCount => Sig.ResultCount;
    public int ParamCount => Sig.ParameterCount;
}

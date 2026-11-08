using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Workspace;

public sealed partial class Il2CppWorkspace
{
    public Il2CppRegistration? Registration { get; private set; }
    public TypeNameService Names { get; private set; } = null!;
    public MethodAddressMap? AddressMap { get; private set; }
    public NativeSymbolizer? Symbolizer { get; private set; }

    /// <summary>x86-64 disassembler feeding the lifter (Windows x64 PE only).</summary>
    public X64Disassembler? Disassembler { get; private set; }

    /// <summary>Instruction text for the native view on any supported CPU (x86, x64, ARMv7, ARM64), or null.</summary>
    public ITextDisassembler? TextDisassembler { get; private set; }

    /// <summary>True when method addresses and type signatures could be recovered from the binary.</summary>
    public bool HasNativeCode => Symbolizer is not null;

    /// <summary>Why method bodies are not lifted for this build (null when lifting is available or not yet attempted).</summary>
    public string? LiftingUnavailableReason { get; private set; }

    /// <summary>x86-64 calling convention of the binary: System V for ELF (Linux), Win64 otherwise.</summary>
    public Lifting.NativeAbi Abi => Binary is { Format: Core.Binary.BinaryFormat.Elf } ? Lifting.NativeAbi.SysV : Lifting.NativeAbi.Win64;

    partial void InitializeNativePipeline()
    {
        // Registration tables (method addresses, field offsets, type signatures) are pointer data: format- and
        // CPU-neutral for 32- and 64-bit images. Lifting needs Windows x86-64 code; text disassembly covers more CPUs.
        if (Binary is null)
        {
            Names = new TypeNameService(Metadata, Model, null);
            LiftingUnavailableReason = "no native binary was found (WebGL/WebAssembly, iOS project or metadata-only input)";
            return;
        }

        Registration = Il2CppRegistration.Load(Binary, Metadata, Sink);
        Names = new TypeNameService(Metadata, Model, Registration);
        if (!Registration.IsComplete)
        {
            LiftingUnavailableReason = "the IL2CPP registration tables were not found";
            return;
        }

        AddressMap = new MethodAddressMap(Metadata, Model, Registration, Names, Binary.GetFunctionTable(), IsEnhanced ? Backup : null, Sink);
        TextDisassembler = TextDisassemblerFactory.For(Binary.Architecture);
        if (TextDisassembler is not null && Binary.Architecture != Core.Binary.CpuArchitecture.Wasm32) TextDisassembler.SymbolResolver = va => AddressMap.SymbolAt(va)?.DisplayName;
        if (Binary.Architecture != Core.Binary.CpuArchitecture.X64)
        {
            LiftingUnavailableReason = $"{Binary.Architecture} machine code is shown as disassembly but not lifted to C# (x86-64 lifter only)";
            Sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported,
                $"{Binary.Format} {Binary.Architecture}: method addresses, field offsets and signatures recovered; " +
                $"disassembly: {TextDisassembler?.Name ?? "unavailable"}; method bodies are not lifted because {LiftingUnavailableReason}.", new() { File = Binary.Path });
            return;
        }
        if (Abi == Lifting.NativeAbi.SysV)
            Sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
                "System V AMD64 calling convention: methods passing or returning structs of 16 bytes or less by value are not lifted yet (reported per method).", new() { File = Binary.Path });
        Symbolizer = new NativeSymbolizer(Metadata, Model, Registration, Names, AddressMap);
        Disassembler = new X64Disassembler(Symbolizer);
    }

    partial void DisposeNativePipeline() => (TextDisassembler as IDisposable)?.Dispose();

    /// <summary>Function index → managed method name (via the function table slots in the method map), plus wasm exports.</summary>
    private Func<ulong, string?> WasmFunctionNames(Core.Binary.WasmBinaryImage wasm)
    {
        var names = new Dictionary<ulong, string>();
        foreach (var s in AddressMap!.Symbols.Values)
            if (!wasm.FunctionBody(s.Va, out var fi).IsEmpty) names.TryAdd(fi, s.DisplayName);
        foreach (var (name, fi) in wasm.GetSymbols()) names.TryAdd(fi, name);
        return fi => names.GetValueOrDefault(fi);
    }

    public ulong GetMethodRva(MethodModel m) => AddressMap?.GetMethodRva(m.Index) ?? 0;

    /// <summary>File offset of a method's code (for hex editors), or -1.</summary>
    public long GetMethodFileOffset(MethodModel m)
    {
        var rva = GetMethodRva(m);
        return rva != 0 && Binary is not null && Binary.TryRvaToFileOffset(rva, out var off) ? off : -1;
    }

    public string? DisassembleMethod(MethodModel m)
    {
        if (AddressMap is null) return null;
        var va = AddressMap.GetMethodVa(m.Index);
        if (va == 0) return null;
        if (Disassembler is not null) return Disassembler.Format(Disassembler.Disassemble(va));
        if (TextDisassembler is null || Registration is null) return null;
        if (Binary is Core.Binary.WasmBinaryImage wasm)
        {
            // WebGL: the method pointer is a function-table slot; the code lives in the function body it names.
            var body = wasm.FunctionBody(va, out var fi);
            if (body.IsEmpty) return $"; table slot {va} does not resolve to a defined function";
            TextDisassembler.SymbolResolver ??= WasmFunctionNames(wasm);
            return $"; {TextDisassembler.Name}, table slot {va}\n" + TextDisassembler.Disassemble(body, fi);
        }
        var (start, end) = AddressMap.GetFunctionBounds(va, 0x4000);
        var code = Registration.Reader.Slice(start, (int)(end - start));
        if (code.IsEmpty) return null;
        var text = TextDisassembler.Disassemble(code, start);   // decode first: ARMv7 picks Thumb-2 or A32 while decoding
        return $"; {TextDisassembler.Name}\n{text}";
    }
}

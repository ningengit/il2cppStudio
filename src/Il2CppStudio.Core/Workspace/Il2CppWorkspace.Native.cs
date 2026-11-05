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
    public X64Disassembler? Disassembler { get; private set; }

    /// <summary>True when method addresses and type signatures could be recovered from the binary.</summary>
    public bool HasNativeCode => Symbolizer is not null;

    partial void InitializeNativePipeline()
    {
        if (Binary is null || !Binary.Is64Bit || Binary.Architecture != Core.Binary.CpuArchitecture.X64)
        {
            Names = new TypeNameService(Metadata, Model, null);
            if (Binary is not null)
                Sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported,
                    $"{Binary.Architecture} binaries are not lifted (x86-64 only); metadata views remain available.", new() { File = Binary.Path });
            return;
        }

        Registration = Il2CppRegistration.Load(Binary, Metadata, Sink);
        Names = new TypeNameService(Metadata, Model, Registration);
        if (!Registration.IsComplete) return;

        AddressMap = new MethodAddressMap(Metadata, Model, Registration, Names, Binary.GetRuntimeFunctions(), IsEnhanced ? Backup : null, Sink);
        Symbolizer = new NativeSymbolizer(Metadata, Model, Registration, Names, AddressMap);
        Disassembler = new X64Disassembler(Symbolizer);
    }

    partial void DisposeNativePipeline() { }

    public ulong GetMethodRva(MethodModel m) => AddressMap?.GetMethodRva(m.Index) ?? 0;

    /// <summary>File offset of a method's code (for hex editors), or -1.</summary>
    public long GetMethodFileOffset(MethodModel m)
    {
        var rva = GetMethodRva(m);
        return rva != 0 && Binary is not null && Binary.TryRvaToFileOffset(rva, out var off) ? off : -1;
    }

    public string? DisassembleMethod(MethodModel m)
    {
        if (Disassembler is null || AddressMap is null) return null;
        var va = AddressMap.GetMethodVa(m.Index);
        return va == 0 ? null : Disassembler.Format(Disassembler.Disassemble(va));
    }
}

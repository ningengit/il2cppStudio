using Il2CppStudio.Core.CodeGen;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Lifting;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

/// <summary>The three synchronized representations of a class or method.</summary>
public enum CodeView { Cpp, Il, CSharp }

public sealed partial class Il2CppWorkspace
{
    private StubAssemblyBuilder? _stubs;
    private DecompilerService? _decompiler;
    private CppHeaderGenerator? _cpp;
    private BclReferenceProvider? _bcl;
    private BackupSourceIndex? _backupSources;

    public StubAssemblyBuilder Stubs => _stubs ??= new StubAssemblyBuilder(Metadata, Model, Registration, Names, Sink);

    public BclReferenceProvider Bcl => _bcl ??= new BclReferenceProvider(
        Options.BclReferenceDirectory, Environment.UnityVersion, IsEnhanced ? Backup?.ManagedFolder : null, Sink);

    public DecompilerService Decompiler => _decompiler ??= CreateDecompiler();

    private DecompilerService CreateDecompiler()
    {
        var d = new DecompilerService(Stubs, Model, Bcl, Options.SubstituteBcl, Sink) { BodyProvider = BodyProvider };
        d.Annotations = new AnnotationContext
        {
            Model = Model,
            MethodLocation = m => (GetMethodRva(m), GetMethodFileOffset(m)),
            FieldOffset = (t, ordinal) => Registration is null ? -1
                : Registration.GetFieldOffset(t.Index, ordinal) is var off and >= 0 && t.Raw.IsValueType && !IsStaticField(t, ordinal) ? off - 16 : Registration.GetFieldOffset(t.Index, ordinal),
            FallbackReason = m => d.BodyProvider switch
            {
                Lifting.LiftingBodyProvider p => p.FallbackReason(m.Index),
                null => "native lifting is unavailable for this binary (not x86-64, or the IL2CPP registration tables were not found)",
                _ => null,
            },
        };
        return d;
    }

    private bool IsStaticField(TypeModel t, int ordinal) =>
        Registration is not null && Registration.TryGetType(t.Fields[ordinal].TypeIndex, out var ft) && (ft.Attrs & 0x10) != 0;

    public CppHeaderGenerator Cpp => _cpp ??= new CppHeaderGenerator(
        Metadata, Model, Registration, Names, AddressMap,
        Disassembler is null ? null : idx => DisassembleMethod(Model.Types[Metadata.Methods[idx].DeclaringType].Methods.First(m => m.Index == idx)),
        BackupSources, IsEnhanced ? Backup : null);

    public BackupSourceIndex? BackupSources =>
        _backupSources ??= IsEnhanced && Backup?.Il2CppOutputFolder is { } dir ? new BackupSourceIndex(dir, Sink) : null;

    /// <summary>Lifter hook; set by the lifting pipeline once available (placeholder bodies otherwise).</summary>
    public IMethodBodyProvider? BodyProvider
    {
        get => _bodyProvider;
        set
        {
            _bodyProvider = value;
            if (_decompiler is not null) _decompiler.BodyProvider = value;
        }
    }

    private IMethodBodyProvider? _bodyProvider;

    /// <summary>"Substitute BCL (Clean/Fast)" vs "Pure Native View". Changing it affects subsequent renders.</summary>
    public bool SubstituteBcl
    {
        get => _decompiler?.SubstituteBcl ?? Options.SubstituteBcl;
        set => Decompiler.SubstituteBcl = value;
    }

    /// <summary>Renders a type (method == null) or a single method in the requested representation.</summary>
    public string Render(TypeModel type, MethodModel? method, CodeView view) => view switch
    {
        CodeView.Cpp => method is null ? Cpp.GenerateTypeHeader(type) : Cpp.GenerateMethod(type, method),
        CodeView.Il => method is null ? Decompiler.DisassembleType(type) : Decompiler.DisassembleMethod(type, method),
        CodeView.CSharp => method is null ? Decompiler.DecompileType(type) : Decompiler.DecompileMethod(type, method),
        _ => "",
    };
}

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
        var d = new DecompilerService(Stubs, Model, Bcl, Options.SubstituteBcl, Sink)
        {
            BodyProvider = BodyProvider,
            RelaxInlinedAccess = Options.RelaxInlinedAccess,
        };
        d.Annotations = new AnnotationContext
        {
            Model = Model,
            MethodLocation = m => (GetMethodRva(m), GetMethodFileOffset(m)),
            FieldOffset = (t, ordinal) => Registration is null ? -1
                : Registration.GetFieldOffset(t.Index, ordinal) is var off and >= 0 && t.Raw.IsValueType && !IsStaticField(t, ordinal) ? off - 16 : Registration.GetFieldOffset(t.Index, ordinal),
            FallbackReason = m => d.BodyProvider switch
            {
                Lifting.LiftingBodyProvider p => p.FallbackReason(m.Index),
                null => $"native lifting is unavailable for this build: {LiftingUnavailableReason ?? "the IL2CPP registration tables were not found"}",
                _ => null,
            },
            Fingerprint = Binary is not null ? global::Il2CppStudio.Core.Registration.RegistrationCache.ComputeKey(Binary, Metadata) : null,
        };
        return d;
    }

    private bool IsStaticField(TypeModel t, int ordinal) =>
        Registration is not null && Registration.TryGetType(t.Fields[ordinal].TypeIndex, out var ft) && (ft.Attrs & 0x10) != 0;

    public CppHeaderGenerator Cpp => _cpp ??= new CppHeaderGenerator(
        Registration, Names, AddressMap,
        Disassembler is null && TextDisassembler is null ? null : idx => DisassembleMethod(Model.Types[Metadata.Methods[idx].DeclaringType].Methods.First(m => m.Index == idx)),
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

    /// <summary>When true, widens private/protected members accessed cross-type by inlined native code to internal.</summary>
    public bool RelaxInlinedAccess
    {
        get => _decompiler?.RelaxInlinedAccess ?? Options.RelaxInlinedAccess;
        set => Decompiler.RelaxInlinedAccess = value;
    }

    /// <summary>Renders a type (method == null) or a single method in the requested representation.</summary>
    private const int RenderCacheCapacity = 128;

    private readonly record struct RenderCacheKey(int TypeIndex, int MethodIndex, CodeView View, bool SubstituteBcl, bool RelaxInlinedAccess);

    private readonly Lock _renderGate = new();
    private readonly Lock _renderCacheGate = new();
    private readonly Dictionary<RenderCacheKey, LinkedListNode<(RenderCacheKey Key, string Text)>> _renderCache = [];
    private readonly LinkedList<(RenderCacheKey Key, string Text)> _renderCacheLru = [];

    /// <summary>
    /// Renders one view. Checks the bounded 128-entry LRU cache first so revisiting recently viewed types costs ~0 allocations;
    /// misses serialize under <see cref="_renderGate"/> so lifting does not interleave across concurrent UI requests.
    /// </summary>
    public string Render(TypeModel type, MethodModel? method, CodeView view) =>
        Render(type, method, view, CancellationToken.None);

    public string Render(TypeModel type, MethodModel? method, CodeView view, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var key = new RenderCacheKey(type.Index, method?.Index ?? -1, view, SubstituteBcl, RelaxInlinedAccess);
        lock (_renderCacheGate)
        {
            if (_renderCache.TryGetValue(key, out var node))
            {
                _renderCacheLru.Remove(node);
                _renderCacheLru.AddFirst(node);
                return node.Value.Text;
            }
        }

        string rendered;
        lock (_renderGate)
        {
            ct.ThrowIfCancellationRequested();
            lock (_renderCacheGate)
            {
                if (_renderCache.TryGetValue(key, out var node))
                {
                    _renderCacheLru.Remove(node);
                    _renderCacheLru.AddFirst(node);
                    return node.Value.Text;
                }
            }
            rendered = RenderCore(type, method, view, ct);
        }

        lock (_renderCacheGate)
        {
            if (!_renderCache.TryGetValue(key, out var node))
            {
                var entry = new LinkedListNode<(RenderCacheKey Key, string Text)>((key, rendered));
                _renderCacheLru.AddFirst(entry);
                _renderCache[key] = entry;
                while (_renderCache.Count > RenderCacheCapacity && _renderCacheLru.Last is { } oldest)
                {
                    _renderCacheLru.RemoveLast();
                    _renderCache.Remove(oldest.Value.Key);
                }
            }
        }
        return rendered;
    }

    public void ClearCaches()
    {
        lock (_renderCacheGate)
        {
            _renderCache.Clear();
            _renderCacheLru.Clear();
        }
        lock (_renderGate)
        {
            _decompiler?.ClearCache();
        }
    }

    partial void DisposeViews()
    {
        lock (_renderCacheGate)
        {
            _renderCache.Clear();
            _renderCacheLru.Clear();
        }
        _decompiler?.Dispose();
    }

    private string RenderCore(TypeModel type, MethodModel? method, CodeView view, CancellationToken ct) => view switch
    {
        CodeView.Cpp => method is null ? CppTypeView.Render(Cpp, type) : Cpp.GenerateMethod(type, method),
        CodeView.Il => method is null ? Decompiler.DisassembleType(type, ct) : Decompiler.DisassembleMethod(type, method, ct),
        CodeView.CSharp => method is null ? Decompiler.DecompileType(type, ct) : Decompiler.DecompileMethod(type, method, ct),
        _ => "",
    };
}

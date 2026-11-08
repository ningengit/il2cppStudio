using System.Collections.Concurrent;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Disassembler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Lifting;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>Supplies lifted method bodies before a stub module is serialized (implemented by the lifter pipeline).</summary>
public interface IMethodBodyProvider
{
    /// <summary>Ensures the given methods have lifted CIL bodies in their stub module. Returns how many were lifted now.</summary>
    int EnsureLifted(IEnumerable<int> methodIndices);
}

/// <summary>
/// Produces the "IL Code" and "C#" views from the AsmResolver stub modules using ICSharpCode.Decompiler.
/// Stubs are serialized to in-memory PE images (cached per image and invalidated when new bodies are lifted).
/// The assembly resolver implements BCL substitution: with <see cref="SubstituteBcl"/> on, references to
/// System.* / Microsoft.* / mscorlib bind to pristine reference assemblies; otherwise to the generated stubs.
/// </summary>
public sealed class DecompilerService
{
    private readonly StubAssemblyBuilder _stubs;
    private readonly MetadataModel _model;
    private readonly BclReferenceProvider _bcl;
    private readonly IDiagnosticSink _sink;
    private readonly ConcurrentDictionary<int, (int LiftedCount, PEFile File)> _images = new();
    private readonly ConcurrentDictionary<string, PEFile?> _bclFiles = new(StringComparer.OrdinalIgnoreCase);

    public bool SubstituteBcl { get; set; }

    /// <summary>Token/RVA annotations and fallback explanations (null = plain output).</summary>
    public AnnotationContext? Annotations { get; set; }

    /// <summary>Decompiler warnings and lift notes removed from the C# output.</summary>
    public DecompilationErrorLog ErrorLog { get; set; } = new();

    private EntityMapper? _mapper;

    private readonly System.Runtime.CompilerServices.ConditionalWeakTable<byte[], HashSet<uint>> _crossTypeTokens = new();

    /// <summary>Cross-type private accesses of the current stub image (recomputed when lifting rebuilds the image).</summary>
    private HashSet<uint> CrossTypeTokens(byte[] image) => _crossTypeTokens.GetValue(image, CrossTypeAccessScanner.Scan);

    /// <summary>Annotates (stub output only), prints and scrubs a syntax tree.</summary>
    private string Finish(ICSharpCode.Decompiler.CSharp.Syntax.SyntaxTree tree, int image, bool annotate,
        Func<ICSharpCode.Decompiler.CSharp.Syntax.SyntaxTree, ICSharpCode.Decompiler.CSharp.Syntax.SyntaxTree?>? select = null)
    {
        if (annotate && Annotations is { } a) new AstAnnotator(a, _mapper ??= new EntityMapper(a.Model)).Annotate(tree);
        // Diagnostic switch: IL2CPPSTUDIO_NO_AST_TRANSFORMS=1 prints the raw ILSpy tree (no coroutine / else-if rewrites).
        if (annotate && Environment.GetEnvironmentVariable("IL2CPPSTUDIO_NO_AST_TRANSFORMS") != "1")
        {
            Transforms.ElseIfFlattener.Apply(tree);
            Transforms.SelfAliasAndTailSink.Apply(tree);
            Coroutines.CoroutineTransform.Apply(tree, (type, reason) =>
                ErrorLog.Add(_stubs.ImageName(image), "CoroutineNotReconstructed", type, null, reason, "State machine class kept as decompiled."));
            Transforms.EmptyIfRemover.Apply(tree);
            Transforms.IdenticalBranchFolder.Apply(tree);
            Transforms.DistanceRecognizer.Apply(tree);
            Transforms.BankersRoundRecognizer.Apply(tree);
            Transforms.StringEqualitySugar.Apply(tree);
            Transforms.BoolNormalizer.Apply(tree);
            Transforms.VectorComponentCollapse.Apply(tree);
            Transforms.UnityInlineRecognizer.Apply(tree);
            Transforms.VectorOperatorRecognizer.Apply(tree);
            Transforms.CompoundFieldFold.Apply(tree);
            Transforms.NamedConstants.Apply(tree);
            foreach (var body in tree.Descendants.OfType<ICSharpCode.Decompiler.CSharp.Syntax.MethodDeclaration>().Select(m => m.Body).Where(b => !b.IsNull).ToList())
                Coroutines.TempInliner.Run(body);
            Transforms.WhileConditionRestorer.Apply(tree);
            Transforms.LoopShapeRestorer.Apply(tree);
        Transforms.ForeachRestorer.Apply(tree);
        Transforms.LoopShapeRestorer.NameLoopCounters(tree);
            Transforms.ElseIfFlattener.Apply(tree);
            Transforms.SelfAliasAndTailSink.Apply(tree);
        }
        if (annotate)
        {
            Transforms.ExplicitImplementationFixer.Apply(tree);
            Transforms.InlinedGetterRestorer.Apply(tree);
            Transforms.CompileHygiene.Apply(tree);
            if (CachedStubBytes(image) is { } stubBytes) Transforms.AccessRelaxer.Apply(tree, CrossTypeTokens(stubBytes));
            Transforms.CompileFixups.Apply(tree);
            Transforms.DefiniteAssignment.Apply(tree);
            Transforms.IdentifierEscaper.Apply(tree);   // last: earlier passes match compiler-generated names
        }
        if (select is not null) tree = select(tree) ?? tree;
        var writer = new StringWriter();
        tree.AcceptVisitor(new ICSharpCode.Decompiler.CSharp.OutputVisitor.InsertParenthesesVisitor { InsertParenthesesForReadability = true });
        tree.AcceptVisitor(new ICSharpCode.Decompiler.CSharp.OutputVisitor.CSharpOutputVisitor(
            new ICSharpCode.Decompiler.CSharp.OutputVisitor.TextWriterTokenWriter(writer), CreateSettings().CSharpFormattingOptions));
        return OutputScrubber.Scrub(writer.ToString(), _stubs.ImageName(image), ErrorLog);
    }
    public IMethodBodyProvider? BodyProvider { get; set; }

    public DecompilerService(StubAssemblyBuilder stubs, MetadataModel model, BclReferenceProvider bcl, bool substituteBcl, IDiagnosticSink sink)
    {
        _stubs = stubs; _model = model; _bcl = bcl; SubstituteBcl = substituteBcl; _sink = sink;
    }

    public static DecompilerSettings CreateSettings() => new(LanguageVersion.Latest)
    {
        ThrowOnAssemblyResolveErrors = false,
        ShowXmlDocumentation = false,
        // Lifted code materializes every call result / condition into a local; inline single-use ones and drop
        // dead spill copies so conditions read naturally and else-if chains can form. (C# only — the IL is unchanged.)
        AggressiveInlining = true,
        RemoveDeadCode = true,
        RemoveDeadStores = true,
        UseDebugSymbols = false,
        AlwaysUseBraces = true,
    };

    // ------------------------------------------------------------------ public API

    public string DecompileType(TypeModel type)
    {
        var image = ImageOf(type);
        if (image < 0) return $"// Type '{type.FullName}' has no image.";
        if (IsSubstitutedBcl(image) && TryPristine(image) is { } pristine)
            return Guard(() => Header(image, true) + Finish(NewDecompiler(pristine).DecompileType(new FullTypeName(TopLevelOf(type).FullName)), image, annotate: false), type.FullName);

        LiftType(type);
        var file = GetStubFile(image);
        return Guard(() => Header(image, false) + Finish(NewDecompiler(file).DecompileType(new FullTypeName(TopLevelOf(type).FullName)), image, annotate: true), type.FullName);
    }

    public string DecompileMethod(TypeModel type, MethodModel method)
    {
        var image = ImageOf(type);
        if (image < 0) return "// no image";
        var substituted = IsSubstitutedBcl(image) ? TryPristine(image) : null;
        if (substituted is null && TopLevelOf(type).NestedTypes.Any(n => n.Name.Contains(">d__", StringComparison.Ordinal)))
            return DecompileMethodInTypeContext(type, method, image);
        if (substituted is null) LiftMethods([method.Index]);
        var file = substituted ?? GetStubFile(image);
        return Guard(() =>
        {
            var handle = FindMethod(file, type, method);
            if (handle.IsNil) return $"// Method {type.FullName}::{method.Name} not found in {(substituted is null ? "stub" : "BCL reference")} module.";
            return Header(image, substituted is not null) + Finish(NewDecompiler(file).Decompile(handle), image, annotate: substituted is null);
        }, $"{type.FullName}::{method.Name}");
    }

    /// <summary>
    /// Single-method view for types with compiler-generated iterators: decompiles the whole type so type-level
    /// rewrites (coroutine reconstruction, factory call-site collapse) apply, then prints only the requested method.
    /// </summary>
    private string DecompileMethodInTypeContext(TypeModel type, MethodModel method, int image)
    {
        LiftType(type);
        var file = GetStubFile(image);
        return Guard(() => Header(image, false) + Finish(NewDecompiler(file).DecompileType(new FullTypeName(TopLevelOf(type).FullName)), image,
            annotate: true, select: t => MemberExtractor.Extract(t, type.Name, method.Name, method.ParameterNames)), $"{type.FullName}::{method.Name}");
    }

    public string DisassembleType(TypeModel type)
    {
        var image = ImageOf(type);
        if (image < 0) return "// no image";
        LiftType(type);
        var file = GetStubFile(image);
        return Guard(() =>
        {
            var handle = FindType(file, type);
            if (handle.IsNil) return $"// Type {type.FullName} not found in stub module.";
            var (dis, writer) = NewDisassembler(file);
            dis.DisassembleType(file, handle);
            return writer.ToString();
        }, type.FullName);
    }

    public string DisassembleMethod(TypeModel type, MethodModel method)
    {
        var image = ImageOf(type);
        if (image < 0) return "// no image";
        LiftMethods([method.Index]);
        var file = GetStubFile(image);
        return Guard(() =>
        {
            var handle = FindMethod(file, type, method);
            if (handle.IsNil) return $"// Method {type.FullName}::{method.Name} not found in stub module.";
            var (dis, writer) = NewDisassembler(file);
            dis.DisassembleMethod(file, handle);
            return writer.ToString();
        }, $"{type.FullName}::{method.Name}");
    }

    /// <summary>Serialized stub image of an assembly (with currently lifted bodies), e.g. for export or accuracy tests.</summary>
    public byte[]? GetStubImage(int imageIndex) => CachedStubBytes(imageIndex);

    private readonly ConcurrentDictionary<int, (int LiftedCount, byte[]? Bytes)> _stubBytes = new();

    /// <summary>
    /// Stub bytes per image, re-serialized only when more methods were lifted. Serializing costs ~0.3 s for
    /// Assembly-CSharp and used to run once per decompiled type; the stable array also lets the cross-type access scan
    /// (keyed by array identity) run once instead of per type.
    /// </summary>
    private byte[]? CachedStubBytes(int imageIndex)
    {
        var stub = _stubs.GetModule(imageIndex);
        if (_stubBytes.TryGetValue(imageIndex, out var c) && c.LiftedCount == stub.LiftedMethods.Count) return c.Bytes;
        var bytes = _stubs.Write(stub);
        _stubBytes[imageIndex] = (stub.LiftedMethods.Count, bytes);
        return bytes;
    }

    public PEFile GetStubFile(int imageIndex)
    {
        var stub = _stubs.GetModule(imageIndex);
        if (_images.TryGetValue(imageIndex, out var cached) && cached.LiftedCount == stub.LiftedMethods.Count) return cached.File;
        var bytes = CachedStubBytes(imageIndex) ?? throw new InvalidOperationException($"Stub for {stub.AssemblyName} could not be written.");
        var file = new PEFile(stub.Module.Name ?? stub.AssemblyName + ".dll", new MemoryStream(bytes), PEStreamOptions.PrefetchEntireImage);
        _images[imageIndex] = (stub.LiftedMethods.Count, file);
        return file;
    }

    /// <summary>
    /// Batch decompilation of top-level types of one image. Bodies must already be lifted. Types are independent, so
    /// they are decompiled in parallel: one shared (thread-safe) type system, one <see cref="CSharpDecompiler"/> per
    /// worker, AST transforms are per-tree. Results are yielded in input order; failures are reported per type.
    /// </summary>
    public IEnumerable<(TypeModel Type, string CSharp, string Il)> DecompileImage(int imageIndex, IEnumerable<TypeModel> topLevelTypes, bool includeIl)
    {
        var pristine = IsSubstitutedBcl(imageIndex) ? TryPristine(imageIndex) : null;
        MetadataFile file = pristine ?? GetStubFile(imageIndex);
        var types = topLevelTypes.ToList();
        if (pristine is null && CachedStubBytes(imageIndex) is { } bytes) _ = CrossTypeTokens(bytes);   // warm shared caches before fan-out
        if (Annotations is { } a) _mapper ??= new EntityMapper(a.Model);
        var typeSystem = new DecompilerTypeSystem(file, new Resolver(this), CreateSettings());
        var results = new (string Cs, string Il)[types.Count];
        Parallel.For(0, types.Count, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            () => new CSharpDecompiler(typeSystem, CreateSettings()),
            (i, _, decompiler) =>
            {
                var t = types[i];
                var cs = Guard(() => Finish(decompiler.DecompileType(new FullTypeName(t.FullName)), imageIndex, annotate: pristine is null), t.FullName);
                var il = includeIl && pristine is null ? Guard(() =>
                {
                    var h = FindType(file, t);
                    if (h.IsNil) return "";
                    var (dis, w) = NewDisassembler(file);
                    dis.DisassembleType(file, h);
                    return w.ToString();
                }, t.FullName) : "";
                results[i] = (cs, il);
                return decompiler;
            },
            _ => { });
        for (var i = 0; i < types.Count; i++) yield return (types[i], results[i].Cs, results[i].Il);
    }

    // ------------------------------------------------------------------ internals

    private int ImageOf(TypeModel type) => _stubs.ImageOfType(type.Index);

    private static TypeModel TopLevelOf(TypeModel t)
    {
        while (t.DeclaringType is not null) t = t.DeclaringType;
        return t;
    }

    private bool IsSubstitutedBcl(int image) => SubstituteBcl && BclReferenceProvider.IsBclAssembly(_stubs.ImageName(image));

    private PEFile? TryPristine(int image) => LoadBcl(_stubs.ImageName(image));

    private PEFile? LoadBcl(string name) => _bclFiles.GetOrAdd(name, n =>
    {
        var path = _bcl.Find(n);
        if (path is null) return null;
        try { return new PEFile(path, new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), PEStreamOptions.PrefetchEntireImage); }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Decompilation, DiagnosticCodes.BclReferenceMissing, $"Cannot load BCL reference '{path}': {ex.Message}", new() { File = path }, ex);
            return null;
        }
    });

    private string Header(int image, bool pristine) => pristine
        ? $"// [BCL substituted] {_stubs.ImageName(image)} decompiled from pristine reference assembly: {_bcl.Find(_stubs.ImageName(image))}\n"
        : $"// {_stubs.ImageName(image)} — reconstructed from IL2CPP metadata + lifted native code\n";

    private void LiftType(TypeModel type)
    {
        var all = new List<int>();
        void Collect(TypeModel t) { all.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) Collect(n); }
        Collect(TopLevelOf(type));
        LiftMethods(all);
    }

    private void LiftMethods(IEnumerable<int> methods) => BodyProvider?.EnsureLifted(methods);

    private CSharpDecompiler NewDecompiler(MetadataFile file) => new(file, new Resolver(this), CreateSettings());

    private (ReflectionDisassembler, StringWriter) NewDisassembler(MetadataFile file)
    {
        var writer = new StringWriter();
        var output = new PlainTextOutput(writer);
        var dis = new ReflectionDisassembler(output, CancellationToken.None)
        {
            AssemblyResolver = new Resolver(this),
            DetectControlStructure = true,
            ShowSequencePoints = false,
        };
        return (dis, writer);
    }

    private string Guard(Func<string> action, string what)
    {
        try { return action(); }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Decompilation, DiagnosticCodes.DecompilationFailed, $"Decompiling {what} failed: {ex.Message}", new() { TypeName = what }, ex);
            ErrorLog.Add("(decompiler)", "DecompilationFailure", what, null, ex.ToString(), "Member output replaced by a failure comment.");
            return $"// Decompilation of {what} failed: {ex.GetType().Name}: {ex.Message}";
        }
    }

    private static TypeDefinitionHandle FindType(MetadataFile file, TypeModel type)
    {
        var reader = file.Metadata;
        var wanted = type.FullName;
        foreach (var h in reader.TypeDefinitions)
            if (h.GetFullTypeName(reader).ReflectionName == wanted) return h;
        return default;
    }

    /// <summary>Methods are matched by name and position among same-named overloads (stub order mirrors metadata order).</summary>
    private static MethodDefinitionHandle FindMethod(MetadataFile file, TypeModel type, MethodModel method)
    {
        var th = FindType(file, type);
        if (th.IsNil) return default;
        var reader = file.Metadata;
        var overloadOrdinal = type.Methods.TakeWhile(m => m != method).Count(m => m.Name == method.Name);
        var seen = 0;
        foreach (var mh in reader.GetTypeDefinition(th).GetMethods())
        {
            if (reader.GetString(reader.GetMethodDefinition(mh).Name) != method.Name) continue;
            if (seen++ == overloadOrdinal) return mh;
        }
        return default;
    }

    /// <summary>Resolves references to other stubs, or to pristine BCL assemblies when substitution is enabled.</summary>
    private sealed class Resolver(DecompilerService owner) : IAssemblyResolver
    {
        public MetadataFile? Resolve(IAssemblyReference reference)
        {
            var name = reference.Name;
            if (owner.SubstituteBcl && BclReferenceProvider.IsBclAssembly(name) && owner.LoadBcl(name) is { } pristine) return pristine;
            var image = owner._stubs.FindImage(name);
            if (image >= 0) return owner.GetStubFile(image);
            // Facade/forwarded BCL assemblies not present in the build (e.g. netstandard) fall back to the reference set.
            return BclReferenceProvider.IsBclAssembly(name) ? owner.LoadBcl(name) : null;
        }

        public MetadataFile? ResolveModule(MetadataFile mainModule, string moduleName) => null;

        public Task<MetadataFile?> ResolveAsync(IAssemblyReference reference) => Task.FromResult(Resolve(reference));

        public Task<MetadataFile?> ResolveModuleAsync(MetadataFile mainModule, string moduleName) => Task.FromResult<MetadataFile?>(null);
    }
}

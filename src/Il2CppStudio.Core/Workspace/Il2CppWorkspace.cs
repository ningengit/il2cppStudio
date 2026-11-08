using Il2CppStudio.Core.Backup;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Fingerprinting;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

public sealed record WorkspaceOptions
{
    /// <summary>Use pristine BCL reference assemblies for System.* / Microsoft.* / mscorlib instead of lifting them.</summary>
    public bool SubstituteBcl { get; init; } = true;

    /// <summary>Optional folder with BCL reference assemblies (overrides auto-detection).</summary>
    public string? BclReferenceDirectory { get; init; }

    /// <summary>Parse *_CodeGen.c tables when a build backup folder exists.</summary>
    public bool UseBuildBackup { get; init; } = true;
}

/// <summary>
/// The root object of an analysis session: locates the game files, loads metadata and the native binary,
/// detects the optional build backup ("Fallback-to-Bare": everything works without it), and exposes the
/// higher-level services (registration, generators, lifter, decompiler) built on top. The GUI and the CLI
/// both drive the tool exclusively through this class, which is what guarantees identical results.
/// </summary>
public sealed partial class Il2CppWorkspace : IDisposable
{
    public GameFiles Files { get; }
    public WorkspaceOptions Options { get; }
    public IDiagnosticSink Sink { get; }
    public GlobalMetadata Metadata { get; }
    public MetadataModel Model { get; }
    public IBinaryImage? Binary { get; }
    public BuildBackupInfo? Backup { get; }
    public GameEnvironment Environment { get; }

    /// <summary>True when a build backup was found and (if a PDB exists) its GUID matches the binary.</summary>
    public bool IsEnhanced => Backup is not null && Backup.PdbMatchesBinary != false;

    public string ModeLabel => IsEnhanced ? Backup!.ModeLabel : BuildBackupDetector.BareModeLabel;

    public string? ResourcesFolder => Metadata.Path is { } p
        ? Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(p)!)!, "Resources")
        : null;

    private Il2CppWorkspace(GameFiles files, WorkspaceOptions options, IDiagnosticSink sink, GlobalMetadata md, MetadataModel model,
        IBinaryImage? binary, BuildBackupInfo? backup, GameEnvironment env)
    {
        Files = files; Options = options; Sink = sink; Metadata = md; Model = model; Binary = binary; Backup = backup; Environment = env;
    }

    /// <summary>Loads a game folder. Throws <see cref="MetadataLoadException"/> when metadata is missing or unusable.</summary>
    public static Il2CppWorkspace Load(string path, IDiagnosticSink sink, WorkspaceOptions? options = null)
    {
        options ??= new WorkspaceOptions();
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.WorkspaceLoadStarted, $"Loading '{path}'.", new() { File = path });

        var files = GameFolderLocator.Locate(path, sink);
        if (files.MetadataPath is null) throw new MetadataLoadException($"global-metadata.dat not found under '{path}'.");

        var md = GlobalMetadata.Load(files.MetadataPath, sink);
        IBinaryImage? pe = null;
        try
        {
            var model = MetadataModel.Build(md, sink);
            if (files.BinaryPath is { } bin && bin.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                pe = PeBinaryImage.Load(bin, sink);
            else if (files.BinaryPath is { } wasmPath && WasmFiles.IsWasmPath(wasmPath))
                pe = WasmBinaryImage.Load(wasmPath, sink);
            else if (files.BinaryPath is { } so && ElfBinaryImage.IsElf(so))
                pe = ElfBinaryImage.Load(so, sink);
            else if (files.BinaryPath is not null)
                sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported,
                    "Unrecognized native binary format; metadata-only features remain available.", new() { File = files.BinaryPath });

            BuildBackupInfo? backup = null;
            if (options.UseBuildBackup)
            {
                var cv = (pe as PeBinaryImage)?.GetCodeView();
                backup = BuildBackupDetector.Detect(files.Root, cv is null ? null : new PdbIdentity(cv.Guid, cv.Age), sink);
            }

            var env = EnvironmentDetector.Detect(files.Root, files.DataFolder, files.BinaryPath, model, sink);
            var ws = new Il2CppWorkspace(files, options, sink, md, model, pe, backup, env);
            ws.InitializeNativePipeline();
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.WorkspaceLoadCompleted,
                $"{ws.ModeLabel} {model.Statistics.Types:N0} types, {model.Statistics.Methods:N0} methods loaded.", new() { File = files.Root });
            return ws;
        }
        catch
        {
            pe?.Dispose();
            md.Dispose();
            throw;
        }
    }

    /// <summary>Extension point for the native (registration / lifting) pipeline; see Il2CppWorkspace.Native.cs.</summary>
    partial void InitializeNativePipeline();

    partial void DisposeNativePipeline();

    /// <summary>Finds types by simple name, full name or "Namespace.Name" (case-insensitive fallback).</summary>
    public IReadOnlyList<TypeModel> FindTypes(string name)
    {
        var exact = Model.Types.Where(t => t.FullName == name || t.Name == name).ToList();
        return exact.Count > 0 ? exact : [.. Model.Types.Where(t =>
            t.FullName.Equals(name, StringComparison.OrdinalIgnoreCase) || t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))];
    }

    public TypeModel? DeclaringTypeOf(MethodModel method) =>
        (uint)Metadata.Methods[method.Index].DeclaringType < (uint)Model.Types.Count ? Model.Types[Metadata.Methods[method.Index].DeclaringType] : null;

    public SerializationSummary ExportArtifacts(string outDir, bool includeRawSections = true) =>
        WorkspaceSerializer.ExportAll(Metadata, Model, ResourcesFolder, outDir, Sink, includeRawSections);

    public void Dispose()
    {
        DisposeNativePipeline();
        Binary?.Dispose();
        Metadata.Dispose();
    }
}

using System.Text;
using System.Text.RegularExpressions;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Backup;

/// <summary>Method pointer table of one assembly, from &lt;Assembly&gt;_CodeGen.c (s_methodPointers).</summary>
/// <remarks>Entry i corresponds to method token 0x06000000 | (i + 1) in that assembly.</remarks>
public sealed record CodeGenModule(string AssemblyName, string SourceFile, IReadOnlyList<string> MethodPointers)
{
    public string? SymbolForToken(uint token)
    {
        var row = (int)(token & 0x00FFFFFF);
        return row >= 1 && row <= MethodPointers.Count ? MethodPointers[row - 1] : null;
    }
}

public sealed record PdbIdentity(Guid Guid, uint Age);

public sealed record BuildBackupInfo
{
    public required string Folder { get; init; }
    public string? Il2CppOutputFolder { get; init; }
    public int CppFileCount { get; init; }
    public long CppTotalBytes { get; init; }
    public string? ManagedFolder { get; init; }
    public IReadOnlyList<string> ManagedAssemblies { get; init; } = [];
    public string? PdbPath { get; init; }
    public PdbIdentity? PdbIdentity { get; init; }
    public bool? PdbMatchesBinary { get; init; }
    public IReadOnlyList<CodeGenModule> CodeGenModules { get; init; } = [];
    public IReadOnlyList<string> MapFiles { get; init; } = [];

    public string ModeLabel => "[Mode: Enhanced (Build Backup Detected)]";

    /// <summary>RVA → function name from the matching PDB (loaded on demand).</summary>
    public IReadOnlyDictionary<ulong, string>? PdbPublicSymbols { get; set; }
}

/// <summary>
/// Opportunistic detection of Unity's "&lt;Product&gt;_BackUpThisFolder_ButDontShipItWithYourGame" folder.
/// Shipped games normally do not contain it; everything here is optional enrichment and the rest of
/// the tool must work identically when <see cref="Detect"/> returns null.
/// </summary>
public static partial class BuildBackupDetector
{
    public const string FolderSuffix = "BackUpThisFolder_ButDontShipItWithYourGame";
    public const string BareModeLabel = "[Mode: Native Heuristic Recovery]";

    [GeneratedRegex(@"s_methodPointers\[\d+\]\s*=\s*\{(?<body>.*?)\};", RegexOptions.Singleline)]
    private static partial Regex MethodPointersRegex();

    [GeneratedRegex(@"const Il2CppCodeGenModule g_\w+_CodeGenModule\s*=\s*\{\s*""(?<name>[^""]+)""", RegexOptions.Singleline)]
    private static partial Regex ModuleNameRegex();

    /// <param name="gameRoot">Game root folder (the one containing the exe).</param>
    /// <param name="binaryPdbGuid">CodeView GUID/age from GameAssembly.dll, if known, to verify the PDB.</param>
    public static BuildBackupInfo? Detect(string gameRoot, PdbIdentity? binaryPdbGuid, IDiagnosticSink sink, bool parseCodeGen = true)
    {
        string? folder = null;
        try
        {
            folder = Directory.EnumerateDirectories(gameRoot, "*" + FolderSuffix, SearchOption.TopDirectoryOnly).FirstOrDefault();
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.BuildBackup, DiagnosticCodes.FileAccessFailed, $"Cannot scan for build backup: {ex.Message}", new() { File = gameRoot }, ex);
        }

        if (folder is null)
        {
            sink.Info(DiagnosticCategory.BuildBackup, DiagnosticCodes.BackupNotPresent, $"No build backup folder. {BareModeLabel}", new() { File = gameRoot });
            return null;
        }

        var il2cppOutput = Path.Combine(folder, "il2cppOutput");
        var managed = Path.Combine(folder, "Managed");
        var pdb = Path.Combine(folder, "GameAssembly.pdb");

        int cppCount = 0; long cppBytes = 0;
        var modules = new List<CodeGenModule>();
        if (Directory.Exists(il2cppOutput))
        {
            foreach (var f in Directory.EnumerateFiles(il2cppOutput, "*.*", SearchOption.AllDirectories))
            {
                var ext = Path.GetExtension(f);
                if (ext is ".cpp" or ".c" or ".h") { cppCount++; cppBytes += new FileInfo(f).Length; }
            }
            if (parseCodeGen)
                foreach (var f in Directory.EnumerateFiles(il2cppOutput, "*_CodeGen.c", SearchOption.AllDirectories))
                    if (ParseCodeGen(f, sink) is { } m) modules.Add(m);
        }

        var managedAssemblies = Directory.Exists(managed)
            ? Directory.EnumerateFiles(managed, "*.dll").Select(Path.GetFileName).OfType<string>().Order(StringComparer.OrdinalIgnoreCase).ToList()
            : [];

        var mapFiles = Directory.EnumerateFiles(folder, "*.map", SearchOption.AllDirectories).ToList();

        PdbIdentity? pdbId = null;
        bool? matches = null;
        if (File.Exists(pdb))
        {
            pdbId = PdbInfoReader.TryRead(pdb, sink);
            if (pdbId is not null && binaryPdbGuid is not null)
            {
                matches = pdbId.Guid == binaryPdbGuid.Guid;
                if (matches == true)
                    sink.Info(DiagnosticCategory.Pdb, DiagnosticCodes.PdbMatchesBinary, $"GameAssembly.pdb matches the binary (GUID {pdbId.Guid}).", new() { File = pdb });
                else
                    sink.Warn(DiagnosticCategory.Pdb, DiagnosticCodes.PdbMismatch,
                        $"GameAssembly.pdb GUID {pdbId.Guid} does not match the binary ({binaryPdbGuid.Guid}); it belongs to a different build and will not be used.", new() { File = pdb });
            }
        }

        var info = new BuildBackupInfo
        {
            Folder = folder,
            Il2CppOutputFolder = Directory.Exists(il2cppOutput) ? il2cppOutput : null,
            CppFileCount = cppCount,
            CppTotalBytes = cppBytes,
            ManagedFolder = Directory.Exists(managed) ? managed : null,
            ManagedAssemblies = managedAssemblies,
            PdbPath = File.Exists(pdb) ? pdb : null,
            PdbIdentity = pdbId,
            PdbMatchesBinary = matches,
            CodeGenModules = modules,
            MapFiles = mapFiles,
        };

        sink.Info(DiagnosticCategory.BuildBackup, DiagnosticCodes.BackupDetected,
            $"{info.ModeLabel} {cppCount} C/C++ files, {managedAssemblies.Count} managed assemblies, " +
            $"{modules.Count} CodeGen modules ({modules.Sum(m => m.MethodPointers.Count):N0} method pointers), PDB: {(info.PdbPath is null ? "no" : "yes")}.",
            new() { File = folder });
        return info;
    }

    public static CodeGenModule? ParseCodeGen(string path, IDiagnosticSink sink)
    {
        try
        {
            var text = File.ReadAllText(path, Encoding.UTF8);
            var nameMatch = ModuleNameRegex().Match(text);
            var assembly = nameMatch.Success ? nameMatch.Groups["name"].Value : Path.GetFileName(path).Replace("_CodeGen.c", ".dll");

            var mp = MethodPointersRegex().Match(text);
            if (!mp.Success)
            {
                // Assemblies without methods emit no table; that is valid.
                return new CodeGenModule(assembly, path, []);
            }

            var pointers = mp.Groups["body"].Value
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            sink.Verbose(DiagnosticCategory.BuildBackup, DiagnosticCodes.CodeGenModuleParsed, $"{assembly}: {pointers.Count} method pointers.", new() { File = path });
            return new CodeGenModule(assembly, path, pointers);
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.BuildBackup, DiagnosticCodes.CodeGenParseFailed, $"Cannot parse CodeGen file: {ex.Message}", new() { File = path }, ex);
            return null;
        }
    }
}

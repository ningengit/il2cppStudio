using AsmResolver.PE;
using AsmResolver.PE.Debug;
using AsmResolver.PE.File;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Binary;

public sealed record PeExport(string? Name, uint Ordinal, uint Rva);

public sealed record PeImportModule(string Name, int SymbolCount);

public sealed record PeCodeViewInfo(Guid Guid, uint Age, string? PdbPath);

/// <summary>Summary of PE header fields, as reported by AsmResolver.</summary>
public sealed record PeHeaderInfo(
    string Machine,
    uint TimeDateStamp,
    string LinkerVersion,
    string Subsystem,
    ulong ImageBase,
    uint SizeOfImage,
    uint SizeOfCode);

/// <summary>
/// Windows PE implementation of <see cref="IBinaryImage"/>. All header, section, export, import and
/// debug-directory interpretation is delegated to AsmResolver.PE. The memory-mapped view is kept only
/// so later phases can slice raw bytes at an RVA without copying.
/// </summary>
public sealed class PeBinaryImage : IBinaryImage
{
    private readonly MappedFile _file;
    private readonly IDiagnosticSink _sink;
    private readonly Lazy<PEImage?> _image;

    public string Path => _file.Path;
    public BinaryFormat Format => BinaryFormat.PE;
    public CpuArchitecture Architecture { get; }
    public bool Is64Bit { get; }
    public ulong ImageBase { get; }
    public IReadOnlyList<BinarySection> Sections { get; }
    public ReadOnlyMemory<byte> Data => _file.Memory;

    public PEFile PeFile { get; }
    public PeHeaderInfo Headers { get; }

    private PeBinaryImage(MappedFile file, PEFile peFile, IDiagnosticSink sink)
    {
        _file = file;
        _sink = sink;
        PeFile = peFile;

        var fh = peFile.FileHeader;
        var oh = peFile.OptionalHeader;

        Architecture = fh.Machine switch
        {
            MachineType.I386 => CpuArchitecture.X86,
            MachineType.Amd64 => CpuArchitecture.X64,
            MachineType.Arm64 => CpuArchitecture.Arm64,
            MachineType.Arm or MachineType.ArmNt or MachineType.Thumb => CpuArchitecture.Arm,
            _ => CpuArchitecture.Unknown,
        };
        Is64Bit = oh.Magic == OptionalHeaderMagic.PE32Plus;
        ImageBase = oh.ImageBase;

        Sections = [.. peFile.Sections.Select(s => new BinarySection(
            s.Name.ToString(), s.Rva, s.GetVirtualSize(), (long)s.Offset, s.GetPhysicalSize(),
            s.IsMemoryExecute, s.IsMemoryWrite, s.IsMemoryRead))];

        Headers = new PeHeaderInfo(
            fh.Machine.ToString(),
            fh.TimeDateStamp,
            $"{oh.MajorLinkerVersion}.{oh.MinorLinkerVersion:D2}",
            oh.SubSystem.ToString(),
            oh.ImageBase,
            oh.SizeOfImage,
            oh.SizeOfCode);

        _image = new Lazy<PEImage?>(() =>
        {
            try { return PEImage.FromFile(PeFile); }
            catch (Exception ex)
            {
                _sink.Error(DiagnosticCategory.Binary, DiagnosticCodes.PeParseFailed,
                    $"AsmResolver could not read the PE data directories: {ex.Message}", new() { File = Path }, ex);
                return null;
            }
        });
    }

    public static PeBinaryImage Load(string path, IDiagnosticSink sink)
    {
        var file = MappedFile.Open(path);
        try
        {
            var pe = PEFile.FromFile(file.Path);
            var image = new PeBinaryImage(file, pe, sink);
            sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
                $"PE {image.Architecture}, image base 0x{image.ImageBase:X}, {image.Sections.Count} sections.", new() { File = path });
            return image;
        }
        catch (Exception ex)
        {
            file.Dispose();
            sink.Error(DiagnosticCategory.Binary, DiagnosticCodes.PeParseFailed, $"Not a valid PE file: {ex.Message}", new() { File = path }, ex);
            throw;
        }
    }

    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(Sections, rva, out fileOffset);

    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(Sections, fileOffset, out rva);

    public IReadOnlyList<PeExport> GetExports()
    {
        try
        {
            var exports = _image.Value?.Exports;
            return exports is null ? [] : [.. exports.Entries.Select(e => new PeExport(e.Name, e.Ordinal, e.Address.Rva))];
        }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.ExportsParseFailed, $"Export directory unreadable: {ex.Message}", new() { File = Path }, ex);
            return [];
        }
    }

    public IReadOnlyList<PeImportModule> GetImports()
    {
        try
        {
            var imports = _image.Value?.Imports;
            return imports is null ? [] : [.. imports.Select(m => new PeImportModule(m.Name ?? "<unnamed>", m.Symbols.Count))];
        }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.ImportsParseFailed, $"Import directory unreadable: {ex.Message}", new() { File = Path }, ex);
            return [];
        }
    }

    public PeCodeViewInfo? GetCodeView()
    {
        try
        {
            var debug = _image.Value?.DebugData;
            if (debug is null) return null;
            foreach (var entry in debug)
                if (entry.Contents is RsdsDataSegment rsds)
                    return new PeCodeViewInfo(rsds.Guid, rsds.Age, rsds.Path);
            return null;
        }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.DebugDirectoryParseFailed, $"Debug directory unreadable: {ex.Message}", new() { File = Path }, ex);
            return null;
        }
    }

    /// <summary>
    /// x64 RUNTIME_FUNCTION entries from the exception directory (.pdata): exact [begin, end) RVA ranges of
    /// every non-leaf function, which gives precise method boundaries without heuristics.
    /// </summary>
    public IReadOnlyList<(uint Begin, uint End)> GetRuntimeFunctions()
    {
        try
        {
            var dir = _image.Value?.Exceptions;
            if (dir is null) return [];
            var raw = new List<(uint Begin, uint End, uint ParentBegin)>();
            foreach (var f in dir.GetFunctions())
                if (f is AsmResolver.PE.Exceptions.X64RuntimeFunction x)
                {
                    var parent = x.UnwindInfo is { IsChained: true, ChainedFunction: { } c } ? c.Begin.Rva : x.Begin.Rva;
                    raw.Add((x.Begin.Rva, x.End.Rva, parent));
                }
            raw.Sort();
            // MSVC splits large functions into chained fragments (one RUNTIME_FUNCTION per prolog-less region, e.g.
            // shrink-wrapped saves). A fragment may chain to another fragment, so follow the chain to its root entry,
            // then merge every fragment of the same root into one extent.
            var parentOf = new Dictionary<uint, uint>();
            foreach (var (begin, _, parent) in raw) parentOf.TryAdd(begin, parent);
            uint Root(uint b)
            {
                for (var guard = 0; guard < 64 && parentOf.TryGetValue(b, out var p) && p != b; guard++) b = p;
                return b;
            }
            var extents = new Dictionary<uint, (uint Begin, uint End)>();
            foreach (var (begin, end, _) in raw)   // sorted by begin
            {
                var root = Root(begin);
                if (!extents.TryGetValue(root, out var e)) { extents[root] = (begin, end); continue; }
                // Adjacent fragment (allowing int3/alignment padding) extends the function; a distant cold fragment
                // is left out so the extent never swallows unrelated functions.
                if (begin <= e.End + 0x40) extents[root] = (e.Begin, Math.Max(e.End, end));
            }
            var list = extents.Values.Select(e => (e.Begin, e.End)).OrderBy(e => e.Begin).ToList();
            return list;
        }
        catch (Exception ex)
        {
            _sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.PeParseFailed, $"Exception directory unreadable: {ex.Message}", new() { File = Path }, ex);
            return [];
        }
    }

    public void Dispose() => _file.Dispose();

    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => [.. GetExports().Where(e => e.Name is not null).Select(e => (e.Name!, (ulong)e.Rva))];

    public IReadOnlyList<(uint Begin, uint End)> GetFunctionTable() => GetRuntimeFunctions();
}

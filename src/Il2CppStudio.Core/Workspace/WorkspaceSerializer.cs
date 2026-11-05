using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Resources;

namespace Il2CppStudio.Core.Workspace;

public sealed record ExportedResource(string Assembly, string Name, string Kind, int Size, string Sha256, string RelativePath, int DecodedEntries);

public sealed record ExportedStringLiteral(int Index, int Length, int DataIndex, string Value);

public sealed record SerializationSummary(
    string OutputDirectory, int ResourceFiles, int Resources, int StringLiterals, int Sections, IReadOnlyList<string> Files);

/// <summary>
/// Serializes workspace artifacts to disk: managed resources from *-resources.dat containers, the string
/// literal table, and the global-metadata.dat section report. Every artifact is written twice — once as
/// structured JSON for diffing/tooling and once as a binary inspection file for hex editors.
///
/// Binary string literal table (strings.bin):  "ISLT" u32 version=1 u32 count, then count × {u32 length, u8[length] UTF-8}.
/// Raw sections: sections/&lt;name&gt;.bin, byte-exact slices of global-metadata.dat.
/// </summary>
public static class WorkspaceSerializer
{
    public static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>Exports everything: resources, string literals and metadata sections.</summary>
    public static SerializationSummary ExportAll(GlobalMetadata md, MetadataModel? model, string? resourcesFolder, string outDir,
        IDiagnosticSink sink, bool includeRawSections = true)
    {
        Directory.CreateDirectory(outDir);
        var files = new List<string>();
        var resources = ExportResources(resourcesFolder, Path.Combine(outDir, "resources"), sink, files);
        var literals = ExportStringLiterals(md, Path.Combine(outDir, "strings"), files);
        var sections = ExportMetadataReport(md, model, Path.Combine(outDir, "metadata"), includeRawSections, files);

        var summary = new SerializationSummary(outDir, resources.Select(r => r.Assembly).Distinct().Count(), resources.Count, literals, sections, files);
        WriteJson(Path.Combine(outDir, "workspace-artifacts.json"), summary, files);
        sink.Info(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionCompleted,
            $"Serialized {resources.Count} resource(s), {literals:N0} string literal(s), {sections} metadata section(s) to '{outDir}'.", new() { File = outDir });
        return summary with { Files = files };
    }

    /// <summary>Extracts every resource from every *-resources.dat in <paramref name="resourcesFolder"/>.</summary>
    public static IReadOnlyList<ExportedResource> ExportResources(string? resourcesFolder, string outDir, IDiagnosticSink sink, List<string>? files = null)
    {
        files ??= [];
        var result = new List<ExportedResource>();
        if (resourcesFolder is null || !Directory.Exists(resourcesFolder)) return result;

        foreach (var datPath in Directory.EnumerateFiles(resourcesFolder, "*-resources.dat").Order(StringComparer.OrdinalIgnoreCase))
        {
            var dat = ResourceDatFile.Load(datPath, sink);
            if (dat is null) continue;
            var asmDir = Path.Combine(outDir, Sanitize(dat.AssemblyName));
            Directory.CreateDirectory(asmDir);

            foreach (var entry in dat.Entries)
            {
                try
                {
                    var data = dat.GetData(entry);
                    var fileName = Sanitize(entry.Name);
                    var target = Path.Combine(asmDir, fileName);
                    File.WriteAllBytes(target, data);
                    files.Add(target);

                    var decoded = 0;
                    if (entry.Kind == ManagedResourceKind.ResourcesContainer)
                    {
                        var pairs = ResourceDatFile.DecodeResourcesContainer(data, $"{dat.AssemblyName}/{entry.Name}", sink);
                        decoded = pairs.Count;
                        WriteJson(target + ".json", pairs.ToDictionary(p => p.Key, p => p.Value), files);
                    }
                    result.Add(new ExportedResource(dat.AssemblyName, entry.Name, entry.Kind.ToString(), entry.Size,
                        Convert.ToHexStringLower(SHA256.HashData(data)), Path.GetRelativePath(outDir, target), decoded));
                }
                catch (Exception ex)
                {
                    sink.Warn(DiagnosticCategory.Extraction, DiagnosticCodes.ExtractionEntryFailed,
                        $"Resource '{entry.Name}' of {dat.AssemblyName} could not be exported: {ex.Message}", new() { File = datPath }, ex);
                }
            }
        }
        WriteJson(Path.Combine(outDir, "resources.json"), result, files);
        return result;
    }

    /// <summary>Writes strings.json and strings.bin. Returns the number of literals.</summary>
    public static int ExportStringLiterals(GlobalMetadata md, string outDir, List<string>? files = null)
    {
        files ??= [];
        Directory.CreateDirectory(outDir);
        var list = new List<ExportedStringLiteral>(md.StringLiterals.Length);
        for (var i = 0; i < md.StringLiterals.Length; i++)
        {
            var lit = md.StringLiterals[i];
            list.Add(new ExportedStringLiteral(i, (int)lit.Length, lit.DataIndex, md.GetStringLiteral(i)));
        }
        WriteJson(Path.Combine(outDir, "strings.json"), list, files);

        var binPath = Path.Combine(outDir, "strings.bin");
        using (var fs = File.Create(binPath))
        {
            Span<byte> header = stackalloc byte[12];
            "ISLT"u8.CopyTo(header);
            BinaryPrimitives.WriteUInt32LittleEndian(header[4..], 1);
            BinaryPrimitives.WriteUInt32LittleEndian(header[8..], (uint)list.Count);
            fs.Write(header);
            Span<byte> len = stackalloc byte[4];
            foreach (var s in list)
            {
                var bytes = Encoding.UTF8.GetBytes(s.Value);
                BinaryPrimitives.WriteUInt32LittleEndian(len, (uint)bytes.Length);
                fs.Write(len);
                fs.Write(bytes);
            }
        }
        files.Add(binPath);
        return list.Count;
    }

    /// <summary>Writes the section report, statistics and (optionally) raw section dumps.</summary>
    public static int ExportMetadataReport(GlobalMetadata md, MetadataModel? model, string outDir, bool includeRawSections, List<string>? files = null)
    {
        files ??= [];
        Directory.CreateDirectory(outDir);
        var report = new
        {
            md.Path,
            Version = md.Version.ToString(),
            md.Version.UnityRange,
            md.FileSize,
            md.Header.HeaderSize,
            Statistics = model?.Statistics,
            Sections = md.SectionReports.OrderBy(r => r.Offset).ToList(),
            Assemblies = model?.Assemblies.Select(a => new { a.Name, a.Version, a.Culture, a.PublicKeyToken, a.ImageName, a.TypeCount, a.References }),
        };
        WriteJson(Path.Combine(outDir, "metadata-report.json"), report, files);

        if (includeRawSections)
        {
            var secDir = Path.Combine(outDir, "sections");
            Directory.CreateDirectory(secDir);
            foreach (var s in md.Header.Sections.Where(s => s.Size > 0))
            {
                var bytes = md.GetSectionBytes(s.Name);
                if (bytes.IsEmpty) continue;
                var p = Path.Combine(secDir, s.Name + ".bin");
                using var fs = File.Create(p);
                fs.Write(bytes.Span);
                files.Add(p);
            }
        }
        return md.SectionReports.Count;
    }

    public static void WriteJson<T>(string path, T value, List<string>? files = null)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        using var fs = File.Create(path);
        JsonSerializer.Serialize(fs, value, Json);
        files?.Add(path);
    }

    /// <summary>Makes a resource / type name safe as a single path segment on every OS.</summary>
    public static string Sanitize(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "_";
        var invalid = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(name.Length);
        foreach (var c in name) sb.Append(invalid.Contains(c) || c is '<' or '>' or '|' or '`' ? '_' : c);
        var s = sb.ToString().Trim('.', ' ');
        return s.Length == 0 ? "_" : s.Length > 150 ? s[..150] : s;
    }
}

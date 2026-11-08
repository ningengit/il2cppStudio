namespace Il2CppStudio.Core.Diagnostics;

/// <summary>Severity levels, aligned 1:1 with Serilog's LogEventLevel.</summary>
public enum DiagnosticSeverity
{
    Verbose,
    Debug,
    Information,
    Warning,
    Error,
    Fatal,
}

/// <summary>
/// Diagnostic categories. Categories for later phases (lifting, decompilation) are declared now so
/// that the logging contract, JSON schema and GUI filters stay stable across phases.
/// </summary>
public enum DiagnosticCategory
{
    Workspace,
    FileSystem,
    Binary,
    Fingerprint,
    Metadata,
    MetadataIntegrity,
    Resources,
    BootConfig,
    DevelopmentPlayer,
    BuildBackup,
    Pdb,
    Extraction,

    // Phase 2+ (lifting / decompilation)
    Registration,
    SignatureParsing,
    VTableResolution,
    InlinedMethod,
    GenericSharing,
    MissingSymbol,
    CorruptMetadata,
    Lifting,
    Decompilation,
    Accuracy,
}

/// <summary>Where a diagnostic applies. Every field is optional; fill in whatever is known.</summary>
public sealed record DiagnosticLocation
{
    public string? File { get; init; }
    public long? FileOffset { get; init; }
    public ulong? Rva { get; init; }
    public ulong? VirtualAddress { get; init; }
    public string? Table { get; init; }
    public int? MetadataIndex { get; init; }
    public uint? Token { get; init; }
    public string? AssemblyName { get; init; }
    public string? TypeName { get; init; }
    public string? MethodName { get; init; }

    public override string ToString()
    {
        var parts = new List<string>(6);
        if (File is not null) parts.Add(System.IO.Path.GetFileName(File));
        if (Table is not null) parts.Add(MetadataIndex is { } i ? $"{Table}[{i}]" : Table);
        if (Token is { } t) parts.Add($"token=0x{t:X8}");
        if (FileOffset is { } o) parts.Add($"offset=0x{o:X}");
        if (Rva is { } r) parts.Add($"rva=0x{r:X}");
        if (VirtualAddress is { } va) parts.Add($"va=0x{va:X}");
        if (TypeName is not null) parts.Add(MethodName is not null ? $"{TypeName}::{MethodName}" : TypeName);
        else if (MethodName is not null) parts.Add(MethodName);
        return string.Join(' ', parts);
    }
}

/// <summary>A single structured diagnostic event.</summary>
public sealed record Diagnostic(
    DateTimeOffset Timestamp,
    DiagnosticSeverity Severity,
    DiagnosticCategory Category,
    string Code,
    string Message,
    DiagnosticLocation? Location = null,
    string? ExceptionType = null,
    string? ExceptionText = null)
{
    public override string ToString()
    {
        var loc = Location is null ? "" : $" [{Location}]";
        return $"{Severity,-11} {Category,-18} {Code} {Message}{loc}";
    }
}

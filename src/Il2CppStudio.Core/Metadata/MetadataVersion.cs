namespace Il2CppStudio.Core.Metadata;

/// <summary>
/// global-metadata.dat version. The file stores only the major number (27, 29, 31); minor revisions
/// such as 27.1, 27.2 or 29.1 change structures in the native binary only, so the metadata layout is
/// fully determined by the major version for the range supported here (27 to 31).
/// </summary>
public readonly record struct MetadataVersion(int Major, int Minor = 0) : IComparable<MetadataVersion>
{
    public static readonly MetadataVersion MinSupported = new(24);
    public static readonly MetadataVersion MaxSupported = new(31);

    public int CompareTo(MetadataVersion other) =>
        Major != other.Major ? Major.CompareTo(other.Major) : Minor.CompareTo(other.Minor);

    public static bool operator <(MetadataVersion a, MetadataVersion b) => a.CompareTo(b) < 0;
    public static bool operator >(MetadataVersion a, MetadataVersion b) => a.CompareTo(b) > 0;
    public static bool operator <=(MetadataVersion a, MetadataVersion b) => a.CompareTo(b) <= 0;
    public static bool operator >=(MetadataVersion a, MetadataVersion b) => a.CompareTo(b) >= 0;

    public bool IsSupported => this >= MinSupported && this <= MaxSupported;

    /// <summary>Custom attributes are stored as encoded blobs from v29 onwards (type ranges before).</summary>
    public bool HasAttributeDataBlobs => Major >= 29;

    /// <summary>Il2CppMethodDefinition gained returnParameterToken in v31.</summary>
    public bool HasReturnParameterToken => Major >= 31;

    // ---- v24 (Unity 2018.3 - 2020.1). Minor revisions are detected structurally by GlobalMetadata.
    public bool IsV24 => Major == 24;

    /// <summary>24.0 only: per-record customAttributeIndex (type, method, field, parameter, property, event, assembly).</summary>
    public bool HasCustomAttributeIndex => Major == 24 && Minor == 0;

    /// <summary>24.0-24.1: methodIndex/invokerIndex/delegateWrapperIndex/rgctx in method and type records, rgctxEntries table.</summary>
    public bool HasCodegenIndices => Major == 24 && Minor <= 1;

    /// <summary>v24: Il2CppTypeDefinition.byrefTypeIndex; metadataUsageLists/Pairs tables in the header.</summary>
    public bool HasByrefTypeIndex => Major == 24;
    public bool HasMetadataUsageTables => Major == 24;

    /// <summary>24.1+: token on assemblies/attribute ranges, customAttributeStart/Count on images.</summary>
    public bool HasImageAttributeRanges => Major > 24 || Minor >= 1;

    /// <summary>Assembly names carry hashValueIndex up to 24.3.</summary>
    public bool HasHashValueIndex => Major == 24 && Minor <= 3;

    /// <summary>windowsRuntimeStrings header pair was added in v27.</summary>
    public bool HasWindowsRuntimeStrings => Major >= 27;

    /// <summary>Approximate Unity editor range that writes this version (for display only).</summary>
    public string UnityRange => Major switch
    {
        24 => Minor switch { 0 => "Unity 2018.1 - 2018.2", 1 => "Unity 2018.3 - 2018.4", _ => "Unity 2019.1 - 2020.1" },
        27 => "Unity 2020.2 - 2021.1",
        29 => "Unity 2021.2 - 2022.1",
        31 => "Unity 2022.2 - 6000.x",
        _ => "unknown",
    };

    public override string ToString() => Minor == 0 ? Major.ToString() : $"{Major}.{Minor}";
}

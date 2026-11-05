namespace Il2CppStudio.Core.Metadata;

/// <summary>
/// global-metadata.dat version. The file stores only the major number (27, 29, 31); minor revisions
/// such as 27.1, 27.2 or 29.1 change structures in the native binary only, so the metadata layout is
/// fully determined by the major version for the range supported here (27 to 31).
/// </summary>
public readonly record struct MetadataVersion(int Major, int Minor = 0) : IComparable<MetadataVersion>
{
    public static readonly MetadataVersion MinSupported = new(27);
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

    /// <summary>Approximate Unity editor range that writes this version (for display only).</summary>
    public string UnityRange => Major switch
    {
        27 => "Unity 2020.2 - 2021.1",
        29 => "Unity 2021.2 - 2022.1",
        31 => "Unity 2022.2 - 6000.x",
        _ => "unknown",
    };

    public override string ToString() => Minor == 0 ? Major.ToString() : $"{Major}.{Minor}";
}

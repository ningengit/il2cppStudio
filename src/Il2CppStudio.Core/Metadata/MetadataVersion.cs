namespace Il2CppStudio.Core.Metadata;

/// <summary>
/// global-metadata.dat version. The file stores only the major number (16, 19-24, 27, 29, 31); minor revisions
/// such as 24.0-24.4 change metadata layout structurally, while 27.1, 27.2 or 29.1 change structures in the
/// native binary only. Supported range: 16 to 31 (Unity 5.3 to 6000.x).
/// </summary>
public readonly record struct MetadataVersion(int Major, int Minor = 0) : IComparable<MetadataVersion>
{
    public static readonly MetadataVersion MinSupported = new(16);
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

    public bool IsPreV27 => Major < 27;
    public bool IsV24 => Major == 24;

    /// <summary>v19+: token on Image, Type, Field, Property, Event; fieldRefs table.</summary>
    public bool HasTokens => Major >= 19;
    public bool HasFieldRefs => Major >= 19;

    /// <summary>v20+: referencedAssemblies header table and per-assembly range fields.</summary>
    public bool HasReferencedAssemblies => Major >= 20;

    /// <summary>v21–v27: attributesInfo and attributeTypes header tables.</summary>
    public bool HasCustomAttributeTables => Major >= 21 && Major < 29;

    /// <summary>v22+: unresolvedIndirectCall / unresolvedVirtualCall parameter metadata tables.</summary>
    public bool HasUnresolvedIndirectCalls => Major >= 22;

    /// <summary>v23+: windowsRuntimeTypeNames header table.</summary>
    public bool HasWindowsRuntimeTypeNames => Major >= 23;

    /// <summary>v24+: exportedTypeDefinitions header table and image exportedTypeStart/Count.</summary>
    public bool HasExportedTypeDefinitions => Major >= 24;

    /// <summary>v16–v24.0: per-record customAttributeIndex (type, method, field, parameter, property, event, assembly).</summary>
    public bool HasCustomAttributeIndex => Major < 24 || (Major == 24 && Minor == 0);

    /// <summary>v16–v24.1: methodIndex/invokerIndex/delegateWrapperIndex/rgctx in method and type records, rgctxEntries table.</summary>
    public bool HasCodegenIndices => Major < 24 || (Major == 24 && Minor <= 1);

    /// <summary>v16–v24: Il2CppTypeDefinition.byrefTypeIndex.</summary>
    public bool HasByrefTypeIndex => Major < 27;

    /// <summary>v19–v24: metadataUsageLists/Pairs tables in header and MetadataRegistration.metadataUsages.</summary>
    public bool HasMetadataUsageTables => Major >= 19 && Major < 27;

    /// <summary>24.1+: token on assemblies/attribute ranges, customAttributeStart/Count on images.</summary>
    public bool HasImageAttributeRanges => Major > 24 || (Major == 24 && Minor >= 1);

    /// <summary>Assembly names carry hashValueIndex up to 24.3.</summary>
    public bool HasHashValueIndex => Major < 24 || (Major == 24 && Minor <= 3);

    /// <summary>v16–v22: delegateWrapperFromManagedToNativeIndex and marshalingFunctionsIndex on TypeDefinition.</summary>
    public bool HasTypeDelegateAndMarshalingIndices => Major <= 22;

    /// <summary>v21–v22: ccwFunctionIndex and guidIndex on TypeDefinition.</summary>
    public bool HasTypeCcwAndGuidIndices => Major >= 21 && Major <= 22;

    /// <summary>v16 only: methodReferences pair present in MetadataRegistration before fieldOffsets.</summary>
    public bool HasMethodReferencesInMetadataReg => Major <= 16;

    /// <summary>windowsRuntimeStrings header pair was added in v27.</summary>
    public bool HasWindowsRuntimeStrings => Major >= 27;

    /// <summary>Approximate Unity editor range that writes this version (for display only).</summary>
    public string UnityRange => Major switch
    {
        16 => "Unity 5.3.0 - 5.3.1",
        19 => "Unity 5.3.2 - 5.3.4",
        20 => "Unity 5.3.5 - 5.4.x",
        21 => "Unity 5.5.x",
        22 => "Unity 5.6.x - 2017.1",
        23 => "Unity 2017.1 - 2017.4",
        24 => Minor switch { 0 => "Unity 2018.1 - 2018.2", 1 => "Unity 2018.3 - 2018.4", _ => "Unity 2019.1 - 2020.1" },
        27 => "Unity 2020.2 - 2021.1",
        29 => "Unity 2021.2 - 2022.1",
        31 => "Unity 2022.2 - 6000.x",
        _ => "unknown",
    };

    public override string ToString() => Minor == 0 ? Major.ToString() : $"{Major}.{Minor}";
}

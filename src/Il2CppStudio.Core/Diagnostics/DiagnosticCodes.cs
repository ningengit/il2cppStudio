namespace Il2CppStudio.Core.Diagnostics;

/// <summary>
/// Stable diagnostic codes. Codes never change meaning once shipped, so logs/JSON reports can be
/// diffed across runs and versions. Format: IS + 4 digits. Ranges:
///   1xxx workspace/filesystem, 2xxx binary/fingerprint, 3xxx metadata, 4xxx resources/extraction,
///   5xxx boot.config/dev player, 6xxx build backup/PDB, 7xxx+ reserved for lifting/decompilation.
/// </summary>
public static class DiagnosticCodes
{
    // Workspace / filesystem
    public const string WorkspaceLoadStarted = "IS1000";
    public const string WorkspaceLoadCompleted = "IS1001";
    public const string GameFolderNotFound = "IS1002";
    public const string BinaryNotFound = "IS1003";
    public const string MetadataNotFound = "IS1004";
    public const string MultipleCandidates = "IS1005";
    public const string FileAccessFailed = "IS1006";
    public const string ScriptingBackendDetected = "IS1007";
    public const string StageFailed = "IS1008";
    public const string PlayerLogLocation = "IS1009";

    // Binary / fingerprint
    public const string BinaryFormatDetected = "IS2000";
    public const string BinaryFormatUnsupported = "IS2001";
    public const string PeParseFailed = "IS2002";
    public const string ElfParseFailed = "IS2003";
    public const string RichHeaderMissing = "IS2004";
    public const string RichHeaderChecksumMismatch = "IS2005";
    public const string AddressUnmapped = "IS2006";
    public const string ExportsParseFailed = "IS2007";
    public const string DebugDirectoryParseFailed = "IS2008";
    public const string ImportsParseFailed = "IS2009";
    public const string CompilerFingerprint = "IS2010";
    public const string BuildConfigurationEstimate = "IS2011";
    public const string UnityVersionDetected = "IS2012";
    public const string UnityVersionUnknown = "IS2013";
    public const string ApiCompatibilityEstimate = "IS2014";
    public const string StrippingEstimate = "IS2015";
    public const string Il2CppExportsMissing = "IS2016";

    // Metadata
    public const string MetadataBadMagic = "IS3000";
    public const string MetadataVersionUnsupported = "IS3001";
    public const string MetadataVersionResolved = "IS3002";
    public const string MetadataSectionOutOfBounds = "IS3003";
    public const string MetadataSectionMisaligned = "IS3004";
    public const string MetadataSectionsOverlap = "IS3005";
    public const string MetadataHeaderSizeMismatch = "IS3006";
    public const string MetadataStringOutOfRange = "IS3007";
    public const string MetadataIndexOutOfRange = "IS3008";
    public const string MetadataLayoutAmbiguous = "IS3009";
    public const string MetadataPossiblyEncrypted = "IS3010";
    public const string MetadataTableRead = "IS3011";
    public const string MetadataModelBuilt = "IS3012";

    // Resources / extraction
    public const string ResourceFileParsed = "IS4000";
    public const string ResourceFileCorrupt = "IS4001";
    public const string ResourceEntryOutOfBounds = "IS4002";
    public const string ManagedResourcesDecodeFailed = "IS4003";
    public const string ExtractionEntryFailed = "IS4004";
    public const string ExtractionCompleted = "IS4005";
    public const string UnsupportedAssetContainer = "IS4006";

    // boot.config / development player
    public const string BootConfigMissing = "IS5000";
    public const string BootConfigPatched = "IS5001";
    public const string BootConfigBackupCreated = "IS5002";
    public const string BootConfigRestored = "IS5003";
    public const string DevPlayerNotFound = "IS5004";
    public const string DevPlayerVersionMismatch = "IS5005";
    public const string DevPlayerSwapped = "IS5006";
    public const string DevPlayerAlreadyInstalled = "IS5007";

    // Build backup / PDB
    public const string BackupDetected = "IS6000";
    public const string BackupNotPresent = "IS6001";
    public const string PdbMatchesBinary = "IS6002";
    public const string PdbMismatch = "IS6003";
    public const string PdbParseFailed = "IS6004";
    public const string CodeGenModuleParsed = "IS6005";
    public const string CodeGenParseFailed = "IS6006";
    public const string MapFileParsed = "IS6007";
    public const string PdbSymbolsLoaded = "IS6008";

    // Registration / type resolution (7xxx)
    public const string RegistrationFound = "IS7000";
    public const string RegistrationNotFound = "IS7001";
    public const string RegistrationAmbiguous = "IS7002";
    public const string TypeIndexOutOfRange = "IS7003";
    public const string MethodPointerMissing = "IS7004";
    public const string MetadataUsagesScanned = "IS7005";
    public const string SignatureMismatch = "IS7006";
    public const string GenericInstanceUnresolved = "IS7007";
    public const string RuntimeHelperIdentified = "IS7008";
    public const string InlineCopyMatched = "IS7009";

    // Lifting / disassembly (8xxx)
    public const string DisassemblyFailed = "IS8000";
    public const string UnresolvedCall = "IS8001";
    public const string UnresolvedVTableCall = "IS8002";
    public const string InlinedFunctionSkipped = "IS8003";
    public const string UnsupportedInstruction = "IS8004";
    public const string LiftCompleted = "IS8005";
    public const string LiftFailed = "IS8006";
    public const string StackImbalance = "IS8007";

    // Decompilation / accuracy (9xxx)
    public const string DecompilationFailed = "IS9000";
    public const string BclSubstituted = "IS9001";
    public const string BclReferenceMissing = "IS9002";
    public const string AccuracyComputed = "IS9003";
    public const string GroundTruthMissing = "IS9004";
    public const string ExportCompleted = "IS9005";
    public const string ExternalToolFailed = "IS9006";
    public const string ExternalToolLaunched = "IS9007";
}

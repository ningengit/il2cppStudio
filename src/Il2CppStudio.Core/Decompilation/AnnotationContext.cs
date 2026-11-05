using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>Native facts used to annotate decompiled output (supplied by the workspace).</summary>
public sealed class AnnotationContext
{
    public required MetadataModel Model { get; init; }
    /// <summary>Method → (RVA, file offset); 0 / -1 when the method has no native code.</summary>
    public required Func<MethodModel, (ulong Rva, long FileOffset)> MethodLocation { get; init; }
    /// <summary>(declaring type, field ordinal) → native field offset, or -1.</summary>
    public required Func<TypeModel, int, int> FieldOffset { get; init; }
    /// <summary>Why a method body is a <c>throw null</c> stub (null when it was lifted).</summary>
    public required Func<MethodModel, string?> FallbackReason { get; init; }
    public bool ShowTokens { get; set; } = true;
}

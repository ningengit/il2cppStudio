using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Connects the x86-64→CIL lifter to the decompiler as an <see cref="IMethodBodyProvider"/>. The factory stays
/// null until the instruction translation stage is complete, in which case front-ends fall back to signature-only
/// stubs (reported once as a diagnostic).
/// </summary>
public static partial class LiftingPipeline
{
    public static Func<Il2CppWorkspace, IMethodBodyProvider?>? Factory => CreateProvider;

    private static IMethodBodyProvider? CreateProvider(Il2CppWorkspace ws)
    {
        IMethodBodyProvider? provider = null;
        CreateLifter(ws, ref provider);
        return provider;
    }

    /// <summary>Implemented in the lifter's driver file once instruction translation is in place.</summary>
    static partial void CreateLifter(Il2CppWorkspace ws, ref IMethodBodyProvider? provider);
}

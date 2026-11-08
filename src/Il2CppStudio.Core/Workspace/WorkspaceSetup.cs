using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Single place where front-ends (CLI and GUI) finish configuring a loaded workspace, so both produce identical
/// output. Currently attaches the native→CIL lifting pipeline when the binary supports it.
/// </summary>
public static class WorkspaceSetup
{
    /// <summary>Factory registered by the lifting pipeline. Null until the lifter is available.</summary>
    public static Func<Il2CppWorkspace, Decompilation.IMethodBodyProvider?>? LifterFactory { get; set; }

    public static void Attach(Il2CppWorkspace ws)
    {
        if (ws.BodyProvider is not null || !ws.HasNativeCode) return;
        // Helper classification + inline-copy/PDB aliases must exist before the first lift (lifted bodies are cached).
        _ = ws.RuntimeHelpers;
        var factory = LifterFactory ?? Lifting.LiftingPipeline.Factory;
        ws.BodyProvider = factory?.Invoke(ws);
        if (ws.BodyProvider is null)
            ws.Sink.Info(DiagnosticCategory.Lifting, DiagnosticCodes.LiftFailed,
                "Native lifting is not available; C# and IL views show signatures with placeholder bodies.");
    }
}

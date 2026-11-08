using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Fingerprinting;

namespace Il2CppStudio.Core.Workspace;

public sealed partial class Il2CppWorkspace
{
    private ToolchainInfo? _toolchain;

    /// <summary>
    /// Compiler/toolchain of the native binary (MSVC, clang-cl, Clang, GCC, Emscripten), detected automatically from the
    /// container and code shape. Profiles that depend on it (calling convention, prologue/epilogue idioms) read this;
    /// the user never selects it.
    /// </summary>
    public ToolchainInfo Toolchain => _toolchain ??= DetectToolchain();

    private ToolchainInfo DetectToolchain()
    {
        ToolchainInfo info;
        if (Files.BinaryPath is not { } path) return new(NativeCompiler.Unknown, "unknown", "none", null, ["No native binary in the game folder."]);
        try
        {
            var bytes = File.ReadAllBytes(path);
            IEnumerable<int>? starts = null;
            if (Binary is { } pe)
                starts = pe.GetFunctionTable()
                    .Select(f => pe.TryRvaToFileOffset(f.Begin, out var off) ? (int)off : -1)
                    .Where(o => o >= 0).ToList();
            info = ToolchainDetector.Detect(bytes, starts);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            info = new(NativeCompiler.Unknown, "unknown", "unknown", null, [$"Binary unreadable: {ex.Message}"]);
        }
        Sink.Info(DiagnosticCategory.Fingerprint, DiagnosticCodes.CompilerFingerprint,
            $"Toolchain: {info.Display}. Evidence: {string.Join(" ", info.Evidence)}");
        return info;
    }
}

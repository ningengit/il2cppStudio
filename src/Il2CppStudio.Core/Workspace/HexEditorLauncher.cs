using System.Diagnostics;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Workspace;

public sealed record HexJumpTarget(string File, ulong Rva, long FileOffset, string Instructions);

/// <summary>
/// "View in HxD": resolves a method's RVA to a file offset in GameAssembly.dll and opens HxD. HxD's command line
/// only accepts a file name (no offset switch), so the offset is returned for the caller to show / place on the
/// clipboard, and the user jumps with Search → Go to (Ctrl+G).
/// </summary>
public static class HexEditorLauncher
{
    public const string DefaultHxdPath = @"C:\Program Files\HxD\HxD.exe";

    public static string? FindHxd(string? configured = null)
    {
        foreach (var p in new[] { configured, Environment.GetEnvironmentVariable("IL2CPPSTUDIO_HXD"), DefaultHxdPath, @"C:\Program Files (x86)\HxD\HxD.exe" })
            if (!string.IsNullOrEmpty(p) && File.Exists(p)) return p;
        return null;
    }

    public static HexJumpTarget? Resolve(Il2CppWorkspace ws, MethodModel method)
    {
        if (ws.Binary is null) return null;
        var rva = ws.GetMethodRva(method);
        if (rva == 0) return null;
        var off = ws.GetMethodFileOffset(method);
        if (off < 0) return null;
        return new HexJumpTarget(ws.Binary.Path, rva, off, $"In HxD press Ctrl+G and enter offset {off:X} (hex) — RVA 0x{rva:X}.");
    }

    /// <summary>Starts HxD on the binary. Returns false (with a diagnostic) if HxD is not installed or fails to start.</summary>
    public static bool Open(HexJumpTarget target, IDiagnosticSink sink, string? hxdPath = null)
    {
        var exe = FindHxd(hxdPath);
        if (exe is null)
        {
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.ExternalToolFailed,
                $"HxD not found (looked in {DefaultHxdPath}; set IL2CPPSTUDIO_HXD to override).", new() { File = target.File });
            return false;
        }
        try
        {
            Process.Start(new ProcessStartInfo(exe) { ArgumentList = { target.File }, UseShellExecute = false });
            sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExternalToolLaunched, target.Instructions,
                new() { File = target.File, Rva = target.Rva, FileOffset = target.FileOffset });
            return true;
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.ExternalToolFailed, $"Starting HxD failed: {ex.Message}", new() { File = exe }, ex);
            return false;
        }
    }
}

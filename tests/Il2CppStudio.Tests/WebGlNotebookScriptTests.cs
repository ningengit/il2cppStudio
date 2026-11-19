using System.Text.RegularExpressions;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

public sealed class WebGlNotebookScriptTests
{
    private static string WebGlTargetOrSkip()
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", "Template il2cpp webgl");
        if (path is null || !Directory.Exists(path))
            Assert.Skip("WebGL test target 'testdata/Template il2cpp webgl' not found.");
        return path!;
    }

    [Fact]
    public void WebGl_NotebookScript_DecompilesCleanly()
    {
        var dir = WebGlTargetOrSkip();
        using var ws = Il2CppWorkspace.Load(dir, NullDiagnosticSink.Instance, new WorkspaceOptions { SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        var notebookType = ws.Model.Types.FirstOrDefault(t => t.FullName == "NotebookScript");
        Assert.NotNull(notebookType);

        var cs = ws.Decompiler.DecompileType(notebookType!);

        Assert.DoesNotContain("(Transform)(object)", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("(GameObject)(object)", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("(Camera)(object)", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("NativeIntrinsics.Asm", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("NativeIntrinsics.Reinterpret", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("Math.Sqrt", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("transform.position = default(Vector3)", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("Raycast(default(Ray)", cs, StringComparison.Ordinal);
        Assert.Contains("Physics.Raycast(ray", cs, StringComparison.Ordinal);

        Assert.Contains("Vector3.Distance", cs, StringComparison.Ordinal);
        Assert.Contains("playerPosition = player.position", cs, StringComparison.Ordinal);
        Assert.DoesNotMatch(new Regex(@"(?m)^\s*(?:\(\(GameObject\)\(object\)[^;]+|\w+)\.GetComponent<MathGameScript>\(\);"), cs);
    }
}

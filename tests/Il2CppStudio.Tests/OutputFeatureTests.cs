using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>Inline-copy matching, dnSpy-style annotations, fallback explanations and the error log.</summary>
[Collection(TemplateBuildCollection.Name)]
public sealed class OutputFeatureTests(TemplateBuildFixture fx)
{
    private string Render(string type, string? method)
    {
        var ws = fx.Workspace;
        var t = ws.FindTypes(type).Single();
        return ws.Render(t, method is null ? null : t.Methods.First(m => m.Name == method), CodeView.CSharp);
    }

    [Fact]
    public void InlineCopy_RoundToIntResolvedFromPdb()
    {
        var cs = Render("AmbienceScript", "PlayAudio");
        Assert.Contains("Mathf.RoundToInt(", cs);
        Assert.DoesNotContain("sub_23E110", cs);
    }

    [Fact]
    public void InlineCopy_ExactFingerprintMatchesVectorDistance()
    {
        var ws = fx.Workspace;
        _ = ws.RuntimeHelpers;   // runs matching
        Assert.Contains(ws.InlineCopies, m => m.Original.DisplayName == "UnityEngine.Vector3::Distance");
        Assert.Contains(ws.InlineCopies, m => m.Original.DisplayName == "UnityEngine.Vector3::get_magnitude");
    }

    [Fact]
    public void Annotations_ShowOriginalTokensRvaAndFieldOffsets()
    {
        var cs = Render("AlarmClockScript", null);
        Assert.Contains("// Token: 0x02000045 RID: 69", cs);
        Assert.Contains("// Token: 0x04000212 RID: 530 Offset: 0x20", cs);
        Assert.Contains("// Token: 0x0600015A RID: 346 RVA: 0x002527A0 File Offset: 0x", cs);
    }

    [Fact]
    public void Fallback_ThrowNullIsExplained()
    {
        var cs = Render("Singleton`1", "get_Instance");
        Assert.Contains("throw null;", cs);
        Assert.Contains("Il2CppStudio Decompilation Fallback: throw null; emitted because", cs);
    }

    [Fact]
    public void Scrubber_MovesIlWarningsToErrorLog()
    {
        var log = new DecompilationErrorLog();
        const string code = "// Token: 0x06000001 RID: 1 RVA: 0x00001000 File Offset: 0x00000400\nvoid M()\n{\n\t//IL_007e: Expected I, but got I8\n\tx = 1;\n}\n";
        var clean = OutputScrubber.Scrub(code, "Test.dll", log);
        Assert.DoesNotContain("IL_007e", clean);
        var entry = Assert.Single(log.EntriesFor("Test.dll"));
        Assert.Equal("void M()", entry.Member);
        Assert.Equal("0x00001000", entry.Rva);
        Assert.Contains("Expected I, but got I8", entry.Message);
    }
}

using Il2CppStudio.Core.Fingerprinting;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>Readability transforms, SIMD/constant lifting, toolchain detection and the C/C++ type view.</summary>
[Collection(TemplateBuildCollection.Name)]
public sealed class ReadabilityTests(TemplateBuildFixture fx)
{
    private string Render(string type, string? method = null, CodeView view = CodeView.CSharp)
    {
        var ws = fx.Workspace;
        var t = ws.FindTypes(type).Single();
        return ws.Render(t, method is null ? null : t.Methods.First(m => m.Name == method), view);
    }

    [Fact]
    public void StringEquality_And_BoolNormalization()
    {
        Assert.Contains("!audioDevice.isPlaying", Render("AmbienceScript"));
        Assert.DoesNotContain("? 1u : 0u", Render("AmbienceScript"));
        Assert.Contains("hitInfo.transform.tag == \"Player\"", Render("FirstPrizeScript"));
    }

    [Fact]
    public void Simd_VectorCollapse_And_NamedConstants()
    {
        var cam = Render("CameraScript");
        Assert.Contains("offset = new Vector3(position.x - position2.x", cam);
        Assert.DoesNotContain("unpcklps", cam);
        Assert.Contains("RenderSettings.ambientLight = Color.red;", Render("NearExitTriggerScript"));
        Assert.Contains("itemSelect.anchoredPosition = new Vector2(", Render("GameControllerScript"));
    }

    [Fact]
    public void StaticFields_StringEmpty_And_NoRawGlobals()
    {
        var chat = Render("ChatController");
        Assert.Contains("string.Empty", chat);
        Assert.DoesNotContain("static_fields", chat);
        Assert.DoesNotContain("global_", Render("PlaytimeScript"));
    }

    [Fact]
    public void VtableTailCall_IsLifted()
    {
        var cs = Render("UIController", "SelectDummy");
        Assert.DoesNotContain("throw null", cs);
    }

    [Fact]
    public void ArrayStoreCheck_IsNotANativeCall()
    {
        var cs = Render("EndlessTextScript");
        Assert.DoesNotContain("CallNative", cs);
        Assert.DoesNotContain("!= null)\n\t\t{\n\t\t}", cs.Replace("\r", ""));
    }

    [Fact]
    public void Toolchain_IsMsvcFromRichHeader()
    {
        var tc = fx.Workspace.Toolchain;
        Assert.Equal(NativeCompiler.Msvc, tc.Compiler);
        Assert.Equal("x86-64", tc.Architecture);
    }

    [Fact]
    public void CppTypeView_ShowsHeaderAndImplementation()
    {
        var cpp = Render("DefaultSettingsScript", view: CodeView.Cpp);
        Assert.Contains("DefaultSettingsScript.h", cpp);
        Assert.Contains("DefaultSettingsScript.cpp", cpp);
        Assert.Contains("DefaultSettingsScript_CloseOptions_m", cpp);
    }

    [Fact]
    public void Output_HasOnlyLegalIdentifiers()
    {
        var cs = Render("ControlsMenu");
        Assert.DoesNotContain("<>1__state", cs);
        Assert.DoesNotContain("System_Collections_IEnumerator_Current", cs);
    }
}

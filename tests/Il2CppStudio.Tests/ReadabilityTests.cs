using Il2CppStudio.Core.Diagnostics;
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
        Assert.Contains("offset = transform.position - player.transform.position;", cam);
        Assert.DoesNotContain("unpcklps", cam);
        Assert.Contains("RenderSettings.ambientLight = Color.red;", Render("NearExitTriggerScript"));
        Assert.Contains("itemSelect.anchoredPosition = new Vector2(", Render("GameControllerScript"));
        var click = Render("ClickableTest", "Update");
        Assert.Contains("Vector3.Distance(player.position,", click);
        Assert.DoesNotContain("Math.Sqrt", click);
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

    [Fact]
    public void NoBackup_2018Win_And_2022Win_ProduceCleanMathGameScriptAndInputManager()
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        if (root is null) Assert.Skip("Repository root not found.");

        var dir2018 = Path.Combine(root!, "testdata", "template il2cpp 2018 win");
        if (!Directory.Exists(dir2018)) Assert.Skip($"Test target '{dir2018}' not found.");
        var sink2018 = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
        using (var ws2018 = Il2CppWorkspace.Load(dir2018, sink2018, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true }))
        {
            WorkspaceSetup.Attach(ws2018);
            var math2018Cs = ws2018.Render(Assert.Single(ws2018.FindTypes("MathGameScript")), null, CodeView.CSharp);
            Assert.Contains("Input.GetKeyDown(\"return\")", math2018Cs);
            Assert.DoesNotContain("GetKeyDownString", math2018Cs);

            // Ensure MemoryResolver handles nested value-type sub-fields without recursion/stack overflow.
            var skewCs = ws2018.Render(Assert.Single(ws2018.FindTypes("SkewTextExample")), null, CodeView.CSharp);
            Assert.Contains("class SkewTextExample", skewCs);
        }

        var dir2022 = Path.Combine(root!, "testdata", "template il2cpp 2022 win");
        if (!Directory.Exists(dir2022)) Assert.Skip($"Test target '{dir2022}' not found.");
        var sink2022 = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
        using (var ws2022 = Il2CppWorkspace.Load(dir2022, sink2022, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true }))
        {
            WorkspaceSetup.Attach(ws2022);
            var agent = Assert.Single(ws2022.FindTypes("AgentTest"));
            var startCs = ws2022.Render(agent, agent.Methods.First(m => m.Name == "Start"), CodeView.CSharp);
            Assert.Contains("agent = GetComponent<NavMeshAgent>();", startCs);
            Assert.DoesNotContain("CallNative", startCs);

            var mathCs = ws2022.Render(Assert.Single(ws2022.FindTypes("MathGameScript")), null, CodeView.CSharp);
            Assert.DoesNotContain("sub_1A4110", mathCs);
            Assert.DoesNotContain("sub_1A4D80", mathCs);
        }
    }

    [Fact]
    public void NoBackup_AndroidArm64_ElidesCovarianceAndColdThrowStubs()
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        if (root is null) Assert.Skip("Repository root not found.");

        var apk = Path.Combine(root!, "testdata", "Template il2cpp android", "Baldi's Basics Classic.arm64-v8a.apk");
        if (!File.Exists(apk)) Assert.Skip($"Test APK not found at '{apk}'.");

        var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
        using var ws = Il2CppWorkspace.Load(apk, sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        var mathCs = ws.Render(Assert.Single(ws.FindTypes("MathGameScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_E6F7FC", mathCs);

        var bullyCs = ws.Render(Assert.Single(ws.FindTypes("BullyScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_E6F914", bullyCs);

        // Call return values spilled to stack before ToString / Box / float getters
        var scoreCs = ws.Render(Assert.Single(ws.FindTypes("ScoreScript")), null, CodeView.CSharp);
        Assert.Contains("PlayerPrefs.GetInt(\"CurrentBooks\")", scoreCs);
        Assert.DoesNotContain("default(int)", scoreCs);

        var endlessTextCs = ws.Render(Assert.Single(ws.FindTypes("EndlessTextScript")), null, CodeView.CSharp);
        Assert.Contains("PlayerPrefs.GetInt(\"HighBooks\")", endlessTextCs);
        Assert.DoesNotContain("default(int)", endlessTextCs);

        var detentionCs = ws.Render(Assert.Single(ws.FindTypes("DetentionTextScript")), null, CodeView.CSharp);
        Assert.Contains("Math.Ceiling", detentionCs);
        Assert.DoesNotContain("default(int)", detentionCs);

        var mouseSliderCs = ws.Render(Assert.Single(ws.FindTypes("MouseSliderScript")), null, CodeView.CSharp);
        Assert.Contains("slider.value = PlayerPrefs.GetFloat(\"MouseSensitivity\")", mouseSliderCs);

        // Merge points (csel, external/virtual tail calls, converged field pointers)
        var moveCs = ws.Render(Assert.Single(ws.FindTypes("PlayerMovement")), null, CodeView.CSharp);
        Assert.Contains("runSpeed : walkSpeed", moveCs);
        Assert.DoesNotContain("= 0L;", moveCs);

        var camCs = ws.Render(Assert.Single(ws.FindTypes("CameraScript")), null, CodeView.CSharp);
        Assert.Contains(".rotation = new Quaternion(", camCs);

        var uiSettingsCs = ws.Render(Assert.Single(ws.FindTypes("UiSettings")), null, CodeView.CSharp);
        Assert.Contains("PlayerPrefs.SetInt(\"UiHeight\", value);", uiSettingsCs);
        Assert.DoesNotContain("PlayerPrefs.SetInt(\"UiHeight\", default(int))", uiSettingsCs);

        var startBtnCs = ws.Render(Assert.Single(ws.FindTypes("StartButton")), null, CodeView.CSharp);
        Assert.Contains("\"endless\"", startBtnCs);
        Assert.Contains("\"story\"", startBtnCs);

        var principalCs = ws.Render(Assert.Single(ws.FindTypes("PrincipalScript")), null, CodeView.CSharp);
        Assert.Contains("audNoFaculty", principalCs);
        Assert.Contains("audNoRunning", principalCs);
        Assert.Contains("audNoDrinking", principalCs);
        Assert.Contains("audNoEscaping", principalCs);

        // Large struct returns/args (Ray) & array indexing / loop bounds / packed stores / base calls
        var notebookCs = ws.Render(Assert.Single(ws.FindTypes("NotebookScript")), null, CodeView.CSharp);
        Assert.Contains("ScreenPointToRay", notebookCs);
        Assert.Contains("Physics.Raycast(ray, out hitInfo)", notebookCs);
        Assert.DoesNotContain("default(Ray)", notebookCs);

        var uiMgrCs = ws.Render(Assert.Single(ws.FindTypes("UiManager")), null, CodeView.CSharp);
        Assert.Contains("array[num4]", uiMgrCs);
        Assert.Contains("while (num4 < array.Length)", uiMgrCs);
        Assert.DoesNotContain("/ 8", uiMgrCs);
        Assert.DoesNotContain("num4 + 1", uiMgrCs);

        var alarmCs = ws.Render(Assert.Single(ws.FindTypes("AlarmClockScript")), null, CodeView.CSharp);
        Assert.Contains("timeLeft = 30f;", alarmCs);
        Assert.Contains("lifeSpan = 35f;", alarmCs);

        var inputMgrCs = ws.Render(Assert.Single(ws.FindTypes("InputManager")), null, CodeView.CSharp);
        Assert.Contains("base.Awake();", inputMgrCs);
        Assert.Contains("while (num != 13)", inputMgrCs);
        Assert.DoesNotContain("/ 8", inputMgrCs);

        // Coroutine factory, IEnumerator.Reset metadata-init recognition, 16-byte SIMD field init
        var defSettingsCs = ws.Render(Assert.Single(ws.FindTypes("DefaultSettingsScript")), null, CodeView.CSharp);
        Assert.Contains("new _CloseOptions_d__1(0)", defSettingsCs);
        Assert.Contains("throw new NotSupportedException();", defSettingsCs);
        Assert.DoesNotContain("throw null;", defSettingsCs);
        Assert.DoesNotContain("sub_E6F90C", defSettingsCs);

        var dpScalerCs = ws.Render(Assert.Single(ws.FindTypes("DpCanvasScaler")), null, CodeView.CSharp);
        Assert.Contains("100f", dpScalerCs);
        Assert.Contains("96f", dpScalerCs);
        Assert.DoesNotContain("m_ReferencePixelsPerUnit = 0L;", dpScalerCs);
    }
}

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
        Assert.Contains("referenceResolution", uiMgrCs);
        Assert.Contains("array[num5]", uiMgrCs);
        Assert.Contains("< array.Length", uiMgrCs);
        Assert.DoesNotContain("/ 8", uiMgrCs);
        Assert.DoesNotContain("num5 + 1", uiMgrCs);

        var alarmCs = ws.Render(Assert.Single(ws.FindTypes("AlarmClockScript")), null, CodeView.CSharp);
        Assert.Contains("timeLeft = 30f;", alarmCs);
        Assert.Contains("lifeSpan = 35f;", alarmCs);

        var inputMgrCs = ws.Render(Assert.Single(ws.FindTypes("InputManager")), null, CodeView.CSharp);
        Assert.Contains("base.Awake();", inputMgrCs);
        Assert.Contains("!= 13", inputMgrCs);
        Assert.DoesNotContain("/ 8", inputMgrCs);

        // Coroutine factory reconstruction (state machine class hidden), 16-byte SIMD field init
        var defSettingsCs = ws.Render(Assert.Single(ws.FindTypes("DefaultSettingsScript")), null, CodeView.CSharp);
        Assert.Contains("coroutine reconstructed from <CloseOptions>d__1.MoveNext", defSettingsCs);
        Assert.Contains("yield return new WaitForEndOfFrame();", defSettingsCs);
        Assert.DoesNotContain("_CloseOptions_d__1", defSettingsCs);
        Assert.DoesNotContain("throw null;", defSettingsCs);
        Assert.DoesNotContain("sub_E6F90C", defSettingsCs);

        var dpScalerCs = ws.Render(Assert.Single(ws.FindTypes("DpCanvasScaler")), null, CodeView.CSharp);
        Assert.Contains("100f", dpScalerCs);
        Assert.Contains("96f", dpScalerCs);
        Assert.DoesNotContain("m_ReferencePixelsPerUnit = 0L;", dpScalerCs);

        // Inlined IEEE banker's rounding (Mathf.RoundToInt) + dead label/temp pruning
        var aiLocCs = ws.Render(Assert.Single(ws.FindTypes("AILocationSelectorScript")), null, CodeView.CSharp);
        Assert.Contains("id = Mathf.RoundToInt(", aiLocCs);
        Assert.DoesNotContain("((Mathf.RoundToInt(", aiLocCs);
        Assert.DoesNotContain("goto IL_", aiLocCs);
        Assert.DoesNotContain("Math.Truncate", aiLocCs);

        var ambienceCs = ws.Render(Assert.Single(ws.FindTypes("AmbienceScript")), null, CodeView.CSharp);
        Assert.Contains("Mathf.RoundToInt(", ambienceCs);
        Assert.DoesNotContain("goto IL_", ambienceCs);
        Assert.DoesNotContain("double num6 = default(double);", ambienceCs);

        var debugFirstCs = ws.Render(Assert.Single(ws.FindTypes("DebugFirstPrizeScript")), null, CodeView.CSharp);
        Assert.Contains("Mathf.RoundToInt(", debugFirstCs);
        Assert.DoesNotContain("goto IL_", debugFirstCs);
        Assert.DoesNotContain("double num5 = default(double);", debugFirstCs);

        Assert.Contains("Mathf.RoundToInt(", bullyCs);
        Assert.DoesNotContain("9218868437227405312L", bullyCs);

        // Compile-breaking AST fixes (--16, undeclared locals after Mathf.RoundToInt collapse)
        var firstSpriteCs = ws.Render(Assert.Single(ws.FindTypes("FirstPrizeSpriteScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("--16", firstSpriteCs);

        var firstPrizeCs = ws.Render(Assert.Single(ws.FindTypes("FirstPrizeScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("float value = 5f;", firstPrizeCs);
        Assert.Contains("double num3 = default(double);", firstPrizeCs);

        Assert.DoesNotContain("sub_21703EC", mathCs);
        Assert.Contains("questionText3.text =", mathCs);
        Assert.Contains("\")+\"", mathCs);
        Assert.Contains("\"X\"", mathCs);
        Assert.DoesNotContain("~value", mathCs);
        Assert.Contains("audioQueue[audioInQueue] = sound;", mathCs);
        Assert.Contains("audioQueue[num3 / 8 - 4] = audioQueue[", mathCs);

        // Tail-calls & ARM64 jump tables (switch statements)
        var audioQCs = ws.Render(Assert.Single(ws.FindTypes("AudioQueueScript")), null, CodeView.CSharp);
        Assert.Contains("PlayQueue();", audioQCs);
        Assert.Contains("audioQueue[audioInQueue] = sound;", audioQCs);
        Assert.Contains("audioQueue[num3 / 8 - 4] = audioQueue[", audioQCs);

        var gcCs = ws.Render(Assert.Single(ws.FindTypes("GameControllerScript")), null, CodeView.CSharp);
        Assert.Contains("switch (item[itemSelected] - 1)", gcCs);
        Assert.Contains("SetActive(value: true)", gcCs);
        Assert.Contains("item[itemSelected] = 0;", gcCs);
        Assert.Contains("item[id] = 0;", gcCs);
        Assert.Contains("if (exitsReached == 1)", gcCs);
        Assert.DoesNotContain("if (exitsReached == 0)", gcCs);

        Assert.Contains("case 0:", uiSettingsCs);
        Assert.Contains("toggle = sAuto;", uiSettingsCs);
        Assert.Contains("toggle = sSmall;", uiSettingsCs);

        // Array element stores & field swap coherence
        Assert.Contains("currentKeyStates[(int)num] =", inputMgrCs);
        Assert.DoesNotContain("previousKeyStates = currentKeyStates;\r\n\t\tcurrentKeyStates = previousKeyStates;", inputMgrCs);
        Assert.DoesNotContain("previousKeyStates = currentKeyStates;\n\t\tcurrentKeyStates = previousKeyStates;", inputMgrCs);

        // Packed 16-bit strh bool stores
        var jumpRopeCs = ws.Render(Assert.Single(ws.FindTypes("JumpRopeScript")), null, CodeView.CSharp);
        Assert.Contains("ropeHit = true;", jumpRopeCs);
        Assert.Contains("jumpStarted = false;", jumpRopeCs);

        Assert.Contains("active = false;", bullyCs);
        Assert.Contains("spoken = false;", bullyCs);

        var playerCs = ws.Render(Assert.Single(ws.FindTypes("PlayerScript")), null, CodeView.CSharp);
        Assert.Contains("hugging = false;", playerCs);
        Assert.Contains("\"1st Prize\"", playerCs);

        // ORN string == + distance conditions & PrincipalScript compound field condition
        var pickupCs = ws.Render(Assert.Single(ws.FindTypes("PickupScript")), null, CodeView.CSharp);
        Assert.Contains("\"Pickup_EnergyFlavoredZestyBar\"", pickupCs);
        Assert.Contains("\"Pickup_BSODA\"", pickupCs);

        Assert.Contains("== \"Notebook\"", notebookCs);
        var endlessNbCs = ws.Render(Assert.Single(ws.FindTypes("EndlessNotebookScript")), null, CodeView.CSharp);
        Assert.Contains("== \"Notebook\"", endlessNbCs);
        var clickableCs = ws.Render(Assert.Single(ws.FindTypes("ClickableTest")), null, CodeView.CSharp);
        Assert.Contains("== \"Notebook\"", clickableCs);

        Assert.DoesNotContain("timeSeenRuleBreak + deltaTime >= 0.5f", principalCs);
        Assert.Contains("timeSeenRuleBreak >= 0.5f", principalCs);

        // Group 1: String/constant merge points, .rodata switch tables, bit-reinterpreted floats, ref deref
        var controlsCs = ws.Render(Assert.Single(ws.FindTypes("ControlsMenu")), null, CodeView.CSharp);
        Assert.Contains("result = \"ESC\";", controlsCs);
        Assert.Contains("result = \"Middle Mouse Button\";", controlsCs);
        Assert.Contains("inputMan.KeyboardMapping", controlsCs);

        Assert.Contains("case InputAction.MoveLeft:", inputMgrCs);
        Assert.Contains("case InputAction.PauseOrCancel:", inputMgrCs);
        Assert.Contains("return action.ToString();", inputMgrCs);
        Assert.Contains("KeyboardMapping[(InputAction)num] = defaultBinding;", inputMgrCs);
        Assert.Contains("result = \"Left Mouse Button\";", inputMgrCs);

        Assert.Contains("\"/7 Notebooks\"", gcCs);
        Assert.Contains("\" Notebooks\"", gcCs);
        Assert.Contains("\"BSODAMachine\"", gcCs);
        Assert.Contains("\"ZestyMachine\"", gcCs);
        Assert.Contains("\"PayPhone\"", gcCs);
        Assert.Contains("\"1st Prize\"", gcCs);

        var gameOverCs = ws.Render(Assert.Single(ws.FindTypes("GameOverScript")), null, CodeView.CSharp);
        Assert.Contains("sprite = images[(int)num3];", gameOverCs);
        Assert.DoesNotContain("rare = ref", gameOverCs);

        Assert.Contains("640f", uiMgrCs);
        Assert.Contains("480f", uiMgrCs);
        Assert.Contains("800f", uiMgrCs);
        Assert.Contains("600f", uiMgrCs);
        Assert.Contains("900f", uiMgrCs);
        Assert.Contains("720f", uiMgrCs);
        Assert.Contains("1024f", uiMgrCs);
        Assert.DoesNotContain("NativeIntrinsics.Reinterpret<float>", uiMgrCs);

        // Group 2: Vector3/NEON, FMAXNM, coroutine state preservation, z component preservation
        var baldiCs = ws.Render(Assert.Single(ws.FindTypes("BaldiScript")), null, CodeView.CSharp);
        Assert.Contains("position.z", baldiCs);
        var craftersCs = ws.Render(Assert.Single(ws.FindTypes("CraftersScript")), null, CodeView.CSharp);
        Assert.Contains("position.z", craftersCs);
        Assert.Contains("Math.Max(stamina - staminaDrop", moveCs);
        Assert.Contains("Vector3.up", playerCs);
        Assert.Contains("yield return null;", gcCs);
        Assert.Contains("coroutine reconstructed from <BootAnimation>d__19.MoveNext", gcCs);
        Assert.DoesNotContain("_BootAnimation_d__19", gcCs);
        Assert.Contains("yield break;", gcCs);

        Assert.Contains("coroutine reconstructed from <CheatText>d__11.MoveNext", mathCs);
        Assert.Contains("yield return new WaitForEndOfFrame();", mathCs);
        Assert.DoesNotContain("_CheatText_d__11", mathCs);

        Assert.Contains("coroutine reconstructed from <RebindCoroutine>d__15.MoveNext", controlsCs);
        Assert.DoesNotContain("_RebindCoroutine_d__15", controlsCs);

        Assert.Contains("Quaternion.Euler(", playerCs);
        Assert.Contains("eulerAngles", playerCs);
        Assert.DoesNotContain("0.017453292f", playerCs);

        Assert.Contains("ref RaycastHit reference = ref hitInfo2;", gcCs);
        Assert.Contains("reference = ref hitInfo2;", gcCs);
        Assert.DoesNotContain("m_FaceID = 0", gcCs);

        var envMapCs = ws.Render(Assert.Single(ws.FindTypes("EnvMapAnimator")), null, CodeView.CSharp);
        Assert.Contains("Vector3.one", envMapCs);

        // Group 3: Struct/enumerator recovery, interface diamonds, HasFieldRva / InitializeArray
        Assert.Contains("KeyboardMapping.GetEnumerator()", inputMgrCs);
        Assert.Contains("enumerator.MoveNext()", inputMgrCs);
        Assert.DoesNotContain("enumerator2 = enumerator;", inputMgrCs);
        Assert.Contains("enumerator.Current", inputMgrCs);
        Assert.Contains("enumerator.Current.Key", inputMgrCs);
        Assert.DoesNotContain("current.Value.secondaryKey =", inputMgrCs);
        Assert.Contains("new List<KeyCode>(simulatedKeys.Keys).GetEnumerator()", inputMgrCs);

        var debugCs = ws.Render(Assert.Single(ws.FindTypes("DebugScript")), null, CodeView.CSharp);
        Assert.Contains("enumerator.MoveNext()", debugCs);
        Assert.Contains("(KeyCode)enumerator.Current", debugCs);
        Assert.DoesNotContain("sub_E6F804", debugCs);
        Assert.Contains("Dispose();", debugCs);
        Assert.DoesNotContain("arm64_indirect", debugCs);

        Assert.Contains("15, 30, 45, 60, 99", principalCs);
        Assert.DoesNotContain("_outSlot", principalCs);
        var monoScriptCs = ws.FindTypes("UnitySourceGeneratedAssemblyMonoScriptTypes_v1")
            .Select(t => ws.Render(t, null, CodeView.CSharp))
            .First(s => s.Contains("new byte[6521]"));
        Assert.Contains("new byte[6521]", monoScriptCs);
        Assert.DoesNotContain("LdMemberToken", monoScriptCs);

        // Group 4: Custom attributes, generic constraints, optional parameter defaults
        Assert.Contains("[Header(\"UI Elements\")]", controlsCs);
        Assert.Contains("[SerializeField]", controlsCs);
        Assert.Contains("KeyCode secondaryKey = KeyCode.None", inputMgrCs);
        Assert.Contains("[RequireComponent(typeof(Canvas))]", dpScalerCs);
        Assert.Contains("[ExecuteInEditMode]", dpScalerCs);
        Assert.Contains("[AddComponentMenu(\"Layout/DP Canvas Scaler\")]", dpScalerCs);
        Assert.Contains("[Tooltip(", dpScalerCs);
        var singletonCs = ws.Render(ws.FindTypes("Singleton`1").First(), null, CodeView.CSharp);
        Assert.Contains("where T : Component", singletonCs);
        Assert.Contains("[RuntimeInitializeOnLoadMethod]", singletonCs);

        // Group 5: Precise AccessRelaxer visibility & stripped-const Rad2Deg fallback
        Assert.Contains("private IEnumerator RebindCoroutine(", controlsCs);
        Assert.DoesNotContain("internal IEnumerator RebindCoroutine", controlsCs);
        Assert.Contains("private IEnumerator BootAnimation()", gcCs);
        Assert.Contains("private IEnumerator Teleporter()", gcCs);
        Assert.Contains("private IEnumerator CheatText(", mathCs);
        Assert.Contains("private IEnumerator BootTimer()", playerCs);
        Assert.Contains("private IEnumerator LogDissappear()", debugCs);
        Assert.Contains("private IEnumerator Start()", envMapCs);

        Assert.Contains("protected virtual void Awake()", singletonCs);
        Assert.DoesNotContain("protected internal", singletonCs);
        Assert.Contains("protected override void Awake()", inputMgrCs);
        Assert.DoesNotContain("protected internal", inputMgrCs);

        Assert.Contains("private int audioInQueue;", audioQCs);
        Assert.DoesNotContain("widened because", audioQCs);
        var doorCs = ws.Render(Assert.Single(ws.FindTypes("DoorScript")), null, CodeView.CSharp);
        Assert.Contains("private bool bDoorLocked;", doorCs);
        Assert.DoesNotContain("widened because", doorCs);

        var vertexShakeACs = ws.Render(Assert.Single(ws.FindTypes("VertexShakeA")), null, CodeView.CSharp);
        Assert.Contains("Quaternion.Euler(", vertexShakeACs);
        Assert.DoesNotContain("Mathf.Rad2Deg", vertexShakeACs);
        var vertexShakeBCs = ws.Render(Assert.Single(ws.FindTypes("VertexShakeB")), null, CodeView.CSharp);
        Assert.Contains("Quaternion.Euler(", vertexShakeBCs);
        Assert.DoesNotContain("Mathf.Rad2Deg", vertexShakeBCs);
    }
}

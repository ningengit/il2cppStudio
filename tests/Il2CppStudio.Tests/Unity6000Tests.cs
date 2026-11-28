using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>
/// Regression tests for Unity 6000.3 (metadata v39) and Unity 6000.6 (metadata v108) across
/// Win64, Win32, and Win ARM64 builds of baldi_s_basics_classic_143_decompile_v5.
/// </summary>
public sealed class Unity6000Tests
{
    private static DiagnosticCollector Sink() => new(new Serilog.LoggerConfiguration().CreateLogger());

    private static string Target(string folder)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", folder);
        if (path is null || !Directory.Exists(path))
            Assert.Skip($"Test target '{folder}' not found.");
        return path!;
    }

    [Theory]
    [InlineData(39, 380, true, false, true)]
    [InlineData(108, 500, true, true, true)]
    public void VersionFlagsAndHeaderSize_MatchUnity6000Abi(
        int major,
        int expectedHeaderSize,
        bool expectV39Compact,
        bool expectV108Embedded,
        bool expectModuleToken)
    {
        var v = new MetadataVersion(major);
        Assert.True(v.IsSupported);
        Assert.True(v.HasSectionCounts);
        Assert.Equal(expectV39Compact, v.HasCompactIndices);
        Assert.Equal(expectV39Compact, v.HasV39StringLiterals);
        Assert.False(v.HasElementTypeIndex);
        Assert.Equal(expectModuleToken, v.HasAssemblyModuleToken);
        Assert.Equal(expectV108Embedded, v.HasV108EmbeddedGenerics);
        Assert.Equal(expectedHeaderSize, MetadataHeader.SizeFor(v));
    }

    [Theory]
    [InlineData("template il2cpp 6000.3 win64", 39, CpuArchitecture.X64, "6000.3.29f1")]
    [InlineData("template il2cpp 6000.3 win32", 39, CpuArchitecture.X86, "6000.3.29f1")]
    [InlineData("template il2cpp 6000.3 win arm64", 39, CpuArchitecture.Arm64, "6000.3.29f1")]
    [InlineData("template il2cpp 6000.6 win64", 108, CpuArchitecture.X64, "6000.6.10f1")]
    [InlineData("template il2cpp 6000.6 win32", 108, CpuArchitecture.X86, "6000.6.10f1")]
    [InlineData("template il2cpp 6000.6 win arm64", 108, CpuArchitecture.Arm64, "6000.6.10f1")]
    public void Unity6000_Build_LoadsCompleteRegistrationAndDecompilesGroundTruthScripts(
        string folder,
        int expectedMetadataMajor,
        CpuArchitecture expectedArch,
        string expectedUnityVersion)
    {
        var dir = Target(folder);
        var sink = Sink();
        using var ws = Il2CppWorkspace.Load(dir, sink);
        WorkspaceSetup.Attach(ws);

        Assert.Equal(expectedMetadataMajor, ws.Metadata.Version.Major);
        Assert.NotNull(ws.Binary);
        Assert.Equal(expectedArch, ws.Binary!.Architecture);
        Assert.StartsWith(expectedUnityVersion, ws.Environment.UnityVersion, StringComparison.Ordinal);
        Assert.All(ws.Metadata.SectionReports, r => { Assert.True(r.InBounds); Assert.True(r.Aligned); });

        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete, string.Join(" | ", sink.Query(DiagnosticSeverity.Warning).Select(e => e.Message)));
        Assert.NotNull(ws.AddressMap);
        Assert.True(ws.AddressMap!.ResolvedMethodCount > 50000);
        Assert.Empty(sink.Query(DiagnosticSeverity.Error));

        // Verify v108 metadata-embedded generics & v39/v108 compact indices populated cleanly.
        Assert.True(ws.Registration.TypeCount > 25000);
        Assert.True(ws.Registration.MethodSpecs.Count > 75000);
        Assert.True(ws.Registration.GenericMethodTable.Count > 50000);
        Assert.True(ws.Registration.MetadataUsages.Count > 25000);

        // Ground-truth scripts from baldi_s_basics_classic_143_decompile_v5.
        var playerType = Assert.Single(ws.FindTypes("PlayerScript"));
        Assert.All(playerType.Methods, m => Assert.NotEqual(0UL, ws.GetMethodRva(m)));
        var playerCode = ws.Render(playerType, null, CodeView.CSharp);
        Assert.Contains("class PlayerScript", playerCode, StringComparison.Ordinal);
        Assert.Contains("playerRotation", playerCode, StringComparison.Ordinal);
        Assert.Contains("BootTimer", playerCode, StringComparison.Ordinal);

        var baldiType = Assert.Single(ws.FindTypes("BaldiScript"));
        var baldiCode = ws.Render(baldiType, null, CodeView.CSharp);
        Assert.Contains("class BaldiScript", baldiCode, StringComparison.Ordinal);
        Assert.Contains("baldiAnger", baldiCode, StringComparison.Ordinal);

        var gcType = Assert.Single(ws.FindTypes("GameControllerScript"));
        var gcCode = ws.Render(gcType, null, CodeView.CSharp);
        Assert.Contains("class GameControllerScript", gcCode, StringComparison.Ordinal);
        Assert.Contains("CurrentMode", gcCode, StringComparison.Ordinal);
        Assert.Contains("endless", gcCode, StringComparison.Ordinal);
    }

    [Fact]
    public void Unity6000_6_NoBackup_ResolvesV108VTableAndArm64Helpers()
    {
        var win64Dir = Target("template il2cpp 6000.6 win64");
        var root = TemplateBuildFixture.RepositoryRoot!;
        var apkPath = Path.Combine(root, "testdata", "template il2cpp 6000.6 android", "android baldi build.apk");
        if (!File.Exists(apkPath))
            Assert.Skip($"Test APK not found at '{apkPath}'.");
        var winArm64Dir = Target("template il2cpp 6000.6 win arm64");

        foreach (var path in new[] { win64Dir, apkPath })
        {
            var sink = Sink();
            using var ws = Il2CppWorkspace.Load(path, sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
            WorkspaceSetup.Attach(ws);

            var mathType = Assert.Single(ws.FindTypes("MathGameScript"));
            var mathCode = ws.Render(mathType, null, CodeView.CSharp);
            Assert.Contains("questionText.text = text;", mathCode, StringComparison.Ordinal);
            Assert.Contains("questionText2.text = string.Empty;", mathCode, StringComparison.Ordinal);
            Assert.DoesNotContain("indirect@", mathCode, StringComparison.Ordinal);
            Assert.DoesNotContain("arm64_indirect@", mathCode, StringComparison.Ordinal);
            Assert.DoesNotContain("sub_137860", mathCode, StringComparison.Ordinal);
            Assert.DoesNotContain("sub_E88A58", mathCode, StringComparison.Ordinal);

            var agentType = Assert.Single(ws.FindTypes("AgentTest"));
            var startMethod = agentType.Methods.First(m => m.Name == "Start");
            var startCode = ws.Render(agentType, startMethod, CodeView.CSharp);
            Assert.Contains("agent = GetComponent<NavMeshAgent>();", startCode, StringComparison.Ordinal);
            Assert.DoesNotContain("CallNative", startCode, StringComparison.Ordinal);
        }

        var arm64Sink = Sink();
        using var arm64Ws = Il2CppWorkspace.Load(winArm64Dir, arm64Sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(arm64Ws);
        var arm64Math = arm64Ws.Render(Assert.Single(arm64Ws.FindTypes("MathGameScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_3120", arm64Math, StringComparison.Ordinal);
    }

    [Fact]
    public void IosIpa_MetadataOnlyIosAndUnity6000_5_V106_LoadCleanlyWithoutStubErrors()
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var ipaPath = root is null ? null : Path.Combine(root, "testdata", "sample game unity ios.ipa");
        if (ipaPath is not null && File.Exists(ipaPath))
        {
            var ipaSink = Sink();
            using var ipaWs = Il2CppWorkspace.Load(ipaPath, ipaSink);
            WorkspaceSetup.Attach(ipaWs);
            Assert.Equal("6000.0.58f2", ipaWs.Environment.UnityVersion);
            Assert.True(ipaWs.Registration?.IsComplete);
            Assert.Empty(ipaSink.Query(DiagnosticSeverity.Warning));
            Assert.Empty(ipaSink.Query(DiagnosticSeverity.Error));
        }

        var iosDir = Target("template il2cpp ios");
        var iosSink = Sink();
        using (var iosWs = Il2CppWorkspace.Load(iosDir, iosSink))
        {
            var acsIdx = iosWs.Model.Assemblies.ToList().FindIndex(a => a.Name == "Assembly-CSharp");
            Assert.True(acsIdx >= 0);
            var stubBytes = iosWs.Stubs.Write(iosWs.Stubs.GetModule(acsIdx));
            Assert.NotNull(stubBytes);
            Assert.NotEmpty(stubBytes!);
            Assert.Empty(iosSink.Query(DiagnosticSeverity.Error));
        }

        var v106Dir = Target(Path.Combine("Cpp2IL-development", "Cpp2IL-development", "TestFiles", "Simple_6000_5_0_a6"));
        var v106Sink = Sink();
        using (var v106Ws = Il2CppWorkspace.Load(v106Dir, v106Sink))
        {
            Assert.Equal(106, v106Ws.Metadata.Version.Major);
            Assert.All(v106Ws.Metadata.SectionReports, r => { Assert.True(r.InBounds); Assert.True(r.Aligned); });
            Assert.True(v106Ws.Registration?.IsComplete);
            Assert.Empty(v106Sink.Query(DiagnosticSeverity.Error));
        }
    }

    [Fact]
    public void Unity6000_3_Win64_PackedSse_Box_Setters_Crt_And_DefaultPrivateVisibility()
    {
        var dir = Target("template il2cpp 6000.3 win64");
        var sink = Sink();
        using var ws = Il2CppWorkspace.Load(dir, sink, new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        // 1. Packed-single SSE (addps/subps/mulps/shufps) lifts cleanly with no Asm/Reinterpret<float> fallbacks.
        foreach (var typeName in new[] { "BaldiScript", "BsodaSparyScript", "HuggingScript", "Billboard" })
        {
            var t = Assert.Single(ws.FindTypes(typeName));
            var cs = ws.Render(t, null, CodeView.CSharp);
            Assert.DoesNotContain("NativeIntrinsics.Asm(", cs, StringComparison.Ordinal);
            Assert.DoesNotContain("NativeIntrinsics.Reinterpret<float>", cs, StringComparison.Ordinal);
        }

        // Check UnsupportedInstruction error log notes for those methods are zero.
        var logText = string.Join("\n", ws.Decompiler.ErrorLog.EntriesFor("Assembly-CSharp.dll").Select(e => $"{e.Kind}: {e.Member}: {e.Message}"));
        Assert.DoesNotContain("addps", logText, StringComparison.Ordinal);
        Assert.DoesNotContain("subps", logText, StringComparison.Ordinal);
        Assert.DoesNotContain("mulps", logText, StringComparison.Ordinal);
        Assert.DoesNotContain("shufps", logText, StringComparison.Ordinal);

        // 2. CallNative fallbacks resolved: il2cpp_value_box, AudioSource.pitch, sinf, modf, ScopeCleanup.
        var endlessCs = ws.Render(Assert.Single(ws.FindTypes("EndlessTextScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("il2cpp_value_box", endlessCs, StringComparison.Ordinal);

        var controlsCs = ws.Render(Assert.Single(ws.FindTypes("ControlsMenu")), null, CodeView.CSharp);
        Assert.DoesNotContain("il2cpp_value_box", controlsCs, StringComparison.Ordinal);

        var mathCs = ws.Render(Assert.Single(ws.FindTypes("MathGameScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("il2cpp_value_box", mathCs, StringComparison.Ordinal);

        var firstPrizeCs = ws.Render(Assert.Single(ws.FindTypes("FirstPrizeScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("inaccessible:AudioSource.SetPitch", firstPrizeCs, StringComparison.Ordinal);
        Assert.Contains(".pitch =", firstPrizeCs, StringComparison.Ordinal);

        var pickupCs = ws.Render(Assert.Single(ws.FindTypes("PickupAnimationScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_236510", pickupCs, StringComparison.Ordinal);
        Assert.Contains("Math.Sin(", pickupCs, StringComparison.Ordinal);

        var bullyCs = ws.Render(Assert.Single(ws.FindTypes("BullyScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_2358F0", bullyCs, StringComparison.Ordinal);

        var debugCs = ws.Render(Assert.Single(ws.FindTypes("DebugScript")), null, CodeView.CSharp);
        Assert.DoesNotContain("sub_267000", debugCs, StringComparison.Ordinal);
        Assert.DoesNotContain("NativeIntrinsics.Reinterpret<Color>", debugCs, StringComparison.Ordinal);

        var inputMgrCs = ws.Render(Assert.Single(ws.FindTypes("InputManager")), null, CodeView.CSharp);
        Assert.DoesNotContain("il2cpp_value_box", inputMgrCs, StringComparison.Ordinal);

        // 3. Exact original metadata visibility (`private`) preserved by default (no AccessRelaxer widening).
        var baldiCs = ws.Render(Assert.Single(ws.FindTypes("BaldiScript")), null, CodeView.CSharp);
        Assert.Contains("private float currentPriority;", baldiCs, StringComparison.Ordinal);
        Assert.Contains("private NavMeshAgent agent;", baldiCs, StringComparison.Ordinal);
        Assert.DoesNotContain("widened because", baldiCs, StringComparison.Ordinal);

        var doorCs = ws.Render(Assert.Single(ws.FindTypes("DoorScript")), null, CodeView.CSharp);
        Assert.Contains("private bool bDoorLocked;", doorCs, StringComparison.Ordinal);
        Assert.DoesNotContain("widened because", doorCs, StringComparison.Ordinal);

        var craftersCs = ws.Render(Assert.Single(ws.FindTypes("CraftersScript")), null, CodeView.CSharp);
        Assert.Contains("private NavMeshAgent agent;", craftersCs, StringComparison.Ordinal);
        Assert.DoesNotContain("widened because", craftersCs, StringComparison.Ordinal);

        Assert.Contains("private Dictionary<KeyCode, SimulatedKeyState>", inputMgrCs, StringComparison.Ordinal);
        Assert.DoesNotContain("widened because", inputMgrCs, StringComparison.Ordinal);

        // Opt-in RelaxInlinedAccess = true still widens cross-type accessed members when requested.
        using var relaxedWs = Il2CppWorkspace.Load(dir, Sink(), new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true, RelaxInlinedAccess = true });
        WorkspaceSetup.Attach(relaxedWs);
        var relaxedDoorCs = relaxedWs.Render(Assert.Single(relaxedWs.FindTypes("DoorScript")), null, CodeView.CSharp);
        Assert.Contains("internal bool bDoorLocked;", relaxedDoorCs, StringComparison.Ordinal);
        Assert.Contains("widened because", relaxedDoorCs, StringComparison.Ordinal);
    }

    [Fact]
    public void Unity6000_3_Win64_AssemblyCSharp_ZeroErrorLog_ZeroNativeIntrinsics_And_SwitchSbbEnumerators()
    {
        var dir = Target("template il2cpp 6000.3 win64");
        using var ws = Il2CppWorkspace.Load(dir, Sink(), new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        var acs = ws.Model.Assemblies.Single(a => a.Name == "Assembly-CSharp");
        foreach (var type in acs.Namespaces.SelectMany(n => n.Types).Where(t => t.Raw.DeclaringTypeIndex < 0 && !t.Name.StartsWith('<')))
        {
            var cs = ws.Render(type, null, CodeView.CSharp);
            Assert.DoesNotContain("NativeIntrinsics", cs, StringComparison.Ordinal);
        }

        Assert.Empty(ws.Decompiler.ErrorLog.EntriesFor("Assembly-CSharp.dll"));

        var camCs = ws.Render(Assert.Single(ws.FindTypes("CameraScript")), null, CodeView.CSharp);
        Assert.Contains("lookBehind =", camCs, StringComparison.Ordinal);
        Assert.Contains("0xB4", camCs, StringComparison.Ordinal);

        var controlsCs = ws.Render(Assert.Single(ws.FindTypes("ControlsMenu")), null, CodeView.CSharp);
        Assert.Contains("return \"0\";", controlsCs, StringComparison.Ordinal);
        Assert.Contains("return \"9\";", controlsCs, StringComparison.Ordinal);
        Assert.Contains("primaryKey", controlsCs, StringComparison.Ordinal);

        var inputMgrCs = ws.Render(Assert.Single(ws.FindTypes("InputManager")), null, CodeView.CSharp);
        Assert.Contains("return \"0\";", inputMgrCs, StringComparison.Ordinal);
        Assert.Contains("return \"9\";", inputMgrCs, StringComparison.Ordinal);
        Assert.Contains("InputAction.MoveRight", inputMgrCs, StringComparison.Ordinal);
        Assert.Contains("InputAction.Slot0", inputMgrCs, StringComparison.Ordinal);
        Assert.Contains("primaryKey", inputMgrCs, StringComparison.Ordinal);
        Assert.DoesNotContain("unsafe void Save", inputMgrCs, StringComparison.Ordinal);
        Assert.DoesNotContain("unsafe void UpdateSimulatedKeys", inputMgrCs, StringComparison.Ordinal);
        Assert.Contains("enumerator.Current.Value.primaryKey", inputMgrCs, StringComparison.Ordinal);
    }

    [Fact]
    public void Unity6000_3_Win64_GenericDefinitionBodyRecovery_Singleton()
    {
        var dir = Target("template il2cpp 6000.3 win64");
        using var ws = Il2CppWorkspace.Load(dir, Sink(), new WorkspaceOptions { UseBuildBackup = false, SubstituteBcl = true });
        WorkspaceSetup.Attach(ws);

        var acs = ws.Model.Assemblies.Single(a => a.Name == "Assembly-CSharp");
        var singletonType = acs.Namespaces.SelectMany(n => n.Types).Single(t => t.Name == "Singleton`1");
        foreach (var name in new[] { "get_Instance", "Init", "Awake", "OnDestroy", "TryGetInstance", "CreateInstance" })
        {
            var method = singletonType.Methods.Single(m => m.Name == name);
            Assert.NotEqual(0UL, ws.GetMethodRva(method));
        }

        var singletonCs = ws.Render(singletonType, null, CodeView.CSharp);
        Assert.Contains("FindObjectOfType<T>()", singletonCs, StringComparison.Ordinal);
        Assert.Contains("typeof(T).Name", singletonCs, StringComparison.Ordinal);
        Assert.Contains("AddComponent<T>()", singletonCs, StringComparison.Ordinal);
        Assert.Contains("DontDestroyOnLoad", singletonCs, StringComparison.Ordinal);
        Assert.Contains("isApplicationQuitting", singletonCs, StringComparison.Ordinal);
        Assert.Contains("TryGetInstance", singletonCs, StringComparison.Ordinal);
        Assert.Contains("CreateInstance", singletonCs, StringComparison.Ordinal);
        Assert.Contains("OnDestroy", singletonCs, StringComparison.Ordinal);
        Assert.DoesNotContain("NativeIntrinsics", singletonCs, StringComparison.Ordinal);
        Assert.DoesNotContain("throw null;", singletonCs, StringComparison.Ordinal);
    }
}

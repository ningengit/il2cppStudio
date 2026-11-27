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
}

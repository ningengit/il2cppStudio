using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>
/// Cross-platform bring-up: every build in testdata/ must locate and parse its metadata, and native images must
/// load and yield registration data where the format allows. Builds that are absent are skipped, not failed.
/// </summary>
public sealed class PlatformMatrixTests
{
    private static string? Build(string name)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", name);
        if (path is null || !Directory.Exists(path)) Assert.Skip($"Test build '{name}' not found.");
        return path;
    }

    private static DiagnosticCollector Sink() => new(new Serilog.LoggerConfiguration().CreateLogger());

    [Theory]
    [InlineData("template il2cpp 2018 win", 24)]
    [InlineData("template il2cpp 2018 win 32", 24)]
    [InlineData("template il2cpp 2022 win", 31)]
    [InlineData("template il2cpp 6000 win 32", 31)]
    [InlineData("template il2cpp 2022 linux", 31)]
    [InlineData("Template il2cpp android", 31)]
    [InlineData("Template il2cpp webgl", 31)]
    [InlineData("template il2cpp webgl with gzip", 31)]
    [InlineData("template il2cpp ios", 31)]
    public void Metadata_IsFoundAndParsed(string build, int major)
    {
        var sink = Sink();
        var files = GameFolderLocator.Locate(Build(build)!, sink);
        Assert.NotNull(files.MetadataPath);
        using var md = GlobalMetadata.Load(files.MetadataPath!, sink);
        Assert.Equal(major, md.Version.Major);
        var model = MetadataModel.Build(md, sink);
        Assert.Contains(model.Types, t => t.FullName == "BaldiScript" || t.FullName == "AmbienceScript");
        Assert.DoesNotContain(md.SectionReports, r => !r.InBounds || !r.Aligned);
    }

    [Theory]
    [InlineData("template il2cpp 2022 linux", CpuArchitecture.X64)]
    [InlineData("Template il2cpp android", CpuArchitecture.Arm64)]
    public void Elf_LoadsWithRelocationsAndExports(string build, CpuArchitecture arch)
    {
        var sink = Sink();
        var files = GameFolderLocator.Locate(Build(build)!, sink);
        Assert.NotNull(files.BinaryPath);
        using var img = ElfBinaryImage.Load(files.BinaryPath!, sink);
        Assert.Equal(arch, img.Architecture);
        Assert.True(img.Is64Bit);
        Assert.Contains(img.Sections, s => s.IsExecutable);
        Assert.Contains(img.GetSymbols(), s => s.Name == "il2cpp_object_new");
    }

    [Theory]
    [InlineData("template il2cpp 2018 win")]
    [InlineData("template il2cpp 2018 win 32")]
    [InlineData("template il2cpp 6000 win 32")]
    [InlineData("template il2cpp 2022 linux")]
    [InlineData("Template il2cpp android")]
    [InlineData("Template il2cpp android/Baldi's Basics Classic.armeabi-v7a")]
    [InlineData("Template il2cpp webgl")]
    [InlineData("template il2cpp webgl with gzip")]
    public void Workspace_RecoversRegistration(string build)
    {
        var sink = Sink();
        using var ws = Il2CppWorkspace.Load(Build(build)!, sink);
        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete, string.Join(" | ", sink.Query(DiagnosticSeverity.Warning).Select(e => e.Message)));
        var t = ws.FindTypes("AmbienceScript").Single();
        Assert.All(t.Methods, m => Assert.NotEqual(0UL, ws.GetMethodRva(m)));
    }

    /// <summary>Every CPU gets a native view, and calls into managed methods resolve to their names.</summary>
    [Theory]
    [InlineData("template il2cpp 2022 win", "Random::Range")]
    [InlineData("template il2cpp 2018 win 32", "Random::Range")]
    [InlineData("Template il2cpp android", "Random::Range")]
    [InlineData("Template il2cpp android/Baldi's Basics Classic.armeabi-v7a", "Random::Range")]
    [InlineData("Template il2cpp webgl", "Random::Range")]
    public void Disassembly_ResolvesManagedCalls(string build, string expected)
    {
        using var ws = Il2CppWorkspace.Load(Build(build)!, Sink());
        var m = ws.FindTypes("AmbienceScript").Single().Methods.Single(x => x.Name == "PlayAudio");
        // Managed name (Random::Range) or, in Enhanced mode, the IL2CPP C++ symbol (Random_Range_m…).
        Assert.Matches(expected.Replace("::", "(::|_)"), ws.DisassembleMethod(m) ?? "");
    }
}

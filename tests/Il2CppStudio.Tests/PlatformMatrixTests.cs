using System.Buffers.Binary;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Plugins;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>
/// Cross-platform bring-up: every build in testdata/ must locate and parse its metadata, and native images must
/// load and yield registration data where the format allows. Builds that are absent are skipped, not failed.
/// </summary>
public sealed class PlatformMatrixTests
{
    private static string? Build(string name) => Target(name, requireDirectory: true);

    private static string? Target(string name, bool requireDirectory = false)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", name);
        var exists = path is not null && (Directory.Exists(path) || (!requireDirectory && File.Exists(path)));
        if (!exists) Assert.Skip($"Test target '{name}' not found.");
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

    [Theory]
    [InlineData("Template il2cpp android/Baldi's Basics Classic.arm64-v8a.apk", "2022.3.62f3")]
    [InlineData("Template il2cpp webgl", "2022.3.62f3")]
    public void UnityFs_DetectsVersionAcrossPlatforms(string target, string expectedVersion)
    {
        using var ws = Il2CppWorkspace.Load(Target(target)!, Sink());
        Assert.Equal(expectedVersion, ws.Environment.UnityVersion);
        Assert.Equal("data.unity3d", ws.Environment.UnityVersionSource);
    }

    [Fact]
    public void SwitchNsoPlugin_DecompressesLz4AndAppliesMod0Relocations()
    {
        var text = new byte[0x80];
        BinaryPrimitives.WriteUInt32LittleEndian(text.AsSpan(4, 4), 0x10);
        "MOD0"u8.CopyTo(text.AsSpan(0x10, 4));
        BinaryPrimitives.WriteInt32LittleEndian(text.AsSpan(0x14, 4), 0x20);
        BinaryPrimitives.WriteInt64LittleEndian(text.AsSpan(0x30, 8), 7);       // DT_RELA
        BinaryPrimitives.WriteUInt64LittleEndian(text.AsSpan(0x38, 8), 0x100);
        BinaryPrimitives.WriteInt64LittleEndian(text.AsSpan(0x40, 8), 8);       // DT_RELASZ
        BinaryPrimitives.WriteUInt64LittleEndian(text.AsSpan(0x48, 8), 24);
        BinaryPrimitives.WriteInt64LittleEndian(text.AsSpan(0x50, 8), 0);       // DT_NULL

        // Raw LZ4 literal-only block for 0x80 bytes: token 0xF0 (litLen=15) + ext byte 113 + literals.
        var compText = new byte[2 + text.Length];
        compText[0] = 0xF0;
        compText[1] = (byte)(text.Length - 15);
        text.CopyTo(compText.AsSpan(2));

        var rodata = new byte[0x40];
        BinaryPrimitives.WriteUInt64LittleEndian(rodata.AsSpan(0, 8), 0x200);   // r_offset
        BinaryPrimitives.WriteUInt64LittleEndian(rodata.AsSpan(8, 8), 1027);    // R_AARCH64_RELATIVE
        BinaryPrimitives.WriteInt64LittleEndian(rodata.AsSpan(16, 8), unchecked((long)0xCAFEBABE12345678UL));

        const uint textFileOff = 0x100;
        var roFileOff = textFileOff + (uint)compText.Length;
        var dataFileOff = roFileOff + (uint)rodata.Length;
        var raw = new byte[dataFileOff + 0x40];

        "NSO0"u8.CopyTo(raw.AsSpan(0, 4));
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x0C, 4), 1);       // .text LZ4 compressed
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x10, 4), textFileOff);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x14, 4), 0x0);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x18, 4), (uint)text.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x20, 4), roFileOff);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x24, 4), 0x100);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x28, 4), (uint)rodata.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x30, 4), dataFileOff);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x34, 4), 0x200);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x38, 4), 0x40);
        BinaryPrimitives.WriteUInt32LittleEndian(raw.AsSpan(0x60, 4), (uint)compText.Length);
        compText.CopyTo(raw.AsSpan((int)textFileOff));
        rodata.CopyTo(raw.AsSpan((int)roFileOff));

        var plugin = new SwitchNsoPlugin();
        Assert.True(plugin.CanLoad(raw.AsSpan(0, 16), "main"));
        using var img = plugin.Load("main", raw, Sink());
        Assert.Equal(BinaryFormat.Nso, img.Format);
        Assert.Equal(CpuArchitecture.Arm64, img.Architecture);
        Assert.Equal(0xCAFEBABE12345678UL, BinaryPrimitives.ReadUInt64LittleEndian(img.Data.Span.Slice(0x200, 8)));
    }

    [Theory]
    [InlineData("template il2cpp 2022 linux")]
    [InlineData("template il2cpp 2018 win 32")]
    [InlineData("Template il2cpp webgl no brotli")]
    [InlineData("Template il2cpp android/Baldi's Basics Classic.arm64-v8a")]
    [InlineData("Template il2cpp android/Baldi's Basics Classic.armeabi-v7a")]
    public void MultiPlatform_LiftsAndDecompilesMethodBody(string build)
    {
        using var ws = Il2CppWorkspace.Load(Build(build)!, Sink());
        WorkspaceSetup.Attach(ws);
        var type = ws.FindTypes("CameraScript").Single();
        var start = type.Methods.Single(m => m.Name == "Start");
        var cs = ws.Render(type, start, CodeView.CSharp);
        Assert.True(cs.Contains("offset =", StringComparison.Ordinal) || cs.Contains("offset.x =", StringComparison.Ordinal), cs);
        Assert.Contains("player.transform.position", cs, StringComparison.Ordinal);
        Assert.DoesNotContain("throw null", cs, StringComparison.Ordinal);
    }
}

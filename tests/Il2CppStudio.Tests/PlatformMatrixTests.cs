using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Fingerprinting;
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
    [InlineData("Template il2cpp android/Baldi's Basics Classic.arm64-v8a.apk", "2022.3.62f3", "data.unity3d")]
    [InlineData("Template il2cpp webgl", "2022.3.62f3", "data.unity3d")]
    [InlineData("2019 game il2cpp switch", "2019.4.27f1", "globalgamemanagers")]
    public void UnityFs_DetectsVersionAcrossPlatforms(string target, string expectedVersion, string expectedSource)
    {
        using var ws = Il2CppWorkspace.Load(Target(target)!, Sink());
        Assert.Equal(expectedVersion, ws.Environment.UnityVersion);
        Assert.Equal(expectedSource, ws.Environment.UnityVersionSource);
    }

    [Fact]
    public void UnityFs_DetectsVersionInSwitchTitleLayoutAndWebBundle()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-ufs-{Guid.NewGuid():N}");
        try
        {
            var dataDir = Path.Combine(tmp, "1", "Data");
            Directory.CreateDirectory(dataDir);
            var ufsPath = Path.Combine(dataDir, "data.unity3d");
            File.WriteAllBytes(ufsPath, BuildUnityBundle("UnityFS\0", 8, "5.x.x", "2021.3.29f1"));
            var env = EnvironmentDetector.Detect(tmp, null, null, null, Sink());
            Assert.Equal("2021.3.29f1", env.UnityVersion);
            Assert.Equal("data.unity3d", env.UnityVersionSource);

            var webPath = Path.Combine(tmp, "web.unity3d");
            File.WriteAllBytes(webPath, BuildUnityBundle("UnityWeb\0", 6, "5.x.x", "2019.4.40f1"));
            Assert.Equal("2019.4.40f1", EnvironmentDetector.ReadVersionFromSerializedFile(webPath));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
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

    [Fact]
    public void SwitchRealBuild_RecoversRegistrationAndDecompiles()
    {
        using var ws = Il2CppWorkspace.Load(Build("2019 game il2cpp switch")!, Sink());
        WorkspaceSetup.Attach(ws);
        Assert.Equal(24, ws.Metadata.Version.Major);
        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete);
        Assert.True(ws.Registration.GenericMethodPointers.Count > 30000);
        var type = ws.FindTypes("AzukariyaWork").Single();
        var clear = type.Methods.Single(m => m.Name == "Clear");
        var cs = ws.Render(type, clear, CodeView.CSharp);
        Assert.Contains("PlayerWork.AzukariyaData.Clear()", cs, StringComparison.Ordinal);
    }

    [Fact]
    public void Ps4RealBuild_UnpacksSelfRecoversRegistrationAndDecompiles()
    {
        var dir = Build("Unity Game Ps4 template")!;
        using var ws = Il2CppWorkspace.Load(dir, Sink());
        WorkspaceSetup.Attach(ws);
        var elf = Assert.IsType<ElfBinaryImage>(ws.Binary);
        Assert.Equal(CpuArchitecture.X64, elf.Architecture);
        Assert.True(elf.AppliedRelocations > 100000);
        Assert.Equal(Core.Lifting.NativeAbi.SysV, ws.Abi);
        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete);
        Assert.True(ws.AddressMap!.ResolvedMethodCount > 1000);

        // Direct .prx load also locates metadata and Unity headers from the PS4 game root.
        var prx = Path.Combine(dir, "Media", "Modules", "Il2CppUserAssemblies.prx");
        using var wsPrx = Il2CppWorkspace.Load(prx, Sink());
        Assert.True(wsPrx.Registration?.IsComplete);
        Assert.Equal(ws.Environment.UnityVersion, wsPrx.Environment.UnityVersion);

        var acs = ws.Model.Assemblies.First(a => a.Name == "Assembly-CSharp");
        var candidates = acs.Namespaces.SelectMany(n => n.Types)
            .Where(t => !t.Raw.IsInterface && !t.Raw.IsEnum && t.GenericParameters.Count == 0 && !t.Name.StartsWith('<'))
            .SelectMany(t => t.Methods.Where(m => !m.IsAbstract && !m.Name.StartsWith('.') && ws.GetMethodRva(m) != 0).Select(m => (t, m)))
            .Take(12);
        var rendered = candidates.Select(p => ws.Render(p.t, p.m, CodeView.CSharp)).ToList();
        Assert.Contains(rendered, cs => cs.Contains('{') && !cs.Contains("throw null;", StringComparison.Ordinal));
    }

    [Fact]
    public void AndroidSampleApk_LoadsAndRecoversRegistration()
    {
        var apk = Target("sample game unity android.apk")!;
        using var ws = Il2CppWorkspace.Load(apk, Sink());
        WorkspaceSetup.Attach(ws);
        var elf = Assert.IsType<ElfBinaryImage>(ws.Binary);
        Assert.Equal(CpuArchitecture.Arm64, elf.Architecture);
        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete);
        Assert.True(ws.AddressMap!.ResolvedMethodCount > 1000);

        var acs = ws.Model.Assemblies.First(a => a.Name == "Assembly-CSharp");
        var candidates = acs.Namespaces.SelectMany(n => n.Types)
            .Where(t => !t.Raw.IsInterface && !t.Raw.IsEnum && t.GenericParameters.Count == 0 && !t.Name.StartsWith('<'))
            .SelectMany(t => t.Methods.Where(m => !m.IsAbstract && !m.Name.StartsWith('.') && ws.GetMethodRva(m) != 0).Select(m => (t, m)))
            .Take(12);
        var rendered = candidates.Select(p => ws.Render(p.t, p.m, CodeView.CSharp)).ToList();
        Assert.Contains(rendered, cs => cs.Contains('{') && !cs.Contains("throw null;", StringComparison.Ordinal));
    }

    [Fact]
    public void Ps4PkgExtractor_ExtractsPfscIndirectAndUserKeyedXtsWithoutExternalTools()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-pkg-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            // >12 direct blocks (14 * 0x1000 = 56 KiB) to exercise ib[0] indirect block table resolution.
            var prxBytes = new byte[14 * 0x1000 + 73];
            new Random(42).NextBytes(prxBytes);
            "SELF"u8.CopyTo(prxBytes);
            var metaBytes = new byte[] { 0xAF, 0x1B, 0xB1, 0xFA, 29, 0, 0, 0 };
            var unityBundle = BuildUnityBundle("UnityFS\0", 8, "5.x.x", "2020.3.42f1");

            // 1. Unencrypted outer PFS wrapping PFSC zlib-compressed inner PFS with indirect blocks.
            var innerPfs = BuildPfsImage(signed: false, blockSize: 0x1000,
            [
                ("Media/Modules/Il2CppUserAssemblies.prx", prxBytes),
                ("Media/Metadata/global-metadata.dat", metaBytes),
                ("Media/data.unity3d", unityBundle),
            ]);
            var pfscPayload = CompressPfsc(innerPfs, blockSize: 0x1000);
            var outerPlainPfs = BuildPfsImage(signed: false, blockSize: 0x1000, [("pfs_image.dat", pfscPayload)]);
            var plainPkgPath = Path.Combine(tmp, "unencrypted-pfsc.pkg");
            File.WriteAllBytes(plainPkgPath, WrapCntPkg(outerPlainPfs));

            var sink = Sink();
            var extractedDir = Ps4PkgExtractor.Extract(plainPkgPath, sink);
            Assert.NotNull(extractedDir);
            Assert.Equal(prxBytes, File.ReadAllBytes(Path.Combine(extractedDir!, "Media", "Modules", "Il2CppUserAssemblies.prx")));
            Assert.Equal(metaBytes, File.ReadAllBytes(Path.Combine(extractedDir, "Media", "Metadata", "global-metadata.dat")));
            var env = EnvironmentDetector.Detect(extractedDir, Path.Combine(extractedDir, "Media"), null, null, sink);
            Assert.Equal("2020.3.42f1", env.UnityVersion);
            Assert.Equal("data.unity3d", env.UnityVersionSource);

            // 2. Signed dinodeS32 + user-supplied EKPFS AES-128-XTS encrypted PFS.
            var outerSignedPfs = BuildPfsImage(signed: true, blockSize: 0x1000, [("pfs_image.dat", pfscPayload)]);
            var ekpfs = new byte[32];
            for (var i = 0; i < ekpfs.Length; i++) ekpfs[i] = (byte)(i + 1);
            var cryptSeed = new byte[16];
            for (var i = 0; i < cryptSeed.Length; i++) cryptSeed[i] = (byte)(0xA0 + i);
            EncryptPfsXts(outerSignedPfs, blockSize: 0x1000, ekpfs, cryptSeed);
            var xtsPkgPath = Path.Combine(tmp, "signed-xts.pkg");
            File.WriteAllBytes(xtsPkgPath, WrapCntPkg(outerSignedPfs));

            var sinkXts = Sink();
            var extractedXts = Ps4PkgExtractor.Extract(xtsPkgPath, sinkXts, ekpfs);
            Assert.NotNull(extractedXts);
            Assert.Equal(prxBytes, File.ReadAllBytes(Path.Combine(extractedXts!, "Media", "Modules", "Il2CppUserAssemblies.prx")));

            // 3. Real encrypted FPKG without EKPFS or external tool reports clear mode=0xD diagnostic (if present).
            var root = TemplateBuildFixture.RepositoryRoot;
            var realPkg = root is null ? null : Path.Combine(root, "testdata", "Unity Game Ps4 template.pkg");
            if (realPkg is not null && File.Exists(realPkg) && Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PKG_EKPFS") is null)
            {
                var sinkReal = Sink();
                var res = Ps4PkgExtractor.Extract(realPkg, sinkReal);
                if (res is null)
                    Assert.Contains(sinkReal.Query(DiagnosticSeverity.Error), d => d.Message.Contains("mode=0xD", StringComparison.OrdinalIgnoreCase));
            }
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void SwitchNspExtractor_ExtractsNestedPfs0AndRomFs_ReportsEncryptedNca()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-nsp-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            var mainNso = new byte[0x100];
            "NSO0"u8.CopyTo(mainNso);
            var exefsPfs = BuildPfs0([("main", mainNso)]);
            var metaBytes = new byte[] { 0xAF, 0x1B, 0xB1, 0xFA, 24, 0, 0, 0 };
            var unityBundle = BuildUnityBundle("UnityFS\0", 8, "5.x.x", "2021.3.15f1");
            var romfs = BuildRomFs(
            [
                ("Data/Managed/Metadata/global-metadata.dat", metaBytes),
                ("Data/data.unity3d", unityBundle),
            ]);

            var nspPath = Path.Combine(tmp, "sample-unencrypted.nsp");
            File.WriteAllBytes(nspPath, BuildPfs0([("exefs.pfs0", exefsPfs), ("romfs.bin", romfs)]));

            var sink = Sink();
            var files = GameFolderLocator.Locate(nspPath, sink);
            Assert.NotNull(files.BinaryPath);
            Assert.EndsWith("main", files.BinaryPath!, StringComparison.OrdinalIgnoreCase);
            Assert.NotNull(files.MetadataPath);
            Assert.EndsWith("global-metadata.dat", files.MetadataPath!, StringComparison.OrdinalIgnoreCase);
            Assert.NotNull(files.DataFolder);
            var env = EnvironmentDetector.Detect(files.Root, files.DataFolder, files.BinaryPath, null, sink);
            Assert.Equal("2021.3.15f1", env.UnityVersion);
            Assert.Equal("data.unity3d", env.UnityVersionSource);

            var encNspPath = Path.Combine(tmp, "encrypted.nsp");
            File.WriteAllBytes(encNspPath, BuildPfs0([("0000000000000000.nca", new byte[64])]));
            var sinkEnc = Sink();
            Assert.Null(SwitchNspExtractor.Extract(encNspPath, sinkEnc));
            Assert.Contains(sinkEnc.Query(DiagnosticSeverity.Error), d => d.Message.Contains(".nca", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    private static byte[] BuildUnityBundle(string magic, int formatVer, string playerVer, string revision)
    {
        using var ms = new MemoryStream();
        ms.Write(Encoding.ASCII.GetBytes(magic));
        Span<byte> ver = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(ver, formatVer);
        ms.Write(ver);
        ms.Write(Encoding.ASCII.GetBytes(playerVer));
        ms.WriteByte(0);
        ms.Write(Encoding.ASCII.GetBytes(revision));
        ms.WriteByte(0);
        ms.Write(new byte[32]);
        return ms.ToArray();
    }

    private static byte[] WrapCntPkg(byte[] pfs)
    {
        const ulong pfsOff = 0x1000;
        var pkg = new byte[(int)pfsOff + pfs.Length];
        BinaryPrimitives.WriteUInt32BigEndian(pkg.AsSpan(0, 4), 0x7F434E54);
        BinaryPrimitives.WriteUInt64BigEndian(pkg.AsSpan(0x410, 8), pfsOff);
        BinaryPrimitives.WriteUInt64BigEndian(pkg.AsSpan(0x418, 8), (ulong)pfs.Length);
        pfs.CopyTo(pkg.AsSpan((int)pfsOff));
        return pkg;
    }

    private static byte[] CompressPfsc(byte[] raw, uint blockSize)
    {
        var blockCount = (int)((raw.Length + blockSize - 1) / blockSize);
        var offsetsStart = 0x40UL;
        var dataStart = offsetsStart + (ulong)((blockCount + 1) * 8);
        using var ms = new MemoryStream();
        ms.Write(new byte[(int)dataStart]);
        var offsets = new ulong[blockCount + 1];
        for (var i = 0; i < blockCount; i++)
        {
            offsets[i] = (ulong)ms.Position;
            var off = i * (int)blockSize;
            var len = Math.Min((int)blockSize, raw.Length - off);
            using var zbuf = new MemoryStream();
            using (var z = new ZLibStream(zbuf, CompressionLevel.Fastest, leaveOpen: true))
                z.Write(raw, off, len);
            var packed = zbuf.ToArray();
            if (len == (int)blockSize && packed.Length >= len)
                ms.Write(raw, off, len);
            else
                ms.Write(packed);
        }
        offsets[blockCount] = (ulong)ms.Position;
        var res = ms.ToArray();
        BinaryPrimitives.WriteUInt32LittleEndian(res.AsSpan(0x00, 4), 0x43534650);
        BinaryPrimitives.WriteUInt32LittleEndian(res.AsSpan(0x0C, 4), blockSize);
        BinaryPrimitives.WriteUInt64LittleEndian(res.AsSpan(0x18, 8), offsetsStart);
        BinaryPrimitives.WriteUInt64LittleEndian(res.AsSpan(0x20, 8), dataStart);
        BinaryPrimitives.WriteUInt64LittleEndian(res.AsSpan(0x28, 8), (ulong)raw.Length);
        for (var i = 0; i <= blockCount; i++)
            BinaryPrimitives.WriteUInt64LittleEndian(res.AsSpan((int)offsetsStart + i * 8, 8), offsets[i]);
        return res;
    }

    private static byte[] BuildPfsImage(bool signed, uint blockSize, (string Path, byte[] Data)[] files)
    {
        // Unified inode allocation (0 = dummy, 1 = superroot).
        var inoDirChildren = new Dictionary<int, List<(string Name, int ChildIno, bool IsDir)>> { [1] = [] };
        var inoData = new Dictionary<int, byte[]>();
        var nextIno = 2;

        int EnsureDir(int parent, string name)
        {
            foreach (var c in inoDirChildren[parent])
                if (c.IsDir && c.Name == name) return c.ChildIno;
            var id = nextIno++;
            inoDirChildren[parent].Add((name, id, true));
            inoDirChildren[id] = [];
            return id;
        }

        foreach (var (path, data) in files)
        {
            var parts = path.Split('/');
            var cur = 1;
            for (var i = 0; i < parts.Length - 1; i++)
                cur = EnsureDir(cur, parts[i]);
            var fIno = nextIno++;
            inoDirChildren[cur].Add((parts[^1], fIno, false));
            inoData[fIno] = data;
        }

        foreach (var (dirIno, children) in inoDirChildren)
        {
            using var dms = new MemoryStream();
            foreach (var (name, childIno, isDir) in children)
            {
                var nb = Encoding.UTF8.GetBytes(name);
                var entSize = ((16 + nb.Length + 7) / 8) * 8;
                var ent = new byte[entSize];
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan(0, 4), childIno);
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan(4, 4), isDir ? 3 : 2);
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan(8, 4), nb.Length);
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan(12, 4), entSize);
                nb.CopyTo(ent.AsSpan(16));
                dms.Write(ent);
            }
            inoData[dirIno] = dms.ToArray();
        }

        var inodeSize = signed ? 0x2C8 : 0xA8;
        var entryStride = signed ? 36 : 4;
        var entryBlockOff = signed ? 32 : 0;
        var ptrsPerBlock = (int)(blockSize / (uint)entryStride);
        var totalInodes = nextIno;
        var inodeBlocks = (int)((totalInodes * inodeSize + blockSize - 1) / blockSize);
        var nextFreeBlock = 1 + inodeBlocks;

        var imgBlocks = new List<byte[]>();
        for (var i = 0; i < nextFreeBlock; i++) imgBlocks.Add(new byte[blockSize]);

        for (var ino = 1; ino < totalInodes; ino++)
        {
            var data = inoData[ino];
            var needed = Math.Max(1, (int)((data.Length + blockSize - 1) / blockSize));
            var alloc = new List<int>(needed);
            for (var b = 0; b < needed; b++)
            {
                var blkBuf = new byte[blockSize];
                var off = b * (int)blockSize;
                if (off < data.Length)
                    Array.Copy(data, off, blkBuf, 0, Math.Min((int)blockSize, data.Length - off));
                alloc.Add(nextFreeBlock++);
                imgBlocks.Add(blkBuf);
            }

            var inoOff = ino * inodeSize;
            var inoBlkIdx = 1 + inoOff / (int)blockSize;
            var inoSpan = imgBlocks[inoBlkIdx].AsSpan(inoOff % (int)blockSize, inodeSize);
            BinaryPrimitives.WriteUInt16LittleEndian(inoSpan.Slice(0, 2), (ushort)(inoDirChildren.ContainsKey(ino) ? 0x416D : 0x816D));
            BinaryPrimitives.WriteUInt64LittleEndian(inoSpan.Slice(8, 8), (ulong)data.Length);
            BinaryPrimitives.WriteUInt32LittleEndian(inoSpan.Slice(0x60, 4), (uint)alloc.Count);

            for (var d = 0; d < 12 && d < alloc.Count; d++)
                BinaryPrimitives.WriteInt32LittleEndian(inoSpan.Slice(0x64 + d * entryStride + entryBlockOff, 4), alloc[d]);

            if (alloc.Count > 12)
            {
                var indBlkNo = nextFreeBlock++;
                var indBuf = new byte[blockSize];
                imgBlocks.Add(indBuf);
                BinaryPrimitives.WriteInt32LittleEndian(inoSpan.Slice(0x64 + 12 * entryStride + entryBlockOff, 4), indBlkNo);
                for (var idx = 12; idx < alloc.Count && idx - 12 < ptrsPerBlock; idx++)
                    BinaryPrimitives.WriteInt32LittleEndian(indBuf.AsSpan((idx - 12) * entryStride + entryBlockOff, 4), alloc[idx]);
            }
        }

        // Write PFS superblock at block 0.
        var super = imgBlocks[0].AsSpan();
        BinaryPrimitives.WriteUInt64LittleEndian(super.Slice(0, 8), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(super.Slice(8, 4), 0x01332A0B);
        BinaryPrimitives.WriteUInt16LittleEndian(super.Slice(0x1C, 2), (ushort)(signed ? 1 : 0));
        BinaryPrimitives.WriteUInt32LittleEndian(super.Slice(0x20, 4), blockSize);
        BinaryPrimitives.WriteUInt64LittleEndian(super.Slice(0x28, 8), (ulong)inodeBlocks);
        BinaryPrimitives.WriteUInt64LittleEndian(super.Slice(0x30, 8), (ulong)totalInodes);
        BinaryPrimitives.WriteUInt64LittleEndian(super.Slice(0x40, 8), 1);

        var output = new byte[imgBlocks.Count * (int)blockSize];
        for (var i = 0; i < imgBlocks.Count; i++)
            imgBlocks[i].CopyTo(output.AsSpan(i * (int)blockSize));
        return output;
    }

    private static void EncryptPfsXts(byte[] pfs, uint blockSize, byte[] ekpfs, byte[] cryptSeed)
    {
        var mode = BinaryPrimitives.ReadUInt16LittleEndian(pfs.AsSpan(0x1C, 2));
        BinaryPrimitives.WriteUInt16LittleEndian(pfs.AsSpan(0x1C, 2), (ushort)(mode | 0x4));
        cryptSeed.CopyTo(pfs.AsSpan(0x370, 16));

        Span<byte> msg = stackalloc byte[20];
        BinaryPrimitives.WriteUInt32LittleEndian(msg, 1);
        cryptSeed.CopyTo(msg[4..]);
        Span<byte> derived = stackalloc byte[32];
        HMACSHA256.HashData(ekpfs, msg, derived);

        using var aesT = Aes.Create();
        aesT.Mode = CipherMode.ECB;
        aesT.Padding = PaddingMode.None;
        aesT.Key = derived[..16].ToArray();

        using var aesD = Aes.Create();
        aesD.Mode = CipherMode.ECB;
        aesD.Padding = PaddingMode.None;
        aesD.Key = derived.Slice(16, 16).ToArray();

        const int sectorSize = 0x1000;
        Span<byte> tweakIn = stackalloc byte[16];
        Span<byte> tweak = stackalloc byte[16];
        Span<byte> tmp = stackalloc byte[16];
        for (var off = (int)blockSize; off < pfs.Length; off += sectorSize)
        {
            var sec = (ulong)(off / sectorSize);
            tweakIn.Clear();
            BinaryPrimitives.WriteUInt64LittleEndian(tweakIn, sec);
            aesT.EncryptEcb(tweakIn, tweak, PaddingMode.None);

            var end = Math.Min(sectorSize, pfs.Length - off);
            for (var sub = 0; sub + 16 <= end; sub += 16)
            {
                var slice = pfs.AsSpan(off + sub, 16);
                for (var i = 0; i < 16; i++) slice[i] ^= tweak[i];
                aesD.EncryptEcb(slice, tmp, PaddingMode.None);
                for (var i = 0; i < 16; i++) slice[i] = (byte)(tmp[i] ^ tweak[i]);

                var carry = 0;
                for (var i = 0; i < 16; i++)
                {
                    var next = (tweak[i] >> 7) & 1;
                    tweak[i] = (byte)((tweak[i] << 1) | carry);
                    carry = next;
                }
                if (carry != 0) tweak[0] ^= 0x87;
            }
        }
    }

    private static byte[] BuildPfs0((string Name, byte[] Data)[] files)
    {
        using var strMs = new MemoryStream();
        var nameOffs = new uint[files.Length];
        for (var i = 0; i < files.Length; i++)
        {
            nameOffs[i] = (uint)strMs.Position;
            strMs.Write(Encoding.UTF8.GetBytes(files[i].Name));
            strMs.WriteByte(0);
        }
        while (strMs.Length % 16 != 0) strMs.WriteByte(0);
        var strTable = strMs.ToArray();

        var dataStart = 16 + files.Length * 24 + strTable.Length;
        using var outMs = new MemoryStream();
        outMs.Write(new byte[dataStart]);
        var relOff = 0UL;
        var meta = outMs.GetBuffer();
        for (var i = 0; i < files.Length; i++)
        {
            var e = 16 + i * 24;
            BinaryPrimitives.WriteUInt64LittleEndian(outMs.GetBuffer().AsSpan(e, 8), relOff);
            BinaryPrimitives.WriteUInt64LittleEndian(outMs.GetBuffer().AsSpan(e + 8, 8), (ulong)files[i].Data.Length);
            BinaryPrimitives.WriteUInt32LittleEndian(outMs.GetBuffer().AsSpan(e + 16, 4), nameOffs[i]);
            relOff += (ulong)files[i].Data.Length;
        }
        foreach (var (_, data) in files) outMs.Write(data);

        var buf = outMs.ToArray();
        "PFS0"u8.CopyTo(buf);
        BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(4, 4), (uint)files.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(8, 4), (uint)strTable.Length);
        strTable.CopyTo(buf.AsSpan(16 + files.Length * 24));
        return buf;
    }

    private static byte[] BuildRomFs((string Path, byte[] Data)[] files)
    {
        // Build simple Switch RomFS (header 0x50, dirTable, fileTable, aligned data at 0x200 + tables).
        using var dirStream = new MemoryStream();
        using var fileStream = new MemoryStream();
        using var dataStream = new MemoryStream();

        var dirs = new Dictionary<string, (uint Off, uint Parent, uint Sibling, uint Child, uint File, string Name)>
        {
            [string.Empty] = (0, 0, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, string.Empty),
        };
        var dirOrder = new List<string> { string.Empty };
        var dirFiles = new Dictionary<string, List<(string Name, uint Off, ulong DataOff, ulong DataSize)>> { [string.Empty] = [] };

        uint curDirOff = 0x18; // root has nameLen=0 -> 0x18 bytes
        uint EnsureDir(string path)
        {
            if (dirs.TryGetValue(path, out var existing)) return existing.Off;
            var slash = path.LastIndexOf('/');
            var parentPath = slash < 0 ? string.Empty : path[..slash];
            var name = slash < 0 ? path : path[(slash + 1)..];
            var parentOff = EnsureDir(parentPath);
            var off = curDirOff;
            var nb = Encoding.UTF8.GetBytes(name);
            curDirOff += (uint)(0x18 + ((nb.Length + 3) & ~3));
            dirs[path] = (off, parentOff, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, name);
            dirOrder.Add(path);
            dirFiles[path] = [];
            var p = dirs[parentPath];
            if (p.Child == 0xFFFFFFFF)
                dirs[parentPath] = (p.Off, p.Parent, p.Sibling, off, p.File, p.Name);
            else
            {
                // Link at front of child chain by pointing new dir's sibling to old child and updating parent.child.
                var d = dirs[path];
                dirs[path] = (d.Off, d.Parent, p.Child, d.Child, d.File, d.Name);
                dirs[parentPath] = (p.Off, p.Parent, p.Sibling, off, p.File, p.Name);
            }
            return off;
        }

        uint curFileOff = 0;
        foreach (var (path, data) in files)
        {
            var slash = path.LastIndexOf('/');
            var dirPath = slash < 0 ? string.Empty : path[..slash];
            var fName = slash < 0 ? path : path[(slash + 1)..];
            var dOff = EnsureDir(dirPath);
            var dataOff = (ulong)dataStream.Position;
            dataStream.Write(data);

            var fOff = curFileOff;
            var nb = Encoding.UTF8.GetBytes(fName);
            curFileOff += (uint)(0x20 + ((nb.Length + 3) & ~3));
            dirFiles[dirPath].Add((fName, fOff, dataOff, (ulong)data.Length));

            var d = dirs[dirPath];
            dirs[dirPath] = (d.Off, d.Parent, d.Sibling, d.Child, fOff, d.Name);
        }

        foreach (var dp in dirOrder)
        {
            var d = dirs[dp];
            var nb = Encoding.UTF8.GetBytes(d.Name);
            var ent = new byte[0x18 + ((nb.Length + 3) & ~3)];
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x00, 4), d.Parent);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x04, 4), d.Sibling);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x08, 4), d.Child);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x0C, 4), d.File);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x10, 4), 0xFFFFFFFF);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x14, 4), (uint)nb.Length);
            nb.CopyTo(ent.AsSpan(0x18));
            dirStream.Write(ent);
        }

        // Write files in offset order, linking each file to the previous file added in the same directory.
        var lastInDir = new Dictionary<string, uint>();
        var flatFiles = new List<(string Dir, string Name, uint Off, ulong DataOff, ulong DataSize)>();
        foreach (var (dp, list) in dirFiles)
            foreach (var f in list)
                flatFiles.Add((dp, f.Name, f.Off, f.DataOff, f.DataSize));
        flatFiles.Sort((a, b) => a.Off.CompareTo(b.Off));
        foreach (var f in flatFiles)
        {
            var nb = Encoding.UTF8.GetBytes(f.Name);
            var ent = new byte[0x20 + ((nb.Length + 3) & ~3)];
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x00, 4), dirs[f.Dir].Off);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x04, 4), lastInDir.TryGetValue(f.Dir, out var prev) ? prev : 0xFFFFFFFF);
            BinaryPrimitives.WriteUInt64LittleEndian(ent.AsSpan(0x08, 8), f.DataOff);
            BinaryPrimitives.WriteUInt64LittleEndian(ent.AsSpan(0x10, 8), f.DataSize);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x18, 4), 0xFFFFFFFF);
            BinaryPrimitives.WriteUInt32LittleEndian(ent.AsSpan(0x1C, 4), (uint)nb.Length);
            nb.CopyTo(ent.AsSpan(0x20));
            fileStream.Write(ent);
            lastInDir[f.Dir] = f.Off;
        }

        const ulong dataBase = 0x200;
        var dirBytes = dirStream.ToArray();
        var fileBytes = fileStream.ToArray();
        var dataBytes = dataStream.ToArray();

        const ulong dirHashOff = dataBase;
        const ulong dirHashSz = 0x10;
        var dirTableOff = dirHashOff + dirHashSz;
        var fileHashOff = dirTableOff + (ulong)dirBytes.Length;
        const ulong fileHashSz = 0x10;
        var fileTableOff = fileHashOff + fileHashSz;
        var payloadOff = fileTableOff + (ulong)fileBytes.Length;

        var total = new byte[(int)payloadOff + dataBytes.Length];
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x00, 8), 0x50);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x08, 8), dirHashOff);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x10, 8), dirHashSz);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x18, 8), dirTableOff);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x20, 8), (ulong)dirBytes.Length);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x28, 8), fileHashOff);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x30, 8), fileHashSz);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x38, 8), fileTableOff);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x40, 8), (ulong)fileBytes.Length);
        BinaryPrimitives.WriteUInt64LittleEndian(total.AsSpan(0x48, 8), payloadOff);
        dirBytes.CopyTo(total.AsSpan((int)dirTableOff));
        fileBytes.CopyTo(total.AsSpan((int)fileTableOff));
        dataBytes.CopyTo(total.AsSpan((int)payloadOff));
        return total;
    }
}

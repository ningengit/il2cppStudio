using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

public sealed class PreV24MetadataTests
{
    [Theory]
    [InlineData(16, 192, 108, 56, 12, 20, 60)]
    [InlineData(19, 216, 112, 56, 16, 24, 60)]
    [InlineData(20, 224, 112, 56, 16, 24, 68)]
    [InlineData(21, 240, 120, 56, 16, 24, 68)]
    [InlineData(22, 256, 120, 56, 16, 24, 68)]
    [InlineData(23, 264, 104, 56, 16, 24, 68)]
    public void PreV24_HeaderAndRecordSizes_MatchUnityAbi(
        int major, int expectedHeader, int expectedTypeSize, int expectedMethodSize,
        int expectedFieldSize, int expectedImageSize, int expectedAssemblySize)
    {
        var v = new MetadataVersion(major);
        Assert.True(v.IsSupported);
        Assert.Equal(expectedHeader, MetadataHeader.SizeFor(v));
        Assert.Equal(expectedTypeSize, Il2CppTypeDefinition.SizeOf(v));
        Assert.Equal(expectedMethodSize, Il2CppMethodDefinition.SizeOf(v));
        Assert.Equal(expectedFieldSize, Il2CppFieldDefinition.SizeOf(v));
        Assert.Equal(expectedImageSize, Il2CppImageDefinition.SizeOf(v));
        Assert.Equal(expectedAssemblySize, Il2CppAssemblyDefinition.SizeOf(v));
    }

    [Theory]
    [InlineData(16)]
    [InlineData(19)]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    public void PreV24_GlobalMetadataAndModel_LoadCleanlyWithoutWarningsOrErrors(int major)
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-prev24-md-{major}-{Guid.NewGuid():N}.dat");
        try
        {
            var metaBytes = BuildSyntheticMetadata(new MetadataVersion(major), out var expectedLiteral);
            File.WriteAllBytes(tmp, metaBytes);

            var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            using var md = GlobalMetadata.Load(tmp, sink);
            Assert.Equal(major, md.Version.Major);
            Assert.Equal(expectedLiteral, md.GetStringLiteral(0));
            Assert.All(md.SectionReports, r => { Assert.True(r.InBounds); Assert.True(r.Aligned); });

            var model = MetadataModel.Build(md, sink);
            var asm = Assert.Single(model.Assemblies, a => a.Name == "Assembly-CSharp");
            if (major >= 20)
                Assert.Equal(["mscorlib"], asm.References);
            else
                Assert.Empty(asm.References);

            var player = Assert.Single(model.Types, t => t.Name == "Player" && t.Namespace == "Game");
            Assert.Equal("Run", Assert.Single(player.Methods).Name);
            Assert.Equal("hp", Assert.Single(player.Fields).Name);
            Assert.Equal("Score", Assert.Single(player.Properties).Name);
            Assert.Equal("OnHit", Assert.Single(player.Events).Name);

            Assert.Empty(sink.Query(DiagnosticSeverity.Warning));
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Theory]
    [InlineData(15)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(25)]
    [InlineData(26)]
    [InlineData(28)]
    [InlineData(30)]
    [InlineData(32)]
    public void UnshippedOrOutOfRangeMetadataVersions_ThrowUnsupportedDiagnostic(int rawVersion)
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-badver-{rawVersion}-{Guid.NewGuid():N}.dat");
        try
        {
            var buf = new byte[300];
            BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(0, 4), MetadataHeader.ExpectedSanity);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(4, 4), rawVersion);
            File.WriteAllBytes(tmp, buf);

            var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            Assert.Throws<MetadataLoadException>(() => GlobalMetadata.Load(tmp, sink));
            Assert.Contains(sink.Query(DiagnosticSeverity.Error), e => e.Code == DiagnosticCodes.MetadataVersionUnsupported);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Theory]
    [InlineData(16, true, false)]
    [InlineData(20, true, false)]
    [InlineData(21, true, false)]
    [InlineData(21, true, true)]
    [InlineData(22, true, true)]
    [InlineData(23, true, true)]
    [InlineData(23, false, true)]
    public void PreV24_RegistrationDiscoveryAndCacheRoundTrip_WorkAcrossAllStructVariants(
        int major, bool is64Bit, bool v21PointerFieldOffsets)
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), $"il2cppstudio-prev24-reg-{major}-{Guid.NewGuid():N}");
        var prevCacheDir = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR");
        Directory.CreateDirectory(tmpDir);
        try
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR", Path.Combine(tmpDir, "cache"));
            var version = new MetadataVersion(major);
            var metaPath = Path.Combine(tmpDir, "global-metadata.dat");
            File.WriteAllBytes(metaPath, BuildSyntheticMetadata(version, out _));

            var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            using var md = GlobalMetadata.Load(metaPath, sink);
            var model = MetadataModel.Build(md, sink);

            using var image = BuildSyntheticBinary(md, is64Bit, v21PointerFieldOffsets,
                out var expectedMethodVa, out var expectedFieldOffset, out var expectedUsageGlobalVa);

            var reg = Il2CppRegistration.Load(image, md, sink);
            Assert.True(reg.IsComplete);
            var expectFlat = (major <= 20) || (major == 21 && !v21PointerFieldOffsets);
            Assert.Equal(!expectFlat, reg.FieldOffsetsArePointers);
            Assert.Equal(!expectFlat, reg.TypeSizesArePointers);
            Assert.Equal(expectedFieldOffset, reg.GetFieldOffset(0, 0));
            Assert.Equal(24, reg.GetInstanceSize(0));

            var names = new TypeNameService(md, model, reg);
            var map = new MethodAddressMap(md, model, reg, names, [], null, sink);
            Assert.Equal(expectedMethodVa, image.ImageBase + map.GetMethodRva(0));

            if (major >= 19)
            {
                Assert.True(reg.MetadataUsages.TryGetValue(expectedUsageGlobalVa, out var usage));
                Assert.Equal(MetadataUsageKind.StringLiteral, usage.Kind);
                Assert.Equal(0, usage.Index);
            }
            else
            {
                Assert.Empty(reg.MetadataUsages);
            }

            // Second load hits RegistrationCache and reconstructs identical state cleanly.
            var regCached = Il2CppRegistration.Load(image, md, sink);
            Assert.True(regCached.IsComplete);
            Assert.Equal(reg.MetadataRegistrationVa, regCached.MetadataRegistrationVa);
            Assert.Equal(reg.CodeRegistrationVa, regCached.CodeRegistrationVa);
            Assert.Equal(expectedFieldOffset, regCached.GetFieldOffset(0, 0));
            Assert.Equal(24, regCached.GetInstanceSize(0));
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR", prevCacheDir);
            if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, recursive: true);
        }
    }

    [Fact]
    public void WebDataExtractor_UnpacksDatagzEmscriptenManifestAndDetectsAsmJs()
    {
        var root = Path.Combine(Path.GetTempPath(), $"il2cppstudio-datagz-manifest-{Guid.NewGuid():N}");
        var release = Path.Combine(root, "Release");
        Directory.CreateDirectory(release);
        try
        {
            var dataUnity3d = BuildUnityFsBundle("5.3.0f1");
            var metaBytes = BuildSyntheticMetadata(new MetadataVersion(16), out _, rawMethodToken: 1u);

            var dataEnd = dataUnity3d.Length;
            var metaStart = dataEnd;
            var metaEnd = metaStart + metaBytes.Length;
            var rawBlob = new byte[metaEnd];
            Buffer.BlockCopy(dataUnity3d, 0, rawBlob, 0, dataUnity3d.Length);
            Buffer.BlockCopy(metaBytes, 0, rawBlob, metaStart, metaBytes.Length);

            var dataGzPath = Path.Combine(release, "game.datagz");
            File.WriteAllBytes(dataGzPath, CompressGzip(rawBlob));
            File.WriteAllBytes(Path.Combine(release, "game.jsgz"), CompressGzip("// asm.js\n"u8.ToArray()));
            File.WriteAllBytes(Path.Combine(release, "game.js.memgz"), CompressGzip([1, 2, 3, 4]));
            File.WriteAllText(Path.Combine(release, "UnityLoader.js"),
                $"new t(0,{dataEnd},0,0).open(\"GET\",\"/data.unity3d\"),new t({metaStart},{metaEnd},0,0).open(\"GET\",\"/Il2CppData/Metadata/global-metadata.dat\")");

            foreach (var target in new[] { root, release, dataGzPath })
            {
                var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
                using var ws = Il2CppWorkspace.Load(target, sink);
                Assert.Empty(sink.Query(DiagnosticSeverity.Error));
                Assert.Empty(sink.Query(DiagnosticSeverity.Warning));
                Assert.Equal(16, ws.Metadata.Version.Major);
                Assert.Equal("5.3.0f1", ws.Environment.UnityVersion);
                Assert.Equal("IL2CPP", ws.Environment.ScriptingBackend);
                Assert.NotNull(ws.Files.LegacyAsmJsPath);
                Assert.Contains("asm.js", ws.LiftingUnavailableReason ?? "", StringComparison.OrdinalIgnoreCase);
                Assert.Contains(sink.Query(DiagnosticSeverity.Information), d =>
                    d.Severity == DiagnosticSeverity.Information && d.Code == DiagnosticCodes.BinaryFormatUnsupported && d.Message.Contains("asm.js", StringComparison.OrdinalIgnoreCase));

                var player = Assert.Single(ws.Model.Types, t => t.Name == "Player" && t.Namespace == "Game");
                var run = Assert.Single(player.Methods);
                Assert.Equal("Run", run.Name);
                Assert.Equal(0x06000001u, run.Token);
                Assert.Equal("hp", Assert.Single(player.Fields).Name);
                Assert.Equal("Score", Assert.Single(player.Properties).Name);
                Assert.Equal("OnHit", Assert.Single(player.Events).Name);
            }
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void WebDataExtractor_UnpacksDatagzRawFallbackAndUnityWebDataGzip()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-datagz-fallback-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            // (a) Compressed/game.datagz raw concatenated blob with NO UnityLoader.js
            var rootRaw = Path.Combine(tmp, "raw");
            var compressedDir = Path.Combine(rootRaw, "Compressed");
            Directory.CreateDirectory(compressedDir);

            var dataUnity3d = BuildUnityFsBundle("5.4.1f1");
            var metaBytes = BuildSyntheticMetadata(new MetadataVersion(20), out _);
            var rawConcat = new byte[dataUnity3d.Length + metaBytes.Length];
            Buffer.BlockCopy(dataUnity3d, 0, rawConcat, 0, dataUnity3d.Length);
            Buffer.BlockCopy(metaBytes, 0, rawConcat, dataUnity3d.Length, metaBytes.Length);
            File.WriteAllBytes(Path.Combine(compressedDir, "game.datagz"), CompressGzip(rawConcat));
            File.WriteAllText(Path.Combine(compressedDir, "game.asm.js"), "// asm.js");

            var sinkRaw = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            using (var wsRaw = Il2CppWorkspace.Load(rootRaw, sinkRaw))
            {
                Assert.Empty(sinkRaw.Query(DiagnosticSeverity.Error));
                Assert.Empty(sinkRaw.Query(DiagnosticSeverity.Warning));
                Assert.Equal(20, wsRaw.Metadata.Version.Major);
                Assert.Equal("5.4.1f1", wsRaw.Environment.UnityVersion);
                Assert.NotNull(wsRaw.Files.LegacyAsmJsPath);
            }

            // (b) Release/game.datagz wrapping gzip-compressed UnityWebData1.0\0
            var rootUwd = Path.Combine(tmp, "uwd");
            var releaseUwd = Path.Combine(rootUwd, "Release");
            Directory.CreateDirectory(releaseUwd);
            var uwdBytes = BuildUnityWebData([
                ("Il2CppData/Metadata/global-metadata.dat", metaBytes),
                ("data.unity3d", dataUnity3d),
            ]);
            File.WriteAllBytes(Path.Combine(releaseUwd, "game.datagz"), CompressGzip(uwdBytes));

            var sinkUwd = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            using (var wsUwd = Il2CppWorkspace.Load(rootUwd, sinkUwd))
            {
                Assert.Empty(sinkUwd.Query(DiagnosticSeverity.Error));
                Assert.Equal(20, wsUwd.Metadata.Version.Major);
                Assert.Equal("5.4.1f1", wsUwd.Environment.UnityVersion);
            }
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void RealUnity53WebGlBuild_DatagzAndV16Metadata_ReconstructsCSharpAndUnityScript()
    {
        const string realRoot = @"C:\Users\13131313\Downloads\webgl";
        var releaseDir = Path.Combine(realRoot, "Release");
        if (!Directory.Exists(releaseDir))
            Assert.Skip($"Local Unity 5.3 WebGL test fixture not found at '{releaseDir}'.");

        foreach (var target in new[] { realRoot, releaseDir })
        {
            var sink = new DiagnosticCollector(new Serilog.LoggerConfiguration().CreateLogger());
            using var ws = Il2CppWorkspace.Load(target, sink);
            WorkspaceSetup.Attach(ws);

            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
            Assert.Empty(sink.Query(DiagnosticSeverity.Warning));
            Assert.Equal(16, ws.Metadata.Version.Major);
            Assert.Equal("5.3.0f1", ws.Environment.UnityVersion);
            Assert.Equal("IL2CPP", ws.Environment.ScriptingBackend);
            Assert.NotNull(ws.Files.LegacyAsmJsPath);
            Assert.Contains("asm.js", ws.LiftingUnavailableReason ?? "", StringComparison.OrdinalIgnoreCase);
            Assert.Contains(sink.Query(DiagnosticSeverity.Information), d =>
                d.Severity == DiagnosticSeverity.Information && d.Code == DiagnosticCodes.BinaryFormatUnsupported && d.Message.Contains("asm.js", StringComparison.OrdinalIgnoreCase));

            Assert.All(ws.Metadata.SectionReports, r =>
            {
                Assert.True(r.InBounds, $"Section {r.Name} out of bounds");
                Assert.True(r.Aligned, $"Section {r.Name} misaligned");
            });

            Assert.Equal(7, ws.Model.Assemblies.Count);
            Assert.Contains(ws.Model.Assemblies, a => a.Name == "Assembly-CSharp");
            Assert.Contains(ws.Model.Assemblies, a => a.Name == "Assembly-UnityScript");

            var pc = Assert.Single(ws.FindTypes("PlayerController"));
            Assert.Equal(["moveSpeed", "turnSpeed", "startColor", "objRenderer"], pc.Fields.Select(f => f.Name));
            Assert.Equal([".ctor", "Start", "Update"], pc.Methods.Select(m => m.Name));
            Assert.Equal([0x06000001u, 0x06000002u, 0x06000003u], pc.Methods.Select(m => m.Token));

            var ur = Assert.Single(ws.FindTypes("objRenderer"));
            Assert.Equal(["moveSpeed", "turnSpeed", "startColor", "objRenderer"], ur.Fields.Select(f => f.Name));
            Assert.Equal([".ctor", "Start", "Update", "Main"], ur.Methods.Select(m => m.Name));

            var literals = Enumerable.Range(0, ws.Metadata.StringLiterals.Length)
                .Select(ws.Metadata.GetStringLiteral)
                .ToHashSet(StringComparer.Ordinal);
            Assert.Contains("Vertical", literals);
            Assert.Contains("Horizontal", literals);
            Assert.Contains("Jump pressed in C#!", literals);
            Assert.Contains("Jump pressed in UnityScript!", literals);

            var csPc = ws.Render(pc, method: null, CodeView.CSharp);
            Assert.Contains("class PlayerController", csPc);
            Assert.Contains("moveSpeed", csPc);
            Assert.Contains("Start()", csPc);
            Assert.Contains("Update()", csPc);
            Assert.Contains("asm.js", csPc, StringComparison.OrdinalIgnoreCase);

            var csUr = ws.Render(ur, method: null, CodeView.CSharp);
            Assert.Contains("class objRenderer", csUr);
            Assert.Contains("Main()", csUr);
            Assert.Contains("asm.js", csUr, StringComparison.OrdinalIgnoreCase);
        }
    }

    // ------------------------------------------------------------------ synthetic fixtures

    private static byte[] BuildSyntheticMetadata(MetadataVersion v, out string expectedLiteral, uint rawMethodToken = 0x06000001u)
    {
        expectedLiteral = "pre_v24_ok";

        // Build string table and track offsets.
        var strStream = new MemoryStream();
        int AddStr(string s)
        {
            var off = (int)strStream.Position;
            strStream.Write(Encoding.UTF8.GetBytes(s));
            strStream.WriteByte(0);
            return off;
        }
        var sEmpty = AddStr("");
        var sGameAsmImg = AddStr("Assembly-CSharp.dll");
        var sGameAsm = AddStr("Assembly-CSharp");
        var sMscorlibImg = AddStr("mscorlib.dll");
        var sMscorlib = AddStr("mscorlib");
        var sNs = AddStr("Game");
        var sPlayer = AddStr("Player");
        var sRun = AddStr("Run");
        var sParamX = AddStr("x");
        var sFieldHp = AddStr("hp");
        var sPropScore = AddStr("Score");
        var sEventHit = AddStr("OnHit");
        var sObj = AddStr("Dummy");
        var stringTable = strStream.ToArray();

        var litData = Encoding.UTF8.GetBytes(expectedLiteral);

        var sections = new Dictionary<string, byte[]>(StringComparer.Ordinal);

        // stringLiteral: 1 entry (length, dataIndex)
        var stringLitTable = new byte[8];
        BinaryPrimitives.WriteUInt32LittleEndian(stringLitTable.AsSpan(0, 4), (uint)litData.Length);
        BinaryPrimitives.WriteInt32LittleEndian(stringLitTable.AsSpan(4, 4), 0);
        sections["stringLiteral"] = stringLitTable;
        sections["stringLiteralData"] = litData;
        sections["string"] = stringTable;

        // events (1), properties (1), methods (1), parameters (1), fields (1)
        sections["events"] = WriteRecord(Il2CppEventDefinition.SizeOf(v), w =>
        {
            w.Write(sEventHit); w.Write(0); w.Write(-1); w.Write(-1); w.Write(-1);
            if (v.HasCustomAttributeIndex) w.Write(0);
            if (v.HasTokens) w.Write(0x14000001u);
        });
        sections["properties"] = WriteRecord(Il2CppPropertyDefinition.SizeOf(v), w =>
        {
            w.Write(sPropScore); w.Write(0); w.Write(-1); w.Write(0u);
            if (v.HasCustomAttributeIndex) w.Write(0);
            if (v.HasTokens) w.Write(0x17000001u);
        });
        sections["methods"] = WriteRecord(Il2CppMethodDefinition.SizeOf(v), w =>
        {
            w.Write(sRun); w.Write(0); w.Write(0);
            if (v.HasReturnParameterToken) w.Write(0u);
            w.Write(0); // paramStart
            if (v.HasCustomAttributeIndex) w.Write(0);
            w.Write(-1); // genericContainerIndex
            if (v.HasCodegenIndices)
            {
                w.Write(0);  // methodIndex
                w.Write(0);  // invokerIndex
                w.Write(-1); // delegateWrapperIndex
                w.Write(-1); // rgctxStartIndex
                w.Write(0);  // rgctxCount
            }
            w.Write(rawMethodToken); // token
            w.Write((ushort)0x0086); // Public
            w.Write((ushort)0);
            w.Write(ushort.MaxValue);
            w.Write((ushort)1);
        });
        sections["parameters"] = WriteRecord(Il2CppParameterDefinition.SizeOf(v), w =>
        {
            w.Write(sParamX);
            w.Write(0x08000001u);
            if (v.HasCustomAttributeIndex) w.Write(0);
            w.Write(0);
        });
        sections["fields"] = WriteRecord(Il2CppFieldDefinition.SizeOf(v), w =>
        {
            w.Write(sFieldHp);
            w.Write(0);
            if (v.HasCustomAttributeIndex) w.Write(0);
            if (v.HasTokens) w.Write(0x04000001u);
        });

        // TypeDefinitions: 2 types (Player with 1 field, Dummy in mscorlib with 0 fields) so Fields.Length (1) != TypeDefinitions.Length (2)
        var tdSize = Il2CppTypeDefinition.SizeOf(v);
        var tdBytes = new byte[tdSize * 2];
        WriteTypeDef(tdBytes.AsSpan(0, tdSize), v, sPlayer, sNs, methodStart: 0, methodCount: 1, fieldStart: 0, fieldCount: 1, propCount: 1, eventCount: 1, token: 0x02000001u);
        WriteTypeDef(tdBytes.AsSpan(tdSize, tdSize), v, sObj, sNs, methodStart: 0, methodCount: 0, fieldStart: 0, fieldCount: 0, propCount: 0, eventCount: 0, token: 0x02000002u);
        sections["typeDefinitions"] = tdBytes;

        // Images (2) & Assemblies (2)
        var imgSize = Il2CppImageDefinition.SizeOf(v);
        var imgBytes = new byte[imgSize * 2];
        WriteImage(imgBytes.AsSpan(0, imgSize), v, sGameAsmImg, asmIdx: 0, typeStart: 0, typeCount: 1);
        WriteImage(imgBytes.AsSpan(imgSize, imgSize), v, sMscorlibImg, asmIdx: 1, typeStart: 1, typeCount: 1);
        sections["images"] = imgBytes;

        var asmSize = Il2CppAssemblyDefinition.SizeOf(v);
        var asmBytes = new byte[asmSize * 2];
        WriteAssembly(asmBytes.AsSpan(0, asmSize), v, imageIdx: 0, refStart: 0, refCount: v.HasReferencedAssemblies ? 1 : 0, nameIdx: sGameAsm, cultureIdx: sEmpty);
        WriteAssembly(asmBytes.AsSpan(asmSize, asmSize), v, imageIdx: 1, refStart: 0, refCount: 0, nameIdx: sMscorlib, cultureIdx: sEmpty);
        sections["assemblies"] = asmBytes;

        if (v.HasReferencedAssemblies)
        {
            var refBytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(refBytes, 1); // mscorlib
            sections["referencedAssemblies"] = refBytes;
        }

        if (v.HasMetadataUsageTables)
        {
            var list = new byte[8];
            BinaryPrimitives.WriteUInt32LittleEndian(list.AsSpan(0, 4), 0u);
            BinaryPrimitives.WriteUInt32LittleEndian(list.AsSpan(4, 4), 1u);
            sections["metadataUsageLists"] = list;

            var pair = new byte[8];
            BinaryPrimitives.WriteUInt32LittleEndian(pair.AsSpan(0, 4), 0u);
            BinaryPrimitives.WriteUInt32LittleEndian(pair.AsSpan(4, 4), (5u << 29) | 0u); // StringLiteral #0
            sections["metadataUsagePairs"] = pair;
        }

        if (v.HasCustomAttributeTables)
        {
            var range = new byte[Il2CppCustomAttributeTypeRange.SizeOf(v)];
            // ImageAttributeRanges is false pre-24.1 -> 8 bytes: (start, count)
            BinaryPrimitives.WriteInt32LittleEndian(range.AsSpan(0, 4), 0);
            BinaryPrimitives.WriteInt32LittleEndian(range.AsSpan(4, 4), 1);
            sections["attributesInfo"] = range;

            var attrTypes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(attrTypes, 0);
            sections["attributeTypes"] = attrTypes;
        }

        // Lay out sections sequentially after the exact header size
        var sectionDefs = MetadataHeader.SectionNames(v);
        var headerSize = MetadataHeader.SizeFor(v);
        using var file = new MemoryStream();
        file.Write(new byte[headerSize]);

        Span<byte> hdr = new byte[headerSize];
        BinaryPrimitives.WriteUInt32LittleEndian(hdr.Slice(0, 4), MetadataHeader.ExpectedSanity);
        BinaryPrimitives.WriteInt32LittleEndian(hdr.Slice(4, 4), v.Major);

        for (var idx = 0; idx < sectionDefs.Count; idx++)
        {
            var name = sectionDefs[idx].Name;
            var payload = sections.GetValueOrDefault(name, []);
            var offset = (int)file.Position;
            if (payload.Length > 0) file.Write(payload);
            BinaryPrimitives.WriteInt32LittleEndian(hdr.Slice(8 + idx * 8, 4), offset);
            BinaryPrimitives.WriteInt32LittleEndian(hdr.Slice(8 + idx * 8 + 4, 4), payload.Length);
        }

        var result = file.ToArray();
        hdr.CopyTo(result.AsSpan(0, headerSize));
        return result;
    }

    private static byte[] WriteRecord(int expectedSize, Action<BinaryWriter> write)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        write(bw);
        bw.Flush();
        var bytes = ms.ToArray();
        Assert.Equal(expectedSize, bytes.Length);
        return bytes;
    }

    private static void W32(Span<byte> d, ref int pos, int val) { BinaryPrimitives.WriteInt32LittleEndian(d.Slice(pos, 4), val); pos += 4; }
    private static void WU32(Span<byte> d, ref int pos, uint val) { BinaryPrimitives.WriteUInt32LittleEndian(d.Slice(pos, 4), val); pos += 4; }
    private static void WU16(Span<byte> d, ref int pos, ushort val) { BinaryPrimitives.WriteUInt16LittleEndian(d.Slice(pos, 2), val); pos += 2; }

    private static void WriteTypeDef(Span<byte> dst, MetadataVersion v, int nameIdx, int nsIdx,
        int methodStart, ushort methodCount, int fieldStart, ushort fieldCount, ushort propCount, ushort eventCount, uint token)
    {
        var pos = 0;
        W32(dst, ref pos, nameIdx);
        W32(dst, ref pos, nsIdx);
        if (v.HasCustomAttributeIndex) W32(dst, ref pos, 0);
        W32(dst, ref pos, 0); // byvalTypeIndex
        if (v.HasByrefTypeIndex) W32(dst, ref pos, -1);
        W32(dst, ref pos, -1); W32(dst, ref pos, -1); W32(dst, ref pos, -1); // declaring, parent, element
        if (v.HasCodegenIndices) { W32(dst, ref pos, -1); W32(dst, ref pos, 0); }
        W32(dst, ref pos, -1); // genericContainerIndex
        if (v.HasTypeDelegateAndMarshalingIndices) { W32(dst, ref pos, -1); W32(dst, ref pos, -1); }
        if (v.HasTypeCcwAndGuidIndices) { W32(dst, ref pos, -1); W32(dst, ref pos, -1); }
        WU32(dst, ref pos, 0x00100001u); // Public class
        W32(dst, ref pos, fieldStart);
        W32(dst, ref pos, methodStart);
        W32(dst, ref pos, 0); // eventStart
        W32(dst, ref pos, 0); // propertyStart
        W32(dst, ref pos, 0); // nestedTypesStart
        W32(dst, ref pos, 0); // interfacesStart
        W32(dst, ref pos, 0); // vtableStart
        W32(dst, ref pos, 0); // interfaceOffsetsStart
        WU16(dst, ref pos, methodCount);
        WU16(dst, ref pos, propCount);
        WU16(dst, ref pos, fieldCount);
        WU16(dst, ref pos, eventCount);
        WU16(dst, ref pos, 0); WU16(dst, ref pos, 0); WU16(dst, ref pos, 0); WU16(dst, ref pos, 0);
        WU32(dst, ref pos, 0); // bitfield
        if (v.HasTokens) WU32(dst, ref pos, token);
        Assert.Equal(dst.Length, pos);
    }

    private static void WriteImage(Span<byte> dst, MetadataVersion v, int nameIdx, int asmIdx, int typeStart, uint typeCount)
    {
        var pos = 0;
        W32(dst, ref pos, nameIdx); W32(dst, ref pos, asmIdx); W32(dst, ref pos, typeStart); WU32(dst, ref pos, typeCount);
        if (v.HasExportedTypeDefinitions) { W32(dst, ref pos, 0); WU32(dst, ref pos, 0); }
        W32(dst, ref pos, -1);
        if (v.HasTokens) WU32(dst, ref pos, 1u);
        if (v.HasImageAttributeRanges) { W32(dst, ref pos, 0); WU32(dst, ref pos, 0); }
        Assert.Equal(dst.Length, pos);
    }

    private static void WriteAssembly(Span<byte> dst, MetadataVersion v, int imageIdx, int refStart, int refCount, int nameIdx, int cultureIdx)
    {
        var pos = 0;
        W32(dst, ref pos, imageIdx);
        W32(dst, ref pos, 0); // customAttributeIndex / token
        if (v.HasReferencedAssemblies) { W32(dst, ref pos, refStart); W32(dst, ref pos, refCount); }
        W32(dst, ref pos, nameIdx); W32(dst, ref pos, cultureIdx);
        if (v.HasHashValueIndex) W32(dst, ref pos, 0);
        for (var i = 0; i < 8; i++) W32(dst, ref pos, 0);
        pos += 8; // publicKeyToken
        Assert.Equal(dst.Length, pos);
    }

    private static SyntheticBinaryImage BuildSyntheticBinary(
        GlobalMetadata md,
        bool is64Bit,
        bool v21PointerFieldOffsets,
        out ulong expectedMethodVa,
        out int expectedFieldOffset,
        out ulong expectedUsageGlobalVa)
    {
        var ptrSize = is64Bit ? 8 : 4;
        const ulong imageBase = 0x100000;
        const ulong textRva = 0x1000;
        const ulong dataRva = 0x2000;
        const int totalSize = 0x5000;
        var buf = new byte[totalSize];

        void WritePtr(int fileOff, ulong val)
        {
            if (is64Bit) BinaryPrimitives.WriteUInt64LittleEndian(buf.AsSpan(fileOff, 8), val);
            else BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(fileOff, 4), checked((uint)val));
        }

        // Code body at RVA 0x1100
        expectedMethodVa = imageBase + 0x1100;
        BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(0x1100, 4), 0xE12FFF1E); // bx lr
        expectedFieldOffset = 0x18;
        expectedUsageGlobalVa = imageBase + 0x3800;

        // Data structures placed inside [0x2000..0x4000)
        var cursor = 0x2000;
        int Alloc(int size)
        {
            var aligned = (cursor + 15) & ~15;
            cursor = aligned + size;
            return aligned;
        }

        // Il2CppType[1]
        var type0Off = Alloc(ptrSize + 8);
        WritePtr(type0Off, 0); // klassIndex = 0
        BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(type0Off + ptrSize, 4), (uint)Il2CppTypeEnum.Class << 16);

        var typesArrOff = Alloc(ptrSize);
        WritePtr(typesArrOff, imageBase + (ulong)type0Off);

        // typeDefinitionsSizes[2]: flat 16-byte structs for v16-v20 (and early v21), pointer table otherwise
        var flatTypeSizes = (md.Version.Major <= 20) || (md.Version.Major == 21 && !v21PointerFieldOffsets);
        int tdSizesArrOff;
        if (flatTypeSizes)
        {
            tdSizesArrOff = Alloc(16 * 2);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(tdSizesArrOff, 4), 24);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(tdSizesArrOff + 16, 4), 24);
        }
        else
        {
            var tdSize0Off = Alloc(16);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(tdSize0Off, 4), 24);
            tdSizesArrOff = Alloc(ptrSize * 2);
            WritePtr(tdSizesArrOff, imageBase + (ulong)tdSize0Off);
            WritePtr(tdSizesArrOff + ptrSize, imageBase + (ulong)tdSize0Off);
        }

        // fieldOffsets (either flat int32[1] or pointer table[2])
        int fieldOffsetsOff;
        ulong fieldOffsetsCount;
        if ((md.Version.Major <= 20) || (md.Version.Major == 21 && !v21PointerFieldOffsets))
        {
            fieldOffsetsCount = (ulong)md.Fields.Length;
            fieldOffsetsOff = Alloc(4);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(fieldOffsetsOff, 4), expectedFieldOffset);
        }
        else
        {
            fieldOffsetsCount = (ulong)md.TypeDefinitions.Length;
            var fo0Off = Alloc(4);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(fo0Off, 4), expectedFieldOffset);
            fieldOffsetsOff = Alloc(ptrSize * 2);
            WritePtr(fieldOffsetsOff, imageBase + (ulong)fo0Off);
            WritePtr(fieldOffsetsOff + ptrSize, 0);
        }

        // metadataUsages[1] for v19+
        var metaUsagesOff = Alloc(ptrSize);
        WritePtr(metaUsagesOff, expectedUsageGlobalVa);

        // methodPointers[1], invokerPointers[1], customAttributeGenerators[1]
        var methodPtrsOff = Alloc(ptrSize);
        WritePtr(methodPtrsOff, expectedMethodVa);
        var invokerPtrsOff = Alloc(ptrSize);
        WritePtr(invokerPtrsOff, imageBase + 0x1100);
        var attrGenOff = Alloc(ptrSize);
        WritePtr(attrGenOff, imageBase + 0x1100);

        // Il2CppMetadataRegistration struct
        var metaRegWords = md.Version.HasMethodReferencesInMetadataReg ? 16 : 16;
        var metaRegOff = Alloc(metaRegWords * ptrSize);
        var fieldOffIdx = md.Version.HasMethodReferencesInMetadataReg ? 12 : 10;
        var typeSizeIdx = fieldOffIdx + 2;
        WritePtr(metaRegOff + 6 * ptrSize, 1); // typesCount
        WritePtr(metaRegOff + 7 * ptrSize, imageBase + (ulong)typesArrOff);
        WritePtr(metaRegOff + fieldOffIdx * ptrSize, fieldOffsetsCount);
        WritePtr(metaRegOff + (fieldOffIdx + 1) * ptrSize, imageBase + (ulong)fieldOffsetsOff);
        WritePtr(metaRegOff + typeSizeIdx * ptrSize, (ulong)md.TypeDefinitions.Length);
        WritePtr(metaRegOff + (typeSizeIdx + 1) * ptrSize, imageBase + (ulong)tdSizesArrOff);
        if (md.Version.HasMetadataUsageTables)
        {
            WritePtr(metaRegOff + 14 * ptrSize, 1);
            WritePtr(metaRegOff + 15 * ptrSize, imageBase + (ulong)metaUsagesOff);
        }

        // Il2CppCodeRegistration struct
        var (gmpIdx, invIdx, attrIdx, totalWords) = md.Version.Major switch
        {
            <= 20 => (8, 10, 12, 14),
            21 => (10, 12, 14, 18),
            22 => (10, 12, 14, 20),
            _ => (4, 6, 8, 14),
        };
        var codeRegOff = Alloc(totalWords * ptrSize);
        WritePtr(codeRegOff + 0 * ptrSize, 1); // methodPointersCount
        WritePtr(codeRegOff + 1 * ptrSize, imageBase + (ulong)methodPtrsOff);
        WritePtr(codeRegOff + gmpIdx * ptrSize, 0);
        WritePtr(codeRegOff + invIdx * ptrSize, 1);
        WritePtr(codeRegOff + (invIdx + 1) * ptrSize, imageBase + (ulong)invokerPtrsOff);
        WritePtr(codeRegOff + attrIdx * ptrSize, (ulong)md.AttributeTypeRanges.Length);
        WritePtr(codeRegOff + (attrIdx + 1) * ptrSize, imageBase + (ulong)attrGenOff);

        var sections = new BinarySection[]
        {
            new(".text", textRva, 0x1000, (long)textRva, 0x1000, IsExecutable: true, IsWritable: false, IsReadable: true),
            new(".data", dataRva, 0x2000, (long)dataRva, 0x2000, IsExecutable: false, IsWritable: true, IsReadable: true),
        };
        return new SyntheticBinaryImage(is64Bit, imageBase, buf, sections);
    }

    private static byte[] CompressGzip(ReadOnlySpan<byte> payload)
    {
        using var ms = new MemoryStream();
        using (var gz = new GZipStream(ms, CompressionLevel.Fastest, leaveOpen: true))
            gz.Write(payload);
        return ms.ToArray();
    }

    private static byte[] BuildUnityFsBundle(string unityRevision)
    {
        using var ms = new MemoryStream();
        ms.Write("UnityFS\0"u8);
        ms.Write([0, 0, 0, 6]); // format 6
        ms.Write("5.x.x\0"u8);
        ms.Write(Encoding.ASCII.GetBytes(unityRevision));
        ms.WriteByte(0);
        ms.Write(new byte[32]);
        return ms.ToArray();
    }

    private static byte[] BuildUnityWebData((string Name, byte[] Data)[] files)
    {
        const string magic = "UnityWebData1.0\0";
        var nameBytes = files.Select(f => Encoding.UTF8.GetBytes(f.Name)).ToArray();
        var headerSize = magic.Length + 4 + nameBytes.Sum(nb => 12 + nb.Length);
        var totalSize = headerSize + files.Sum(f => f.Data.Length);
        var buf = new byte[totalSize];
        Encoding.ASCII.GetBytes(magic, buf.AsSpan(0, magic.Length));
        BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(magic.Length, 4), headerSize);

        var hdrPos = magic.Length + 4;
        var dataPos = headerSize;
        for (var i = 0; i < files.Length; i++)
        {
            var nb = nameBytes[i];
            var payload = files[i].Data;
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(hdrPos, 4), dataPos);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(hdrPos + 4, 4), payload.Length);
            BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(hdrPos + 8, 4), nb.Length);
            nb.CopyTo(buf.AsSpan(hdrPos + 12));
            hdrPos += 12 + nb.Length;

            payload.CopyTo(buf.AsSpan(dataPos));
            dataPos += payload.Length;
        }
        return buf;
    }

    private sealed class SyntheticBinaryImage(bool is64Bit, ulong imageBase, byte[] data, IReadOnlyList<BinarySection> sections) : IBinaryImage
    {
        public string Path => is64Bit ? "synthetic64.so" : "synthetic32.so";
        public BinaryFormat Format => BinaryFormat.Elf;
        public CpuArchitecture Architecture => is64Bit ? CpuArchitecture.Arm64 : CpuArchitecture.Arm;
        public bool Is64Bit => is64Bit;
        public ulong ImageBase => imageBase;
        public IReadOnlyList<BinarySection> Sections => sections;
        public ReadOnlyMemory<byte> Data => data;
        public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(sections, rva, out fileOffset);
        public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(sections, fileOffset, out rva);
        public void Dispose() { }
    }
}

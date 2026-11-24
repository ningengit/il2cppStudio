using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

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
            Assert.Equal(!((major <= 20) || (major == 21 && !v21PointerFieldOffsets)), reg.FieldOffsetsArePointers);
            Assert.Equal(expectedFieldOffset, reg.GetFieldOffset(0, 0));

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
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR", prevCacheDir);
            if (Directory.Exists(tmpDir)) Directory.Delete(tmpDir, recursive: true);
        }
    }

    // ------------------------------------------------------------------ synthetic fixtures

    private static byte[] BuildSyntheticMetadata(MetadataVersion v, out string expectedLiteral)
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
            w.Write(0x06000001u); // token
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

        // typeDefinitionsSizes[2]
        var tdSize0Off = Alloc(16);
        BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(tdSize0Off, 4), 24);
        var tdSizesArrOff = Alloc(ptrSize * 2);
        WritePtr(tdSizesArrOff, imageBase + (ulong)tdSize0Off);
        WritePtr(tdSizesArrOff + ptrSize, imageBase + (ulong)tdSize0Off);

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

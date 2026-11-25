using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

public sealed class ArchiveAndSplitApkTests
{
    private static DiagnosticCollector Sink() => new(new Serilog.LoggerConfiguration().CreateLogger());

    private static byte[] BuildSyntheticV29Metadata()
    {
        // Minimal header so GlobalMetadata recognizes magic + version cleanly or test raw byte slicing.
        var buf = new byte[512];
        BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(0, 4), MetadataHeader.ExpectedSanity);
        BinaryPrimitives.WriteInt32LittleEndian(buf.AsSpan(4, 4), 29);
        return buf;
    }

    private static byte[] BuildUnityWebData(params (string Name, byte[] Payload)[] entries)
    {
        var magic = "UnityWebData1.0\0"u8;
        var nameBytes = entries.Select(e => Encoding.UTF8.GetBytes(e.Name)).ToArray();
        var headerSize = magic.Length + 4 + entries.Length * 12 + nameBytes.Sum(b => b.Length);
        var totalSize = headerSize + entries.Sum(e => e.Payload.Length);
        var blob = new byte[totalSize];
        magic.CopyTo(blob);
        BinaryPrimitives.WriteInt32LittleEndian(blob.AsSpan(magic.Length, 4), headerSize);
        var hPos = magic.Length + 4;
        var dPos = headerSize;
        for (var i = 0; i < entries.Length; i++)
        {
            BinaryPrimitives.WriteInt32LittleEndian(blob.AsSpan(hPos, 4), dPos);
            BinaryPrimitives.WriteInt32LittleEndian(blob.AsSpan(hPos + 4, 4), entries[i].Payload.Length);
            BinaryPrimitives.WriteInt32LittleEndian(blob.AsSpan(hPos + 8, 4), nameBytes[i].Length);
            nameBytes[i].CopyTo(blob.AsSpan(hPos + 12));
            hPos += 12 + nameBytes[i].Length;
            entries[i].Payload.CopyTo(blob.AsSpan(dPos));
            dPos += entries[i].Payload.Length;
        }
        return blob;
    }

    [Fact]
    public void ApkExtractor_ExtractBundle_MergesSplitApksAndNativeLibraryDir()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-split-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            // Minimal valid ELF64 ARM64 header + PT_LOAD header
            var fakeSo = new byte[128];
            fakeSo[0] = 0x7F; fakeSo[1] = (byte)'E'; fakeSo[2] = (byte)'L'; fakeSo[3] = (byte)'F';
            fakeSo[4] = 2; // ELF64
            fakeSo[5] = 1; // LE
            BinaryPrimitives.WriteUInt16LittleEndian(fakeSo.AsSpan(18, 2), 0xB7); // EM_AARCH64

            var fakeMeta = new byte[64];
            BinaryPrimitives.WriteUInt32LittleEndian(fakeMeta.AsSpan(0, 4), MetadataHeader.ExpectedSanity);
            BinaryPrimitives.WriteInt32LittleEndian(fakeMeta.AsSpan(4, 4), 29);

            // Base APK contains only global-metadata.dat
            var baseApk = Path.Combine(tmp, "base.apk");
            using (var zip = ZipFile.Open(baseApk, ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat", CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(fakeMeta);
            }

            // Split APK contains lib/arm64-v8a/libil2cpp.so
            var splitApk = Path.Combine(tmp, "split_config.arm64_v8a.apk");
            using (var zip = ZipFile.Open(splitApk, ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("lib/arm64-v8a/libil2cpp.so", CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(fakeSo);
            }

            var extracted1 = ApkExtractor.ExtractBundle(baseApk, [splitApk], nativeLibraryDir: null, Sink());
            Assert.NotNull(extracted1);
            Assert.True(File.Exists(Path.Combine(extracted1!, "assets", "bin", "Data", "Managed", "Metadata", "global-metadata.dat")));
            Assert.True(File.Exists(Path.Combine(extracted1, "lib", "arm64-v8a", "libil2cpp.so")));

            // NativeLibraryDir fallback when split APK lacks libil2cpp.so (extracted native libs)
            var nativeDir = Path.Combine(tmp, "native", "arm64");
            Directory.CreateDirectory(nativeDir);
            var diskSo = Path.Combine(nativeDir, "libil2cpp.so");
            File.WriteAllBytes(diskSo, fakeSo);

            var extracted2 = ApkExtractor.ExtractBundle(baseApk, splitApkPaths: null, nativeLibraryDir: nativeDir, Sink());
            Assert.NotNull(extracted2);
            Assert.NotEqual(extracted1, extracted2);
            var fallbackSo = Path.Combine(extracted2!, "lib", "arm64-v8a", "libil2cpp.so");
            Assert.True(File.Exists(fallbackSo));
            Assert.Equal(fakeSo, File.ReadAllBytes(fallbackSo));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void ApkAndXapk_ZeroCopyStored_MapsWithoutTempFiles()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-zc-apk-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            var metaPayload = BuildSyntheticV29Metadata();
            var soPayload = new byte[256];
            soPayload[0] = 0x7F; soPayload[1] = (byte)'E'; soPayload[2] = (byte)'L'; soPayload[3] = (byte)'F';
            soPayload[4] = 2; soPayload[5] = 1;
            BinaryPrimitives.WriteUInt16LittleEndian(soPayload.AsSpan(18, 2), 0xB7);

            // 1. Direct Stored APK
            var apkPath = Path.Combine(tmp, "stored.apk");
            using (var zip = ZipFile.Open(apkPath, ZipArchiveMode.Create))
            {
                var me = zip.CreateEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat", CompressionLevel.NoCompression);
                using (var s = me.Open()) s.Write(metaPayload);
                var se = zip.CreateEntry("lib/arm64-v8a/libil2cpp.so", CompressionLevel.NoCompression);
                using (var s = se.Open()) s.Write(soPayload);
            }

            using (var session = ApkExtractor.OpenZeroCopy(apkPath, null, null, Sink()))
            {
                Assert.NotNull(session);
                Assert.NotNull(session!.Metadata);
                Assert.True(session.Metadata!.IsZeroCopy);
                Assert.Equal(metaPayload, session.Metadata.Span.ToArray());
                Assert.NotNull(session.Binary);
                Assert.True(session.Binary!.IsZeroCopy);
                Assert.Equal(soPayload, session.Binary.Span.ToArray());
            }

            // 2. Outer Stored XAPK wrapping a nested Stored APK
            var xapkPath = Path.Combine(tmp, "bundle.xapk");
            using (var zip = ZipFile.Open(xapkPath, ZipArchiveMode.Create))
            {
                var nestedEntry = zip.CreateEntry("config.arm64_v8a.apk", CompressionLevel.NoCompression);
                using var ns = nestedEntry.Open();
                ns.Write(File.ReadAllBytes(apkPath));
            }

            using (var session = ApkExtractor.OpenZeroCopy(xapkPath, null, null, Sink()))
            {
                Assert.NotNull(session);
                Assert.NotNull(session!.Metadata);
                Assert.True(session.Metadata!.IsZeroCopy);
                Assert.Equal(metaPayload, session.Metadata.Span.ToArray());
                Assert.NotNull(session.Binary);
                Assert.True(session.Binary!.IsZeroCopy);
            }

            // 3. Deflate-compressed APK streams purely in memory
            var deflatedApk = Path.Combine(tmp, "deflated.apk");
            using (var zip = ZipFile.Open(deflatedApk, ZipArchiveMode.Create))
            {
                var me = zip.CreateEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat", CompressionLevel.SmallestSize);
                using (var s = me.Open()) s.Write(metaPayload);
            }

            using (var session = ApkExtractor.OpenZeroCopy(deflatedApk, null, null, Sink()))
            {
                Assert.NotNull(session);
                Assert.NotNull(session!.Metadata);
                Assert.False(session.Metadata!.IsZeroCopy);
                Assert.Equal(metaPayload, session.Metadata.Span.ToArray());
            }
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void WebData_ZeroCopyUncompressedAndInMemoryCompressed_ResolveCleanly()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-zc-web-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            var metaPayload = BuildSyntheticV29Metadata();
            var rawBlob = BuildUnityWebData(("Il2CppData/Metadata/global-metadata.dat", metaPayload));
            var rawPath = Path.Combine(tmp, "build.data");
            File.WriteAllBytes(rawPath, rawBlob);

            using (var session = WebDataExtractor.OpenZeroCopy(rawPath, Sink()))
            {
                Assert.NotNull(session);
                Assert.NotNull(session!.Metadata);
                Assert.True(session.Metadata!.IsZeroCopy);
                Assert.Equal(metaPayload, session.Metadata.Span.ToArray());
            }

            var gzPath = Path.Combine(tmp, "build.data.gz");
            using (var fs = File.Create(gzPath))
            using (var gz = new GZipStream(fs, CompressionLevel.SmallestSize))
                gz.Write(rawBlob);

            using (var session = WebDataExtractor.OpenZeroCopy(gzPath, Sink()))
            {
                Assert.NotNull(session);
                Assert.NotNull(session!.Metadata);
                Assert.False(session.Metadata!.IsZeroCopy);
                Assert.Equal(metaPayload, session.Metadata.Span.ToArray());
            }
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Theory]
    [InlineData("sample game unity android.apk")]
    [InlineData("unity 5.3 android/sum.apk")]
    [InlineData("Template il2cpp webgl")]
    public void RealContainers_LoadCleanlyViaZeroCopySession(string target)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", target);
        if (path is null || (!File.Exists(path) && !Directory.Exists(path)))
            Assert.Skip($"Test target '{target}' not found.");

        var sink = Sink();
        using var ws = Il2CppWorkspace.Load(path!, sink);
        Assert.NotNull(ws.Files.Archive);
        Assert.NotNull(ws.Registration);
        Assert.True(ws.Registration!.IsComplete);
        Assert.Empty(sink.Query(DiagnosticSeverity.Error));
    }

    [Fact]
    public void ZeroCopyArchive_ExportsManagedResourcesDatWithFullParity()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-zc-res-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            const string entryName = "strings.txt";
            var resNameBytes = Encoding.UTF8.GetBytes(entryName);
            var resPayload = Encoding.UTF8.GetBytes("greeting=hello-zero-copy\n");
            var headerSize = 4 + 8 + resNameBytes.Length;
            var datBytes = new byte[4 + headerSize + resPayload.Length];
            BinaryPrimitives.WriteInt32LittleEndian(datBytes.AsSpan(0, 4), headerSize);
            BinaryPrimitives.WriteInt32LittleEndian(datBytes.AsSpan(4, 4), 1);
            BinaryPrimitives.WriteInt32LittleEndian(datBytes.AsSpan(8, 4), resPayload.Length);
            BinaryPrimitives.WriteInt32LittleEndian(datBytes.AsSpan(12, 4), resNameBytes.Length);
            resNameBytes.CopyTo(datBytes.AsSpan(16));
            resPayload.CopyTo(datBytes.AsSpan(4 + headerSize));

            var apkPath = Path.Combine(tmp, "res-test.apk");
            using (var zip = ZipFile.Open(apkPath, ZipArchiveMode.Create))
            {
                var re = zip.CreateEntry("assets/bin/Data/Managed/Resources/mscorlib.dll-resources.dat", CompressionLevel.NoCompression);
                using var s = re.Open();
                s.Write(datBytes);
            }

            var sink = Sink();
            using var session = ApkExtractor.OpenZeroCopy(apkPath, null, null, sink);
            Assert.NotNull(session);
            Assert.True(session!.TryGetDataFile("mscorlib.dll-resources.dat", out var datBuf));
            Assert.True(datBuf.IsZeroCopy);

            var outDir = Path.Combine(tmp, "exported-resources");
            var files = new List<string>();
            var exported = WorkspaceSerializer.ExportResources(resourcesFolder: null, outDir, sink, files, session);
            Assert.Single(exported);
            Assert.Equal("mscorlib.dll", exported[0].Assembly);
            Assert.Equal(entryName, exported[0].Name);

            var extractedPath = Path.Combine(outDir, "mscorlib.dll", entryName);
            Assert.True(File.Exists(extractedPath));
            Assert.Equal(resPayload, File.ReadAllBytes(extractedPath));
            Assert.True(File.Exists(Path.Combine(outDir, "resources.json")));
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Theory]
    [InlineData(".apks")]
    [InlineData(".apkm")]
    public void SplitContainerArchive_ZeroCopyLocatesMetadataAndSplitNativeSo(string extension)
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-split-{extension.TrimStart('.')}-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            var metaPayload = BuildSyntheticV29Metadata();
            var soPayload = new byte[64];
            soPayload[0] = 0x7F; soPayload[1] = (byte)'E'; soPayload[2] = (byte)'L'; soPayload[3] = (byte)'F';
            soPayload[4] = 2; soPayload[5] = 1; soPayload[18] = 0xB7;

            var baseApk = Path.Combine(tmp, "base.apk");
            using (var zip = System.IO.Compression.ZipFile.Open(baseApk, System.IO.Compression.ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat", System.IO.Compression.CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(metaPayload);
            }

            var splitApk = Path.Combine(tmp, "split_config.arm64_v8a.apk");
            using (var zip = System.IO.Compression.ZipFile.Open(splitApk, System.IO.Compression.ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("lib/arm64-v8a/libil2cpp.so", System.IO.Compression.CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(soPayload);
            }

            var bundlePath = Path.Combine(tmp, $"game{extension}");
            using (var zip = System.IO.Compression.ZipFile.Open(bundlePath, System.IO.Compression.ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(baseApk, "base.apk", System.IO.Compression.CompressionLevel.NoCompression);
                zip.CreateEntryFromFile(splitApk, "split_config.arm64_v8a.apk", System.IO.Compression.CompressionLevel.NoCompression);
            }

            var sink = Sink();
            var files = GameFolderLocator.Locate(bundlePath, sink);
            using var archive = files.Archive;

            Assert.NotNull(files.MetadataPath);
            Assert.NotNull(archive);
            Assert.NotNull(archive!.Metadata);
            Assert.NotNull(archive.Binary);
            Assert.Equal(metaPayload, archive.Metadata!.ToByteArray());
            Assert.Equal(soPayload, archive.Binary!.ToByteArray());
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }

    [Fact]
    public void ExtractedSplitApkFolder_LocatesBaseMetadataAndSplitNativeSo()
    {
        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-splitdir-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmp);
        try
        {
            var metaPayload = BuildSyntheticV29Metadata();
            var soPayload = new byte[64];
            soPayload[0] = 0x7F; soPayload[1] = (byte)'E'; soPayload[2] = (byte)'L'; soPayload[3] = (byte)'F';
            soPayload[4] = 2; soPayload[5] = 1; soPayload[18] = 0xB7;

            var baseApk = Path.Combine(tmp, "base.apk");
            using (var zip = System.IO.Compression.ZipFile.Open(baseApk, System.IO.Compression.ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("assets/bin/Data/Managed/Metadata/global-metadata.dat", System.IO.Compression.CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(metaPayload);
            }

            var splitApk = Path.Combine(tmp, "split_config.arm64_v8a.apk");
            using (var zip = System.IO.Compression.ZipFile.Open(splitApk, System.IO.Compression.ZipArchiveMode.Create))
            {
                var e = zip.CreateEntry("lib/arm64-v8a/libil2cpp.so", System.IO.Compression.CompressionLevel.NoCompression);
                using var s = e.Open();
                s.Write(soPayload);
            }

            var sink = Sink();
            var files = GameFolderLocator.Locate(tmp, sink);
            using var archive = files.Archive;

            Assert.NotNull(files.MetadataPath);
            Assert.NotNull(files.BinaryPath);
            Assert.Contains("base.apk", files.MetadataPath, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("split_config.arm64_v8a.apk", files.BinaryPath, StringComparison.OrdinalIgnoreCase);
            Assert.NotNull(archive);
            Assert.NotNull(archive!.Metadata);
            Assert.NotNull(archive.Binary);
            Assert.Equal(metaPayload, archive.Metadata!.ToByteArray());
            Assert.Equal(soPayload, archive.Binary!.ToByteArray());
            Assert.Empty(sink.Query(DiagnosticSeverity.Error));
        }
        finally
        {
            if (Directory.Exists(tmp)) Directory.Delete(tmp, recursive: true);
        }
    }
}

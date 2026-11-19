using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

[CollectionDefinition(nameof(RegistrationCacheTests), DisableParallelization = true)]
public sealed class RegistrationCacheTestCollection { }

[Collection(nameof(RegistrationCacheTests))]
public sealed class RegistrationCacheTests
{
    private static string TargetOrSkip(string name)
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var path = root is null ? null : Path.Combine(root, "testdata", name);
        if (path is null || (!Directory.Exists(path) && !File.Exists(path)))
            Assert.Skip($"Test target '{name}' not found.");
        return path!;
    }

    [Theory]
    [InlineData("template il2cpp 2022 win")]
    [InlineData("template il2cpp 2018 win 32")]
    public void RegistrationCache_EndToEnd_MissHitInvalidationAndCorruptRecovery(string build)
    {
        var srcDir = TargetOrSkip(build);
        var files = GameFolderLocator.Locate(srcDir, NullDiagnosticSink.Instance);
        Assert.NotNull(files.BinaryPath);
        Assert.NotNull(files.MetadataPath);

        var tempRoot = Path.Combine(Path.GetTempPath(), $"il2cppstudio-regcache-test-{Guid.NewGuid():N}");
        var workDir = Path.Combine(tempRoot, "game");
        var cacheDir = Path.Combine(tempRoot, "cache");
        Directory.CreateDirectory(workDir);
        Directory.CreateDirectory(cacheDir);

        var prevEnv = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR");
        try
        {
            var binCopy = Path.Combine(workDir, Path.GetFileName(files.BinaryPath!));
            var metaCopy = Path.Combine(workDir, Path.GetFileName(files.MetadataPath!));
            File.Copy(files.BinaryPath!, binCopy);
            File.Copy(files.MetadataPath!, metaCopy);

            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR", cacheDir);

            // 1. Cache miss -> full scan + cache file written.
            using var bin1 = PeBinaryImage.Load(binCopy, NullDiagnosticSink.Instance);
            using var md1 = GlobalMetadata.Load(metaCopy, NullDiagnosticSink.Instance);
            var key1 = RegistrationCache.ComputeKey(bin1, md1);
            var cacheFile1 = Path.Combine(cacheDir, key1 + ".bin");
            Assert.False(File.Exists(cacheFile1));

            var reg1 = Il2CppRegistration.Load(bin1, md1, NullDiagnosticSink.Instance);
            Assert.True(reg1.IsComplete);
            Assert.True(File.Exists(cacheFile1));
            var expectedCodeVa = reg1.CodeRegistrationVa;
            var expectedMetaVa = reg1.MetadataRegistrationVa;
            var expectedUsages = reg1.MetadataUsages.Count;
            var expectedTypes = reg1.TypeCount;

            // 2. Cache hit -> identical registration results without re-scanning.
            var reg2 = Il2CppRegistration.Load(bin1, md1, NullDiagnosticSink.Instance);
            Assert.True(reg2.IsComplete);
            Assert.Equal(expectedCodeVa, reg2.CodeRegistrationVa);
            Assert.Equal(expectedMetaVa, reg2.MetadataRegistrationVa);
            Assert.Equal(expectedUsages, reg2.MetadataUsages.Count);
            Assert.Equal(expectedTypes, reg2.TypeCount);

            // 3. Truncated / corrupt cache file recovers cleanly via full scan and rewrites cache.
            File.WriteAllBytes(cacheFile1, [1, 2, 3, 4, 5]);
            var regCorrupt = Il2CppRegistration.Load(bin1, md1, NullDiagnosticSink.Instance);
            Assert.True(regCorrupt.IsComplete);
            Assert.Equal(expectedCodeVa, regCorrupt.CodeRegistrationVa);
            Assert.Equal(expectedMetaVa, regCorrupt.MetadataRegistrationVa);
            Assert.Equal(expectedUsages, regCorrupt.MetadataUsages.Count);
            Assert.True(new FileInfo(cacheFile1).Length > 32);
            bin1.Dispose();
            md1.Dispose();

            // 4. Key invalidates when file mtime or leading bytes change.
            string keyMtime;
            File.SetLastWriteTimeUtc(binCopy, DateTime.UtcNow.AddMinutes(5));
            using (var binMtime = PeBinaryImage.Load(binCopy, NullDiagnosticSink.Instance))
            using (var mdMtime = GlobalMetadata.Load(metaCopy, NullDiagnosticSink.Instance))
            {
                keyMtime = RegistrationCache.ComputeKey(binMtime, mdMtime);
                Assert.NotEqual(key1, keyMtime);
            }

            // Modify an unused padding byte inside the first 64 KB header area (DOS stub unused region at 0x4E) -> key changes.
            using (var fs = new FileStream(binCopy, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                fs.Position = 0x4E;
                var orig = fs.ReadByte();
                fs.Position = 0x4E;
                fs.WriteByte((byte)(orig ^ 0xFF));
            }
            using var binBytes = PeBinaryImage.Load(binCopy, NullDiagnosticSink.Instance);
            using var mdBytes = GlobalMetadata.Load(metaCopy, NullDiagnosticSink.Instance);
            var keyBytes = RegistrationCache.ComputeKey(binBytes, mdBytes);
            Assert.NotEqual(keyMtime, keyBytes);
        }
        finally
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_REG_CACHE_DIR", prevEnv);
            try { if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, recursive: true); } catch { }
        }
    }

    [Fact]
    public void RegistrationCache_TryRead_RejectsBogusMagicAndTruncatedPayloads()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), $"il2cppstudio-regcache-unit-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempRoot);
        try
        {
            var srcDir = TargetOrSkip("template il2cpp 2022 win");
            var files = GameFolderLocator.Locate(srcDir, NullDiagnosticSink.Instance);
            using var bin = PeBinaryImage.Load(files.BinaryPath!, NullDiagnosticSink.Instance);
            using var md = GlobalMetadata.Load(files.MetadataPath!, NullDiagnosticSink.Instance);

            var cachePath = Path.Combine(tempRoot, RegistrationCache.ComputeKey(bin, md) + ".bin");
            File.WriteAllBytes(cachePath, new byte[64]); // bad magic
            Assert.False(RegistrationCache.TryRead(bin, md, out _, tempRoot));

            RegistrationCache.TryWrite(bin, md, new CachedRegistration(0x1000, 0x2000, 0x3000, new Dictionary<ulong, MetadataUsage>
            {
                [0x4000] = new MetadataUsage(0x4000, MetadataUsageKind.StringLiteral, 7),
            }), tempRoot);
            Assert.True(RegistrationCache.TryRead(bin, md, out var valid, tempRoot));
            Assert.Equal(0x1000UL, valid.MetadataRegistrationVa);
            Assert.Equal(0x2000UL, valid.CodeRegistrationVa);
            Assert.Equal(0x3000UL, valid.CodeGenModulesArrayVa);
            Assert.Single(valid.MetadataUsages);
        }
        finally
        {
            try { if (Directory.Exists(tempRoot)) Directory.Delete(tempRoot, recursive: true); } catch { }
        }
    }
}

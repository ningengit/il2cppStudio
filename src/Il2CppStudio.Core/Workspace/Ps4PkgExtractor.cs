using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts IL2CPP binaries (<c>Media/Modules/Il2CppUserAssemblies.prx</c>), <c>global-metadata.dat</c>, and Unity
/// data headers from an unencrypted PlayStation 4 <c>.pkg</c> container (or delegates encrypted PFS images to an
/// optional user-supplied <c>pkgextract</c> binary on <c>PATH</c> / <c>IL2CPPSTUDIO_PKGEXTRACT</c>) into a temp cache.
/// </summary>
public static class Ps4PkgExtractor
{
    private const uint CntMagicBe = 0x7F434E54;
    private const uint PfsMagic = 0x01332A0B;
    private const int Dinode32Size = 0xA8;

    private static readonly HashSet<string> WantedNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Il2CppUserAssemblies.prx",
        "global-metadata.dat",
        "globalgamemanagers",
        "data.unity3d",
        "boot.config",
        "ScriptingAssemblies.json",
        "app.info",
        "eboot.bin",
    };

    public static bool IsPkgPath(string path) =>
        Path.GetExtension(path).Equals(".pkg", StringComparison.OrdinalIgnoreCase);

    public static string? Extract(string pkgPath, IDiagnosticSink sink)
    {
        var info = new FileInfo(pkgPath);
        if (!info.Exists)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.GameFolderNotFound,
                $"'{pkgPath}' does not exist.", new() { File = pkgPath });
            return null;
        }

        var cache = Path.Combine(Path.GetTempPath(), "Il2CppStudio", "pkg",
            $"{Path.GetFileName(pkgPath)}-{info.Length:X}-{info.LastWriteTimeUtc.Ticks:X}");
        if (File.Exists(Path.Combine(cache, ".complete")) && HasMetadata(cache))
            return cache;

        try
        {
            using var fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length < 0x500)
                throw new InvalidDataException("File is too small to be a PS4 .pkg container.");

            Span<byte> hdr = stackalloc byte[0x480];
            fs.ReadExactly(hdr);
            if (BinaryPrimitives.ReadUInt32BigEndian(hdr) != CntMagicBe)
                throw new InvalidDataException("Missing PS4 CNT (.pkg) header magic 0x7F434E54.");

            var pfsOffset = BinaryPrimitives.ReadUInt64BigEndian(hdr.Slice(0x410, 8));
            var pfsSize = BinaryPrimitives.ReadUInt64BigEndian(hdr.Slice(0x418, 8));
            if (pfsOffset == 0 || pfsOffset >= (ulong)fs.Length)
                throw new InvalidDataException($"Invalid PFS image offset 0x{pfsOffset:X} in PS4 .pkg.");

            fs.Position = (long)pfsOffset;
            Span<byte> pfsSuper = stackalloc byte[0x60];
            fs.ReadExactly(pfsSuper);
            if (BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(8, 4)) != PfsMagic)
                throw new InvalidDataException("Missing PFS superblock magic in PS4 .pkg.");

            var mode = BinaryPrimitives.ReadUInt16LittleEndian(pfsSuper.Slice(26, 2));
            if ((mode & 0x5) != 0)
            {
                if (TryExtractWithTool(pkgPath, cache, sink))
                    return cache;

                sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                    $"PS4 package '{Path.GetFileName(pkgPath)}' uses an encrypted PFS image (mode=0x{mode:X}). " +
                    "Open the unpacked PS4 game folder containing Media/Modules/Il2CppUserAssemblies.prx and Media/Metadata/global-metadata.dat, " +
                    "or configure pkgextract via PATH or IL2CPPSTUDIO_PKGEXTRACT.",
                    new() { File = pkgPath });
                return null;
            }

            Directory.CreateDirectory(cache);
            var count = ExtractUnencryptedPfs(fs, (long)pfsOffset, pfsSize, cache);
            if (count > 0 && HasMetadata(cache))
            {
                File.WriteAllText(Path.Combine(cache, ".complete"), pkgPath);
                sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                    $"PKG: extracted {count} IL2CPP files from '{Path.GetFileName(pkgPath)}' to '{cache}'.",
                    new() { File = pkgPath });
                return cache;
            }

            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound,
                $"No global-metadata.dat found inside PS4 package '{Path.GetFileName(pkgPath)}'.",
                new() { File = pkgPath });
            return null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract PS4 package '{Path.GetFileName(pkgPath)}': {ex.Message}", new() { File = pkgPath }, ex);
            return null;
        }
    }

    private static bool TryExtractWithTool(string pkgPath, string cache, IDiagnosticSink sink)
    {
        var tool = FindPkgExtractTool();
        if (tool is null) return false;

        Directory.CreateDirectory(cache);
        using var proc = Process.Start(new ProcessStartInfo
        {
            FileName = tool,
            ArgumentList = { "-o", cache, "-f", pkgPath },
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });
        if (proc is null) return false;
        if (!proc.WaitForExit(120_000))
        {
            try { proc.Kill(entireProcessTree: true); } catch { /* best effort */ }
            return false;
        }
        if (proc.ExitCode != 0 || !HasMetadata(cache))
        {
            var err = proc.StandardError.ReadToEnd().Trim();
            if (err.Length > 0)
                sink.Warn(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed, $"pkgextract: {err}", new() { File = pkgPath });
            return false;
        }

        File.WriteAllText(Path.Combine(cache, ".complete"), pkgPath);
        sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
            $"PKG: extracted '{Path.GetFileName(pkgPath)}' via pkgextract to '{cache}'.", new() { File = pkgPath });
        return true;
    }

    private static string? FindPkgExtractTool()
    {
        if (Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PKGEXTRACT") is { Length: > 0 } envPath && File.Exists(envPath))
            return envPath;
        var names = OperatingSystem.IsWindows() ? new[] { "pkgextract.exe", "pkgextract" } : new[] { "pkgextract" };
        foreach (var n in names)
        {
            var local = Path.Combine(AppContext.BaseDirectory, n);
            if (File.Exists(local)) return local;
        }
        var pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        foreach (var dir in pathVar.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (var n in names)
            {
                var candidate = Path.Combine(dir.Trim(), n);
                if (File.Exists(candidate)) return candidate;
            }
        }
        return null;
    }

    private static int ExtractUnencryptedPfs(Stream stream, long pfsBase, ulong pfsSize, string cache)
    {
        stream.Position = pfsBase;
        Span<byte> super = stackalloc byte[0x50];
        stream.ReadExactly(super);
        if (BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(8, 4)) != PfsMagic)
            return 0;

        var blockSize = BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(0x20, 4));
        var dinodeBlockCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x28, 8));
        var dinodeCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x30, 8));
        var superrootIno = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x40, 8));
        if (blockSize < 0x1000 || dinodeBlockCount == 0 || dinodeCount == 0 || superrootIno >= dinodeCount)
            return 0;

        var inodeTableBytes = checked((int)(dinodeBlockCount * blockSize));
        var inodeBuf = new byte[Math.Min(inodeTableBytes, (int)Math.Min(pfsSize > 0 ? pfsSize : ulong.MaxValue, 32UL * 1024 * 1024))];
        stream.Position = pfsBase + blockSize;
        stream.ReadExactly(inodeBuf);

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var extracted = 0;

        byte[] ReadInodeData(int ino)
        {
            var off = ino * Dinode32Size;
            if (off + Dinode32Size > inodeBuf.Length) return [];
            var fileSize = BinaryPrimitives.ReadUInt64LittleEndian(inodeBuf.AsSpan(off + 8, 8));
            if (fileSize == 0 || fileSize > 256UL * 1024 * 1024) return [];
            var res = new byte[fileSize];
            var remaining = (long)fileSize;
            var dst = 0;
            for (var d = 0; d < 12 && remaining > 0; d++)
            {
                var blk = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(off + 0x64 + d * 4, 4));
                if (blk <= 0) break;
                var toRead = (int)Math.Min(blockSize, remaining);
                stream.Position = pfsBase + (long)blk * blockSize;
                stream.ReadExactly(res.AsSpan(dst, toRead));
                dst += toRead;
                remaining -= toRead;
            }
            return res;
        }

        void WalkDir(int dirIno, string relDir)
        {
            var raw = ReadInodeData(dirIno);
            for (var p = 0; p + 16 <= raw.Length;)
            {
                var entryIno = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p, 4));
                var type = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 4, 4));
                var nameLen = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 8, 4));
                var entSize = BinaryPrimitives.ReadInt32LittleEndian(raw.AsSpan(p + 12, 4));
                if (entSize < 16 || p + entSize > raw.Length) break;
                if (entryIno > 0 && nameLen > 0 && p + 16 + nameLen <= raw.Length)
                {
                    var name = Encoding.UTF8.GetString(raw.AsSpan(p + 16, nameLen)).TrimEnd('\0');
                    if (name is not "." and not "..")
                    {
                        var relPath = relDir.Length == 0 ? name : $"{relDir}/{name}";
                        if (type == 3)
                        {
                            WalkDir(entryIno, relPath);
                        }
                        else if (type == 2)
                        {
                            if (name.Equals("pfs_image.dat", StringComparison.OrdinalIgnoreCase))
                            {
                                var inoOff = entryIno * Dinode32Size;
                                var startBlk = inoOff + 0x68 <= inodeBuf.Length ? BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(inoOff + 0x64, 4)) : 0;
                                var innerSize = inoOff + 16 <= inodeBuf.Length ? BinaryPrimitives.ReadUInt64LittleEndian(inodeBuf.AsSpan(inoOff + 8, 8)) : 0;
                                if (startBlk > 0 && innerSize > 0x1000)
                                    extracted += ExtractUnencryptedPfs(stream, pfsBase + (long)startBlk * blockSize, innerSize, cache);
                            }
                            else if (WantedNames.Contains(name))
                            {
                                var target = Path.GetFullPath(Path.Combine(cache, relPath.Replace('/', Path.DirectorySeparatorChar)));
                                if (target.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                                    File.WriteAllBytes(target, ReadInodeData(entryIno));
                                    extracted++;
                                }
                            }
                        }
                    }
                }
                p += entSize;
            }
        }

        WalkDir((int)superrootIno, string.Empty);
        return extracted;
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();
}

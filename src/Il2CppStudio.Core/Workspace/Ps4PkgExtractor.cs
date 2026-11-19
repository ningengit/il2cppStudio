using System.Buffers.Binary;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>
/// Extracts IL2CPP binaries (<c>Media/Modules/Il2CppUserAssemblies.prx</c>), <c>global-metadata.dat</c>, and Unity
/// data headers from a PlayStation 4 <c>.pkg</c> container (supporting native unencrypted PFS, signed <c>dinodeS32</c> /
/// unsigned <c>dinodeD32</c> inodes, indirect blocks, inner <c>pfs_image.dat</c> with <c>PFSC</c> zlib compression,
/// and optional user-supplied AES-128-XTS EKPFS key via <c>IL2CPPSTUDIO_PKG_EKPFS</c>, or an external user-supplied
/// <c>pkgextract</c> binary on <c>PATH</c> / <c>IL2CPPSTUDIO_PKGEXTRACT</c>) into a temp cache.
/// </summary>
public static class Ps4PkgExtractor
{
    private const uint CntMagicBe = 0x7F434E54;
    private const uint PfsMagic = 0x01332A0B;
    private const uint PfscMagic = 0x43534650; // "PFSC"
    private const int Dinode32Size = 0xA8;
    private const int DinodeS32Size = 0x2C8;

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

    public static string? Extract(string pkgPath, IDiagnosticSink sink) =>
        Extract(pkgPath, sink, TryParseUserEkpfs(out var envKey) ? envKey : null);

    public static string? Extract(string pkgPath, IDiagnosticSink sink, byte[]? ekpfs)
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

            var availablePfsLen = Math.Min((long)pfsSize, fs.Length - (long)pfsOffset);
            if (availablePfsLen < 0x400)
                throw new InvalidDataException($"PFS image at 0x{pfsOffset:X} is truncated.");

            fs.Position = (long)pfsOffset;
            Span<byte> pfsSuper = stackalloc byte[0x390];
            fs.ReadExactly(pfsSuper);
            if (BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(8, 4)) != PfsMagic)
                throw new InvalidDataException("Missing PFS superblock magic in PS4 .pkg.");

            // Superblock+0x1C holds PFS mode flags (bit 0 = signed, bit 2 = XTS encrypted, bit 3 = 64-bit).
            var mode = BinaryPrimitives.ReadUInt16LittleEndian(pfsSuper.Slice(0x1C, 2));
            var blockSize = BinaryPrimitives.ReadUInt32LittleEndian(pfsSuper.Slice(0x20, 4));
            var isEncrypted = (mode & 0x4) != 0;

            Stream pfsStream = new SubStream(fs, (long)pfsOffset, availablePfsLen);
            if (isEncrypted)
            {
                if (ekpfs is { Length: 32 })
                {
                    var cryptSeed = pfsSuper.Slice(0x370, 16);
                    pfsStream = new PfsXtsStream(pfsStream, blockSize, ekpfs, cryptSeed);
                }
                else
                {
                    pfsStream.Dispose();
                    if (TryExtractWithTool(pkgPath, cache, sink))
                        return cache;

                    sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                        $"PS4 package '{Path.GetFileName(pkgPath)}' uses an encrypted PFS image (mode=0x{mode:X}). " +
                        "Open the unpacked PS4 game folder containing Media/Modules/Il2CppUserAssemblies.prx and Media/Metadata/global-metadata.dat, " +
                        "set IL2CPPSTUDIO_PKG_EKPFS (32-byte hex EKPFS key), or configure pkgextract via PATH or IL2CPPSTUDIO_PKGEXTRACT.",
                        new() { File = pkgPath });
                    return null;
                }
            }

            Directory.CreateDirectory(cache);
            using (pfsStream)
            {
                var count = ExtractPfsImage(pfsStream, cache, allowNested: true);
                if (count > 0 && HasMetadata(cache))
                {
                    File.WriteAllText(Path.Combine(cache, ".complete"), pkgPath);
                    sink.Info(DiagnosticCategory.Workspace, DiagnosticCodes.ExtractionCompleted,
                        $"PKG: extracted {count} IL2CPP files from '{Path.GetFileName(pkgPath)}' to '{cache}'.",
                        new() { File = pkgPath });
                    return cache;
                }
            }

            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.MetadataNotFound,
                $"No global-metadata.dat found inside PS4 package '{Path.GetFileName(pkgPath)}'.",
                new() { File = pkgPath });
            return null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException or CryptographicException)
        {
            sink.Error(DiagnosticCategory.Workspace, DiagnosticCodes.FileAccessFailed,
                $"Failed to extract PS4 package '{Path.GetFileName(pkgPath)}': {ex.Message}", new() { File = pkgPath }, ex);
            return null;
        }
    }

    private static bool TryParseUserEkpfs(out byte[] ekpfs)
    {
        ekpfs = [];
        var raw = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PKG_EKPFS")?.Trim();
        if (string.IsNullOrEmpty(raw)) return false;
        raw = raw.Replace("-", string.Empty).Replace(" ", string.Empty);
        if (raw.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) raw = raw[2..];
        if (raw.Length != 64) return false;
        try
        {
            ekpfs = Convert.FromHexString(raw);
            return ekpfs.Length == 32;
        }
        catch (FormatException)
        {
            return false;
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

    internal static int ExtractPfsImage(Stream pfsStream, string cache, bool allowNested)
    {
        if (pfsStream.Length < 0x50)
            return 0;

        pfsStream.Position = 0;
        Span<byte> super = stackalloc byte[0x50];
        pfsStream.ReadExactly(super);
        if (BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(0, 4)) == PfscMagic)
        {
            using var pfsc = new PfscStream(pfsStream, leaveOpen: true);
            return ExtractPfsImage(pfsc, cache, allowNested);
        }
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(8, 4));
        if (magic != PfsMagic)
            return 0;

        var mode = BinaryPrimitives.ReadUInt16LittleEndian(super.Slice(0x1C, 2));
        var isSigned = (mode & 0x1) != 0;
        var blockSize = BinaryPrimitives.ReadUInt32LittleEndian(super.Slice(0x20, 4));
        var dinodeBlockCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x28, 8));
        var dinodeCount = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x30, 8));
        var superrootIno = BinaryPrimitives.ReadUInt64LittleEndian(super.Slice(0x40, 8));
        if (blockSize < 0x1000 || dinodeBlockCount == 0 || dinodeCount == 0 || superrootIno >= dinodeCount)
            return 0;

        var inodeSize = isSigned ? DinodeS32Size : Dinode32Size;
        var entryStride = isSigned ? 36 : 4;
        var entryBlockOffset = isSigned ? 32 : 0;
        var ptrsPerBlock = (int)(blockSize / (uint)entryStride);

        var maxInodeBytes = Math.Min((long)(dinodeBlockCount * blockSize), Math.Min(Math.Max(0L, pfsStream.Length - blockSize), 64L * 1024 * 1024));
        if (maxInodeBytes < inodeSize)
            return 0;

        var inodeBuf = new byte[maxInodeBytes];
        pfsStream.Position = blockSize;
        pfsStream.ReadExactly(inodeBuf);

        var fullPrefix = Path.GetFullPath(cache).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var extracted = 0;

        List<int> ResolveBlocks(int ino, out ulong fileSize)
        {
            fileSize = 0;
            var off = ino * inodeSize;
            if (off < 0 || off + inodeSize > inodeBuf.Length) return [];
            fileSize = BinaryPrimitives.ReadUInt64LittleEndian(inodeBuf.AsSpan(off + 8, 8));
            if (fileSize == 0 || fileSize > 512UL * 1024 * 1024) return [];

            var neededBlocks = (int)((fileSize + blockSize - 1) / blockSize);
            var blocksField = BinaryPrimitives.ReadUInt32LittleEndian(inodeBuf.AsSpan(off + 0x60, 4));
            var db0 = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(off + 0x64 + entryBlockOffset, 4));
            var db1 = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(off + 0x64 + entryStride + entryBlockOffset, 4));
            var result = new List<int>(Math.Min(neededBlocks, 65536));

            // Contiguous layout sets db[0] > 0 and db[1] == -1 with multiple allocated blocks.
            if (db0 > 0 && neededBlocks > 1 && db1 == -1 && blocksField >= neededBlocks)
            {
                for (var i = 0; i < neededBlocks; i++)
                    result.Add(db0 + i);
                return result;
            }

            for (var d = 0; d < 12 && result.Count < neededBlocks; d++)
            {
                var blk = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(off + 0x64 + d * entryStride + entryBlockOffset, 4));
                if (blk <= 0) break;
                result.Add(blk);
            }

            var ibBase = off + 0x64 + 12 * entryStride;
            byte[]? indBuf1 = null;
            byte[]? indBuf2 = null;

            // ib[0]: single indirect block
            if (result.Count < neededBlocks && ibBase + entryStride <= off + inodeSize)
            {
                var ib0 = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(ibBase + entryBlockOffset, 4));
                if (ib0 > 0)
                {
                    indBuf1 = new byte[blockSize];
                    ReadPfsBlock(pfsStream, blockSize, ib0, indBuf1);
                    for (var i = 0; i < ptrsPerBlock && result.Count < neededBlocks; i++)
                    {
                        var blk = BinaryPrimitives.ReadInt32LittleEndian(indBuf1.AsSpan(i * entryStride + entryBlockOffset, 4));
                        if (blk <= 0) break;
                        result.Add(blk);
                    }
                }
            }

            // ib[1]: double indirect block
            if (result.Count < neededBlocks && ibBase + 2 * entryStride <= off + inodeSize)
            {
                var ib1 = BinaryPrimitives.ReadInt32LittleEndian(inodeBuf.AsSpan(ibBase + entryStride + entryBlockOffset, 4));
                if (ib1 > 0)
                {
                    indBuf1 ??= new byte[blockSize];
                    indBuf2 ??= new byte[blockSize];
                    ReadPfsBlock(pfsStream, blockSize, ib1, indBuf1);
                    for (var outer = 0; outer < ptrsPerBlock && result.Count < neededBlocks; outer++)
                    {
                        var l1 = BinaryPrimitives.ReadInt32LittleEndian(indBuf1.AsSpan(outer * entryStride + entryBlockOffset, 4));
                        if (l1 <= 0) break;
                        ReadPfsBlock(pfsStream, blockSize, l1, indBuf2);
                        for (var inner = 0; inner < ptrsPerBlock && result.Count < neededBlocks; inner++)
                        {
                            var blk = BinaryPrimitives.ReadInt32LittleEndian(indBuf2.AsSpan(inner * entryStride + entryBlockOffset, 4));
                            if (blk <= 0) break;
                            result.Add(blk);
                        }
                    }
                }
            }

            return result;
        }

        byte[] ReadInodeData(int ino)
        {
            var blocks = ResolveBlocks(ino, out var fileSize);
            if (blocks.Count == 0 || fileSize == 0) return [];
            var res = new byte[fileSize];
            var remaining = (long)fileSize;
            var dst = 0;
            foreach (var blk in blocks)
            {
                if (remaining <= 0) break;
                var toRead = (int)Math.Min(blockSize, remaining);
                var filePos = (long)blk * blockSize;
                if (filePos < 0 || filePos + toRead > pfsStream.Length) break;
                pfsStream.Position = filePos;
                pfsStream.ReadExactly(res.AsSpan(dst, toRead));
                dst += toRead;
                remaining -= toRead;
            }
            return dst == (long)fileSize ? res : res[..dst];
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
                            if (allowNested && name.Equals("pfs_image.dat", StringComparison.OrdinalIgnoreCase))
                            {
                                var blocks = ResolveBlocks(entryIno, out var innerSize);
                                if (blocks.Count > 0 && innerSize > 0x400)
                                {
                                    // Fast-path contiguous range when blocks are consecutive.
                                    var contiguous = true;
                                    for (var i = 1; i < blocks.Count; i++)
                                    {
                                        if (blocks[i] != blocks[i - 1] + 1) { contiguous = false; break; }
                                    }

                                    using Stream innerStream = contiguous
                                        ? new SubStream(pfsStream, (long)blocks[0] * blockSize, (long)innerSize)
                                        : new BlockMappedStream(pfsStream, blocks, blockSize, (long)innerSize);
                                    extracted += ExtractPfsImage(innerStream, cache, allowNested: false);
                                }
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

    private static void ReadPfsBlock(Stream pfsStream, uint blockSize, int blockIndex, Span<byte> dest)
    {
        var pos = (long)blockIndex * blockSize;
        if (pos < 0 || pos + dest.Length > pfsStream.Length)
        {
            dest.Clear();
            return;
        }
        pfsStream.Position = pos;
        pfsStream.ReadExactly(dest);
    }

    private static bool HasMetadata(string dir) =>
        Directory.Exists(dir) && Directory.EnumerateFiles(dir, "global-metadata.dat", SearchOption.AllDirectories).Any();

    private sealed class SubStream(Stream inner, long offset, long length) : Stream
    {
        private long _pos;
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => Math.Min(length, Math.Max(0, inner.Length - offset));
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int off, int count)
        {
            if (_pos >= Length || count <= 0) return 0;
            var avail = (int)Math.Min(count, Length - _pos);
            inner.Position = offset + _pos;
            var read = inner.Read(buffer, off, avail);
            _pos += read;
            return read;
        }

        public override long Seek(long off, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => off,
            SeekOrigin.Current => _pos + off,
            SeekOrigin.End => Length + off,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    private sealed class BlockMappedStream(Stream inner, List<int> blocks, uint blockSize, long length) : Stream
    {
        private long _pos;
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => length;
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos >= length || count <= 0) return 0;
            var remaining = (int)Math.Min(count, length - _pos);
            var total = 0;
            while (remaining > 0)
            {
                var bi = (int)(_pos / blockSize);
                if ((uint)bi >= (uint)blocks.Count) break;
                var inBlk = (int)(_pos % blockSize);
                var toRead = Math.Min(remaining, (int)blockSize - inBlk);
                inner.Position = (long)blocks[bi] * blockSize + inBlk;
                var n = inner.Read(buffer, offset + total, toRead);
                if (n <= 0) break;
                _pos += n;
                total += n;
                remaining -= n;
            }
            return total;
        }

        public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _pos + offset,
            SeekOrigin.End => length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    /// <summary>Random-access zlib block reader for PlayStation <c>PFSC</c> compressed filesystem images.</summary>
    internal sealed class PfscStream : Stream
    {
        private readonly Stream _inner;
        private readonly bool _leaveOpen;
        private readonly uint _blockSize;
        private readonly ulong[] _offsets;
        private readonly long _length;
        private readonly byte[] _cache;
        private int _cachedBlock = -1;
        private long _pos;

        public PfscStream(Stream inner, bool leaveOpen = false)
        {
            _inner = inner;
            _leaveOpen = leaveOpen;
            _inner.Position = 0;
            Span<byte> hdr = stackalloc byte[0x30];
            _inner.ReadExactly(hdr);
            if (BinaryPrimitives.ReadUInt32LittleEndian(hdr) != PfscMagic)
                throw new InvalidDataException("Missing PFSC header magic.");

            _blockSize = BinaryPrimitives.ReadUInt32LittleEndian(hdr.Slice(0x0C, 4));
            var blockOffsetsStart = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x18, 8));
            var dataLength = BinaryPrimitives.ReadUInt64LittleEndian(hdr.Slice(0x28, 8));
            if (_blockSize < 0x1000 || _blockSize > 0x400000 || dataLength == 0 || dataLength > int.MaxValue)
                throw new InvalidDataException($"Unsupported PFSC geometry (blockSize=0x{_blockSize:X}, dataLength=0x{dataLength:X}).");

            var blockCount = (int)((dataLength + _blockSize - 1) / _blockSize);
            _offsets = new ulong[blockCount + 1];
            var rawOff = new byte[(blockCount + 1) * 8];
            _inner.Position = (long)blockOffsetsStart;
            _inner.ReadExactly(rawOff);
            for (var i = 0; i <= blockCount; i++)
                _offsets[i] = BinaryPrimitives.ReadUInt64LittleEndian(rawOff.AsSpan(i * 8, 8));

            _length = (long)dataLength;
            _cache = new byte[_blockSize];
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => _length;
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos >= _length || count <= 0) return 0;
            var total = 0;
            var remaining = (int)Math.Min(count, _length - _pos);
            while (remaining > 0)
            {
                var bi = (int)(_pos / _blockSize);
                if (bi < 0 || bi >= _offsets.Length - 1) break;
                EnsureBlock(bi);
                var inBlk = (int)(_pos % _blockSize);
                var avail = Math.Min(remaining, (int)_blockSize - inBlk);
                Buffer.BlockCopy(_cache, inBlk, buffer, offset + total, avail);
                _pos += avail;
                total += avail;
                remaining -= avail;
            }
            return total;
        }

        private void EnsureBlock(int blockIndex)
        {
            if (_cachedBlock == blockIndex) return;
            var start = _offsets[blockIndex];
            var end = _offsets[blockIndex + 1];
            var packedLen = (int)(end - start);
            _inner.Position = (long)start;
            if (packedLen <= 0 || packedLen > _blockSize * 2)
            {
                Array.Clear(_cache);
            }
            else if (packedLen == _blockSize)
            {
                _inner.ReadExactly(_cache);
            }
            else
            {
                var comp = new byte[packedLen];
                _inner.ReadExactly(comp);
                using var ms = new MemoryStream(comp);
                using var zlib = new ZLibStream(ms, CompressionMode.Decompress);
                Array.Clear(_cache);
                var read = 0;
                while (read < _cache.Length)
                {
                    var n = zlib.Read(_cache, read, _cache.Length - read);
                    if (n <= 0) break;
                    read += n;
                }
            }
            _cachedBlock = blockIndex;
        }

        public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _pos + offset,
            SeekOrigin.End => _length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_leaveOpen) _inner.Dispose();
            base.Dispose(disposing);
        }
    }

    /// <summary>Random-access AES-128-XTS sector decryptor for encrypted PS4 PFS images (sectors of 0x1000 bytes; block 0 plaintext).</summary>
    internal sealed class PfsXtsStream : Stream
    {
        private const int SectorSize = 0x1000;
        private readonly Stream _inner;
        private readonly uint _blockSize;
        private readonly Aes _aesTweak;
        private readonly Aes _aesData;
        private readonly byte[] _sectorCache = new byte[SectorSize];
        private long _cachedSector = -1;
        private long _pos;

        public PfsXtsStream(Stream inner, uint blockSize, byte[] ekpfs, ReadOnlySpan<byte> cryptSeed)
        {
            _inner = inner;
            _blockSize = blockSize;

            Span<byte> msg = stackalloc byte[4 + 16];
            BinaryPrimitives.WriteUInt32LittleEndian(msg, 1);
            cryptSeed[..16].CopyTo(msg[4..]);
            Span<byte> derived = stackalloc byte[32];
            HMACSHA256.HashData(ekpfs, msg, derived);

            _aesTweak = Aes.Create();
            _aesTweak.Mode = CipherMode.ECB;
            _aesTweak.Padding = PaddingMode.None;
            _aesTweak.Key = derived[..16].ToArray();

            _aesData = Aes.Create();
            _aesData.Mode = CipherMode.ECB;
            _aesData.Padding = PaddingMode.None;
            _aesData.Key = derived.Slice(16, 16).ToArray();
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => _inner.Length;
        public override long Position { get => _pos; set => _pos = Math.Max(0, value); }
        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_pos >= Length || count <= 0) return 0;
            var total = 0;
            var remaining = (int)Math.Min(count, Length - _pos);
            while (remaining > 0)
            {
                if (_pos < _blockSize)
                {
                    var plainLen = (int)Math.Min(remaining, _blockSize - _pos);
                    _inner.Position = _pos;
                    var n = _inner.Read(buffer, offset + total, plainLen);
                    if (n <= 0) break;
                    _pos += n;
                    total += n;
                    remaining -= n;
                    continue;
                }

                var sector = _pos / SectorSize;
                EnsureSector(sector);
                var inSec = (int)(_pos % SectorSize);
                var avail = Math.Min(remaining, SectorSize - inSec);
                Buffer.BlockCopy(_sectorCache, inSec, buffer, offset + total, avail);
                _pos += avail;
                total += avail;
                remaining -= avail;
            }
            return total;
        }

        private void EnsureSector(long sectorIndex)
        {
            if (_cachedSector == sectorIndex) return;
            var offset = sectorIndex * SectorSize;
            _inner.Position = offset;
            var avail = (int)Math.Min(SectorSize, Math.Max(0, _inner.Length - offset));
            if (avail < SectorSize) Array.Clear(_sectorCache);
            if (avail > 0) _inner.ReadExactly(_sectorCache.AsSpan(0, avail));

            DecryptSector(_sectorCache, sectorIndex);
            _cachedSector = sectorIndex;
        }

        private void DecryptSector(Span<byte> buf, long sectorIndex)
        {
            Span<byte> tweakInput = stackalloc byte[16];
            tweakInput.Clear();
            BinaryPrimitives.WriteUInt64LittleEndian(tweakInput, (ulong)sectorIndex);

            Span<byte> tweak = stackalloc byte[16];
            _aesTweak.EncryptEcb(tweakInput, tweak, PaddingMode.None);

            Span<byte> temp = stackalloc byte[16];
            for (var off = 0; off < SectorSize; off += 16)
            {
                var slice = buf.Slice(off, 16);
                for (var i = 0; i < 16; i++) slice[i] ^= tweak[i];
                _aesData.DecryptEcb(slice, temp, PaddingMode.None);
                for (var i = 0; i < 16; i++) slice[i] = (byte)(temp[i] ^ tweak[i]);

                // Multiply tweak by x in GF(2^128).
                var carry = 0;
                for (var i = 0; i < 16; i++)
                {
                    var nextCarry = (tweak[i] >> 7) & 1;
                    tweak[i] = (byte)((tweak[i] << 1) | carry);
                    carry = nextCarry;
                }
                if (carry != 0) tweak[0] ^= 0x87;
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => _pos + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _aesTweak.Dispose();
                _aesData.Dispose();
                _inner.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

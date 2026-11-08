using System.Buffers.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Backup;

/// <summary>
/// Reads only the identity (GUID + age) of a Microsoft PDB 7.0 file, from the PDB info stream (stream 1)
/// of the MSF container. Used to confirm a backup PDB belongs to the exact GameAssembly.dll being analysed.
/// </summary>
public static class PdbInfoReader
{
    private static ReadOnlySpan<byte> Magic => "Microsoft C/C++ MSF 7.00\r\n\u001aDS\0\0\0"u8;

    public static PdbIdentity? TryRead(string path, IDiagnosticSink sink)
    {
        try
        {
            using var file = MappedFile.Open(path);
            var d = file.Span;
            if (d.Length < 56 || !d[..Magic.Length].SequenceEqual(Magic))
            {
                sink.Warn(DiagnosticCategory.Pdb, DiagnosticCodes.PdbParseFailed, "Not an MSF 7.0 PDB file.", new() { File = path });
                return null;
            }

            var blockSize = BinaryPrimitives.ReadInt32LittleEndian(d[32..]);
            var dirBytes = BinaryPrimitives.ReadInt32LittleEndian(d[44..]);
            var blockMapAddr = BinaryPrimitives.ReadInt32LittleEndian(d[52..]);
            if (blockSize is not (512 or 1024 or 2048 or 4096 or 8192 or 16384 or 32768) || dirBytes <= 0)
            {
                sink.Warn(DiagnosticCategory.Pdb, DiagnosticCodes.PdbParseFailed, $"Implausible MSF header (block size {blockSize}).", new() { File = path });
                return null;
            }

            // Stream directory = concatenation of the blocks listed at blockMapAddr.
            var dirBlockCount = (dirBytes + blockSize - 1) / blockSize;
            var directory = new byte[dirBlockCount * blockSize];
            for (var i = 0; i < dirBlockCount; i++)
            {
                var block = BinaryPrimitives.ReadInt32LittleEndian(d[(blockMapAddr * blockSize + i * 4)..]);
                d.Slice(block * blockSize, blockSize).CopyTo(directory.AsSpan(i * blockSize));
            }

            var dir = directory.AsSpan();
            var numStreams = BinaryPrimitives.ReadInt32LittleEndian(dir);
            if (numStreams < 2) return null;
            var sizes = new int[numStreams];
            for (var i = 0; i < numStreams; i++) sizes[i] = BinaryPrimitives.ReadInt32LittleEndian(dir[(4 + i * 4)..]);

            // Skip stream 0's block list to reach stream 1's first block.
            var pos = 4 + numStreams * 4;
            var stream0Blocks = sizes[0] <= 0 ? 0 : (sizes[0] + blockSize - 1) / blockSize;
            pos += stream0Blocks * 4;
            var firstBlockOfInfo = BinaryPrimitives.ReadInt32LittleEndian(dir[pos..]);

            // PDB info stream: uint32 version, uint32 signature, uint32 age, GUID.
            var info = d.Slice(firstBlockOfInfo * blockSize, 28);
            var age = BinaryPrimitives.ReadUInt32LittleEndian(info[8..]);
            var guid = new Guid(info.Slice(12, 16));
            return new PdbIdentity(guid, age);
        }
        catch (Exception ex)
        {
            sink.Warn(DiagnosticCategory.Pdb, DiagnosticCodes.PdbParseFailed, $"Cannot read PDB identity: {ex.Message}", new() { File = path }, ex);
            return null;
        }
    }
}

using System.IO.Compression;

namespace Il2CppStudio.Core.Binary;

/// <summary>LEB128 readers for WebAssembly.</summary>
internal static class Leb
{
    public static uint U32(byte[] d, ref int p)
    {
        uint result = 0;
        var shift = 0;
        byte b;
        do { b = d[p++]; result |= (uint)(b & 0x7F) << shift; shift += 7; } while ((b & 0x80) != 0 && shift < 35);
        return result;
    }

    public static long S64(byte[] d, ref int p)
    {
        long result = 0;
        var shift = 0;
        byte b;
        do { b = d[p++]; result |= (long)(b & 0x7F) << shift; shift += 7; } while ((b & 0x80) != 0 && shift < 70);
        if (shift < 64 && (b & 0x40) != 0) result |= -1L << shift;
        return result;
    }
}

/// <summary>Locating and decompressing the WebGL code module (<c>Build/*.wasm</c>, <c>.wasm.br</c>, <c>.wasm.gz</c>, <c>.wasm.unityweb</c>).</summary>
public static class WasmFiles
{
    public static string? Find(string root) =>
        new[] { System.IO.Path.Combine(root, "Build"), root }.Where(Directory.Exists).SelectMany(Directory.EnumerateFiles)
            .FirstOrDefault(f => f.EndsWith(".wasm", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".wasm.br", StringComparison.OrdinalIgnoreCase)
                              || f.EndsWith(".wasm.gz", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".wasm.unityweb", StringComparison.OrdinalIgnoreCase));

    public static bool IsWasmPath(string path) => path.Contains(".wasm", StringComparison.OrdinalIgnoreCase);

    /// <summary>Raw module bytes: plain, gzip (1F 8B) or Brotli (anything else that is not already <c>\0asm</c>).</summary>
    public static byte[] ReadDecompressed(string path)
    {
        var raw = File.ReadAllBytes(path);
        if (raw.Length >= 4 && BitConverter.ToUInt32(raw) == 0x6D736100) return raw;
        using var input = new MemoryStream(raw);
        using Stream dec = raw.Length > 2 && raw[0] == 0x1F && raw[1] == 0x8B
            ? new GZipStream(input, CompressionMode.Decompress) : new BrotliStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        dec.CopyTo(output);
        return output.ToArray();
    }
}

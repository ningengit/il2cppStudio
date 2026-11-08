using System.Buffers;
using System.IO.MemoryMappedFiles;

namespace Il2CppStudio.Core.IO;

/// <summary>
/// Read-only memory-mapped file exposed as <see cref="Memory{T}"/> / <see cref="ReadOnlySpan{T}"/>.
/// All parsers slice into this without copying. Files are opened with FileShare.ReadWrite so the
/// game, HxD or other tools can keep them open at the same time.
/// </summary>
public sealed unsafe class MappedFile : MemoryManager<byte>
{
    private readonly MemoryMappedFile? _mmf;
    private readonly MemoryMappedViewAccessor? _view;
    private byte* _pointer;
    private readonly int _length;
    private bool _disposed;

    public string Path { get; }
    public int Length => _length;

    private MappedFile(string path, MemoryMappedFile? mmf, MemoryMappedViewAccessor? view, int length)
    {
        Path = path;
        _mmf = mmf;
        _view = view;
        _length = length;
        if (view is not null)
        {
            byte* p = null;
            view.SafeMemoryMappedViewHandle.AcquirePointer(ref p);
            _pointer = p + view.PointerOffset;
        }
    }

    /// <exception cref="IOException">The file is larger than 2 GiB or cannot be opened.</exception>
    public static MappedFile Open(string path)
    {
        var fullPath = System.IO.Path.GetFullPath(path);
        var info = new FileInfo(fullPath);
        if (!info.Exists) throw new FileNotFoundException("File not found.", fullPath);
        if (info.Length > int.MaxValue)
            throw new IOException($"'{fullPath}' is {info.Length:N0} bytes; files over 2 GiB are not supported by span-based parsing.");
        if (info.Length == 0) return new MappedFile(fullPath, null, null, 0);

        var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        var mmf = MemoryMappedFile.CreateFromFile(fs, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: false);
        var view = mmf.CreateViewAccessor(0, info.Length, MemoryMappedFileAccess.Read);
        return new MappedFile(fullPath, mmf, view, (int)info.Length);
    }

    public ReadOnlySpan<byte> Span => GetSpan();

    public override Span<byte> GetSpan()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _length == 0 ? [] : new Span<byte>(_pointer, _length);
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)elementIndex, (uint)_length);
        return new MemoryHandle(_pointer + elementIndex);
    }

    public override void Unpin() { }

    /// <summary>Releases the view and mapping. (MemoryManager implements IDisposable explicitly.)</summary>
    public void Dispose() => ((IDisposable)this).Dispose();

    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
        if (_view is not null)
        {
            _view.SafeMemoryMappedViewHandle.ReleasePointer();
            _pointer = null;
            _view.Dispose();
        }
        _mmf?.Dispose();
    }
}

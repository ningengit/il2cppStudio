using System.Buffers;

namespace Il2CppStudio.Core.IO;

/// <summary>
/// Reference-counted wrapper around a <see cref="MappedFile"/> shared across zero-copy archive slices
/// so the underlying memory-mapped view stays alive until every slice and owner is disposed.
/// </summary>
public sealed class SharedMappedFile : IDisposable
{
    private readonly MappedFile _file;
    private int _refCount = 1;

    public SharedMappedFile(MappedFile file) => _file = file;

    public string Path => _file.Path;
    public int Length => _file.Length;
    public ReadOnlySpan<byte> Span => _file.Span;
    public ReadOnlyMemory<byte> Memory => _file.Memory;
    internal Span<byte> RawSpan => _file.GetSpan();

    public bool TryAddRef()
    {
        while (true)
        {
            var current = Volatile.Read(ref _refCount);
            if (current <= 0) return false;
            if (Interlocked.CompareExchange(ref _refCount, current + 1, current) == current)
                return true;
        }
    }

    public void Dispose()
    {
        if (Interlocked.Decrement(ref _refCount) == 0)
            _file.Dispose();
    }
}

/// <summary>
/// Zero-copy slice into a <see cref="SharedMappedFile"/>, exposed as <see cref="Memory{T}"/> without allocating.
/// </summary>
public sealed class MappedSlice : MemoryManager<byte>
{
    private readonly SharedMappedFile _owner;
    private readonly int _offset;
    private readonly int _length;
    private bool _disposed;

    public MappedSlice(SharedMappedFile owner, int offset, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + length, owner.Length);
        if (!owner.TryAddRef())
            throw new ObjectDisposedException(nameof(SharedMappedFile));
        _owner = owner;
        _offset = offset;
        _length = length;
    }

    public override Span<byte> GetSpan()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _owner.RawSpan.Slice(_offset, _length);
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)elementIndex, (uint)_length);
        return _owner.Memory.Slice(_offset + elementIndex, _length - elementIndex).Pin();
    }

    public override void Unpin() { }

    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;
        _disposed = true;
        _owner.Dispose();
    }
}

/// <summary>
/// Unified view over either a zero-copy <see cref="MappedSlice"/> (<see cref="IsZeroCopy"/> = <c>true</c>) or
/// an in-memory decompressed <see cref="byte"/>[] buffer.
/// </summary>
public sealed class VirtualFileBuffer : IDisposable
{
    private readonly IDisposable? _owner;
    private readonly byte[]? _array;

    private VirtualFileBuffer(string virtualPath, ReadOnlyMemory<byte> memory, IDisposable? owner, byte[]? array, bool isZeroCopy)
    {
        VirtualPath = virtualPath;
        Memory = memory;
        _owner = owner;
        _array = array;
        IsZeroCopy = isZeroCopy;
    }

    public string VirtualPath { get; }
    public ReadOnlyMemory<byte> Memory { get; }
    public ReadOnlySpan<byte> Span => Memory.Span;
    public int Length => Memory.Length;
    public bool IsZeroCopy { get; }

    public static VirtualFileBuffer FromMappedSlice(string virtualPath, SharedMappedFile owner, int offset, int length)
    {
        var slice = new MappedSlice(owner, offset, length);
        return new VirtualFileBuffer(virtualPath, slice.Memory, slice, null, isZeroCopy: true);
    }

    public static VirtualFileBuffer FromArray(string virtualPath, byte[] bytes) =>
        new(virtualPath, bytes, owner: null, array: bytes, isZeroCopy: false);

    public byte[] ToByteArray() => _array ?? Memory.ToArray();

    public void Dispose() => _owner?.Dispose();
}

/// <summary>
/// Owns all mapped source archives and virtual entries resolved from a container (.apk/.xapk/.ipa/.data).
/// Disposed cleanly when <see cref="Workspace.Il2CppWorkspace.Dispose"/> runs.
/// </summary>
public sealed class VirtualArchiveSession : IDisposable
{
    private readonly List<IDisposable> _owners = [];

    public string? BinaryVirtualPath { get; init; }
    public VirtualFileBuffer? Binary { get; init; }
    public string? BinaryFilePathOnDisk { get; init; }
    public string? MetadataVirtualPath { get; init; }
    public VirtualFileBuffer? Metadata { get; init; }
    public string? PrimaryArchivePath { get; init; }
    public IReadOnlyDictionary<string, VirtualFileBuffer> DataFiles { get; init; } =
        new Dictionary<string, VirtualFileBuffer>(StringComparer.OrdinalIgnoreCase);

    public void AddOwner(IDisposable owner) => _owners.Add(owner);

    public bool TryGetDataFile(string fileName, out VirtualFileBuffer buffer)
    {
        if (DataFiles.TryGetValue(fileName, out buffer!)) return true;
        foreach (var kv in DataFiles)
        {
            if (Path.GetFileName(kv.Key).Equals(fileName, StringComparison.OrdinalIgnoreCase))
            {
                buffer = kv.Value;
                return true;
            }
        }
        buffer = null!;
        return false;
    }

    public void Dispose()
    {
        Metadata?.Dispose();
        Binary?.Dispose();
        foreach (var kv in DataFiles) kv.Value.Dispose();
        foreach (var o in _owners) o.Dispose();
    }
}

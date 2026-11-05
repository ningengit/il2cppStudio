using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Struct-typed stack slots (hidden return buffers, by-reference struct arguments), keyed by canonical offset.</summary>
public sealed class StructSlotTable(RegisterTracker regs)
{
    private readonly Dictionary<long, (CilLocalVariable Local, int Size)> _slots = [];

    public bool TryFind(long key, out CilLocalVariable local, out long fieldOffset)
    {
        foreach (var (k, v) in _slots)
        {
            if (key >= k && key < k + Math.Max(1, v.Size))
            {
                local = v.Local;
                fieldOffset = key - k;
                return true;
            }
        }
        local = null!;
        fieldOffset = 0;
        return false;
    }

    /// <summary>Unbinds any struct local covering <paramref name="key"/> (the slot is being rebuilt piecewise).</summary>
    public void Forget(long key)
    {
        foreach (var k in _slots.Where(e => key >= e.Key && key < e.Key + Math.Max(1, e.Value.Size)).Select(e => e.Key).ToList())
            _slots.Remove(k);
    }

    /// <summary> Every new struct value written to a slot gets its own local, so values
    /// read earlier from the previous occupant (evaluated lazily) are never overwritten.
    /// </summary>
    public CilLocalVariable GetOrCreate(long key, TypeSignature type, int size)
    {
        var local = regs.NewLocal(type);
        _slots[key] = (local, size);
        return local;
    }
}

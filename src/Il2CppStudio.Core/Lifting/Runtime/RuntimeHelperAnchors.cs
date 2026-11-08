using Iced.Intel;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Disassembly;
using Decoder = Iced.Intel.Decoder;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Identifies runtime helpers through the exported il2cpp_* API. Exports and codegen helpers are usually thin
/// thunks (<c>jmp X</c>, or <c>sub rsp; call X</c>) around one internal implementation, so following both to
/// their final target links a codegen helper to its API name without any version-specific offsets.
/// </summary>
public static class RuntimeHelperAnchors
{
    private static readonly Dictionary<string, RuntimeHelperKind> ApiKinds = new(StringComparer.Ordinal)
    {
        ["il2cpp_object_new"] = RuntimeHelperKind.ObjectNew,
        ["il2cpp_array_new_specific"] = RuntimeHelperKind.ArrayNew,
        ["il2cpp_value_box"] = RuntimeHelperKind.Box,
        ["il2cpp_runtime_class_init"] = RuntimeHelperKind.ClassInit,
        ["il2cpp_raise_exception"] = RuntimeHelperKind.Raise,
        ["il2cpp_object_unbox"] = RuntimeHelperKind.Unbox,
    };

    /// <summary>Maps internal implementation VAs (and the export VAs themselves) to helper kinds.</summary>
    public static Dictionary<ulong, (RuntimeHelperKind Kind, string Api)> Build(VirtualReader reader, IEnumerable<(string Name, ulong Va)> exports)
    {
        var map = new Dictionary<ulong, (RuntimeHelperKind, string)>();
        foreach (var (name, va) in exports)
        {
            if (!ApiKinds.TryGetValue(name, out var kind)) continue;
            map.TryAdd(va, (kind, name));
            var impl = ResolveImplementation(reader, va);
            if (impl != va) map.TryAdd(impl, (kind, name));
        }
        return map;
    }

    /// <summary>
    /// Follows <c>jmp rel</c> chains, and a leading <c>[sub rsp,N] [mov/xor …] call X</c> thunk prologue, to the
    /// function doing the real work. Stops after a few hops or at anything that is not a thunk.
    /// </summary>
    public static ulong ResolveImplementation(VirtualReader reader, ulong va, int maxHops = 4)
    {
        for (var hop = 0; hop < maxHops; hop++)
        {
            var next = ThunkTarget(reader, va);
            if (next is null || next == va) break;
            va = next.Value;
        }
        return va;
    }

    private static ulong? ThunkTarget(VirtualReader reader, ulong va)
    {
        var mem = reader.SliceMemory(va, 32);
        if (mem.IsEmpty) return null;
        var decoder = Decoder.Create(64, new MemoryCodeReader(mem), va);
        for (var i = 0; i < 4; i++)
        {
            decoder.Decode(out var ins);
            if (ins.IsInvalid) return null;
            switch (ins.Mnemonic)
            {
                case Mnemonic.Jmp when ins.Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32:
                case Mnemonic.Call when ins.Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32 && i > 0:
                    return ins.NearBranchTarget;
                case Mnemonic.Sub when ins.Op0Register == Register.RSP:
                case Mnemonic.Mov or Mnemonic.Xor when ins.Op0Kind == OpKind.Register && ins.Op1Kind == OpKind.Register:
                    continue;
                default:
                    return null;
            }
        }
        return null;
    }
}

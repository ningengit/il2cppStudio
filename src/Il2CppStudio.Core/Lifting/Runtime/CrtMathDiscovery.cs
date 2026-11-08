using Iced.Intel;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Disassembly;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>
/// Discovers C runtime math thunks/functions (<c>sqrt</c>, <c>sin</c>, <c>cos</c>, <c>floor</c>, <c>ceil</c>, …) in
/// stripped builds without PDB symbols by inspecting the bodies of <c>System.Math</c> and <c>UnityEngine.Mathf</c>
/// metadata methods, each of which delegates its slow path (or entire body) to the corresponding CRT routine.
/// </summary>
public static class CrtMathDiscovery
{
    private static readonly Dictionary<(string Name, int Arity), (string DoubleCrt, string SingleCrt)> KnownMethods = new()
    {
        [("Sqrt", 1)] = ("sqrt", "sqrtf"),
        [("Sin", 1)] = ("sin", "sinf"),
        [("Cos", 1)] = ("cos", "cosf"),
        [("Tan", 1)] = ("tan", "tanf"),
        [("Asin", 1)] = ("asin", "asinf"),
        [("Acos", 1)] = ("acos", "acosf"),
        [("Atan", 1)] = ("atan", "atanf"),
        [("Atan2", 2)] = ("atan2", "atan2f"),
        [("Pow", 2)] = ("pow", "powf"),
        [("Exp", 1)] = ("exp", "expf"),
        [("Log", 1)] = ("log", "logf"),
        [("Log10", 1)] = ("log10", "log10f"),
        [("Floor", 1)] = ("floor", "floorf"),
        [("FloorToInt", 1)] = ("floor", "floorf"),
        [("Ceiling", 1)] = ("ceil", "ceilf"),
        [("Ceil", 1)] = ("ceil", "ceilf"),
        [("CeilToInt", 1)] = ("ceil", "ceilf"),
    };

    public static void Discover(NativeSymbolizer sym, X64Disassembler disasm, RuntimeHelperMap map)
    {
        foreach (var type in sym.Model.Types)
        {
            if (type.FullName is not ("System.Math" or "UnityEngine.Mathf")) continue;
            foreach (var m in type.Methods)
            {
                if (!KnownMethods.TryGetValue((m.Name, m.ParameterTypeIndices.Count), out var crt)) continue;
                if (!sym.Registration.TryGetType(m.ParameterTypeIndices[0], out var pt)
                    || pt.Type is not (Il2CppTypeEnum.R4 or Il2CppTypeEnum.R8)) continue;
                var va = sym.Map.GetMethodVa(m.Index);
                if (va == 0) continue;
                var fn = disasm.Disassemble(va, 0x200);
                if (fn.Instructions.Count == 0) continue;

                ulong callee = 0;
                var count = 0;
                var convertsToDouble = pt.Type == Il2CppTypeEnum.R8;
                foreach (var ins in fn.Instructions)
                {
                    if (ins.Mnemonic == Mnemonic.Int3) break;
                    if (ins.Mnemonic == Mnemonic.Cvtss2sd) convertsToDouble = true;
                    if (ins.Mnemonic is not (Mnemonic.Call or Mnemonic.Jmp)) continue;
                    if (ins.Op0Kind is not (OpKind.NearBranch64 or OpKind.NearBranch32)) continue;
                    var target = ins.NearBranchTarget;
                    if (target >= fn.StartVa && target < fn.EndVa) continue;
                    if (map.KindOf(target) != RuntimeHelperKind.Unknown || sym.Map.SymbolAt(target) is not null) continue;
                    if (callee != target) { callee = target; count++; }
                }
                if (count == 1 && callee != 0)
                {
                    var name = convertsToDouble ? crt.DoubleCrt : crt.SingleCrt;
                    map.ExportNames.TryAdd(callee, name);
                }
            }
        }
        foreach (var (otherVa, _) in map.Stats)
        {
            if (map.KindOf(otherVa) != RuntimeHelperKind.Unknown || map.ExportNames.ContainsKey(otherVa)) continue;
            var targetVa = otherVa;
            for (var hop = 0; hop < 3; hop++)
            {
                var fn = disasm.Disassemble(targetVa, 0x20);
                if (fn.Instructions.Count > 0 && fn.Instructions[0].Mnemonic == Mnemonic.Jmp
                    && fn.Instructions[0].Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32)
                    targetVa = fn.Instructions[0].NearBranchTarget;
                else break;
            }
            var subFn = disasm.Disassemble(targetVa, 0x100);
            var round = FindRound(subFn.Instructions);
            if (round.IsInvalid && subFn.Instructions.Count >= 2 && subFn.Instructions[0].Mnemonic == Mnemonic.Cmp
                && subFn.Instructions[1].Op0Kind is OpKind.NearBranch64 or OpKind.NearBranch32)
            {
                round = FindRound(disasm.Disassemble(subFn.Instructions[1].NearBranchTarget, 0x40).Instructions);
            }
            if (!round.IsInvalid)
            {
                var imm = round.Immediate8 & 3;
                var single = round.Mnemonic is Mnemonic.Roundps or Mnemonic.Roundss or Mnemonic.Vroundps or Mnemonic.Vroundss
                    || subFn.Instructions.Any(x => x.Mnemonic == Mnemonic.Cvtsd2ss);
                if (imm == 2) { map.ExportNames.TryAdd(otherVa, single ? "ceilf" : "ceil"); continue; }
                if (imm == 1) { map.ExportNames.TryAdd(otherVa, single ? "floorf" : "floor"); continue; }
            }
            if (subFn.Instructions.Count > 12 || !subFn.Instructions.Any(x => x.Mnemonic == Mnemonic.Cvtsd2ss)) continue;
            foreach (var x in subFn.Instructions)
            {
                if (x.Mnemonic is not (Mnemonic.Call or Mnemonic.Jmp) || x.Op0Kind is not (OpKind.NearBranch64 or OpKind.NearBranch32)) continue;
                var t = x.NearBranchTarget;
                if (t >= subFn.StartVa && t < subFn.EndVa) continue;
                if (map.ExportNames.TryGetValue(t, out var dName) && CrtMath.IsKnown(dName)) { map.ExportNames.TryAdd(otherVa, dName.EndsWith('f') ? dName : dName + "f"); break; }
            }
        }
    }

    private static Instruction FindRound(IReadOnlyList<Instruction> insns) =>
        insns.FirstOrDefault(x => x.Mnemonic is Mnemonic.Roundps or Mnemonic.Roundss or Mnemonic.Roundpd or Mnemonic.Roundsd
            or Mnemonic.Vroundps or Mnemonic.Vroundss or Mnemonic.Vroundpd or Mnemonic.Vroundsd);
}

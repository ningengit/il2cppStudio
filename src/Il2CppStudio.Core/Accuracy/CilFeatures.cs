using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;

namespace Il2CppStudio.Core.Accuracy;

/// <summary>Opcode normalization and per-method structural features used by the differential scorer.</summary>
public static class CilFeatures
{
    /// <summary>
    /// Collapses encoding variants that carry no semantic difference (short forms, macro forms, constant
    /// immediates) so the comparison measures what the code does, not how the compiler encoded it.
    /// </summary>
    public static string Normalize(CilOpCode op) => op.Code switch
    {
        CilCode.Ldloc_0 or CilCode.Ldloc_1 or CilCode.Ldloc_2 or CilCode.Ldloc_3 or CilCode.Ldloc_S => "ldloc",
        CilCode.Stloc_0 or CilCode.Stloc_1 or CilCode.Stloc_2 or CilCode.Stloc_3 or CilCode.Stloc_S => "stloc",
        CilCode.Ldarg_0 or CilCode.Ldarg_1 or CilCode.Ldarg_2 or CilCode.Ldarg_3 or CilCode.Ldarg_S => "ldarg",
        CilCode.Ldloca_S => "ldloca",
        CilCode.Ldarga_S => "ldarga",
        CilCode.Starg_S => "starg",
        CilCode.Ldc_I4_M1 or CilCode.Ldc_I4_0 or CilCode.Ldc_I4_1 or CilCode.Ldc_I4_2 or CilCode.Ldc_I4_3 or CilCode.Ldc_I4_4
            or CilCode.Ldc_I4_5 or CilCode.Ldc_I4_6 or CilCode.Ldc_I4_7 or CilCode.Ldc_I4_8 or CilCode.Ldc_I4_S => "ldc.i4",
        CilCode.Br_S => "br",
        CilCode.Brtrue_S => "brtrue",
        CilCode.Brfalse_S => "brfalse",
        CilCode.Beq_S => "beq", CilCode.Bne_Un_S => "bne.un",
        CilCode.Bge_S => "bge", CilCode.Bge_Un_S => "bge.un", CilCode.Bgt_S => "bgt", CilCode.Bgt_Un_S => "bgt.un",
        CilCode.Ble_S => "ble", CilCode.Ble_Un_S => "ble.un", CilCode.Blt_S => "blt", CilCode.Blt_Un_S => "blt.un",
        CilCode.Leave_S => "leave",
        _ => op.Mnemonic,
    };

    /// <summary>Opcodes that are pure encoding noise in compiler output (debug nops) and are excluded.</summary>
    public static bool IsNoise(CilOpCode op) => op.Code is CilCode.Nop or CilCode.Volatile or CilCode.Readonly;

    public static List<CilInstruction> Significant(CilMethodBody body) =>
        [.. body.Instructions.Where(i => !IsNoise(i.OpCode))];

    public static string[] OpcodeSequence(IReadOnlyList<CilInstruction> ins) => [.. ins.Select(i => Normalize(i.OpCode))];

    /// <summary>Net stack effect (pushes − pops) of each instruction.</summary>
    public static int[] StackDeltas(CilMethodBody body, IReadOnlyList<CilInstruction> ins) =>
        [.. ins.Select(i => i.GetStackPushCount() - i.GetStackPopCount(body))];

    public static int MaxStack(CilMethodBody body, IReadOnlyList<CilInstruction> ins)
    {
        int depth = 0, max = 0;
        foreach (var i in ins)
        {
            depth = Math.Max(0, depth - i.GetStackPopCount(body)) + i.GetStackPushCount();
            max = Math.Max(max, depth);
            if (i.OpCode.FlowControl is CilFlowControl.Branch or CilFlowControl.Return or CilFlowControl.Throw) depth = 0;
        }
        return max;
    }

    /// <summary>Basic-block level control-flow summary of a CIL body.</summary>
    public sealed record CfgShape(int Blocks, int Edges, int ConditionalBranches, int BackEdges, int Exits, int Switches, int[] OutDegrees);

    public static CfgShape Cfg(CilMethodBody body)
    {
        var ins = Significant(body);
        if (ins.Count == 0) return new(0, 0, 0, 0, 0, 0, []);
        // Branch targets that land on removed noise instructions (nop) map to the next significant instruction.
        var byOffset = new Dictionary<int, int>();
        var next = 0;
        foreach (var raw in body.Instructions)
        {
            byOffset.TryAdd(raw.Offset, Math.Min(next, ins.Count - 1));
            if (!IsNoise(raw.OpCode)) next++;
        }

        int Target(object? operand) => operand is ICilLabel l && byOffset.TryGetValue(l.Offset, out var t) ? t : -1;

        var leaders = new SortedSet<int> { 0 };
        for (var i = 0; i < ins.Count; i++)
        {
            var fc = ins[i].OpCode.FlowControl;
            if (fc is CilFlowControl.ConditionalBranch or CilFlowControl.Branch)
            {
                if (ins[i].Operand is IList<ICilLabel> labels) foreach (var l in labels) { var t = Target(l); if (t >= 0) leaders.Add(t); }
                else if (Target(ins[i].Operand) is var t and >= 0) leaders.Add(t);
                if (i + 1 < ins.Count) leaders.Add(i + 1);
            }
            else if (fc is CilFlowControl.Return or CilFlowControl.Throw && i + 1 < ins.Count) leaders.Add(i + 1);
        }

        var starts = leaders.ToList();
        int BlockOf(int instr) { var b = starts.BinarySearch(instr); return b >= 0 ? b : ~b - 1; }
        int edges = 0, cond = 0, back = 0, exits = 0, switches = 0;
        var outDeg = new int[starts.Count];
        for (var b = 0; b < starts.Count; b++)
        {
            var end = b + 1 < starts.Count ? starts[b + 1] : ins.Count;
            var last = ins[end - 1];
            var succ = new HashSet<int>();
            switch (last.OpCode.FlowControl)
            {
                case CilFlowControl.ConditionalBranch:
                    if (last.Operand is IList<ICilLabel> labels) { switches++; foreach (var l in labels) if (Target(l) is var t and >= 0) succ.Add(BlockOf(t)); }
                    else { cond++; if (Target(last.Operand) is var t and >= 0) succ.Add(BlockOf(t)); }
                    if (end < ins.Count) succ.Add(b + 1);
                    break;
                case CilFlowControl.Branch:
                    if (Target(last.Operand) is var u and >= 0) succ.Add(BlockOf(u));
                    break;
                case CilFlowControl.Return or CilFlowControl.Throw:
                    exits++;
                    break;
                default:
                    if (end < ins.Count) succ.Add(b + 1);
                    break;
            }
            edges += succ.Count;
            back += succ.Count(s => s <= b);
            outDeg[b] = succ.Count;
        }
        return new(starts.Count, edges, cond, back, exits, switches, outDeg);
    }

    /// <summary>Sørensen–Dice similarity over the longest common subsequence: 2·LCS / (|a| + |b|).</summary>
    public static double LcsSimilarity<T>(IReadOnlyList<T> a, IReadOnlyList<T> b) where T : IEquatable<T>
    {
        if (a.Count == 0 && b.Count == 0) return 1;
        if (a.Count == 0 || b.Count == 0) return 0;
        // O(n·m) time, O(min) memory; bodies are capped to keep pathological methods bounded.
        var n = Math.Min(a.Count, 4000);
        var m = Math.Min(b.Count, 4000);
        var prev = new int[m + 1];
        var cur = new int[m + 1];
        for (var i = 1; i <= n; i++)
        {
            for (var j = 1; j <= m; j++)
                cur[j] = a[i - 1].Equals(b[j - 1]) ? prev[j - 1] + 1 : Math.Max(prev[j], cur[j - 1]);
            (prev, cur) = (cur, prev);
        }
        return 2.0 * prev[m] / (n + m);
    }

    /// <summary>1 − |a−b| / max(a, b), with 1 for two zeros.</summary>
    public static double Ratio(int a, int b) => a == b ? 1 : 1 - (double)Math.Abs(a - b) / Math.Max(Math.Abs(a), Math.Abs(b));

    public static double CfgSimilarity(CfgShape a, CfgShape b)
    {
        var degA = a.OutDegrees.Order().ToArray();
        var degB = b.OutDegrees.Order().ToArray();
        var parts = new[]
        {
            Ratio(a.Blocks, b.Blocks), Ratio(a.Edges, b.Edges), Ratio(a.ConditionalBranches, b.ConditionalBranches),
            Ratio(a.BackEdges, b.BackEdges), Ratio(a.Exits, b.Exits), Ratio(a.Switches, b.Switches),
            LcsSimilarity(degA, degB),
        };
        return parts.Average();
    }

    /// <summary>Stable cross-assembly method key: declaring type full name, name and parameter type names.</summary>
    public static string MethodKey(MethodDefinition m) =>
        $"{m.DeclaringType?.FullName}::{m.Name}({string.Join(",", m.Signature?.ParameterTypes.Select(p => p.FullName) ?? [])})";
}

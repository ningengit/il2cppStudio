using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Accuracy;

public sealed record MethodAccuracy(
    string Method, bool Lifted, int GroundTruthInstructions, int LiftedInstructions,
    double OpcodeSequence, double StackDelta, double ControlFlow, int GroundTruthMaxStack, int LiftedMaxStack);

public sealed record AssemblyAccuracy(
    string Assembly, int GroundTruthMethods, int MatchedMethods, int LiftedMethods,
    double OpcodeSequenceMatchRate, double StackEvaluationAccuracy, double ControlFlowMatchRate,
    double StrictOpcodeSequenceMatchRate, double StrictStackEvaluationAccuracy, double StrictControlFlowMatchRate);

/// <summary>
/// Report of the IL-level differential comparison. "Rates" average over methods that were actually lifted;
/// "Strict" rates count every ground-truth method with a body, scoring unlifted ones as 0, so coverage gaps
/// cannot inflate the result.
/// </summary>
public sealed record AccuracyReport(
    DateTimeOffset Timestamp, string GroundTruthDirectory, string Target, string Mode,
    double OpcodeSequenceMatchRate, double StackEvaluationAccuracy, double ControlFlowMatchRate,
    double StrictOpcodeSequenceMatchRate, double StrictStackEvaluationAccuracy, double StrictControlFlowMatchRate,
    double LiftCoverage, IReadOnlyList<AssemblyAccuracy> Assemblies, IReadOnlyList<MethodAccuracy> Methods);

/// <summary>
/// Compares ground-truth CIL (the managed assemblies the game was compiled from, e.g. the build backup's
/// Managed/ folder) against lifted CIL opcode by opcode:
/// <list type="bullet">
/// <item><b>Opcode Sequence Match Rate</b> — Dice/LCS similarity of normalized opcode sequences.</item>
/// <item><b>Stack Delta / Stack Evaluation Accuracy</b> — mean of LCS similarity of per-instruction stack deltas and the max-stack ratio.</item>
/// <item><b>Branch &amp; CFG Match Rate</b> — similarity of basic blocks, edges, conditional branches, back edges (loops), exits, switches and the out-degree distribution.</item>
/// </list>
/// Methods are matched by declaring type, name and parameter types. Placeholder bodies (<c>ldnull; throw</c>)
/// are treated as "not lifted".
/// </summary>
public static class CilAccuracyScorer
{
    public static AccuracyReport Compare(string groundTruthDir, IReadOnlyDictionary<string, byte[]> liftedImages, string target, string mode,
        IDiagnosticSink sink, Func<string, bool>? assemblyFilter = null)
    {
        var methods = new List<MethodAccuracy>();
        var assemblies = new List<AssemblyAccuracy>();

        foreach (var (name, bytes) in liftedImages.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
        {
            if (assemblyFilter is not null && !assemblyFilter(name)) continue;
            var gtPath = Path.Combine(groundTruthDir, name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? name : name + ".dll");
            if (!File.Exists(gtPath))
            {
                sink.Warn(DiagnosticCategory.Accuracy, DiagnosticCodes.GroundTruthMissing, $"No ground truth for {name} in '{groundTruthDir}'.", new() { File = gtPath });
                continue;
            }

            ModuleDefinition gt, lifted;
            try
            {
                gt = ModuleDefinition.FromFile(gtPath);
                lifted = ModuleDefinition.FromBytes(bytes);
            }
            catch (Exception ex)
            {
                sink.Error(DiagnosticCategory.Accuracy, DiagnosticCodes.GroundTruthMissing, $"Cannot read {name}: {ex.Message}", new() { File = gtPath }, ex);
                continue;
            }

            var liftedByKey = new Dictionary<string, MethodDefinition>(StringComparer.Ordinal);
            foreach (var t in lifted.GetAllTypes())
                foreach (var m in t.Methods)
                    liftedByKey.TryAdd(CilFeatures.MethodKey(m), m);

            var asmMethods = new List<MethodAccuracy>();
            var gtCount = 0;
            foreach (var t in gt.GetAllTypes())
            {
                foreach (var m in t.Methods)
                {
                    if (m.CilMethodBody is not { } gtBody) continue;
                    gtCount++;
                    var key = CilFeatures.MethodKey(m);
                    if (!liftedByKey.TryGetValue(key, out var lm) || lm.CilMethodBody is not { } lBody) continue;
                    asmMethods.Add(Score(key, gtBody, lBody));
                }
            }
            methods.AddRange(asmMethods);

            var liftedOnly = asmMethods.Where(a => a.Lifted).ToList();
            assemblies.Add(new AssemblyAccuracy(name, gtCount, asmMethods.Count, liftedOnly.Count,
                Avg(liftedOnly, a => a.OpcodeSequence), Avg(liftedOnly, a => a.StackDelta), Avg(liftedOnly, a => a.ControlFlow),
                StrictAvg(liftedOnly, gtCount, a => a.OpcodeSequence), StrictAvg(liftedOnly, gtCount, a => a.StackDelta), StrictAvg(liftedOnly, gtCount, a => a.ControlFlow)));
        }

        var all = methods.Where(m => m.Lifted).ToList();
        var totalGt = assemblies.Sum(a => a.GroundTruthMethods);
        var report = new AccuracyReport(DateTimeOffset.Now, groundTruthDir, target, mode,
            Avg(all, a => a.OpcodeSequence), Avg(all, a => a.StackDelta), Avg(all, a => a.ControlFlow),
            StrictAvg(all, totalGt, a => a.OpcodeSequence), StrictAvg(all, totalGt, a => a.StackDelta), StrictAvg(all, totalGt, a => a.ControlFlow),
            totalGt == 0 ? 0 : Math.Round(100.0 * all.Count / totalGt, 2), assemblies, methods);

        sink.Info(DiagnosticCategory.Accuracy, DiagnosticCodes.AccuracyComputed,
            $"Accuracy over {all.Count:N0} lifted / {totalGt:N0} ground-truth methods: opcode {report.OpcodeSequenceMatchRate:F2}%, " +
            $"stack {report.StackEvaluationAccuracy:F2}%, CFG {report.ControlFlowMatchRate:F2}% (strict: {report.StrictOpcodeSequenceMatchRate:F2}% / " +
            $"{report.StrictStackEvaluationAccuracy:F2}% / {report.StrictControlFlowMatchRate:F2}%), coverage {report.LiftCoverage:F2}%.");
        return report;
    }

    public static MethodAccuracy Score(string key, CilMethodBody gt, CilMethodBody lifted)
    {
        var g = CilFeatures.Significant(gt);
        var l = CilFeatures.Significant(lifted);
        var isPlaceholder = l.Count == 2 && l[0].OpCode.Code == AsmResolver.PE.DotNet.Cil.CilCode.Ldnull && l[1].OpCode.Code == AsmResolver.PE.DotNet.Cil.CilCode.Throw;
        if (isPlaceholder) return new MethodAccuracy(key, false, g.Count, l.Count, 0, 0, 0, CilFeatures.MaxStack(gt, g), 0);

        var opcode = CilFeatures.LcsSimilarity(CilFeatures.OpcodeSequence(g), CilFeatures.OpcodeSequence(l));
        var deltas = CilFeatures.LcsSimilarity(CilFeatures.StackDeltas(gt, g), CilFeatures.StackDeltas(lifted, l));
        var gMax = CilFeatures.MaxStack(gt, g);
        var lMax = CilFeatures.MaxStack(lifted, l);
        var stack = (deltas + CilFeatures.Ratio(gMax, lMax)) / 2;
        var cfg = CilFeatures.CfgSimilarity(CilFeatures.Cfg(gt), CilFeatures.Cfg(lifted));
        return new MethodAccuracy(key, true, g.Count, l.Count, Math.Round(opcode * 100, 2), Math.Round(stack * 100, 2), Math.Round(cfg * 100, 2), gMax, lMax);
    }

    private static double Avg(IReadOnlyCollection<MethodAccuracy> xs, Func<MethodAccuracy, double> f) =>
        xs.Count == 0 ? 0 : Math.Round(xs.Average(f), 2);

    private static double StrictAvg(IReadOnlyCollection<MethodAccuracy> lifted, int total, Func<MethodAccuracy, double> f) =>
        total == 0 ? 0 : Math.Round(lifted.Sum(f) / total, 2);
}

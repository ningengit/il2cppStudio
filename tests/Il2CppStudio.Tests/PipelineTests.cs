using Il2CppStudio.Core.Accuracy;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Lifting.Runtime;

namespace Il2CppStudio.Tests;

/// <summary>
/// Registration / helper identification invariants and accuracy floors. Floors sit slightly below the last
/// measured values: improvements raise them, regressions fail the build.
/// </summary>
[Collection(TemplateBuildCollection.Name)]
public sealed class PipelineTests(TemplateBuildFixture fx)
{
    // Last measured (2026-11-05, after vtable-call fix / idiv / flag spill fix): opcode 61.23 %, stack 81.96 %, CFG 86.83 %, coverage 98.15 %.
    private const double OpcodeFloor = 60.8, StackFloor = 81.4, CfgFloor = 86.3, CoverageFloor = 98.0;

    [Fact]
    public void Registration_IsFoundAndComplete()
    {
        var ws = fx.Workspace;
        Assert.True(ws.IsEnhanced);
        Assert.NotNull(ws.Registration);
        Assert.Equal(ws.Metadata.Images.Length, ws.Registration!.CodeGenModules.Count);
        Assert.True(ws.AddressMap!.ResolvedMethodCount > 45_000);
        Assert.True(ws.Registration.MetadataUsages.Count > 30_000);
    }

    [Fact]
    public void RuntimeHelpers_CoreKindsIdentified()
    {
        var helpers = fx.Workspace.RuntimeHelpers!;
        foreach (var kind in new[] { RuntimeHelperKind.WriteBarrier, RuntimeHelperKind.ObjectNew, RuntimeHelperKind.ArrayNew,
                     RuntimeHelperKind.Box, RuntimeHelperKind.Cast, RuntimeHelperKind.Raise, RuntimeHelperKind.MetadataInit, RuntimeHelperKind.ClassInit })
            Assert.Contains(helpers.Helpers, h => h.Kind == kind);
        // The dominant write barrier must have thousands of sites (guards against threshold drift).
        Assert.Contains(helpers.Helpers, h => h.Kind == RuntimeHelperKind.WriteBarrier && h.CallSites > 10_000);
    }

    [Fact]
    public void Accuracy_AssemblyCSharp_StaysAboveFloors()
    {
        var ws = fx.Workspace;
        if (TemplateBuildFixture.GroundTruthManaged is not { } managed) { Assert.Skip("No ground-truth Managed folder."); return; }
        var asm = ws.Model.Assemblies.Single(a => a.Name == "Assembly-CSharp");
        var all = new List<int>();
        void Collect(Core.Metadata.TypeModel t) { all.AddRange(t.Methods.Select(m => m.Index)); foreach (var n in t.NestedTypes) Collect(n); }
        foreach (var t in asm.Namespaces.SelectMany(n => n.Types)) Collect(t);
        ws.BodyProvider!.EnsureLifted(all);
        var image = ws.Stubs.FindImage(asm.ImageName);
        var bytes = ws.Decompiler.GetStubImage(image)!;

        var report = CilAccuracyScorer.Compare(managed, new Dictionary<string, byte[]> { [asm.ImageName] = bytes }, ws.Files.Root, ws.ModeLabel, NullDiagnosticSink.Instance);
        Assert.True(report.OpcodeSequenceMatchRate >= OpcodeFloor, $"Opcode match {report.OpcodeSequenceMatchRate:F2}% < {OpcodeFloor}%");
        Assert.True(report.StackEvaluationAccuracy >= StackFloor, $"Stack accuracy {report.StackEvaluationAccuracy:F2}% < {StackFloor}%");
        Assert.True(report.ControlFlowMatchRate >= CfgFloor, $"CFG match {report.ControlFlowMatchRate:F2}% < {CfgFloor}%");
        Assert.True(report.LiftCoverage >= CoverageFloor, $"Coverage {report.LiftCoverage:F2}% < {CoverageFloor}%");
    }
}

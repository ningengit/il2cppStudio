using System.Collections.Concurrent;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// <see cref="IMethodBodyProvider"/> backed by <see cref="LifterDriver"/>: lifts each requested method once and
/// keeps aggregate statistics. Lifting is serialized because stub modules are shared mutable state.
/// </summary>
public sealed class LiftingBodyProvider(LifterDriver driver, IDiagnosticSink sink) : IMethodBodyProvider
{
    private readonly ConcurrentDictionary<int, LiftResult> _results = new();
    private readonly Lock _gate = new();

    public IReadOnlyDictionary<int, LiftResult> Results => _results;

    /// <summary>Receives conversion notes and lift failures (per assembly, outside the C# output).</summary>
    public DecompilationErrorLog? ErrorLog { get; set; }

    /// <summary>method index → (assembly, member, RVA) for log entries.</summary>
    public Func<int, (string Assembly, string Member, ulong Rva)>? Describe { get; set; }

    /// <summary>Human-readable reason a method body is a <c>throw null</c> stub, or null when it was lifted.</summary>
    public string? FallbackReason(int methodIndex)
    {
        if (!_results.TryGetValue(methodIndex, out var r))
            return "the method was not lifted (its assembly was not selected for native lifting, e.g. BCL substitution is on)";
        if (r.Success) return null;
        return r.Error switch
        {
            "no native code" => "IL2CPP emitted no native body for this method: it is a generic definition (only its instantiations are compiled), " +
                                "abstract/extern, or was stripped as unused",
            "no image" => "the declaring type belongs to no IL2CPP image",
            _ => $"lifting failed: {r.Error}. Details and stack trace are in the assembly's _Error_Logs.txt",
        };
    }

    private void Log(int m, LiftResult r)
    {
        if (ErrorLog is null || Describe is null) return;
        var (asm, member, rva) = Describe(m);
        var rvaText = rva == 0 ? null : $"0x{rva:X8}";
        if (!r.Success && r.Error is not ("no native code" or "no image"))
            ErrorLog.Add(asm, "LiftFailure", member, rvaText, r.Error ?? "unknown error", "Body replaced by `throw null;` with an explanatory comment.");
        foreach (var note in r.Notes)
            ErrorLog.Add(asm, note.StartsWith("unsupported") ? "UnsupportedInstruction" : "TypeConversion", member, rvaText, note,
                note.StartsWith("unsupported") ? "Destination register set to NativeIntrinsics.Asm(\"<instruction>\")." : "Value reinterpreted with NativeIntrinsics.Reinterpret<T>.");
        if (r.UnresolvedCalls > 0)
            ErrorLog.Add(asm, "UnresolvedCall", member, rvaText, $"{r.UnresolvedCalls} call(s) to functions without a metadata method or known helper.",
                "Emitted as NativeIntrinsics.CallNative(\"sub_RVA\", …); see diagnostics (IS8001/IS8002) for targets.");
    }

    public int EnsureLifted(IEnumerable<int> methodIndices)
    {
        var lifted = 0;
        lock (_gate)
        {
            foreach (var m in methodIndices)
            {
                if (_results.ContainsKey(m)) continue;
                var r = driver.Lift(m);
                _results[m] = r;
                Log(m, r);
                if (r.Success) lifted++;
            }
        }
        if (lifted > 0)
        {
            var ok = _results.Values.Where(r => r.Success).ToList();
            sink.Debug(DiagnosticCategory.Lifting, DiagnosticCodes.LiftCompleted,
                $"Lifted {lifted} method(s); total {ok.Count:N0}, {ok.Sum(r => r.UnresolvedCalls):N0} unresolved calls, " +
                $"{ok.Sum(r => r.NativeFallbacks):N0} unsupported instructions, {ok.Sum(r => r.ElidedRuntimeChecks):N0} runtime checks elided.");
        }
        return lifted;
    }
}

/// <summary>Workspace-level entry points for the lifter.</summary>
public static class LifterWorkspaceExtensions
{
    /// <summary>Creates the lifting body provider for a workspace with recovered native code, or null.</summary>
    public static LiftingBodyProvider? CreateLifter(this Il2CppWorkspace ws)
    {
        if (ws.Symbolizer is null || ws.Disassembler is null) return null;
        return new LiftingBodyProvider(new LifterDriver(ws.Symbolizer, ws.Stubs, ws.Disassembler, ws.Sink, ws.RuntimeHelpers), ws.Sink)
        {
            ErrorLog = ws.Decompiler.ErrorLog,
            Describe = m =>
            {
                var def = ws.Metadata.Methods[m];
                var type = ws.Model.Types[def.DeclaringType];
                return (type.ImageName, $"{type.FullName}::{ws.Metadata.GetString(def.NameIndex)} (token 0x{def.Token:X8})", ws.AddressMap?.GetMethodRva(m) ?? 0);
            },
        };
    }

    /// <summary>Lifts one method immediately and returns the detailed result (used by tests and tooling).</summary>
    public static LiftResult? LiftMethod(this Il2CppWorkspace ws, int methodIndex)
    {
        WorkspaceSetup.Attach(ws);
        if (ws.BodyProvider is not LiftingBodyProvider p) return null;
        p.EnsureLifted([methodIndex]);
        return p.Results.GetValueOrDefault(methodIndex);
    }
}

public static partial class LiftingPipeline
{
    static partial void CreateLifter(Il2CppWorkspace ws, ref IMethodBodyProvider? provider) => provider = ws.CreateLifter();
}

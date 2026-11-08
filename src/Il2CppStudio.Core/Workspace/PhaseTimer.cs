using System.Diagnostics;

namespace Il2CppStudio.Core.Workspace;

/// <summary>Accumulates wall-clock time per named export phase (header, lift, stub, decompile, references) for profiling.</summary>
public sealed class PhaseTimer
{
    private readonly Dictionary<string, TimeSpan> _phases = [];
    private readonly List<string> _order = [];

    public T Measure<T>(string phase, Func<T> work)
    {
        var sw = Stopwatch.StartNew();
        try { return work(); }
        finally { Add(phase, sw.Elapsed); }
    }

    public void Measure(string phase, Action work) => Measure<object?>(phase, () => { work(); return null; });

    public void Add(string phase, TimeSpan t)
    {
        if (!_phases.TryAdd(phase, t)) _phases[phase] += t;
        else _order.Add(phase);
    }

    public override string ToString() => string.Join(", ", _order.Select(p => $"{p} {_phases[p].TotalSeconds:F2}s"));
}

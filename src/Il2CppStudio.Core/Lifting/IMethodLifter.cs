namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// Lifts one metadata method's native body into the stub module. Implementations are architecture specific
/// (<see cref="LifterDriver"/> for x86-64, <see cref="Wasm.WasmLifter"/> for WebAssembly) and are selected from
/// <see cref="Binary.CpuArchitecture"/> by the workspace.
/// </summary>
public interface IMethodLifter
{
    /// <summary>True when <see cref="Lift"/> can run concurrently across distinct method indices.</summary>
    bool SupportsParallelLifting => false;

    /// <summary>Lifts the method once; a failed lift leaves the explanatory <c>throw null</c> placeholder body.</summary>
    LiftResult Lift(int methodIndex);
}

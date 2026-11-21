using System.Diagnostics;

namespace Il2CppStudio.Core.Decompilation;

/// <summary>
/// Lightweight opt-in phase profiler for <c>--export-all</c> controlled by <c>IL2CPPSTUDIO_PROFILE_EXPORT=1</c>.
/// When disabled, every check is a single static field read with zero allocation or syscall overhead.
/// </summary>
internal static class ExportProfiler
{
    public static volatile bool Enabled = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PROFILE_EXPORT") == "1";

    private static long _preLiftTicks;
    private static long _stubPeTicks;
    private static long _ilspyBclTicks;
    private static long _ilspyNativeTicks;
    private static long _finishAnnotateTicks;
    private static long _finishCoreTransformTicks;
    private static long _finishLoopTransformTicks;
    private static long _finishHygieneTicks;
    private static long _hygCoreTicks;
    private static long _hygGetterTicks;
    private static long _hygInheritedStaticTicks;
    private static long _hygExplicitImplTicks;
    private static long _hygAccessRelaxTicks;
    private static long _hygDefAssignTicks;
    private static long _hygFixupsTicks;
    private static long _hygSanitizeTicks;
    private static long _hygEscapeTicks;
    private static long _finishPrintScrubTicks;
    private static long _ilDisasmTicks;
    private static long _fileWriteTicks;
    private static long _cppHeaderTicks;
    private static long _projectRefsTicks;
    private static long _artifactsTicks;
    private static long _bclTypes;
    private static long _nativeTypes;

    public static void Reset()
    {
        Enabled = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_PROFILE_EXPORT") == "1";
        if (!Enabled) return;
        Interlocked.Exchange(ref _preLiftTicks, 0);
        Interlocked.Exchange(ref _stubPeTicks, 0);
        Interlocked.Exchange(ref _ilspyBclTicks, 0);
        Interlocked.Exchange(ref _ilspyNativeTicks, 0);
        Interlocked.Exchange(ref _finishAnnotateTicks, 0);
        Interlocked.Exchange(ref _finishCoreTransformTicks, 0);
        Interlocked.Exchange(ref _finishLoopTransformTicks, 0);
        Interlocked.Exchange(ref _finishHygieneTicks, 0);
        Interlocked.Exchange(ref _hygCoreTicks, 0);
        Interlocked.Exchange(ref _hygGetterTicks, 0);
        Interlocked.Exchange(ref _hygInheritedStaticTicks, 0);
        Interlocked.Exchange(ref _hygExplicitImplTicks, 0);
        Interlocked.Exchange(ref _hygAccessRelaxTicks, 0);
        Interlocked.Exchange(ref _hygDefAssignTicks, 0);
        Interlocked.Exchange(ref _hygFixupsTicks, 0);
        Interlocked.Exchange(ref _hygSanitizeTicks, 0);
        Interlocked.Exchange(ref _hygEscapeTicks, 0);
        Interlocked.Exchange(ref _finishPrintScrubTicks, 0);
        Interlocked.Exchange(ref _ilDisasmTicks, 0);
        Interlocked.Exchange(ref _fileWriteTicks, 0);
        Interlocked.Exchange(ref _cppHeaderTicks, 0);
        Interlocked.Exchange(ref _projectRefsTicks, 0);
        Interlocked.Exchange(ref _artifactsTicks, 0);
        Interlocked.Exchange(ref _bclTypes, 0);
        Interlocked.Exchange(ref _nativeTypes, 0);
    }

    public static long Start() => Enabled ? Stopwatch.GetTimestamp() : 0;

    public static void RecordPreLift(long start) { if (start != 0) Interlocked.Add(ref _preLiftTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordStubPe(long start) { if (start != 0) Interlocked.Add(ref _stubPeTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordIlspyBcl(long start) { if (start != 0) { Interlocked.Add(ref _ilspyBclTicks, Stopwatch.GetTimestamp() - start); Interlocked.Increment(ref _bclTypes); } }
    public static void RecordIlspyNative(long start) { if (start != 0) { Interlocked.Add(ref _ilspyNativeTicks, Stopwatch.GetTimestamp() - start); Interlocked.Increment(ref _nativeTypes); } }
    public static void RecordFinishAnnotate(long start) { if (start != 0) Interlocked.Add(ref _finishAnnotateTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordFinishCoreTransform(long start) { if (start != 0) Interlocked.Add(ref _finishCoreTransformTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordFinishLoopTransform(long start) { if (start != 0) Interlocked.Add(ref _finishLoopTransformTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordFinishHygiene(long start) { if (start != 0) Interlocked.Add(ref _finishHygieneTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygCore(long start) { if (start != 0) Interlocked.Add(ref _hygCoreTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygGetter(long start) { if (start != 0) Interlocked.Add(ref _hygGetterTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygInheritedStatic(long start) { if (start != 0) Interlocked.Add(ref _hygInheritedStaticTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygExplicitImpl(long start) { if (start != 0) Interlocked.Add(ref _hygExplicitImplTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygAccessRelax(long start) { if (start != 0) Interlocked.Add(ref _hygAccessRelaxTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygDefAssign(long start) { if (start != 0) Interlocked.Add(ref _hygDefAssignTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygFixups(long start) { if (start != 0) Interlocked.Add(ref _hygFixupsTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygSanitize(long start) { if (start != 0) Interlocked.Add(ref _hygSanitizeTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordHygEscape(long start) { if (start != 0) Interlocked.Add(ref _hygEscapeTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordFinishPrintScrub(long start) { if (start != 0) Interlocked.Add(ref _finishPrintScrubTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordIlDisasm(long start) { if (start != 0) Interlocked.Add(ref _ilDisasmTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordFileWrite(long start) { if (start != 0) Interlocked.Add(ref _fileWriteTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordCppHeader(long start) { if (start != 0) Interlocked.Add(ref _cppHeaderTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordProjectRefs(long start) { if (start != 0) Interlocked.Add(ref _projectRefsTicks, Stopwatch.GetTimestamp() - start); }
    public static void RecordArtifacts(long start) { if (start != 0) Interlocked.Add(ref _artifactsTicks, Stopwatch.GetTimestamp() - start); }

    public static void Report(TimeSpan wallElapsed)
    {
        if (!Enabled) return;
        static double Sec(long ticks) => (double)ticks / Stopwatch.Frequency;

        var preLift = Sec(Volatile.Read(ref _preLiftTicks));
        var stubPe = Sec(Volatile.Read(ref _stubPeTicks));
        var bcl = Sec(Volatile.Read(ref _ilspyBclTicks));
        var native = Sec(Volatile.Read(ref _ilspyNativeTicks));
        var ann = Sec(Volatile.Read(ref _finishAnnotateTicks));
        var core = Sec(Volatile.Read(ref _finishCoreTransformTicks));
        var loop = Sec(Volatile.Read(ref _finishLoopTransformTicks));
        var hyg = Sec(Volatile.Read(ref _finishHygieneTicks));
        var hygCore = Sec(Volatile.Read(ref _hygCoreTicks));
        var hygGet = Sec(Volatile.Read(ref _hygGetterTicks));
        var hygInh = Sec(Volatile.Read(ref _hygInheritedStaticTicks));
        var hygExp = Sec(Volatile.Read(ref _hygExplicitImplTicks));
        var hygAcc = Sec(Volatile.Read(ref _hygAccessRelaxTicks));
        var hygDef = Sec(Volatile.Read(ref _hygDefAssignTicks));
        var hygFix = Sec(Volatile.Read(ref _hygFixupsTicks));
        var hygSan = Sec(Volatile.Read(ref _hygSanitizeTicks));
        var hygEsc = Sec(Volatile.Read(ref _hygEscapeTicks));
        var print = Sec(Volatile.Read(ref _finishPrintScrubTicks));
        var il = Sec(Volatile.Read(ref _ilDisasmTicks));
        var io = Sec(Volatile.Read(ref _fileWriteTicks));
        var cpp = Sec(Volatile.Read(ref _cppHeaderTicks));
        var refs = Sec(Volatile.Read(ref _projectRefsTicks));
        var art = Sec(Volatile.Read(ref _artifactsTicks));
        var bclN = Volatile.Read(ref _bclTypes);
        var natN = Volatile.Read(ref _nativeTypes);
        var finishTotal = ann + core + loop + hyg + print;

        Console.Error.WriteLine("=== ExportAll Release Profile (IL2CPPSTUDIO_PROFILE_EXPORT=1) ===");
        Console.Error.WriteLine($"  Wall total             : {wallElapsed.TotalSeconds,8:F3} s  (PreLift EnsureLifted: {preLift,7:F3} s)");
        Console.Error.WriteLine($"  StubWrite + PEFile     : {stubPe,8:F3} s  (wall/serial)");
        Console.Error.WriteLine($"  ILSpy Decompile (BCL)  : {bcl,8:F3} s CPU ({bclN} types)");
        Console.Error.WriteLine($"  ILSpy Decompile (Nat)  : {native,8:F3} s CPU ({natN} types)");
        Console.Error.WriteLine($"  DecompilerService.Finish: {finishTotal,7:F3} s CPU total");
        Console.Error.WriteLine($"    - AstAnnotate        : {ann,8:F3} s CPU");
        Console.Error.WriteLine($"    - CoreTransforms     : {core,8:F3} s CPU (ElseIf..NamedConst)");
        Console.Error.WriteLine($"    - TempInline+Loops   : {loop,8:F3} s CPU (TempInliner..TailSink)");
        Console.Error.WriteLine($"    - CompileHygiene     : {hyg,8:F3} s CPU (Static..Escaper)");
        Console.Error.WriteLine($"      · Core={hygCore:F2}s Get={hygGet:F2}s Inh={hygInh:F2}s Exp={hygExp:F2}s Acc={hygAcc:F2}s Fix={hygFix:F2}s Def={hygDef:F2}s San={hygSan:F2}s Esc={hygEsc:F2}s");
        Console.Error.WriteLine($"    - Print+Scrub        : {print,8:F3} s CPU (Parens+Visitor+Scrub)");
        Console.Error.WriteLine($"  IL Disassembler        : {il,8:F3} s CPU");
        Console.Error.WriteLine($"  File.WriteAll* (.cs/il): {io,8:F3} s CPU");
        Console.Error.WriteLine($"  C++ Headers            : {cpp,8:F3} s");
        Console.Error.WriteLine($"  ProjectRefs + .csproj  : {refs,8:F3} s");
        Console.Error.WriteLine($"  ExportArtifacts        : {art,8:F3} s");
    }
}

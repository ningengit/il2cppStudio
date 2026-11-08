namespace Il2CppStudio.Core.Lifting.Runtime;

/// <summary>IL2CPP runtime/codegen helpers that generated code calls and that have a CIL meaning.</summary>
public enum RuntimeHelperKind
{
    Unknown,
    /// <summary>Il2CppCodeGenWriteBarrier(void** fieldAddress, void* value) — GC bookkeeping after a reference store; no CIL.</summary>
    WriteBarrier,
    /// <summary>il2cpp_codegen_object_new(Il2CppClass*) → <c>newobj</c> (fused with the following .ctor call).</summary>
    ObjectNew,
    /// <summary>SZArrayNew(Il2CppClass* arrayClass, length) → <c>newarr</c>.</summary>
    ArrayNew,
    /// <summary>Box(Il2CppClass*, void* value) → <c>box</c>.</summary>
    Box,
    /// <summary>Castclass/IsInst family (object, Il2CppClass*) → <c>castclass</c> / <c>isinst</c>.</summary>
    Cast,
    /// <summary>il2cpp_codegen_raise_exception(Exception*, MethodInfo*) → <c>throw</c>.</summary>
    Raise,
    /// <summary>Creates a runtime exception (InvalidCast, IndexOutOfRange, …) that is raised immediately — implicit in CIL.</summary>
    ExceptionFactory,
    /// <summary>Argument-less no-return helper (ThrowNullReference / ThrowIndexOutOfRange) — implicit in CIL.</summary>
    ThrowHelper,
    /// <summary>il2cpp_codegen_initialize_runtime_metadata(uintptr_t*) — runtime plumbing.</summary>
    MetadataInit,
    /// <summary>il2cpp_runtime_class_init(Il2CppClass*) — static constructor trigger; implicit in CIL.</summary>
    ClassInit,
    /// <summary>il2cpp_object_unbox(Il2CppObject*) — returns pointer to boxed value data at obj+0x10.</summary>
    Unbox,
    /// <summary>GetInterfaceInvokeDataFromVTableSlowPath(obj, iface, slot) — resolves VirtualInvokeData* for interface dispatch.</summary>
    InterfaceLookup,
    /// <summary>Compiler/runtime scope guard cleanup helper (e.g. finally-block IDisposable guard) — implicit in foreach.</summary>
    ScopeCleanup,
}

/// <summary>Classification of one helper address with the evidence used (logged for verification).</summary>
public sealed record RuntimeHelperInfo(ulong Va, RuntimeHelperKind Kind, string Evidence, int CallSites);

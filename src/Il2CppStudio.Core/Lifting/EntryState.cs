using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Cil;
using Iced.Intel;

namespace Il2CppStudio.Core.Lifting;

/// <summary>Value of a struct <c>this</c> (ldarg.0 is a managed pointer in value-type methods): <c>ldarg.0; ldobj T</c>.</summary>
public sealed class ThisStructExpr(TypeSignature type) : Expr
{
    public override TypeSignature Type => type;
    public override ValueKind Kind => ValueKind.Struct;
    public override void Emit(EmitContext ctx)
    {
        ctx.IL.Add(CilOpCodes.Ldarg_0);
        ctx.IL.Add(CilOpCodes.Ldobj, type.ToTypeDefOrRef());
    }
}

/// <summary>
/// Function-entry state of the lifted method: maps RCX/RDX/R8/R9/XMM0-3 and incoming stack slots to the CIL
/// parameters following <c>[ret buffer*] [this] params… [MethodInfo*]</c>, and allocates the return-buffer local.
/// </summary>
public sealed class EntryState(MethodDefinition method, MethodShape shape, RegisterTracker regs, MemoryResolver mem)
{
    public CilLocalVariable? ReturnBuffer { get; private set; }

    public void Initialize()
    {
        var position = 0;
        if (shape.HasReturnBuffer)
        {
            ReturnBuffer ??= regs.NewLocal(shape.ReturnType);
            regs.Set(Register.RCX, new AddressExpr(new LocalExpr(ReturnBuffer)));
            position++;
        }
        if (!shape.IsStatic)
        {
            var thisType = method.DeclaringType!.ToTypeSignature(shape.DeclaringIsValueType);
            Expr value = shape.DeclaringIsValueType
                ? new AddressExpr(new ThisStructExpr(thisType))
                : new ArgExpr(method.Parameters.ThisParameter!, thisType);
            Assign(position++, value, isFloat: false);
        }
        for (var i = 0; i < shape.Parameters.Count && i < method.Parameters.Count; i++)
        {
            var type = shape.Parameters[i];
            Expr arg = new ArgExpr(method.Parameters[i], type);
            if (MethodShape.PassedByPointer(type, mem)) arg = new AddressExpr(arg);
            Assign(position++, arg, MethodShape.IsFloat(type));
        }
        // The trailing `const MethodInfo*` argument is runtime plumbing.
        var mi = new MarkerExpr(MarkerKind.MethodInfo, shape.MethodIndex, "method");
        Assign(position, mi, isFloat: false);
    }

    private void Assign(int position, Expr value, bool isFloat)
    {
        if (position < 4)
            regs.Set(isFloat ? ArgumentLocations.FloatRegister(position) : ArgumentLocations.IntRegister(position), value);
        else
            regs.SetSlot(ArgumentLocations.IncomingStackKey(position), value);
    }
}

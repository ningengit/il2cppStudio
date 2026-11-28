using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.PE.DotNet.Metadata.Tables;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Lifting;

/// <summary>
/// When an open generic method body is lifted by borrowing a concrete generic instantiation (donor),
/// maps the donor's concrete type arguments back to the open generic parameters (<c>!0</c>, <c>!!0</c>) across
/// locals, member references, generic instantiations, and type tokens.
/// </summary>
internal static class GenericBodyReparameterizer
{
    public static void Apply(
        CilMethodBody body, ModuleDefinition module, Il2CppRegistration reg, StubAssemblyBuilder stubs, int donorSpecIndex)
    {
        if ((uint)donorSpecIndex >= (uint)reg.MethodSpecs.Count) return;
        var spec = reg.MethodSpecs[donorSpecIndex];
        var rules = new List<(TypeSignature Concrete, GenericParameterSignature OpenParam)>();

        void AddInst(int instIdx, GenericParameterType kind)
        {
            if (instIdx < 0 || (uint)instIdx >= (uint)reg.GenericInstVas.Count) return;
            var args = reg.ReadGenericInst(reg.GenericInstVas[instIdx]).TypeArgVas;
            for (var i = 0; i < args.Count; i++)
            {
                var concrete = stubs.ToSig(module, reg.GetTypeAt(args[i]));
                if (concrete.ElementType == ElementType.Object) continue;
                rules.Add((concrete, new GenericParameterSignature(kind, i)));
            }
        }

        AddInst(spec.ClassInstIndex, GenericParameterType.Type);
        AddInst(spec.MethodInstIndex, GenericParameterType.Method);
        if (rules.Count == 0) return;

        // Only allow bare concrete-type replacement when each concrete type maps uniquely to one generic parameter.
        var uniqueRules = rules
            .GroupBy(r => r.Concrete, SignatureComparer.Default)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToList();
        if (uniqueRules.Count == 0) return;

        TypeSignature RewriteType(TypeSignature sig)
        {
            foreach (var (concrete, openParam) in uniqueRules)
                if (SignatureComparer.Default.Equals(sig, concrete))
                    return openParam;

            return sig switch
            {
                GenericInstanceTypeSignature gi => new GenericInstanceTypeSignature(
                    gi.GenericType, gi.IsValueType, gi.TypeArguments.Select(RewriteType).ToArray()),
                SzArrayTypeSignature sz => RewriteType(sz.BaseType).MakeSzArrayType(),
                PointerTypeSignature ptr => RewriteType(ptr.BaseType).MakePointerType(),
                ByReferenceTypeSignature br => RewriteType(br.BaseType).MakeByReferenceType(),
                ArrayTypeSignature arr => new ArrayTypeSignature(RewriteType(arr.BaseType), arr.Rank),
                PinnedTypeSignature pin => RewriteType(pin.BaseType).MakePinnedType(),
                _ => sig,
            };
        }

        ITypeDefOrRef RewriteTypeDefOrRef(ITypeDefOrRef tdr) =>
            tdr is TypeSpecification { Signature: { } ts }
                ? new TypeSpecification(RewriteType(ts))
                : Expr.TypeToken(RewriteType(tdr.ToTypeSignature(false)));

        IMemberRefParent RewriteParent(IMemberRefParent? parent) => parent switch
        {
            TypeSpecification { Signature: { } ps } => new TypeSpecification(RewriteType(ps)),
            ITypeDefOrRef tdr => RewriteTypeDefOrRef(tdr),
            _ => parent!,
        };

        MethodSignature RewriteMethodSig(MethodSignature ms) => new(
            ms.Attributes, RewriteType(ms.ReturnType), ms.ParameterTypes.Select(RewriteType))
        { GenericParameterCount = ms.GenericParameterCount };

        IMethodDescriptor RewriteMethod(IMethodDescriptor md) => md switch
        {
            MethodSpecification mspec => new MethodSpecification(
                (IMethodDefOrRef)RewriteMethod(mspec.Method!),
                new GenericInstanceMethodSignature(mspec.Signature!.TypeArguments.Select(RewriteType))),
            MemberReference { Signature: MethodSignature mSig } mr =>
                new MemberReference(RewriteParent(mr.Parent), mr.Name, RewriteMethodSig(mSig)),
            _ => md,
        };

        IFieldDescriptor RewriteField(IFieldDescriptor fd) => fd switch
        {
            MemberReference { Signature: FieldSignature fSig } mr =>
                new MemberReference(RewriteParent(mr.Parent), mr.Name, new FieldSignature(RewriteType(fSig.FieldType))),
            _ => fd,
        };

        foreach (var lv in body.LocalVariables)
            lv.VariableType = RewriteType(lv.VariableType);

        foreach (var ins in body.Instructions)
        {
            switch (ins.Operand)
            {
                case ITypeDefOrRef tdr:
                    ins.Operand = RewriteTypeDefOrRef(tdr);
                    break;
                case IMethodDescriptor md:
                    ins.Operand = RewriteMethod(md);
                    break;
                case IFieldDescriptor fd:
                    ins.Operand = RewriteField(fd);
                    break;
            }
        }
    }
}

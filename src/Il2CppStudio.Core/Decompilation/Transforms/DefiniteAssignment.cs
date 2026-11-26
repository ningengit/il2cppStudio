using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Satisfies C# definite-assignment rules where lifted control flow is correct but not provably so to the compiler:
/// locals declared without an initializer get <c>= default</c>, and every <c>out</c> parameter is set to
/// <c>default</c> on entry. Both only add a store the original code overwrites before any read.
/// </summary>
public static class DefiniteAssignment
{
    public static void Apply(SyntaxTree tree) => Apply(tree, AstWalk.ScanFlags(tree));

    internal static void Apply(SyntaxTree tree, in TreeFlags flags)
    {
        if (!flags.HasBodies) return;
        var decls = new List<VariableDeclarationStatement>();
        var blocks = new List<BlockStatement>();
        var methods = new List<MethodDeclaration>();
        AstWalk.ForEachNode(tree, n =>
        {
            switch (n)
            {
                case VariableDeclarationStatement vds: decls.Add(vds); break;
                case BlockStatement bs: blocks.Add(bs); break;
                case MethodDeclaration md: methods.Add(md); break;
            }
        });

        foreach (var decl in decls)
        {
            if (decl.Type is ComposedType { HasRefSpecifier: true } ct)
            {
                foreach (var v in decl.Variables)
                {
                    if (!v.Initializer.IsNull && v.Initializer is not DefaultValueExpression) continue;
                    v.Initializer = MakeDefaultRefExpr(ct);
                }
                continue;
            }
            if (decl.Modifiers.HasFlag(Modifiers.Const) || decl.Type is SimpleType { Identifier: "var" }) continue;
            foreach (var v in decl.Variables)
                if (v.Initializer.IsNull) v.Initializer = new DefaultValueExpression(decl.Type.Clone());
            if (decl.Variables.Count == 1 && decl.Variables.Single() is { Initializer: DefaultValueExpression } dv
                && decl.NextSibling is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression id, Right: DefaultValueExpression } } next
                && id.Identifier == dv.Name)
                next.Remove();
        }

        foreach (var block in blocks)
        {
            if (!AstWalk.HasAny<LabelStatement>(block)) continue;
            var toHoist = new List<VariableDeclarationStatement>();
            var earlierVarsByStmt = new HashSet<string>(StringComparer.Ordinal);
            var shadowedStmts = new HashSet<VariableDeclarationStatement>();

            foreach (var stmt in block.Statements)
            {
                if (stmt is VariableDeclarationStatement d && !d.Modifiers.HasFlag(Modifiers.Const) && d.Type is not SimpleType { Identifier: "var" } && d.Variables.Count > 0)
                {
                    var hasShadow = false;
                    foreach (var dv in d.Variables)
                        if (earlierVarsByStmt.Contains(dv.Name)) { hasShadow = true; break; }
                    if (hasShadow) shadowedStmts.Add(d);
                    toHoist.Add(d);
                }
                AstWalk.Visit<VariableInitializer>(stmt, vi => earlierVarsByStmt.Add(vi.Name));
            }

            for (var hi = toHoist.Count - 1; hi >= 0; hi--)
            {
                var decl = toHoist[hi];
                if (shadowedStmts.Contains(decl)) continue;
                if (decl.Type is ComposedType { HasRefSpecifier: true } rct)
                {
                    if (decl.Variables.Count == 1 && decl.Variables.Single() is var rv)
                    {
                        if (IsDummyRefInit(rv.Initializer))
                        {
                            if (block.Statements.FirstOrDefault() == decl) continue;
                            decl.Remove();
                            block.Statements.InsertBefore(block.Statements.FirstOrDefault(), decl);
                        }
                        else if (!rv.Initializer.IsNull)
                        {
                            var rhs = rv.Initializer is DirectionExpression ? rv.Initializer.Detach() : new DirectionExpression(FieldDirection.Ref, rv.Initializer.Detach());
                            var hoisted = new VariableDeclarationStatement(rct.Clone(), rv.Name, MakeDefaultRefExpr(rct));
                            decl.ReplaceWith(new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(rv.Name), rhs)));
                            block.Statements.InsertBefore(block.Statements.FirstOrDefault(), hoisted);
                        }
                    }
                    continue;
                }
                if (decl.Variables.All(v => v.Initializer is DefaultValueExpression))
                {
                    if (block.Statements.FirstOrDefault() == decl) continue;
                    decl.Remove();
                    block.Statements.InsertBefore(block.Statements.FirstOrDefault(), decl);
                }
                else if (decl.Variables.Count == 1
                    && decl.Variables.Single() is { Initializer: { IsNull: false } init } v
                    && init is not ArrayInitializerExpression)
                {
                    var hoisted = new VariableDeclarationStatement(decl.Type.Clone(), v.Name, new DefaultValueExpression(decl.Type.Clone()));
                    decl.ReplaceWith(new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(v.Name), init.Detach())));
                    block.Statements.InsertBefore(block.Statements.FirstOrDefault(), hoisted);
                }
            }
        }

        BindDummyRefInitsToLocalTargets(blocks);

        foreach (var method in methods)
        {
            if (method.Body.IsNull) continue;
            var outs = method.Parameters.Where(p => p.ParameterModifier == ICSharpCode.Decompiler.TypeSystem.ReferenceKind.Out).ToList();
            for (var pi = outs.Count - 1; pi >= 0; pi--)
            {
                var p = outs[pi];
                var init = new ExpressionStatement(new AssignmentExpression(new IdentifierExpression(p.Name), new DefaultValueExpression(p.Type.Clone())));
                if (method.Body.Statements.FirstOrDefault() is { } first) method.Body.Statements.InsertBefore(first, init);
                else method.Body.Statements.Add(init);
            }
        }
    }

    private static bool IsDummyRefInit(Expression expr) =>
        expr is DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: IndexerExpression { Target: ArrayCreateExpression } };

    private static DirectionExpression MakeDefaultRefExpr(ComposedType ct)
    {
        AstType baseElem = ct.BaseType.Clone();
        if (ct.PointerRank > 0 || ct.HasNullableSpecifier)
            baseElem = new ComposedType { BaseType = baseElem, PointerRank = ct.PointerRank, HasNullableSpecifier = ct.HasNullableSpecifier };
        var arrCreate = new ArrayCreateExpression { Type = baseElem };
        arrCreate.Arguments.Add(new PrimitiveExpression(1));
        foreach (var spec in ct.ArraySpecifiers) arrCreate.AdditionalArraySpecifiers.Add((ArraySpecifier)spec.Clone());
        return new DirectionExpression(FieldDirection.Ref, new IndexerExpression(arrCreate, new PrimitiveExpression(0)));
    }

    private static void BindDummyRefInitsToLocalTargets(List<BlockStatement> blocks)
    {
        foreach (var block in blocks)
        {
            if (block.Ancestors.Any(a => a is MethodDeclaration { ReturnType: ComposedType { HasRefSpecifier: true } }
                or PropertyDeclaration { ReturnType: ComposedType { HasRefSpecifier: true } }
                or IndexerDeclaration { ReturnType: ComposedType { HasRefSpecifier: true } }))
                continue;

            var refDecls = block.Statements.OfType<VariableDeclarationStatement>()
                .Where(d => d.Type is ComposedType { HasRefSpecifier: true } && d.Variables.Count == 1
                    && IsDummyRefInit(d.Variables.Single().Initializer))
                .ToList();
            if (refDecls.Count == 0) continue;

            foreach (var refDecl in refDecls)
            {
                var rv = refDecl.Variables.Single();
                foreach (var assign in block.Descendants.OfType<AssignmentExpression>())
                {
                    if (assign.Operator != AssignmentOperatorType.Assign
                        || assign.Left is not IdentifierExpression lid || lid.Identifier != rv.Name
                        || assign.Right is not DirectionExpression { FieldDirection: FieldDirection.Ref, Expression: var rhs })
                        continue;

                    var cur = rhs;
                    while (cur is ParenthesizedExpression pe) cur = pe.Expression;
                    while (cur is MemberReferenceExpression mr)
                    {
                        cur = mr.Target;
                        while (cur is ParenthesizedExpression pe2) cur = pe2.Expression;
                    }
                    if (cur is not IdentifierExpression targetId) continue;

                    var stmts = block.Statements.ToList();
                    var refIdx = stmts.IndexOf(refDecl);
                    var targetIdx = stmts.FindIndex(s => s is VariableDeclarationStatement { Type: not ComposedType { HasRefSpecifier: true } } td
                        && td.Variables.Any(v => v.Name == targetId.Identifier));
                    if (targetIdx < 0) continue;

                    var targetDecl = (VariableDeclarationStatement)stmts[targetIdx];
                    if (targetIdx > refIdx)
                    {
                        targetDecl.Remove();
                        block.Statements.InsertBefore(refDecl, targetDecl);
                    }
                    rv.Initializer = new DirectionExpression(FieldDirection.Ref, new IdentifierExpression(targetId.Identifier));
                    break;
                }
            }
        }
    }
}

using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Transforms;

/// <summary>
/// Folds inlined field compound updates:
/// <list type="bullet">
/// <item><c>float num = F; … F = num - E;</c> → <c>… F -= E;</c> (also across an <c>if</c> in the same block);</item>
/// <item><c>float num = F + E; F = num; … use(num)</c> → <c>F += E; … use(F)</c>.</item>
/// </list>
/// </summary>
public static class CompoundFieldFold
{
    public static void Apply(SyntaxTree tree)
    {
        var decls = AstWalk.Collect<VariableDeclarationStatement>(tree, static d => d.Variables.Count == 1 && d.Parent is BlockStatement);
        if (decls.Count == 0) return;
        FoldDelayedCompound(tree, decls);
        FoldEagerCompound(tree, decls);
    }

    private static void FoldDelayedCompound(SyntaxTree tree, List<VariableDeclarationStatement> decls)
    {
        Dictionary<BlockStatement, Dictionary<string, List<IdentifierExpression>>>? blockUses = null;
        foreach (var decl in decls)
        {
            if (decl.Parent is not BlockStatement block || !AstWalk.IsAttachedTo(decl, tree)) continue;
            var v = decl.Variables.Single();
            if (!IsField(v.Initializer, decl)) continue;
            var field = v.Initializer.ToString();
            blockUses ??= new Dictionary<BlockStatement, Dictionary<string, List<IdentifierExpression>>>();
            if (!blockUses.TryGetValue(block, out var byName))
                blockUses[block] = byName = CollectBlockUses(block);
            var uses = byName.TryGetValue(v.Name, out var rawUses)
                ? rawUses.Where(u => AstWalk.IsAttachedTo(u, block)).ToList()
                : null;
            if (uses is null || uses.Count != 1 || uses[0].Parent is not BinaryOperatorExpression bin) continue;
            var tempOnLeft = bin.Left == uses[0];
            if (!tempOnLeft && !(bin.Right == uses[0] && bin.Operator is BinaryOperatorType.Add or BinaryOperatorType.Multiply)) continue;
            if (bin.Parent is not AssignmentExpression { Operator: AssignmentOperatorType.Assign } assign || assign.Right != bin || assign.Left.ToString() != field) continue;
            var op = ToCompoundOp(bin.Operator);
            if (op is null || WritesBetween(block, decl, assign, field)) continue;
            if (AstWalk.IsAttachedTo(assign, block))
                assign.ReplaceWith(new AssignmentExpression(assign.Left.Detach(), op.Value, (tempOnLeft ? bin.Right : bin.Left).Detach()));
            if (AstWalk.IsAttachedTo(decl, block)) decl.Remove();
        }
    }

    private static void FoldEagerCompound(SyntaxTree tree, List<VariableDeclarationStatement> decls)
    {
        Dictionary<BlockStatement, Dictionary<string, List<IdentifierExpression>>>? blockUses = null;
        foreach (var decl in decls)
        {
            if (decl.Parent is not BlockStatement block || !AstWalk.IsAttachedTo(decl, tree)) continue;
            var v = decl.Variables.Single();
            if (v.Initializer is not BinaryOperatorExpression bin || ToCompoundOp(bin.Operator) is not { } op) continue;

            Expression? fieldExpr = IsField(bin.Left, decl) ? bin.Left : IsField(bin.Right, decl) && bin.Operator is BinaryOperatorType.Add or BinaryOperatorType.Multiply ? bin.Right : null;
            if (fieldExpr is null) continue;
            var fieldText = fieldExpr.ToString();

            if (decl.NextSibling is not ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Right: IdentifierExpression rhs } store } nextStmt
                || rhs.Identifier != v.Name || store.Left.ToString() != fieldText) continue;

            var deltaExpr = fieldExpr == bin.Left ? bin.Right : bin.Left;
            blockUses ??= new Dictionary<BlockStatement, Dictionary<string, List<IdentifierExpression>>>();
            if (!blockUses.TryGetValue(block, out var byName))
                blockUses[block] = byName = CollectBlockUses(block);
            var trailing = byName.TryGetValue(v.Name, out var uses)
                ? uses.Where(u => u != rhs && AstWalk.IsAttachedTo(u, block)).ToList()
                : [];
            if (trailing.Any(u => !CanReplaceWithField(block, nextStmt, u, fieldText))) continue;

            if (AstWalk.IsAttachedTo(store, block))
                store.ReplaceWith(new AssignmentExpression(store.Left.Detach(), op, deltaExpr.Detach()));
            foreach (var u in trailing)
                if (AstWalk.IsAttachedTo(u, block)) u.ReplaceWith(fieldExpr.Clone());
            if (AstWalk.IsAttachedTo(decl, block)) decl.Remove();
        }
    }

    private static Dictionary<string, List<IdentifierExpression>> CollectBlockUses(BlockStatement block)
    {
        var byName = new Dictionary<string, List<IdentifierExpression>>(StringComparer.Ordinal);
        AstWalk.Visit<IdentifierExpression>(block, i =>
        {
            if (!byName.TryGetValue(i.Identifier, out var list))
                byName[i.Identifier] = list = new List<IdentifierExpression>();
            list.Add(i);
        });
        return byName;
    }

    private static AssignmentOperatorType? ToCompoundOp(BinaryOperatorType op) => op switch
    {
        BinaryOperatorType.Add => AssignmentOperatorType.Add,
        BinaryOperatorType.Subtract => AssignmentOperatorType.Subtract,
        BinaryOperatorType.Multiply => AssignmentOperatorType.Multiply,
        BinaryOperatorType.Divide => AssignmentOperatorType.Divide,
        _ => null,
    };

    private static bool IsField(Expression e, AstNode context)
    {
        if (e is MemberReferenceExpression { Target: ThisReferenceExpression }) return true;
        if (e is not IdentifierExpression id) return false;
        if (id.Annotation<ICSharpCode.Decompiler.Semantics.MemberResolveResult>() is not null) return true;
        var type = context.Ancestors.OfType<TypeDeclaration>().FirstOrDefault();
        return type is not null && type.Members.OfType<FieldDeclaration>().SelectMany(f => f.Variables).Any(v => v.Name == id.Identifier);
    }

    private static bool CanReplaceWithField(BlockStatement block, AstNode afterNode, IdentifierExpression read, string field) =>
        !WritesBetween(block, afterNode, read, field);

    private static bool WritesBetween(BlockStatement block, AstNode startNode, AstNode target, string field)
    {
        var started = false;
        foreach (var n in block.Descendants)
        {
            if (n == startNode) { started = true; continue; }
            if (!started) continue;
            if (n == target) return false;
            if (n is AssignmentExpression a && a.Left.ToString() == field) return true;
            if (n is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement } u && u.Expression.ToString() == field) return true;
        }
        return true;
    }
}

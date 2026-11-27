using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Il2CppStudio.Core.Decompilation.Coroutines;

/// <summary>
/// Turns the state dispatch at the top of <c>MoveNext</c> into straight-line code with resume labels:
/// the entry arm (state 0) first, then every arm a resume state jumps to (labelled), then the shared remainder
/// (labelled when a state resumes there). Arms that can fall through get an explicit jump to the remainder, so the
/// layout is exactly equivalent; the simplifier removes jumps that land on the next statement.
/// </summary>
public static class DispatchLayout
{
    public const string RestLabel = "resume_rest";

    public static List<Statement>? Build(BlockStatement body, IReadOnlyCollection<long> resumeStates, Dictionary<long, string> labels)
    {
        var stmts = body.Statements.ToList();
        var aliases = new HashSet<string>();
        var i = 0;
        var exitCounter = 0;
        while (i < stmts.Count)
        {
            if (stmts[i] is VariableDeclarationStatement { Variables.Count: 1 } v)
            {
                var init = v.Variables.Single().Initializer;
                if (init.IsNull || init is DefaultValueExpression) { i++; continue; }
                if (IsThisField(init)) { InlineThisAlias(body, v.Variables.Single().Name, v); stmts.RemoveAt(i); continue; }
                if (!StateEvaluator.IsStateOnly(init, aliases)) break;
                aliases.Add(v.Variables.Single().Name);
                i++;
                continue;
            }
            if (stmts[i] is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression lhsId, Right: var rhs } } es)
            {
                if (IsThisField(rhs))
                {
                    RemoveEmptyDecl(body, stmts, ref i, lhsId.Identifier);
                    InlineThisAlias(body, lhsId.Identifier, es);
                    stmts.RemoveAt(i);
                    continue;
                }
                if (StateEvaluator.IsStateOnly(rhs, aliases) && IsEmptyDeclFor(stmts, i, lhsId.Identifier))
                {
                    RemoveEmptyDecl(body, stmts, ref i, lhsId.Identifier);
                    aliases.Add(lhsId.Identifier);
                    es.Remove();
                    stmts.RemoveAt(i);
                    continue;
                }
            }
            break;
        }
        LowerStateSwitchDispatch(body, aliases, i, ref exitCounter, resumeStates);
        var liftedTails = new List<Statement>();
        DispatchNormalizer.Normalize(body, aliases, nested: false, rootBody: body, liftedTails: liftedTails);
        stmts = body.Statements.ToList();
        NormalizeStateOnlyTrailingReturn(body, stmts, i, aliases, resumeStates);
        stmts = body.Statements.ToList();
        UnwrapInvertedDispatch(body, stmts, i, aliases);
        stmts = body.Statements.ToList();
        MergeSequentialArms(stmts, i, aliases);
        var chain = i < stmts.Count && stmts[i] is IfElseStatement ifs && StateEvaluator.IsStateOnly(ifs.Condition, aliases) ? ifs : null;
        var rest = stmts.Skip(chain is null ? i : i + 1).Where(s => s.Parent == body).ToList();

        var armOf = new Dictionary<long, Statement?>();
        foreach (var state in resumeStates.Prepend(0).Distinct())
        {
            if (chain is null) { armOf[state] = null; continue; }
            if (!TrySelect(chain, state, aliases, out var arm)) return null;
            armOf[state] = arm;
        }
        if (!SubstituteAliases(armOf, aliases)) return null;

        var output = new List<Statement>();
        var restUsed = false;
        var emitted = new List<Statement>();
        foreach (var arm in armOf.Values.Where(a => a is not null).Distinct().OrderBy(a => a == armOf[0] ? 0 : 1))
        {
            var label = $"resume_arm{emitted.Count}";
            foreach (var (state, a) in armOf) if (a == arm && state != 0) labels[state] = label;
            if (armOf.Any(kv => kv.Value == arm && kv.Key != 0)) output.Add(new LabelStatement { Label = label });
            var armStmts = arm is BlockStatement b ? b.Statements.ToList() : [arm!];
            foreach (var s in armStmts) output.Add(s.Detach());
            if (CanFallThrough(armStmts.LastOrDefault())) { output.Add(new GotoStatement(RestLabel)); restUsed = true; }
            emitted.Add(arm!);
        }
        foreach (var (state, a) in armOf) if (a is null && state != 0) { labels[state] = RestLabel; restUsed = true; }
        if (liftedTails.OfType<GotoStatement>().Any(g => g.Label == RestLabel)) restUsed = true;
        if (armOf[0] is null && !restUsed) { /* entry falls into the remainder directly */ }
        if (restUsed) output.Add(new LabelStatement { Label = RestLabel });
        foreach (var s in rest) output.Add(s.Detach());
        foreach (var s in liftedTails) output.Add(s);
        foreach (var a in aliases)
            if (output.Any(s => s.Descendants.OfType<IdentifierExpression>().Any(id => id.Identifier == a)))
                output.Insert(0, new VariableDeclarationStatement(new PrimitiveType("int"), a));
        return output;
    }

    private static bool IsEmptyDeclFor(List<Statement> stmts, int beforeIdx, string name)
    {
        for (var k = 0; k < beforeIdx; k++)
            if (stmts[k] is VariableDeclarationStatement { Variables.Count: 1 } vd
                && vd.Variables.Single() is { Name: var vn, Initializer: var init }
                && vn == name && (init.IsNull || init is DefaultValueExpression))
                return true;
        return false;
    }

    private static void RemoveEmptyDecl(BlockStatement body, List<Statement> stmts, ref int curIdx, string name)
    {
        for (var k = 0; k < curIdx; k++)
        {
            if (stmts[k] is VariableDeclarationStatement { Variables.Count: 1 } vd
                && vd.Variables.Single() is { Name: var vn, Initializer: var init }
                && vn == name && (init.IsNull || init is DefaultValueExpression))
            {
                vd.Remove();
                stmts.RemoveAt(k);
                curIdx--;
                break;
            }
        }
    }

    private static void LowerStateSwitchDispatch(BlockStatement body, ISet<string> aliases, int start, ref int exitCounter, IReadOnlyCollection<long> resumeStates)
    {
        var stmts = body.Statements.ToList();
        if (start >= stmts.Count) return;

        IfElseStatement? guardIf = null;
        SwitchStatement? sw = null;
        if (stmts[start] is SwitchStatement directSw && StateEvaluator.IsStateOnly(directSw.Expression, aliases))
        {
            sw = directSw;
        }
        else if (stmts[start] is IfElseStatement { FalseStatement.IsNull: true, TrueStatement: BlockStatement gtb } gif
            && StateEvaluator.IsStateOnly(gif.Condition, aliases))
        {
            var gStmts = gtb.Statements.ToList();
            var gi = 0;
            while (gi < gStmts.Count)
            {
                if (gStmts[gi] is VariableDeclarationStatement { Variables.Count: 1 } gv)
                {
                    var gInit = gv.Variables.Single().Initializer;
                    if (gInit.IsNull || gInit is DefaultValueExpression) { gi++; continue; }
                    if (IsThisField(gInit)) { InlineThisAlias(body, gv.Variables.Single().Name, gv); gi++; continue; }
                    break;
                }
                if (gStmts[gi] is ExpressionStatement { Expression: AssignmentExpression { Operator: AssignmentOperatorType.Assign, Left: IdentifierExpression lhsId, Right: var rhs } } ges
                    && IsThisField(rhs))
                {
                    InlineThisAlias(body, lhsId.Identifier, ges);
                    gi++;
                    continue;
                }
                break;
            }
            if (gi < gStmts.Count && gStmts[gi] is SwitchStatement innerSw && StateEvaluator.IsStateOnly(innerSw.Expression, aliases))
            {
                // Drop leading decls (ScopeRepair restores needed locals) and trailing no-ops after the switch.
                for (var k = 0; k < gi; k++)
                    if (gStmts[k].Parent == gtb && gStmts[k] is VariableDeclarationStatement vd && (vd.Variables.Single().Initializer.IsNull || vd.Variables.Single().Initializer is DefaultValueExpression))
                        vd.Remove();
                guardIf = gif;
                sw = innerSw;
            }
        }
        if (sw is null) return;

        TryInlineTrailingStateReturn(sw, guardIf, aliases, resumeStates);

        var exitLabel = $"switch_exit_{exitCounter++}";
        var usedExit = RewriteSwitchBreaks(sw, exitLabel);

        var extractedTail = new List<Statement>();
        var sections = sw.SwitchSections.ToList();
        foreach (var sec in sections)
        {
            var secStmts = sec.Statements.ToList();
            var lblIdx = secStmts.FindIndex(s => s is LabelStatement);
            if (lblIdx >= 0 && (lblIdx == 0 || !CanFallThrough(secStmts[lblIdx - 1])))
            {
                for (var sIdx = lblIdx; sIdx < secStmts.Count; sIdx++)
                    extractedTail.Add(secStmts[sIdx].Detach());
            }
        }

        IfElseStatement? head = null;
        IfElseStatement? tail = null;
        BlockStatement? defaultBlock = null;
        foreach (var sec in sections)
        {
            var block = new BlockStatement();
            foreach (var s in sec.Statements.ToList()) block.Statements.Add(s.Detach());
            if (sec.CaseLabels.Any(cl => cl.Expression.IsNull))
            {
                defaultBlock = block;
                continue;
            }
            Expression? cond = null;
            foreach (var cl in sec.CaseLabels)
            {
                var eq = new BinaryOperatorExpression(sw.Expression.Clone(), BinaryOperatorType.Equality, cl.Expression.Detach());
                cond = cond is null ? eq : new BinaryOperatorExpression(cond, BinaryOperatorType.ConditionalOr, eq);
            }
            if (cond is null) continue;
            var branch = new IfElseStatement(cond, block);
            if (head is null) head = tail = branch;
            else { tail!.FalseStatement = branch; tail = branch; }
        }
        if (tail is not null && defaultBlock is not null)
            tail.FalseStatement = defaultBlock;

        Statement replacement = head ?? (Statement?)defaultBlock ?? new EmptyStatement();
        if (guardIf is not null)
        {
            sw.ReplaceWith(replacement);
            if (extractedTail.Count > 0 || usedExit)
            {
                guardIf.FalseStatement = new BlockStatement { Statements = { new GotoStatement(exitLabel) } };
                usedExit = true;
            }
            Statement anchor = guardIf;
            foreach (var ts in extractedTail) { body.Statements.InsertAfter(anchor, ts); anchor = ts; }
            if (usedExit) body.Statements.InsertAfter(anchor, new LabelStatement { Label = exitLabel });
        }
        else
        {
            Statement anchor = sw;
            sw.ReplaceWith(replacement);
            anchor = replacement;
            foreach (var ts in extractedTail) { body.Statements.InsertAfter(anchor, ts); anchor = ts; }
            if (usedExit) body.Statements.InsertAfter(anchor, new LabelStatement { Label = exitLabel });
        }
    }

    private static void TryInlineTrailingStateReturn(
        SwitchStatement sw,
        IfElseStatement? guardIf,
        ISet<string> aliases,
        IReadOnlyCollection<long> resumeStates)
    {
        if (guardIf is not null && guardIf.TrueStatement is BlockStatement gtb && gtb.Statements.LastOrDefault() != sw)
            return;
        Statement anchor = guardIf ?? (Statement)sw;
        if (anchor.GetNextSibling(n => n is Statement) is not ReturnStatement trailingRet
            || trailingRet.Expression.IsNull
            || !StateEvaluator.IsStateOnly(trailingRet.Expression, aliases))
            return;

        if (sw.SwitchSections.Any(sec => sec.Descendants.OfType<IdentifierExpression>().Any(id => aliases.Contains(id.Identifier) && IsAliasWrite(id))))
            return;

        var sectionReturns = new List<(SwitchSection Section, bool Value)>();
        SwitchSection? defaultSection = null;
        var coveredStates = new HashSet<long>();

        foreach (var sec in sw.SwitchSections)
        {
            if (sec.CaseLabels.Any(cl => cl.Expression.IsNull))
            {
                if (sec.CaseLabels.Count != 1) return;
                defaultSection = sec;
                continue;
            }

            bool? commonBool = null;
            foreach (var cl in sec.CaseLabels)
            {
                if (cl.Expression is not PrimitiveExpression { Value: IConvertible raw } || raw is bool)
                    return;
                var caseVal = Convert.ToInt64(raw);
                if (StateEvaluator.Eval(trailingRet.Expression, caseVal, aliases) is not { } eval)
                    return;
                var b = eval != 0;
                if (commonBool is null) commonBool = b;
                else if (commonBool.Value != b) return;
                coveredStates.Add(caseVal);
            }
            if (commonBool is null) return;
            sectionReturns.Add((sec, commonBool.Value));
        }

        if (!resumeStates.Prepend(0L).All(coveredStates.Contains))
            return;
        if (StateEvaluator.Eval(trailingRet.Expression, -1L, aliases) is not { } defaultEval)
            return;
        var defaultBool = defaultEval != 0;

        foreach (var (sec, val) in sectionReturns)
            ReplaceSwitchBreaksWithReturn(sec, val);
        if (defaultSection is not null)
            ReplaceSwitchBreaksWithReturn(defaultSection, defaultBool);

        trailingRet.Expression = new PrimitiveExpression(defaultBool);
    }

    private static void ReplaceSwitchBreaksWithReturn(SwitchSection sec, bool value)
    {
        var stack = new Stack<AstNode>();
        stack.Push(sec);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is BreakStatement bs)
            {
                bs.ReplaceWith(new ReturnStatement(new PrimitiveExpression(value)));
                continue;
            }
            if (n is SwitchStatement or WhileStatement or DoWhileStatement or ForStatement or ForeachStatement)
                continue;
            for (var ch = n.LastChild; ch is not null; ch = ch.PrevSibling)
                stack.Push(ch);
        }
        if (CanFallThrough(sec.Statements.LastOrDefault()))
            sec.Statements.Add(new ReturnStatement(new PrimitiveExpression(value)));
    }

    private static bool RewriteSwitchBreaks(SwitchStatement sw, string exitLabel)
    {
        var used = false;
        var stack = new Stack<AstNode>();
        foreach (var sec in sw.SwitchSections) stack.Push(sec);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (n is BreakStatement bs)
            {
                bs.ReplaceWith(new GotoStatement(exitLabel));
                used = true;
                continue;
            }
            if (n is SwitchStatement or WhileStatement or DoWhileStatement or ForStatement or ForeachStatement)
                continue;
            for (var ch = n.LastChild; ch is not null; ch = ch.PrevSibling)
                stack.Push(ch);
        }
        return used;
    }

    private static void NormalizeStateOnlyTrailingReturn(BlockStatement body, List<Statement> stmts, int start, ISet<string> aliases, IReadOnlyCollection<long> resumeStates)
    {
        if (start + 1 >= stmts.Count) return;
        if (stmts[start] is not IfElseStatement { FalseStatement.IsNull: true, TrueStatement: BlockStatement tb } ifs
            || !StateEvaluator.IsStateOnly(ifs.Condition, aliases) || !CanFallThrough(tb))
            return;
        if (stmts[start + 1] is not ReturnStatement ret || ret.Expression.IsNull || !StateEvaluator.IsStateOnly(ret.Expression, aliases))
            return;
        if (tb.Descendants.OfType<IdentifierExpression>().Any(id => aliases.Contains(id.Identifier)))
            return;
        foreach (var st in resumeStates.Append(-1L).Append(0L).Append(999L).Distinct())
        {
            var vc = StateEvaluator.Eval(ifs.Condition, st, aliases);
            var vr = StateEvaluator.Eval(ret.Expression, st, aliases);
            if (vc is null || vr is null || (vc.Value != 0) != (vr.Value != 0))
                return;
        }
        tb.Statements.Add(new ReturnStatement(new PrimitiveExpression(true)));
        ret.Expression = new PrimitiveExpression(false);
    }

    private static void UnwrapInvertedDispatch(BlockStatement body, List<Statement> stmts, int start, ISet<string> aliases)
    {
        if (start >= stmts.Count || stmts[start] is not IfElseStatement { FalseStatement.IsNull: true, TrueStatement: BlockStatement tb } ifs
            || !StateEvaluator.IsStateOnly(ifs.Condition, aliases) || tb.Statements.Count <= 1
            || tb.Statements.First() is not IfElseStatement inner || !StateEvaluator.IsStateOnly(inner.Condition, aliases)
            || CanFallThrough(tb)) return;
        var falseArm = new List<Statement>();
        for (var k = start + 1; k < stmts.Count && stmts[k] is not LabelStatement; k++)
        {
            falseArm.Add(stmts[k]);
            if (!CanFallThrough(stmts[k])) break;
        }
        if (falseArm.Count == 0 || CanFallThrough(falseArm[^1])) return;
        var fb = new BlockStatement();
        foreach (var s in falseArm) fb.Statements.Add(s.Detach());
        ifs.FalseStatement = fb;
        Statement anchor = ifs;
        foreach (var s in tb.Statements.Skip(1).ToList()) { s.Remove(); body.Statements.InsertAfter(anchor, s); anchor = s; }
    }

    /// <summary>
    /// <c>if (s == 0) { …; return …; } if (s == 1) { … }</c> is an else-if chain in disguise: an arm that cannot fall
    /// through makes the following state test its else branch. Nests such tests so <see cref="TrySelect"/> sees one chain.
    /// </summary>
    private static void MergeSequentialArms(List<Statement> stmts, int start, ISet<string> aliases)
    {
        if (start >= stmts.Count || stmts[start] is not IfElseStatement head || !StateEvaluator.IsStateOnly(head.Condition, aliases)) return;
        var tail = head;
        for (var k = start + 1; k < stmts.Count; k++)
        {
            if (!tail.FalseStatement.IsNull || CanFallThrough(LastOf(tail.TrueStatement))) return;
            if (stmts[k] is not IfElseStatement next || !StateEvaluator.IsStateOnly(next.Condition, aliases)) return;
            tail.FalseStatement = next.Detach();
            tail = next;
            while (tail.FalseStatement is IfElseStatement deeper) tail = deeper;
        }
    }

    private static Statement? LastOf(Statement s) => s is BlockStatement b ? b.Statements.LastOrDefault() : s;

    /// <summary>Replaces state aliases inside an arm by the single state that reaches it; allows write-before-read reuse in shared arms.</summary>
    private static bool SubstituteAliases(Dictionary<long, Statement?> armOf, ISet<string> aliases)
    {
        foreach (var group in armOf.Where(kv => kv.Value is not null).GroupBy(kv => kv.Value!))
        {
            var uses = group.Key.Descendants.OfType<IdentifierExpression>().Where(id => aliases.Contains(id.Identifier)).ToList();
            if (uses.Count == 0) continue;
            if (group.Count() != 1)
            {
                if (AllAliasesWrittenBeforeRead(group.Key, uses)) continue;
                return false;
            }
            var writtenAliases = new HashSet<string>(StringComparer.Ordinal);
            foreach (var u in uses)
            {
                if (IsAliasWrite(u))
                {
                    writtenAliases.Add(u.Identifier);
                    continue;
                }
                if (writtenAliases.Contains(u.Identifier)) continue;
                var constant = new PrimitiveExpression((int)group.First().Key);
                u.ReplaceWith(constant);
                ConstantFolder.FoldUpwards(constant);
            }
        }
        return true;
    }

    private static bool IsAliasWrite(IdentifierExpression u) =>
        (u.Parent is AssignmentExpression a && a.Left == u && a.Operator == AssignmentOperatorType.Assign)
        || u.Parent is DirectionExpression { FieldDirection: FieldDirection.Out }
        || u.Parent is UnaryOperatorExpression { Operator: UnaryOperatorType.Increment or UnaryOperatorType.Decrement or UnaryOperatorType.PostIncrement or UnaryOperatorType.PostDecrement };

    private static bool AllAliasesWrittenBeforeRead(Statement armRoot, List<IdentifierExpression> uses)
    {
        foreach (var byName in uses.GroupBy(u => u.Identifier, StringComparer.Ordinal))
        {
            var first = byName.First();
            if (first.Parent is not AssignmentExpression { Operator: AssignmentOperatorType.Assign } assign || assign.Left != first)
                return false;
            if (assign.Right.DescendantsAndSelf.OfType<IdentifierExpression>().Any(id => id.Identifier == byName.Key))
                return false;
            if (assign.Parent is not ExpressionStatement es || es.Parent is not BlockStatement writeBlock)
                return false;
            if (writeBlock != armRoot && armRoot is not BlockStatement) return false;
            var writeStmtIdx = IndexOfStatement(writeBlock, es);
            if (writeStmtIdx < 0) return false;
            foreach (var u in byName.Skip(1))
            {
                var useIdx = -1;
                for (AstNode? a = u; a is not null; a = a.Parent)
                {
                    if (a.Parent == writeBlock && a is Statement st)
                    {
                        useIdx = IndexOfStatement(writeBlock, st);
                        break;
                    }
                }
                if (useIdx <= writeStmtIdx) return false;
            }
        }
        return true;
    }

    private static int IndexOfStatement(BlockStatement block, Statement target)
    {
        var idx = 0;
        foreach (var s in block.Statements)
        {
            if (ReferenceEquals(s, target)) return idx;
            idx++;
        }
        return -1;
    }

    private static bool IsThisField(Expression e) =>
        e is IdentifierExpression { Identifier: StateMachineInfo.ThisField }
          or MemberReferenceExpression { Target: ThisReferenceExpression, MemberName: StateMachineInfo.ThisField };

    /// <summary><c>TeleType teleType = &lt;&gt;4__this;</c> → every <c>teleType</c> reads the outer instance directly.</summary>
    private static void InlineThisAlias(BlockStatement body, string name, Statement declOrAssign)
    {
        foreach (var id in body.Descendants.OfType<IdentifierExpression>().Where(x => x.Identifier == name).ToList())
            id.ReplaceWith(new IdentifierExpression(StateMachineInfo.ThisField));
        declOrAssign.Remove();
    }

    private static bool TrySelect(IfElseStatement chain, long state, ISet<string> aliases, out Statement? arm)
    {
        arm = null;
        for (IfElseStatement? cur = chain; cur is not null;)
        {
            if (StateEvaluator.Eval(cur.Condition, state, aliases) is not { } v) return false;
            var next = v != 0 ? cur.TrueStatement : cur.FalseStatement;
            if (next.IsNull) return true;   // falls through to the remainder
            if (InnerChain(next, aliases) is { } nested) { cur = nested; continue; }
            arm = next;
            return true;
        }
        return true;
    }

    /// <summary>A branch that is itself a state test (directly, or as the only statement of a block).</summary>
    private static IfElseStatement? InnerChain(Statement s, ISet<string> aliases)
    {
        var candidate = s is BlockStatement { Statements.Count: 1 } b ? b.Statements.Single() : s;
        return candidate is IfElseStatement ifs && StateEvaluator.IsStateOnly(ifs.Condition, aliases) ? ifs : null;
    }

    public static bool CanFallThrough(Statement? last) => last switch
    {
        null => true,
        BlockStatement b => CanFallThrough(b.Statements.LastOrDefault()),
        IfElseStatement ifs => ifs.FalseStatement.IsNull || CanFallThrough(ifs.TrueStatement) || CanFallThrough(ifs.FalseStatement),
        ReturnStatement or GotoStatement or ThrowStatement or YieldBreakStatement or BreakStatement or ContinueStatement => false,
        _ => true,
    };
}

using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>Coroutine reconstruction, else-if flattening, vtable dispatch and integer idioms on the template build.</summary>
[Collection(TemplateBuildCollection.Name)]
public sealed class StructuringTests(TemplateBuildFixture fx)
{
    private string Render(string type, string? method = null)
    {
        var ws = fx.Workspace;
        var t = ws.FindTypes(type).Single();
        return ws.Render(t, method is null ? null : t.Methods.First(m => m.Name == method), CodeView.CSharp);
    }

    [Fact]
    public void Coroutine_BootTimerRebuiltAsWhileLoop()
    {
        var cs = Render("PlayerScript");
        Assert.Contains("float time = 15f;", cs);
        Assert.Contains("while (time > 0f)", cs);
        Assert.Contains("yield return null;", cs);
        Assert.Contains("StartCoroutine(BootTimer());", cs);
        Assert.DoesNotContain("<>1__state", cs);
        Assert.DoesNotContain("<>2__current", cs);
        Assert.DoesNotContain("class <BootTimer>d__14", cs);
    }

    [Fact]
    public void Coroutine_SingleMethodViewShowsReconstructedBody()
    {
        var cs = Render("PlayerScript", "KeepTheHudOff");
        Assert.Contains("while (gameOver)", cs);
        Assert.Contains("yield return new WaitForEndOfFrame();", cs);
        Assert.DoesNotContain("<KeepTheHudOff>d__6", cs.Replace("coroutine reconstructed from <KeepTheHudOff>d__6", ""));
    }

    [Fact]
    public void Loops_RotatedDoWhileBecomesFor_WithCorrectBound()
    {
        // `inc ebx; cmp ebx, n; jl` used to compare the stale `num + 1` after storing num++ (an off-by-one), and the
        // guarded do-while produced by loop rotation is restored to the source `for` loop.
        var cs = Render("AudioQueueScript");
        Assert.Contains("for (int i = 1; i < audioInQueue; i++)", cs);
        Assert.Contains("audioQueue[i - 1]", cs);
        Assert.DoesNotContain("+ 1 < audioInQueue", cs);
    }

    [Fact]
    public void Coroutine_SequentialDispatchAndStateRegisterReuse()
    {
        // `if (state == 0) {...} if (num == 1) {...}` dispatch; `canvas.enabled = (byte)num != 0` folds to true.
        var cs = Render("DefaultSettingsScript");
        Assert.Contains("yield return new WaitForEndOfFrame();", cs);
        Assert.Contains("canvas.enabled = true;", cs);
        Assert.Contains("StartCoroutine(CloseOptions());", cs);
    }

    [Fact]
    public void Coroutine_DecrementChainDispatch_VirtualCalls_AndRemainder()
    {
        var cs = Render("TMPro.Examples.TeleType", "Start");
        Assert.Contains("counter % (totalVisibleCharacters + 1)", cs);   // cdq; idiv
        Assert.Contains("m_textMeshPro.text = label02;", cs);              // call [rax+558h] vtable dispatch
        Assert.Contains("while (true)", cs);
        Assert.DoesNotContain("goto ", cs);
    }

    [Fact]
    public void ElseIf_PickupScriptChainIsFlat()
    {
        var cs = Render("PickupScript", "Update");
        Assert.Contains("else if (", cs);
        Assert.Contains("CollectItem(item_ID)", cs);
        Assert.Contains("InputAction.Interact", cs);
    }

    [Fact]
    public void Iterator_KeptStateMachineResetThrowsNotSupported()
    {
        // WarpText is not reconstructed yet; the kept class must still show the compiler's Reset body.
        var cs = Render("TMPro.Examples.WarpTextExample");
        if (!cs.Contains("class <WarpText>d__", StringComparison.Ordinal)) return;   // reconstructed meanwhile: nothing to check
        Assert.Contains("IEnumerator.Reset()", cs);
        Assert.Contains("throw new NotSupportedException();", cs);
    }
}

using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

/// <summary>
/// Golden-behaviour checks on lifted C#. Each test pins a correctness fix so it cannot silently regress:
/// struct ABI/return buffers (Billboard), control flow + fields (AmbienceScript), generic instantiation +
/// write barriers (AgentTest), object allocation + delegates (DebugScript), float literals (AlarmClockScript).
/// </summary>
[Collection(TemplateBuildCollection.Name)]
public sealed class LiftedOutputTests(TemplateBuildFixture fx)
{
    private string CSharp(string type, string method)
    {
        var ws = fx.Workspace;
        var t = ws.FindTypes(type).Single();
        var m = t.Methods.First(x => x.Name == method);
        return ws.Render(t, m, CodeView.CSharp);
    }

    [Fact]
    public void Billboard_LateUpdate_LooksAtCameraForward()
    {
        var cs = CSharp("Billboard", "LateUpdate");
        Assert.Contains("m_Camera.transform", cs);
        Assert.Contains(".rotation", cs);
        Assert.Contains("Vector3.forwardVector", cs);
        Assert.Contains(".LookAt(", cs);
        Assert.DoesNotContain("NativeIntrinsics", cs);
    }

    [Fact]
    public void AmbienceScript_PlayAudio_KeepsFieldsAndBranch()
    {
        var cs = CSharp("AmbienceScript", "PlayAudio");
        Assert.Contains("audioDevice.isPlaying", cs);
        Assert.Contains("aiLocation.position", cs);
        Assert.Contains("transform.position = ", cs);
        Assert.Contains("PlayOneShot(sounds[", cs);
        Assert.Contains("if (", cs);
    }

    [Fact]
    public void AgentTest_Start_UsesExactGenericAndKeepsFieldStore()
    {
        var cs = CSharp("AgentTest", "Start");
        Assert.Contains("GetComponent<NavMeshAgent>()", cs);
        Assert.Contains("agent = ", cs);
        Assert.Contains("Wander()", cs);
        Assert.DoesNotContain("CallNative", cs);   // write barrier must not leak as a native call
        Assert.DoesNotContain("GetComponent<object>", cs);
    }

    [Fact]
    public void DebugScript_OnEnable_AllocatesDelegate()
    {
        var cs = CSharp("DebugScript", "OnEnable");
        Assert.Contains("Application.logMessageReceived += ", cs);
        Assert.Contains("LogError", cs);
        Assert.DoesNotContain("NativeIntrinsics", cs);
    }

    [Fact]
    public void AlarmClockScript_Start_FloatLiteralsAreBitExact()
    {
        var cs = CSharp("AlarmClockScript", "Start");
        Assert.Contains("timeLeft = 30f", cs);
        Assert.Contains("lifeSpan = 35f", cs);
        Assert.DoesNotContain("E+09", cs);
    }
}

using Il2CppStudio.Core.Accuracy;
using Il2CppStudio.Core.Config;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Lifting;
using Iced.Intel;

namespace Il2CppStudio.Tests;

/// <summary>Unit tests that need no game data.</summary>
public sealed class UnitTests
{
    [Fact]
    public void MoveTyping_GprConstantIntoXmm_BecomesFloatBits()
    {
        var ins = Instruction.Create(Code.Movd_xmm_rm32, Register.XMM0, Register.EAX);
        var typed = MoveTyping.Retype(ins, new ConstExpr(0x41F00000, ValueKind.I4));
        var f = Assert.IsType<FloatExpr>(typed);
        Assert.Equal(30.0, f.Value);
    }

    [Fact]
    public void MoveTyping_FloatIntoGpr_BecomesRawBits()
    {
        var ins = Instruction.Create(Code.Movd_rm32_xmm, Register.EAX, Register.XMM1);
        var typed = MoveTyping.Retype(ins, new FloatExpr(35.0, single: true));
        Assert.Equal(0x420C0000, Assert.IsType<ConstExpr>(typed).Value);
    }

    [Fact]
    public void BitMove_DetectsIntFloatPairsOnly()
    {
        Assert.True(Expr.IsBitMove(ValueKind.I4, ValueKind.R4));
        Assert.True(Expr.IsBitMove(ValueKind.R8, ValueKind.I8));
        Assert.False(Expr.IsBitMove(ValueKind.I4, ValueKind.I8));
        Assert.False(Expr.IsBitMove(ValueKind.R4, ValueKind.R8));
    }

    [Fact]
    public void Lcs_SimilarityIsDiceCoefficient()
    {
        Assert.Equal(1.0, CilFeatures.LcsSimilarity(new[] { "a", "b" }, new[] { "a", "b" }));
        Assert.Equal(0.5, CilFeatures.LcsSimilarity(new[] { "a", "b" }, new[] { "a", "c" }));
        Assert.Equal(0.0, CilFeatures.LcsSimilarity(new[] { "a" }, Array.Empty<string>()));
    }

    [Fact]
    public void BootConfig_AppliesFlagsAndRestoresBackup()
    {
        var dir = Directory.CreateTempSubdirectory("il2cppstudio-test");
        try
        {
            var file = Path.Combine(dir.FullName, "boot.config");
            File.WriteAllText(file, "gfx-enable-gfx-jobs=1\nplayer-connection-debug=0\n");
            var cfg = BootConfigManager.Open(file, NullDiagnosticSink.Instance);
            var changes = cfg.ApplyDiagnosticFlags();
            cfg.Save();
            Assert.Equal(4, changes.Count);
            var text = File.ReadAllText(file);
            Assert.Contains("gfx-enable-gfx-jobs=1", text);   // unknown keys preserved
            Assert.Contains("player-connection-debug=1", text);
            Assert.True(BootConfigManager.RestoreBackup(file, NullDiagnosticSink.Instance));
            Assert.Contains("player-connection-debug=0", File.ReadAllText(file));
        }
        finally { dir.Delete(true); }
    }
}

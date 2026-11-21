using Il2CppStudio.Core.Accuracy;
using Il2CppStudio.Core.Config;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Decompilation.Transforms;
using Il2CppStudio.Core.Diagnostics;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Il2CppStudio.Core.Disassembly;
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

    [Fact]
    public void X64Disassembler_InvalidBytes_CapturesOffsetAndRawHex_AndDiagnosticCodeIsIS8000()
    {
        Assert.Equal("IS8000", DiagnosticCodes.DisassemblyFailed);

        // 0x90 = nop; 0x06 (PUSH ES) is invalid in 64-bit mode.
        var bad = new byte[] { 0x90, 0x06, 0xFE, 0x38 };
        var fnBad = X64Disassembler.DisassembleBytes(bad, start: 0x180001000, imageBase: 0x180000000, bitness: 64);
        Assert.Equal(0x180001001UL, fnBad.FailedVa);
        Assert.Equal("06FE38", fnBad.FailedBytesHex);
        Assert.True(fnBad.Truncated);

        // Trailing 0x00 / 0xCC padding after `ret` (0xC3) must not emit spurious decode failures.
        var pad = new byte[] { 0xC3, 0xCC, 0xCC, 0x00 };
        var fnPad = X64Disassembler.DisassembleBytes(pad, start: 0x180002000, imageBase: 0x180000000, bitness: 64);
        Assert.Equal(0UL, fnPad.FailedVa);
        Assert.Null(fnPad.FailedBytesHex);
        Assert.Null(fnPad.DecodeWarning);
    }

    [Fact]
    public void X64Disassembler_InlineJumpTableAfterTerminator_DecodesCleanlyWithoutWarning()
    {
        // nop; nop; ret; int3 padding; two inline 32-bit RVAs pointing to [start, start+2] inside function; trailing cc.
        // Byte sequence: 90 90 C3 CC  | 00 20 00 00 (RVA 0x2000) | 01 20 00 00 (RVA 0x2001) | CC CC CC CC
        var code = new byte[]
        {
            0x90, 0x90, 0xC3, 0xCC,
            0x00, 0x20, 0x00, 0x00,
            0x01, 0x20, 0x00, 0x00,
            0xCC, 0xCC, 0xCC, 0xCC,
        };
        var fn = X64Disassembler.DisassembleBytes(code, start: 0x180002000, imageBase: 0x180000000, bitness: 64);
        Assert.False(fn.Truncated);
        Assert.Null(fn.DecodeWarning);
        Assert.Equal(0UL, fn.FailedVa);
        Assert.Equal(3, fn.Instructions.Count);
    }

    [Fact]
    public void X64Disassembler_JmpPlusPadding_DecodesCleanly()
    {
        // jmp +0 (EB 00); ret (C3); trailing 0xCC / 0x00 padding
        var code = new byte[] { 0xEB, 0x00, 0xC3, 0xCC, 0xCC, 0x00 };
        var fn = X64Disassembler.DisassembleBytes(code, start: 0x180003000, imageBase: 0x180000000, bitness: 64);
        Assert.False(fn.Truncated);
        Assert.Null(fn.DecodeWarning);
        Assert.Equal(0UL, fn.FailedVa);
    }

    [Fact]
    public void X64Disassembler_MultiByteNopAndInlineJumpTableAfterInt3_DecodesCleanlyWithoutWarning()
    {
        // int3 (CC); 3-byte nop (0F 1F 00) aligning to +4; two 32-bit RVAs pointing inside [0x3000..0x3010]; trailing CC.
        var code = new byte[]
        {
            0xCC, 0x0F, 0x1F, 0x00,
            0x00, 0x30, 0x00, 0x00,
            0x01, 0x30, 0x00, 0x00,
            0xCC, 0xCC, 0xCC, 0xCC,
        };
        var fn = X64Disassembler.DisassembleBytes(code, start: 0x180003000, imageBase: 0x180000000, bitness: 64);
        Assert.False(fn.Truncated);
        Assert.Null(fn.DecodeWarning);
        Assert.Equal(0UL, fn.FailedVa);
        Assert.Single(fn.Instructions);
    }

    [Fact]
    public void X64Disassembler_JumpTableWithByteIndexTable_DecodesCleanlyWithoutWarning()
    {
        // ret (C3); 66 90 nop; 2x 32-bit RVAs inside [0x3000..0x3020]; 1-byte switch index table (00 01 00 01 01); CC.
        var code = new byte[]
        {
            0x90, 0xC3, 0x66, 0x90,
            0x00, 0x30, 0x00, 0x00,
            0x01, 0x30, 0x00, 0x00,
            0x00, 0x01, 0x00, 0x01, 0x01,
            0xCC,
        };
        var fn = X64Disassembler.DisassembleBytes(code, start: 0x180003000, imageBase: 0x180000000, bitness: 64);
        Assert.False(fn.Truncated);
        Assert.Null(fn.DecodeWarning);
        Assert.Equal(0UL, fn.FailedVa);
        Assert.Equal(2, fn.Instructions.Count);
    }

    [Fact]
    public void DecompileCache_RoundTrip_PreservesEntries()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"il2cppstudio-dcache-test-{Guid.NewGuid():N}");
        var prev = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE");
        try
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE", null);
            var key = DecompileCache.ComputeKey("fp-1", "Assembly-CSharp.dll", includeIl: true, substituteBcl: true, showTokens: true, "C:\\bcl");
            var input = new List<CachedDecompiledType>
            {
                new(10, "PlayerController", "public class PlayerController { }", ".class PlayerController"),
                new(42, "Enemy_Brain", "public class Enemy_Brain { int hp = 5; }", ""),
            };
            DecompileCache.Write(key, input, dir);
            Assert.True(File.Exists(Path.Combine(dir, key + ".dcache")));
            Assert.True(DecompileCache.TryRead(key, out var output, dir));
            Assert.Equal(input, output);
        }
        finally
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE", prev);
            try { if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void DecompileCache_ComputeKey_InvalidatesOnOptionOrFingerprintChange()
    {
        var k0 = DecompileCache.ComputeKey("fp-a", "Game.dll", includeIl: false, substituteBcl: true, showTokens: true, "bclA");
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-b", "Game.dll", includeIl: false, substituteBcl: true, showTokens: true, "bclA"));
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-a", "Other.dll", includeIl: false, substituteBcl: true, showTokens: true, "bclA"));
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-a", "Game.dll", includeIl: true, substituteBcl: true, showTokens: true, "bclA"));
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-a", "Game.dll", includeIl: false, substituteBcl: false, showTokens: true, "bclA"));
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-a", "Game.dll", includeIl: false, substituteBcl: true, showTokens: false, "bclA"));
        Assert.NotEqual(k0, DecompileCache.ComputeKey("fp-a", "Game.dll", includeIl: false, substituteBcl: true, showTokens: true, "bclB"));
    }

    [Fact]
    public void DecompileCache_DisabledEnvVar_SkipsReadAndWrite()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"il2cppstudio-dcache-off-{Guid.NewGuid():N}");
        var prev = Environment.GetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE");
        try
        {
            var key = DecompileCache.ComputeKey("fp-x", "Game.dll", includeIl: false, substituteBcl: true, showTokens: true, null);
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE", "1");
            DecompileCache.Write(key, [new(1, "Foo", "class Foo {}", "")], dir);
            Assert.False(File.Exists(Path.Combine(dir, key + ".dcache")));
            Assert.False(DecompileCache.TryRead(key, out _, dir));
        }
        finally
        {
            Environment.SetEnvironmentVariable("IL2CPPSTUDIO_NO_CACHE", prev);
            try { if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Fact]
    public void IdentifierEscaper_Escape_MatchesExpectedMappings()
    {
        Assert.Equal("_", IdentifierEscaper.Escape("?"));
        Assert.Equal("_BootTimer_d__14", IdentifierEscaper.Escape("<BootTimer>d__14"));
        Assert.Equal("__c", IdentifierEscaper.Escape("<>c"));
        Assert.Equal("_0abc", IdentifierEscaper.Escape("0abc"));
        Assert.Equal("Valid_1", IdentifierEscaper.Escape("Valid_1"));
    }

    [Fact]
    public void AstWalk_ScanFlags_DetectsNativeIntrinsicsOnTypeReference()
    {
        var tree = new SyntaxTree();
        var method = new MethodDeclaration
        {
            Name = "Foo",
            ReturnType = new PrimitiveType("void"),
            Body = new BlockStatement
            {
                new ExpressionStatement(new InvocationExpression(
                    new MemberReferenceExpression(new TypeReferenceExpression(new SimpleType("NativeIntrinsics")), "Asm"),
                    new PrimitiveExpression("x"))),
            },
        };
        tree.Members.Add(method);
        var flags = AstWalk.ScanFlags(tree);
        Assert.True(flags.HasNativeIntrinsics);
    }
}

using System.Buffers.Binary;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;
using Il2CppStudio.Core.Lifting;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;
using Il2CppStudio.Core.Workspace;

namespace Il2CppStudio.Tests;

public sealed class Arm32DisassemblerTests
{
    private static byte[] EncodeWords32(params uint[] words)
    {
        var buf = new byte[words.Length * 4];
        for (var i = 0; i < words.Length; i++)
            BinaryPrimitives.WriteUInt32LittleEndian(buf.AsSpan(i * 4, 4), words[i]);
        return buf;
    }

    private static byte[] EncodeHalfwords(params ushort[] hws)
    {
        var buf = new byte[hws.Length * 2];
        for (var i = 0; i < hws.Length; i++)
            BinaryPrimitives.WriteUInt16LittleEndian(buf.AsSpan(i * 2, 2), hws[i]);
        return buf;
    }

    [Fact]
    public void Disassemble_A32PrologueVfpAndBranches_DecodesAccuratelyWithSymbols()
    {
        // A32 sequence at 0x00844A6C:
        //   push     {r4, r5, fp, lr}   (0xE92D4830)
        //   vpush    {d8, d9, d10}      (0xED2D8B06)
        //   sub      sp, sp, #0x10      (0xE24DD010)
        //   mov      r4, r0             (0xE1A04000)
        //   movw     r1, #0x1234        (0xE3011234)
        //   movt     r1, #0x4244        (0xE3441244)
        //   bl       #0x00844A90        (pc=0x00844A8C -> off=(0x844A90 - 0x844A8C)/4 = +1? No: addr=0x844A84, pc+8=0x844A8C -> off=+4 => imm24=1 -> 0xEB000001)
        //   ldr      r5, [r4, #0x10]    (0xE5945010)
        //   vldr     s16, [sp, #4]      (0xED9D8A01)
        //   vsub.f32 s0, s16, s0        (0xEE380A40)
        //   vstr     s0, [r4, #0x38]    (0xED840A0E)
        //   vcmp.f32 s0, #0             (0xEEB50A40)
        //   vmrs     apsr_nzcv, fpscr   (0xEEF1FA10)
        //   bge      #0x00844AA8        (addr=0x844AA0, pc+8=0x844AA8 -> off=0 -> 0xAA000000)
        //   add      sp, sp, #0x10      (0xE28DD010)
        //   vpop     {d8, d9, d10}      (0xECBD8B06)
        //   pop      {r4, r5, fp, pc}   (0xE8BD8830)
        var code = EncodeWords32(
            0xE92D4830,
            0xED2D8B06,
            0xE24DD010,
            0xE1A04000,
            0xE3011234,
            0xE3441244,
            0xEB000001,
            0xE5945010,
            0xED9D8A01,
            0xEE380A40,
            0xED840A0E,
            0xEEB50A40,
            0xEEF1FA10,
            0xAA000000,
            0xE28DD010,
            0xECBD8B06,
            0xE8BD8830);

        var disasm = new Arm32Disassembler
        {
            SymbolResolver = va => va == 0x00844A90 ? "UnityEngine.Component::get_transform" : null,
        };

        var insns = disasm.DisassembleInstructions(code, 0x00844A6C, out var isThumb, out var decodedBytes);
        Assert.False(isThumb);
        Assert.Equal(code.Length, decodedBytes);
        Assert.Equal(17, insns.Count);

        Assert.Equal(("push", "{r4, r5, fp, lr}"), (insns[0].Mnemonic, insns[0].Operands));
        Assert.Equal(("vpush", "{d8, d9, d10}"), (insns[1].Mnemonic, insns[1].Operands));
        Assert.Equal(("sub", "sp, sp, #0x10"), (insns[2].Mnemonic, insns[2].Operands));
        Assert.Equal(("mov", "r4, r0"), (insns[3].Mnemonic, insns[3].Operands));
        Assert.Equal(("movw", "r1, #0x1234"), (insns[4].Mnemonic, insns[4].Operands));
        Assert.Equal(("movt", "r1, #0x4244"), (insns[5].Mnemonic, insns[5].Operands));
        Assert.Equal(("bl", "#0x844a90"), (insns[6].Mnemonic, insns[6].Operands));
        Assert.Equal(("ldr", "r5, [r4, #0x10]"), (insns[7].Mnemonic, insns[7].Operands));
        Assert.Equal(("vldr", "s16, [sp, #4]"), (insns[8].Mnemonic, insns[8].Operands));
        Assert.Equal(("vsub.f32", "s0, s16, s0"), (insns[9].Mnemonic, insns[9].Operands));
        Assert.Equal(("vstr", "s0, [r4, #0x38]"), (insns[10].Mnemonic, insns[10].Operands));
        Assert.Equal(("vcmp.f32", "s0, #0"), (insns[11].Mnemonic, insns[11].Operands));
        Assert.Equal(("vmrs", "apsr_nzcv, fpscr"), (insns[12].Mnemonic, insns[12].Operands));
        Assert.Equal(("bge", "#0x844aa8"), (insns[13].Mnemonic, insns[13].Operands));
        Assert.Equal(("add", "sp, sp, #0x10"), (insns[14].Mnemonic, insns[14].Operands));
        Assert.Equal(("vpop", "{d8, d9, d10}"), (insns[15].Mnemonic, insns[15].Operands));
        Assert.Equal(("pop", "{r4, r5, fp, pc}"), (insns[16].Mnemonic, insns[16].Operands));

        var text = disasm.Disassemble(code, 0x00844A6C);
        Assert.Contains("; UnityEngine.Component::get_transform", text);
        Assert.DoesNotContain("decoding stopped", text);
    }

    [Fact]
    public void Disassemble_ThumbAndThumb2_DecodesMixed16And32BitWithItAndVfp()
    {
        // Thumb-2 sequence at 0x00010000:
        //   push    {r4, lr}            (0xB510)
        //   cbz     r0, #0x10008        (addr=0x10002, pc=0x10006, imm6=+2 => 0xB108)
        //   ldr.w   r4, [r0, #0x10]     (0xF8D0 0x4010)
        //   vldr    s0, [r4, #4]        (0xED94 0x0A01)
        //   it      eq                  (0xBF08)
        //   moveq   r0, #1              (0x2001 -> conditioned by IT)
        //   pop     {r4, pc}            (0xBD10)
        var code = EncodeHalfwords(
            0xB510,
            0xB108,
            0xF8D0, 0x4010,
            0xED94, 0x0A01,
            0xBF08,
            0x2001,
            0xBD10);

        var disasm = new Arm32Disassembler();
        var insns = disasm.DisassembleInstructions(code, 0x00010000, out var isThumb, out var decodedBytes);
        Assert.True(isThumb);
        Assert.Equal(code.Length, decodedBytes);
        Assert.Equal(7, insns.Count);

        Assert.Equal(("push", "{r4, lr}"), (insns[0].Mnemonic, insns[0].Operands));
        Assert.Equal(("cbz", "r0, #0x10008"), (insns[1].Mnemonic, insns[1].Operands));
        Assert.Equal(("ldr", "r4, [r0, #0x10]"), (insns[2].Mnemonic, insns[2].Operands));
        Assert.Equal(("vldr", "s0, [r4, #4]"), (insns[3].Mnemonic, insns[3].Operands));
        Assert.Equal(("it", "eq"), (insns[4].Mnemonic, insns[4].Operands));
        Assert.Equal(("moveq", "r0, #1"), (insns[5].Mnemonic, insns[5].Operands));
        Assert.Equal(("pop", "{r4, pc}"), (insns[6].Mnemonic, insns[6].Operands));
    }

    [Fact]
    public void RealAndroidArmv7_DisassemblesAndLiftsWithPureCSharpDecoder()
    {
        var root = TemplateBuildFixture.RepositoryRoot;
        var dir = root is null ? null : Path.Combine(root, "testdata", "Template il2cpp android", "Baldi's Basics Classic.armeabi-v7a");
        if (dir is null || !Directory.Exists(dir))
            Assert.Skip("ARMv7 testdata build not found.");

        using var ws = Il2CppWorkspace.Load(dir, NullDiagnosticSink.Instance);
        WorkspaceSetup.Attach(ws);
        var armDisasm = Assert.IsType<Arm32Disassembler>(ws.TextDisassembler);

        foreach (var (typeName, methodName) in new[] { ("CameraScript", "Start"), ("AmbienceScript", "PlayAudio"), ("BaldiScript", "Start") })
        {
            var type = ws.FindTypes(typeName).Single();
            var method = type.Methods.Single(m => m.Name == methodName);
            var asm = ws.DisassembleMethod(method);
            Assert.NotNull(asm);
            Assert.Contains("ARMv7 (Pure C#,", asm);
            Assert.Contains("push", asm);
            Assert.Contains("pop", asm);

            var cs = ws.Render(type, method, CodeView.CSharp);
            Assert.DoesNotContain("throw null;", cs);
        }
    }
}

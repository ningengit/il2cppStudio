using System.Buffers.Binary;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Il2CppStudio.Core.Accuracy;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Config;
using Il2CppStudio.Core.Decompilation;
using Il2CppStudio.Core.Decompilation.Transforms;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Disassembly;
using Il2CppStudio.Core.Lifting;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Workspace;
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

    [Fact]
    public void WasmBinaryImage_UnalignedDataSegmentBase_AlignsTo8BytesAndPreservesData()
    {
        // Build a minimal WASM binary with one active data segment at unaligned linear address 0x402.
        using var ms = new MemoryStream();
        ms.Write("\0asm\x01\0\0\0"u8);

        // Section 11 (Data): 1 segment, flags=0 (active), i32.const 0x402 end, 4-byte payload [11, 22, 33, 44].
        var seg = new List<byte>();
        seg.Add(1);    // segment count = 1
        seg.Add(0);    // flags = 0 (active)
        seg.Add(0x41); // i32.const
        seg.Add(0x82); seg.Add(0x08); // SLEB128(0x402 = 1026) -> (0x02|0x80), (1026>>7 = 8)
        seg.Add(0x0B); // end
        seg.Add(4);    // payload size
        seg.AddRange([0x11, 0x22, 0x33, 0x44]);

        ms.WriteByte(11);
        ms.WriteByte((byte)seg.Count);
        ms.Write(seg.ToArray());

        var tmp = Path.Combine(Path.GetTempPath(), $"il2cppstudio-wasm-align-{Guid.NewGuid():N}.wasm");
        try
        {
            File.WriteAllBytes(tmp, ms.ToArray());
            using var wasm = WasmBinaryImage.Load(tmp, NullDiagnosticSink.Instance);
            Assert.Single(wasm.Sections);
            Assert.Equal(0x400UL, wasm.Sections[0].Rva);
            var reader = new VirtualReader(wasm);
            Assert.Equal(0x44332211u, reader.ReadU32(0x402UL));
        }
        finally
        {
            try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
        }
    }

    [Fact]
    public void ArrayIndexAndBoundsCleaner_FoldsStrideAndStripsDeadBoundsTraps()
    {
        static Expression MakeShiftDiv(Expression lhs, int shift, object divisor) =>
            new BinaryOperatorExpression(
                new ParenthesizedExpression(new BinaryOperatorExpression(lhs, BinaryOperatorType.ShiftLeft, new PrimitiveExpression(shift))),
                BinaryOperatorType.Divide,
                new PrimitiveExpression(divisor));

        var trapCall = new ExpressionStatement(new InvocationExpression(
            new MemberReferenceExpression(new IdentifierExpression("NativeIntrinsics"), "CallNative"),
            new PrimitiveExpression("sub_9B120"),
            new PrimitiveExpression(0L),
            new PrimitiveExpression(0L),
            new PrimitiveExpression(0L),
            new PrimitiveExpression(0L)));

        var body = new BlockStatement
        {
            // a[(x << 3) / 8] = 1;
            new AssignmentExpression(
                new IndexerExpression(new IdentifierExpression("a"), MakeShiftDiv(new IdentifierExpression("x"), 3, 8)),
                new PrimitiveExpression(1)),
            // a[(int)((y << 3) / 8L)] = 2;
            new AssignmentExpression(
                new IndexerExpression(new IdentifierExpression("a"),
                    new CastExpression(new PrimitiveType("int"), MakeShiftDiv(new IdentifierExpression("y"), 3, 8L))),
                new PrimitiveExpression(2)),
            // a[(int)(((long)x << 3) / 8L)] = 3;
            new AssignmentExpression(
                new IndexerExpression(new IdentifierExpression("a"),
                    new CastExpression(new PrimitiveType("int"),
                        MakeShiftDiv(new CastExpression(new PrimitiveType("long"), new IdentifierExpression("x")), 3, 8L))),
                new PrimitiveExpression(3)),
            // Genuine user throw must be preserved
            new IfElseStatement(
                new BinaryOperatorExpression(new IdentifierExpression("x"), BinaryOperatorType.LessThan, new PrimitiveExpression(0)),
                new ThrowStatement(new ObjectCreateExpression(new SimpleType("ArgumentException"), new PrimitiveExpression("bad")))),
            // Unsigned bounds check + else/goto trap must be stripped
            new IfElseStatement(
                new BinaryOperatorExpression(
                    new CastExpression(new PrimitiveType("uint"), new IdentifierExpression("x")),
                    BinaryOperatorType.LessThan,
                    new CastExpression(new PrimitiveType("uint"), new MemberReferenceExpression(new IdentifierExpression("a"), "Length"))),
                new BlockStatement
                {
                    new ExpressionStatement(new AssignmentExpression(
                        new IndexerExpression(new IdentifierExpression("a"), new PrimitiveExpression(0)),
                        new PrimitiveExpression(4))),
                    new GotoStatement("L_ok"),
                },
                new BlockStatement
                {
                    new GotoStatement("L_trap"),
                }),
            new LabelStatement { Label = "L_trap" },
            trapCall,
            new ThrowStatement(new NullReferenceExpression()),
            new LabelStatement { Label = "L_ok" },
            new ReturnStatement(),
        };

        var tree = new SyntaxTree();
        tree.Members.Add(new TypeDeclaration
        {
            Name = "Sample",
            Members =
            {
                new MethodDeclaration
                {
                    Name = "Test",
                    ReturnType = new PrimitiveType("void"),
                    Body = body,
                }
            },
        });

        ArrayIndexAndBoundsCleaner.Apply(tree);
        var printed = tree.ToString(DecompilerService.CreateSettings().CSharpFormattingOptions).Replace("\r", "");

        Assert.DoesNotContain("<< 3", printed);
        Assert.DoesNotContain("/ 8", printed);
        Assert.Contains("a[x] = 1;", printed);
        Assert.Contains("a[(int)y] = 2;", printed);
        Assert.Contains("a[(int)x] = 3;", printed);
        Assert.Contains("throw new ArgumentException(\"bad\");", printed);
        Assert.DoesNotContain("sub_9B120", printed);
        Assert.DoesNotContain("throw null;", printed);
        Assert.DoesNotContain("L_trap", printed);
        Assert.Contains("a[0] = 4;", printed);
    }

    [Fact]
    public void CompileHygiene_PruneUnusedNativeUsing_RemovesOnlyWhenUnused()
    {
        static SyntaxTree BuildTree(bool withNative)
        {
            var tree = new SyntaxTree();
            tree.Members.Add(new UsingDeclaration("System"));
            tree.Members.Add(new UsingDeclaration("Il2CppStudio.Native"));
            var body = new BlockStatement();
            if (withNative)
            {
                body.Add(new ExpressionStatement(new InvocationExpression(
                    new MemberReferenceExpression(new IdentifierExpression("NativeIntrinsics"), "Stmt"),
                    new PrimitiveExpression("nop"))));
            }
            else
            {
                body.Add(new VariableDeclarationStatement(new PrimitiveType("int"), "x", new PrimitiveExpression(1)));
            }
            tree.Members.Add(new TypeDeclaration
            {
                Name = "Sample",
                Members = { new MethodDeclaration { Name = "M", ReturnType = new PrimitiveType("void"), Body = body } },
            });
            return tree;
        }

        var cleanTree = BuildTree(withNative: false);
        CompileHygiene.PruneUnusedNativeUsing(cleanTree);
        var cleanText = cleanTree.ToString();
        Assert.Contains("using System;", cleanText);
        Assert.DoesNotContain("Il2CppStudio.Native", cleanText);

        var usedTree = BuildTree(withNative: true);
        CompileHygiene.PruneUnusedNativeUsing(usedTree);
        Assert.Contains("using Il2CppStudio.Native;", usedTree.ToString());
    }

    [Fact]
    public void WorkspaceSearchIndex_CapsResultsHonorsCancellationAndStaysLowMemory()
    {
        var raw = new Il2CppTypeDefinition();
        var types = new List<TypeModel>(5000);
        for (var t = 0; t < 5000; t++)
        {
            var tm = new TypeModel
            {
                Index = t,
                Name = $"ItemWidget{t}",
                Namespace = "Game.Core",
                Token = (uint)(0x02000001 + t),
                Flags = 0,
                Raw = raw,
                GenericParameters = [],
                ImageName = "Assembly-CSharp.dll",
            };
            for (var m = 0; m < 4; m++)
            {
                tm.Methods.Add(new MethodModel
                {
                    Index = t * 4 + m,
                    Name = m == 0 ? $"UpdateState_{t}" : $"Helper_{t}_{m}",
                    Token = (uint)(0x06000001 + t * 4 + m),
                    Flags = 0,
                    ImplFlags = 0,
                    Slot = ushort.MaxValue,
                    ParameterNames = [],
                    GenericParameters = [],
                    ReturnTypeIndex = 0,
                    ParameterTypeIndices = [],
                });
            }
            types.Add(tm);
        }
        var asm = new AssemblyModel(0, "Assembly-CSharp", "1.0.0.0", "", "", "Assembly-CSharp.dll", [], [new NamespaceModel("Game.Core", types)], types.Count);
        var index = WorkspaceSearchIndex.Build([asm], types.Count, types.Count * 4);

        var capped = index.Search("updatestate", maxResults: 500);
        Assert.Equal(500, capped.Matches.Count);
        Assert.Equal(5000, capped.TotalMatches);
        Assert.True(capped.IsTruncated);

        var exact = index.Search("itemwidget42", maxResults: 500);
        Assert.Contains(exact.Matches, m => m.Method is null && m.Type.Name == "ItemWidget42");

        using var cts = new CancellationTokenSource();
        cts.Cancel();
        Assert.Throws<OperationCanceledException>(() => index.Search("item", 500, cts.Token));

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var baselineMem = GC.GetTotalMemory(forceFullCollection: true);
        for (var i = 0; i < 20; i++)
        {
            var res = index.Search("widget", 500);
            Assert.Equal(500, res.Matches.Count);
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var afterMem = GC.GetTotalMemory(forceFullCollection: true);
        Assert.True(afterMem - baselineMem < 10 * 1024 * 1024, $"Expected flat search to stay low memory, delta={afterMem - baselineMem} bytes");
    }

    [Fact]
    public void MachOBinaryImage_Parses64BitSectionsChainedFixupsAndUniversalFat()
    {
        // Reject Java .class magic (0xCAFEBABE + major version 61 >= 45 in fat nfat_arch slot)
        Assert.False(MachOBinaryImage.IsMachO([0xCA, 0xFE, 0xBA, 0xBE, 0x00, 0x00, 0x00, 0x3D]));

        // Construct a synthetic 64-bit ARM64 Mach-O image wrapped in a FAT_MAGIC Universal container.
        // Layout inside slice (0x700 bytes):
        //   0x000..0x020: mach_header_64 (ncmds=4)
        //   0x020..0x188: Load commands:
        //     - LC_SEGMENT_64 __TEXT (vm=0x100000000, foff=0, fsz=0x300) + __text section (va=0x100000200, sz=0x10, foff=0x200)
        //     - LC_SEGMENT_64 __DATA (vm=0x100001000, foff=0x300, fsz=0x100) + __data section (va=0x100001000, sz=0x20, foff=0x300)
        //     - LC_DYLD_CHAINED_FIXUPS (off=0x400, sz=0x60)
        //     - LC_FUNCTION_STARTS (off=0x500, sz=0x10)
        const int sliceSize = 0x700;
        var slice = new byte[sliceSize];
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x00), 0xFEEDFACF);   // MH_MAGIC_64
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x04), 0x0100000C);   // CPU_TYPE_ARM64
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x0C), 2);            // MH_EXECUTE
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x10), 4);            // ncmds = 4

        var cmdPos = 0x20;
        // 1. LC_SEGMENT_64 __TEXT (72 + 80 = 152 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos), 0x19);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 4), 152);
        "__TEXT\0"u8.CopyTo(slice.AsSpan(cmdPos + 8));
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 24), 0x100000000UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 32), 0x1000UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 40), 0UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 48), 0x300UL);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 60), 5); // r-x
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 64), 1); // 1 section
        var sText = cmdPos + 72;
        "__text\0"u8.CopyTo(slice.AsSpan(sText));
        "__TEXT\0"u8.CopyTo(slice.AsSpan(sText + 16));
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(sText + 32), 0x100000200UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(sText + 40), 0x20UL);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(sText + 48), 0x200u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(sText + 64), 0x80000400u);
        cmdPos += 152;

        // 2. LC_SEGMENT_64 __DATA (72 + 80 = 152 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos), 0x19);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 4), 152);
        "__DATA\0"u8.CopyTo(slice.AsSpan(cmdPos + 8));
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 24), 0x100001000UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 32), 0x1000UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 40), 0x300UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(cmdPos + 48), 0x100UL);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 60), 3); // rw-
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 64), 1);
        var sData = cmdPos + 72;
        "__data\0"u8.CopyTo(slice.AsSpan(sData));
        "__DATA\0"u8.CopyTo(slice.AsSpan(sData + 16));
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(sData + 32), 0x100001000UL);
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(sData + 40), 0x20UL);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(sData + 48), 0x300u);
        cmdPos += 152;

        // 3. LC_DYLD_CHAINED_FIXUPS (16 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos), 0x80000034u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 4), 16u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 8), 0x400u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 12), 0x60u);
        cmdPos += 16;

        // 4. LC_FUNCTION_STARTS (16 bytes)
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos), 0x26u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 4), 16u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 8), 0x500u);
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(cmdPos + 12), 0x10u);
        cmdPos += 16;

        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x14), (uint)(cmdPos - 0x20)); // sizeofcmds

        // Encoded DYLD_CHAINED_PTR_64_OFFSET at __DATA offset 0x300: target RVA = 0x1234, high8 = 0, next = 0, bind = 0.
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(0x300), 0x1234UL);

        // Chained fixups table at 0x400:
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x400), 0u);    // fixups_version = 0
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x404), 0x20u); // starts_offset = 0x20 -> 0x420
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x420), 2u);    // seg_count = 2
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x424), 0u);    // seg 0 (__TEXT): none
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x428), 0x10u); // seg 1 (__DATA) info at 0x430
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x430), 24u);   // struct_size
        BinaryPrimitives.WriteUInt16LittleEndian(slice.AsSpan(0x434), 0x4000);// page_size
        BinaryPrimitives.WriteUInt16LittleEndian(slice.AsSpan(0x436), 6);     // DYLD_CHAINED_PTR_64_OFFSET
        BinaryPrimitives.WriteUInt64LittleEndian(slice.AsSpan(0x438), 0x300UL); // segment_offset = 0x300
        BinaryPrimitives.WriteUInt32LittleEndian(slice.AsSpan(0x440), 0u);    // max_valid_pointer
        BinaryPrimitives.WriteUInt16LittleEndian(slice.AsSpan(0x444), 1);     // page_count = 1
        BinaryPrimitives.WriteUInt16LittleEndian(slice.AsSpan(0x446), 0);     // page_start[0] = 0

        // Function starts at 0x500: ULEB128(0x200) = [0x80, 0x04], terminator 0.
        slice[0x500] = 0x80;
        slice[0x501] = 0x04;
        slice[0x502] = 0x00;

        // Wrap in FAT_MAGIC (header 8B + 1 fat_arch 20B = 28B, align slice at 0x100).
        var fat = new byte[0x100 + slice.Length];
        BinaryPrimitives.WriteUInt32BigEndian(fat.AsSpan(0), 0xCAFEBABEu);
        BinaryPrimitives.WriteUInt32BigEndian(fat.AsSpan(4), 1u);
        BinaryPrimitives.WriteUInt32BigEndian(fat.AsSpan(8), 0x0100000Cu);  // CPU_TYPE_ARM64
        BinaryPrimitives.WriteUInt32BigEndian(fat.AsSpan(16), 0x100u);      // offset
        BinaryPrimitives.WriteUInt32BigEndian(fat.AsSpan(20), (uint)slice.Length);
        Buffer.BlockCopy(slice, 0, fat, 0x100, slice.Length);

        Assert.True(MachOBinaryImage.IsMachO(fat));
        using var img = MachOBinaryImage.Load("synthetic.macho", fat, NullDiagnosticSink.Instance);
        Assert.Equal(BinaryFormat.MachO, img.Format);
        Assert.Equal(CpuArchitecture.Arm64, img.Architecture);
        Assert.True(img.Is64Bit);
        Assert.Equal(0x100000000UL, img.ImageBase);
        Assert.Equal(1, img.AppliedFixups);

        var reader = new VirtualReader(img);
        Assert.Equal(0x100001234UL, reader.ReadU64(0x100001000UL));
        Assert.Equal([(0x200u, 0x220u)], img.GetFunctionTable());
    }
}

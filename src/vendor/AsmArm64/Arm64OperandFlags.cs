// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

namespace AsmArm64;

/// <summary>
/// Represents the flags of an ARM64 operand.
/// </summary>
[Flags]
public enum Arm64OperandFlags : byte
{
    /// <summary>
    /// No flags.
    /// </summary>
    None = 0,
    /// <summary>
    /// The operand is read.
    /// </summary>
    Read = 1,
    /// <summary>
    /// The operand is written.
    /// </summary>
    Write = 2,
    /// <summary>
    /// The operand is read and written.
    /// </summary>
    ReadWrite = Read | Write,
    /// <summary>
    /// The operand is optional.
    /// </summary>
    Optional = 8,
}
/*
 * Implementation taken and inspired by
 * https://github.com/Mirsario/TerrariaOverhaul/tree/dev/Common/Ambience
 */

using System;

namespace EndlessEscapade.Common.Surroundings;

public sealed class SurroundingsUpdaterAttribute : Attribute
{
    public readonly string? Name;

    public SurroundingsUpdaterAttribute(string? name = null) {
        Name = name;
    }
}

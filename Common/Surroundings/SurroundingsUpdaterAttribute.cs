using System;

namespace EndlessEscapade.Common.Surroundings;

public sealed class SurroundingsUpdaterAttribute : Attribute
{
    public readonly string? Name;

    public SurroundingsUpdaterAttribute(string? name = null) {
        Name = name;
    }
}

using System;

namespace EndlessEscapade.Common.Surroundings;

public sealed class SurroundingsCallbackAttribute : Attribute
{
    public readonly string Name;

    public SurroundingsCallbackAttribute(string name) {
        Name = name;
    }
}

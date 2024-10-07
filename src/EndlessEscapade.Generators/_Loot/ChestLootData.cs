using System;
using Newtonsoft.Json;

namespace EndlessEscapade.Generators;

public sealed class ChestLootData : IEquatable<ChestLootData>
{
    [JsonRequired]
    public string ItemPath;

    [JsonRequired]
    public string TilePath;

    [JsonRequired]
    public int Chance;

    [JsonRequired]
    public int[] Frames;

    public int MinStack;
    public int MaxStack;

    public bool RandomSlot;

    public bool Equals(ChestLootData other) {
        return other.ItemPath == ItemPath
            && other.TilePath == TilePath
            && other.Chance == Chance
            && other.Frames.AsSpan().SequenceEqual(Frames)
            && other.MinStack == MinStack
            && other.MaxStack == MaxStack
            && other.RandomSlot == RandomSlot;
    }

    public override bool Equals(object obj) {
        return Equals(obj as ChestLootData);
    }

    public override int GetHashCode() {
        return HashCode.Combine(ItemPath, TilePath, Chance, Frames.GetHashCode(), MinStack, MaxStack, RandomSlot);
    }
}

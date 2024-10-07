using Newtonsoft.Json;

namespace EndlessEscapade.Generators;

public sealed class ChestLootData
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
}

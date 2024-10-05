using Newtonsoft.Json;

namespace EndlessEscapade.Common.World;

public interface IChestLoot : ILoadable
{
    [JsonRequired]
    string ItemPath { get; }

    [JsonRequired]
    string TilePath { get; }

    [JsonRequired]
    int Chance { get; }

    [JsonRequired]
    int[] Frames { get; }

    int MinStack { get; }
    int MaxStack { get; }

    bool RandomSlot { get; }

    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }
}

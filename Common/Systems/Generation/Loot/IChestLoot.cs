using System.Collections.Generic;
using Terraria.ID;

namespace EndlessEscapade.Common.Systems.Generation.Loot;

public interface IChestLoot
{
    public IReadOnlyList<int> Frames { get; }

    public int Chance { get; }

    public int TileType => TileID.Containers;
    public int MinStack => 1;
    public int MaxStack => 1;

    public bool RandomSlot => false;
}

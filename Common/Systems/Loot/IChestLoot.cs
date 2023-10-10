using Terraria.ID;
using System;
using System.Collections.Generic;

namespace EndlessEscapade.Common.Systems.Loot;

public interface IChestLoot
{
    public IReadOnlyList<ChestFrame> Frames { get; }

    public int Chance { get; }
    
    public int TileType => TileID.Containers;
    public int MinStack => 1;
    public int MaxStack => 1;

    public bool RandomSlot => false;
}


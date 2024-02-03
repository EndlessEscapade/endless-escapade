using System.Collections.Generic;
using Newtonsoft.Json;

namespace EndlessEscapade.Common.Loot;

public struct ChestLoot
{
    public bool RandomSlot;
    
    public int MinStack;
    public int MaxStack;

    [JsonRequired]
    public string ItemType;

    [JsonRequired]
    public string TileType;
    
    [JsonRequired]
    public int Chance;
    
    [JsonRequired]
    public int[] Frames;
}

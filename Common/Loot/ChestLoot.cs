using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Loot;

public struct ChestLoot
{
    [JsonIgnore]
    public int ItemType {
        get {
            var split = ItemPath.Split('/');

            var prefix = split[0];
            var postfix = split[1];

            if (prefix == "Terraria") {
                return ItemID.Search.GetId(postfix);
            }

            return ModContent.Find<ModItem>(ItemPath).Type;
        }
    }
    
    [JsonIgnore]
    public int TileType {
        get {
            var split = TilePath.Split('/');

            var prefix = split[0];
            var postfix = split[1];
            
            if (prefix == "Terraria") { 
                return TileID.Search.GetId(postfix);
            }

            return ModContent.Find<ModTile>(TilePath).Type;
        }
    }
    
    public bool RandomSlot { get; set; }
    
    public int MinStack { get; set; }
    public int MaxStack { get; set; }

    [JsonRequired]
    public string ItemPath { get; set; }

    [JsonRequired]
    public string TilePath { get; set; }
    
    [JsonRequired]
    public int Chance { get; set; }
    
    [JsonRequired]
    public int[] Frames { get; set; }
}

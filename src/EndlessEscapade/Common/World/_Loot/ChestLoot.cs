using Newtonsoft.Json;

namespace EndlessEscapade.Common.World;

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

    public bool RandomSlot { get; }

    public int MinStack { get; }
    public int MaxStack { get; }

    [JsonRequired]
    public string ItemPath { get; }

    [JsonRequired]
    public string TilePath { get; }

    [JsonRequired]
    public int Chance { get; }

    [JsonRequired]
    public int[] Frames { get; }
}

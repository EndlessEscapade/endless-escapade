using Newtonsoft.Json;

namespace EndlessEscapade.Common.World;

public interface IChestLoot : ILoadable
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

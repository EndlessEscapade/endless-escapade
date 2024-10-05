using EndlessEscapade.Common.World;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Provides <see cref="IChestLoot"/> extensions.
/// </summary>
public static class IChestLootExtensions
{
    public static int GetTileType(this IChestLoot loot) {
        var split = loot.TilePath.Split('/');

        var prefix = split[0];
        var suffix = split[1];

        return prefix == "Terraria" ? TileID.Search.GetId(suffix) : ModContent.Find<ModTile>(loot.TilePath).Type;
    }

    public static int GetItemType(this IChestLoot loot) {
        var split = loot.ItemPath.Split('/');

        var prefix = split[0];
        var suffix = split[1];

        return prefix == "Terraria" ? ItemID.Search.GetId(suffix) : ModContent.Find<ModItem>(loot.ItemPath).Type;
    }
}

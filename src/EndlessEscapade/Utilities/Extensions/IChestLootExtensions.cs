using EndlessEscapade.Common.World;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Provides <see cref="IChestLoot"/> extensions.
/// </summary>
public static class IChestLootExtensions
{
    /// <summary>
    ///     Parses the tile identifier from a chest loot to its type.
    /// </summary>
    /// <param name="loot">The chest loot to retrieve the tile type from.</param>
    /// <returns>The type of the tile retrieved if found; otherwise, <c>-1</c>.</returns>
    public static int GetTileType(this IChestLoot loot) {
        var split = loot.TilePath.Split('/');

        var prefix = split[0];
        var suffix = split[1];

        return prefix == "Terraria" ? TileID.Search.GetId(suffix) : ModContent.Find<ModTile>(loot.TilePath).Type;
    }

    /// <summary>
    ///     Parses the item identifier from a chest loot to its type.
    /// </summary>
    /// <param name="loot">The chest loot to retrieve the item type from.</param>
    /// <returns>The type of the item retrieved if found; otherwise, <c>-1</c>.</returns>
    public static int GetItemType(this IChestLoot loot) {
        var split = loot.ItemPath.Split('/');

        var prefix = split[0];
        var suffix = split[1];

        return prefix == "Terraria" ? ItemID.Search.GetId(suffix) : ModContent.Find<ModItem>(loot.ItemPath).Type;
    }
}

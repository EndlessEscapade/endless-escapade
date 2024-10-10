using System.Collections.Generic;

namespace EndlessEscapade.Utilities.Extensions;

/// <summary>
///     Provides <see cref="Chest" /> extension methods.
/// </summary>
public static class ChestExtensions
{
    /// <summary>
    ///     Checks if the chest contains a specified item type.
    /// </summary>
    /// <param name="chest">The chest to check.</param>
    /// <param name="type">The type of the item.</param>
    /// <returns>Whether the chest has the specified item type or not.</returns>
    public static bool HasItem(this Chest chest, int type) {
        for (var i = 0; i < Chest.maxItems; i++) {
            var item = chest.item[i];

            if (!item.IsAir && item.type == type) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Attempts to add an item to a chest.
    /// </summary>
    /// <param name="chest">The chest to add.</param>
    /// <param name="type">The type of the item.</param>
    /// <param name="stack">The stack of the item.</param>
    /// <param name="randomSlot">Whether to add the item in a random slot or not.</param>
    /// <returns>Whether the item was successfully added to the chest or not.</returns>
    public static bool TryAddItem(this Chest chest, int type, int stack, bool randomSlot) {
        if (!chest.TryGetEmptySlot(out var index, randomSlot) || type == ItemID.None) {
            return false;
        }

        chest.item[index].SetDefaults(type);
        chest.item[index].stack = stack;

        return true;
    }

    /// <summary>
    ///     Attemps to retrieve an empty slot from a chest.
    /// </summary>
    /// <param name="chest">The chest to check.</param>
    /// <param name="index">The index of the slot.</param>
    /// <param name="randomSlot">Whether to retrieve a random slot or not.</param>
    /// <returns>Whether a slot was successfully retrieved from the chest or not.</returns>
    public static bool TryGetEmptySlot(this Chest chest, out int index, bool randomSlot) {
        var indices = new List<int>();

        for (var i = 0; i < Chest.maxItems; i++) {
            var item = chest.item[i];

            if (item != null && item.IsAir) {
                indices.Add(i);
            }
        }

        if (indices.Count <= 0) {
            index = -1;

            return false;
        }

        index = randomSlot ? Main.rand.Next(indices) : indices[0];

        return true;
    }
}

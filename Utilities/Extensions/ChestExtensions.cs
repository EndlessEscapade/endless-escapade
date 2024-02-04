using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace EndlessEscapade.Utilities.Extensions;

public static class ChestExtensions
{
    public static bool HasItem(this Chest chest, int type) {
        for (var i = 0; i < Chest.maxItems; i++) {
            var item = chest.item[i];

            if (!item.IsAir && item.type == type) {
                return true;
            }
        }

        return false;
    }

    public static bool TryAddItem(this Chest chest, int type, int stack, bool randomSlot) {
        if (!chest.TryGetEmptySlot(out var index, randomSlot) || type == ItemID.None) {
            return false;
        }

        chest.item[index].SetDefaults(type);
        chest.item[index].stack = stack;

        return true;
    }

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

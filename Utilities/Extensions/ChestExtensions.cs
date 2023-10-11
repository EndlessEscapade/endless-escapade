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
        var index = -1;
        var foundIndex = false;

        if (randomSlot) {
            foundIndex |= chest.TryGetEmptyRandomSlot(out index);
        }
        else {
            foundIndex |= chest.TryGetFirstEmptySlot(out index);
        }

        if (!foundIndex || type == ItemID.None) {
            return false;
        }

        chest.item[index].SetDefaults(type);
        chest.item[index].stack = stack;

        return true;
    }

    public static bool TryAddShopItem(this Chest chest, int type, ref int nextSlot) {
        if (!chest.TryGetFirstEmptySlot(out _) || type == ItemID.None || nextSlot == -1) {
            return false;
        }

        chest.item[nextSlot].SetDefaults(type);
        nextSlot++;

        return true;
    }

    public static bool TryGetEmptyRandomSlot(this Chest chest, out int index) {
        var indices = new List<int>();

        for (var i = 0; i < Chest.maxItems; i++) {
            var item = chest.item[i];

            if (item != null && item.IsAir) {
                indices.Add(i);
            }
        }

        if (indices.Count > 0) {
            index = Main.rand.Next(indices);
            return true;
        }

        index = -1;

        return false;
    }

    public static bool TryGetFirstEmptySlot(this Chest chest, out int index) {
        for (var i = 0; i < Chest.maxItems; i++) {
            var item = chest.item[i];

            if (item != null && item.IsAir) {
                index = i;
                return true;
            }
        }

        index = -1;

        return false;
    }
}

using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    public static bool HasItemStack(this Player player, int type, int stack) {
        var count = 0;
        
        for (var i = 0; i < player.inventory.Length; i++) {
            var item = player.inventory[i];

            if (!item.IsAir && item.type == type) {
                count += item.stack;
            }
        }

        return count >= stack;
    }

    public static void ConsumeItemStack(this Player player, int type, int stack) {
        if (!player.HasItemStack(type, stack)) {
            return;
        }

        var count = stack;
        
        for (var i = 0; i < player.inventory.Length; i++) {
            var item = player.inventory[i];

            if (count <= 0) {
                break;
            }

            if (!item.IsAir && item.type == type) {
                var previousStack = item.stack;
                
                item.stack -= count;
                count -= previousStack;
            }
        }
    }
}

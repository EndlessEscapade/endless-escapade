using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    public static bool HasItem(this Player player, int type, int stack = 1) {
        for (int i = 0; i < player.inventory.Length; i++) {
            var item = player.inventory[i];

            if (!item.IsAir && item.type == type && item.stack >= stack) {
                return true;
            }
        }

        return false;
    }
}

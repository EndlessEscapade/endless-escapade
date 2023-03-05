using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    public static bool HasItemStack(this Player player, int type, int minStack) {
        for (int i = 0; i < 58; i++) {
            if (player.inventory[i].type == type && player.inventory[i].stack >= minStack) {
                return true;
            }
        }

        return false;
    }
}
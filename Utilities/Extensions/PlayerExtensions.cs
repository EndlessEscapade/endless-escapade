using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    public static bool IsDrowning(this Player player) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }

    public static bool HasHelmet(this Player player, int type) {
        return player.armor[0].type == type;
    }

    public static bool HasHelmet<T>(this Player player) where T : ModItem {
        return player.HasHelmet(ModContent.ItemType<T>());
    }
}

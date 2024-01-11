using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    public static bool IsDrowning(this Player player) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }
}

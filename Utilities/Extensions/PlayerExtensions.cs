using Terraria;

namespace EndlessEscapade.Utilities.Extensions;

public static class PlayerExtensions
{
    // TODO: Generic overload for ModItem shorthands. Maybe something to enumerate accessories since they have a specific index start/end.
    public static bool HasAccessory(this Player player, int type) {
        var hasAccessory = false;

        for (var i = 3; i < 10; i++) {
            var item = player.armor[i];

            if (item != null && !item.IsAir && item.type == type) {
                hasAccessory = true;
                break;
            }
        }

        return hasAccessory;
    }
}

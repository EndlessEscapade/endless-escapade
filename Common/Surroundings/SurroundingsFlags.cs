using EndlessEscapade.Content.Biomes;
using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.Surroundings;

public static class SurroundingsFlags
{
    [SurroundingsCallback("Shipyard")]
    public static bool Shipyard(in SurroundingsInfo info) {
        return info.Player.InModBiome<Shipyard>();
    }

    [SurroundingsCallback("Beach")]
    public static bool Beach(in SurroundingsInfo info) {
        return info.Player.ZoneBeach;
    }

    [SurroundingsCallback("Submerged")]
    public static bool Submerged(in SurroundingsInfo info) {
        return info.Player.IsDrowning();
    }
}

using EndlessEscapade.Content.Biomes;
using Terraria.ID;

namespace EndlessEscapade.Common.Surroundings;

public static class SurroundingsFlags
{
    [SurroundingsCallback("Ocean")]
    public static bool Ocean(in SurroundingsInfo info) {
        return info.Metrics.GetLiquidCount(LiquidID.Water) > 100;
    }

    [SurroundingsCallback("Beach")]
    public static bool Beach(in SurroundingsInfo info) {
        return info.Player.ZoneBeach;
    }

    [SurroundingsCallback("Shipyard")]
    public static bool Shipyard(in SurroundingsInfo info) {
        return info.Player.InModBiome<Shipyard>();
    }
}

using EndlessEscapade.Content.Biomes;
using Terraria;
using Terraria.ID;

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
}

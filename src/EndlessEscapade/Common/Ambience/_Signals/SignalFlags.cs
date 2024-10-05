using EndlessEscapade.Content.Biomes;
using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.Ambience;

public static class SignalFlags
{
    [SignalUpdater]
    public static bool Underwater(in SignalContext context) {
        return context.Player.IsUnderwater();
    }

    [SignalUpdater]
    public static bool Beach(in SignalContext context) {
        return context.Player.ZoneBeach;
    }

    [SignalUpdater]
    public static bool Shipyard(in SignalContext context) {
        return context.Player.InModBiome<ShipyardBiome>();
    }
}

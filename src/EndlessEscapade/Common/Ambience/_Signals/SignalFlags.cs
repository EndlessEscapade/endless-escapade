using EndlessEscapade.Utilities.Extensions;

namespace EndlessEscapade.Common.Ambience;

public static class SignalFlags
{
    [SignalUpdater]
    public static bool Underwater(in SignalContext context) {
        return context.Player.IsUnderwater();
    }
}

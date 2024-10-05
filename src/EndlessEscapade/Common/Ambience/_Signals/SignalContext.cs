namespace EndlessEscapade.Common.Ambience;

public readonly struct SignalContext
{
    public static SignalContext Default => new() {
        Player = Main.LocalPlayer
    };

    /// <summary>
    ///     The player associated with this signal context.
    /// </summary>
    public Player? Player { get; init; }
}

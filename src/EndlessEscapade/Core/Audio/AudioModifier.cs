namespace EndlessEscapade.Core.Audio;

public struct AudioModifier(string identifier, int timeLeft, AudioModifier.ModifierCallback callback)
{
    public delegate void ModifierCallback(ref AudioParameters parameters, float progress);

    public ModifierCallback Callback = callback;

    /// <summary>
    ///     The current remaining duration of this modifier in ticks.
    /// </summary>
    public int TimeLeft = timeLeft;

    /// <summary>
    ///     The maximum duration of this modifier in ticks.
    /// </summary>
    public int TimeMax = timeLeft;

    /// <summary>
    ///     The unique identifier of this modifier.
    /// </summary>
    public readonly string Identifier = identifier;
}

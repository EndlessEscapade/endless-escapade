/*
 * Implementation taken and inspired by
 * https://github.com/Mirsario/TerrariaOverhaul/tree/dev/Core/AudioEffects
 */

namespace EndlessEscapade.Common.Audio;

public struct AudioModifier
{
    public delegate void ModifierDelegate(float intensity, ref AudioParameters soundParameters);

    public readonly string Id;

    public int TimeLeft { get; set; }
    public int TimeMax { get; set; }

    public ModifierDelegate Modifier { get; set; }

    public AudioModifier(string id, int timeLeft, ModifierDelegate modifier) : this() {
        Id = id;
        TimeLeft = timeLeft;
        TimeMax = timeLeft;
        Modifier = modifier;
    }
}

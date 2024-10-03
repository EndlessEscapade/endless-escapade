namespace EndlessEscapade.Common.Audio;

public struct AudioModifier
{
    public delegate void ModifierCallback(ref AudioParameters parameters, float progress);

    public ModifierCallback Callback;

    public int TimeLeft { get; set; }
    public int TimeMax { get; set; }

    public readonly string Context;

    public AudioModifier(string context, int timeLeft, ModifierCallback callback) {
        Context = context;
        TimeLeft = timeLeft;
        TimeMax = timeLeft;
        Callback = callback;
    }
}

using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Systems.Audio;

public struct SoundModifiers
{
    private float lowPass = 0f;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    private float highPass = 0f;

    public float HighPass {
        get => highPass;
        set => highPass = MathHelper.Clamp(value, 0f, 1f);
    }

    public SoundModifiers() { }
}

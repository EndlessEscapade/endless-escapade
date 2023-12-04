using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Audio;

public struct AudioModifiers
{
    private float lowPass = 0f;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    public AudioModifiers() { }
}

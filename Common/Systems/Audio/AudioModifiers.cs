using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Systems.Audio;

public struct AudioModifiers
{
    private float reverb;

    public float Reverb {
        get => reverb;
        set => reverb = MathHelper.Clamp(value, 0f, 1f);
    }

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    private float highPass;

    public float HighPass {
        get => highPass;
        set => highPass = MathHelper.Clamp(value, 0f, 1f);
    }
}

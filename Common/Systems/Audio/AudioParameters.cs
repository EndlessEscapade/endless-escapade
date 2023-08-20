using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Systems.Audio;

public struct AudioParameters
{
    private float lowPass = 0f;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    public AudioParameters() { }
}

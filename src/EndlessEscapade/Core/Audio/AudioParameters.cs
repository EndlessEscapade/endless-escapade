using Microsoft.Xna.Framework;

namespace EndlessEscapade.Core.Audio;

public struct AudioParameters
{
    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }
}

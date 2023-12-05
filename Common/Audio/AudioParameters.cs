/*
 * Implementation taken and inspired by
 * https://github.com/Mirsario/TerrariaOverhaul/tree/dev/Core/AudioEffects
 */

using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Audio;

public struct AudioParameters
{
    private float lowPass = 0f;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    public AudioParameters() { }
}

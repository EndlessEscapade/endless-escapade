/*
 * Inspiration taken from
 * https://github.com/Mirsario/TerrariaOverhaul
 */

using Microsoft.Xna.Framework;

namespace EndlessEscapade.Common.Audio;

public struct SoundModifiers
{
    private float lowPass = 0f;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 1f);
    }

    public SoundModifiers() { }
}

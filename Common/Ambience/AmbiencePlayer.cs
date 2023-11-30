using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public sealed class AmbiencePlayer : ModPlayer
{
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSplash", SoundType.Ambient);

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 0.9f);
    }
}

using EndlessEscapade.Common.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class WaterMufflePlayer : ModPlayer
{
    private float intensity;

    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0f, 0.9f);
    }

    public override void PostUpdate() {
        if (Player.IsDrowning()) {
            Intensity += 0.05f;
        }
        else {
            Intensity -= 0.05f;
        }

        if (Intensity < 0f) {
            return;
        }

        AudioSystem.AddModifier(
            $"{nameof(EndlessEscapade)}:{nameof(WaterMufflePlayer)}",
            60,
            (ref AudioParameters parameters, float progress) => {
                parameters.LowPass = Intensity * progress;
            }
        );
    }
}

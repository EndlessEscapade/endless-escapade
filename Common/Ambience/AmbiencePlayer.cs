using EndlessEscapade.Common.Audio;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public sealed class AmbiencePlayer : ModPlayer
{
    private static readonly SoundStyle splash = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSplash", SoundType.Ambient);

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 0.9f);
    }

    public override void PreUpdate() {
        UpdateIntensity();
        UpdateAudio();
    }

    private void UpdateIntensity() {
        if (!Player.wet) {
            LowPass -= 0.05f;
            return;
        }

        LowPass += 0.05f;
    }

    private void UpdateAudio() {
        if (LowPass <= 0f) {
            return;
        }

        SoundSystem.SetParameters(new SoundModifiers {
            LowPass = LowPass
        });
    }
}

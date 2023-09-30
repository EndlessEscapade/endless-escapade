using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience.Effects;

public sealed class PlayerWaterEffects : ModPlayer
{
    private static readonly SoundStyle splash = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Water/Splash", SoundType.Ambient);

    private bool oldWetHead;
    private bool oldWetFeet;

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 0.8f);
    }
    
    public override void PreUpdate() {
        oldWetHead = Player.WetHead();
        oldWetFeet = Player.WetFeet();

        UpdateIntensity();
        UpdateAudio();
        UpdateSplash();
    }

    private void UpdateIntensity() {
        if (!Player.WetHead()) {
            LowPass -= 0.05f;
            return;
        }

        LowPass += 0.05f;
    }

    private void UpdateAudio() {
        if (LowPass <= 0f) {
            return;
        }

        SoundSystem.SetParameters(
            new SoundModifiers {
                LowPass = LowPass
            }
        );
    }
    
    private void UpdateSplash() {
        if (Player.WetFeet() && !oldWetFeet) {
            SoundEngine.PlaySound(in splash);
        }

        if (!Player.WetHead() && oldWetHead) {
            SoundEngine.PlaySound(in splash);
        }
    }
}

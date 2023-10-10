using EndlessEscapade.Common.Systems.Audio;
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

    private bool wetHead;
    private bool wetFeet;

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 0.9f);
    }
    
    public override void PreUpdate() {
        oldWetHead = wetHead;
        oldWetFeet = wetFeet;
        
        var headPosition = Player.Center - new Vector2(0f, 16f);
        var feetPosition = Player.Center + new Vector2(0f, 16f);
        
        wetHead = Collision.WetCollision(headPosition, 10, 10);
        wetFeet = Collision.WetCollision(feetPosition, 10, 10);
        
        UpdateIntensity();
        UpdateAudio();
        UpdateSplash();
    }

    private void UpdateIntensity() {
        if (!wetHead) {
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
        if (wetFeet && !oldWetFeet) {
            SoundEngine.PlaySound(in splash);
        }

        if (!wetHead && oldWetHead) {
            SoundEngine.PlaySound(in splash);
        }
    }
}

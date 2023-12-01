using EndlessEscapade.Common.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbiencePlayer : ModPlayer
{
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSplash", SoundType.Ambient);

    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, 0.9f);
    }

    public override void PostUpdate() {
        UpdateFilter();
        UpdateSplash();
    }

    private void UpdateFilter() {
        SoundSystem.SetParameters(new SoundModifiers {
            LowPass = LowPass
        });
        
        if (!Player.wet) {
            LowPass -= 0.05f;
            return;
        }

        LowPass += 0.05f;
    }
    
    private void UpdateSplash() {
        // The game sets Player.wetCount to 10 whenever the player exits/enters water. We check for 5 to make the splash play midway through.     
        if (Player.wetCount != 5) {
            return;
        }
        
        SoundEngine.PlaySound(in WaterSplashSound, Player.Center);
    }
}

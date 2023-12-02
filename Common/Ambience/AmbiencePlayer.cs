using EndlessEscapade.Common.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class AmbiencePlayer : ModPlayer
{
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSplash", SoundType.Ambient);

    public static readonly SoundStyle WaterSubmergedSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSubmergedLoop", SoundType.Ambient) {
        IsLooped = true
    };
    
    private float intensity;

    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0f, 0.9f);
    }
    
    public override void PostUpdate() {
        UpdateFilter();
        UpdateSplash();
    }

    private void UpdateFilter() {
        SoundSystem.SetParameters(new SoundModifiers {
            LowPass = Intensity
        });
        
        if (!Player.IsSubmerged()) {
            Intensity -= 0.05f;
            return;
        }

        Intensity += 0.05f;
    }

    private void UpdateSplash() {
        // The game sets Player.wetCount to 10 whenever the player exits/enters water. We check for 5 to make the splash play midway through.     
        if (Player.wetCount != 5) {
            return;
        }

        SoundEngine.PlaySound(in WaterSplashSound);
    }
}

using EndlessEscapade.Core.Audio;
using EndlessEscapade.Utilities.Extensions;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class WaterAudioEffects : ModPlayer
{
    /// <summary>
    ///     The sound style used by water splashes.
    /// </summary>
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSplash");

    /// <summary>
    ///     The intensity of water audio muffling. Ranges from <c>0f</c> to <c>0.9f</c>.
    /// </summary>
    public float Intensity {
        get => _intensity;
        set => _intensity = MathHelper.Clamp(value, 0f, 0.9f);
    }

    private float _intensity;

    public override void PostUpdate() {
        base.PostUpdate();

        UpdateSplash();
        UpdateMuffling();
    }

    private void UpdateSplash() {
        // The game sets Player.wetCount to 10 whenever the player exits/enters water. We check for 5 to make the splash play midway through.
        if (Player.wetCount != 5) {
            return;
        }

        SoundEngine.PlaySound(in WaterSplashSound, Player.Center);
    }

    private void UpdateMuffling() {
        if (Player.IsUnderwater()) {
            Intensity += 0.05f;
        }
        else {
            Intensity -= 0.05f;
        }

        if (Intensity <= 0f) {
            return;
        }

        AudioManager.AddModifier(
            $"{nameof(EndlessEscapade)}:{nameof(WaterAudioEffects)}",
            60,
            (ref AudioParameters parameters, float progress) => parameters.LowPass = Intensity * progress
        );
    }
}

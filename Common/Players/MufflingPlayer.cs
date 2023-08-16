using EndlessEscapade.Common.Systems.Audio;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class MufflingPlayer : ModPlayer
{
    public const float Duration = 180f;

    private float intensity;

    private float timer;

    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0f, 1f);
    }

    public float Timer {
        get => timer;
        set => timer = MathHelper.Clamp(value, 0f, Duration);
    }

    public override void PreUpdate() {
        var headPosition = Player.Center - new Vector2(0f, 20f);
        var wetHead = Collision.WetCollision(headPosition, 10, 10);

        if (wetHead) {
            Timer++;

            if (Timer < 180) {
                Intensity += 0.025f;
            }
            else {
                Intensity -= 0.025f;
            }
        }
        else {
            Timer--;

            Intensity -= 0.05f;
        }

        if (Intensity <= 0f) {
            return;
        }

        AudioSystem.SetParameters(new AudioParameters() with { LowPass = Intensity }, default);
    }
}

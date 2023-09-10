using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class WaterImmersionPlayer : ModPlayer
{
    private float intensity;

    public bool WetHead {
        get {
            var headPosition = Player.Center - new Vector2(0f, 16f);
            var wetHead = Collision.WetCollision(headPosition, 10, 10) || Player.HasItemEquip(ItemID.FishBowl);

            return wetHead;
        }
    }

    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0f, 0.9f);
    }

    public override void PreUpdate() {
        UpdateIntensity();
        UpdateAudio();
    }

    private void UpdateIntensity() {
        if (WetHead) {
            Intensity += 0.05f;
            return;
        }

        Intensity -= 0.05f;
    }

    private void UpdateAudio() {
        if (Intensity <= 0f) {
            return;
        }

        SoundSystem.SetParameters(new SoundModifiers { LowPass = Intensity });
    }
}

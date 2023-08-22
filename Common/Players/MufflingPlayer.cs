using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class MufflingPlayer : ModPlayer
{
    private float intensity;

    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, Player.wet ? 0.25f : 0f, 1f);
    }

    private bool wasWetHead;

    public override void PreUpdate() {
        var headPosition = Player.Center - new Vector2(0f, 20f);
        var wetHead = Collision.WetCollision(headPosition, 10, 10) || Player.HasItemEquip(ItemID.FishBowl);

        wetHead |= Player.armor[0].type == ItemID.FishBowl;

        if (wetHead) {
            if (!wasWetHead) {
                Intensity = 1f;
            }
            
            Intensity -= 0.001f;
        }
        else {
            Intensity -= 0.05f;
        }

        wasWetHead = wetHead;

        if (Intensity <= 0f) {
            return;
        }

        var parameters = new AudioParameters() with { LowPass = Intensity };

        AudioSystem.SetParameters(parameters, parameters);
    }
}

using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class MufflingPlayer : ModPlayer
{
    public bool WetHead {
        get {
            var headPosition = Player.Center - new Vector2(0f, 16f);
            var wetHead = Collision.WetCollision(headPosition, 10, 10) || Player.HasItemEquip(ItemID.FishBowl);

            return wetHead;
        }
    }
    
    private float intensity;

    public float Intensity {
        get => intensity;
        set {
            intensity = MathHelper.Clamp(value, WetHead ? 0.5f : 0f, 1f);
        }
    }
    
    private bool wetFadeOut;

    public override void PreUpdate() {
        if (WetHead) {
            if (!wetFadeOut){
                Intensity += 0.05f;
                
                if (Intensity >= 1f) {
                    wetFadeOut = true;
                }
            }
            else {
                Intensity -= 0.0025f;
            }
        }
        else {
            Intensity -= 0.025f;

            wetFadeOut = false;
        }

        if (Intensity <= 0f) {
            return;
        }

        var parameters = new AudioParameters() with { LowPass = Intensity };

        AudioSystem.SetParameters(parameters, parameters);
    }
}

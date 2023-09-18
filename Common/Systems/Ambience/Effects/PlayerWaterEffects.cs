using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience.Effects;

public class PlayerWaterEffects : ModPlayer
{
    private float lowPass;
    private float highPass;
    private float maxLowPass;
    
    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, maxLowPass);
    }
    
    public float HighPass {
        get => highPass;
        set => highPass = MathHelper.Clamp(value, 0f, 0.5f);
    }

    public override void PreUpdate() {
        UpdateIntensity();
        UpdateAudio();
    }

    private void UpdateIntensity() {
        var headPosition = Player.Center - new Vector2(0f, 16f);
        var tile = Framing.GetTileSafely(headPosition.ToTileCoordinates());

        if (Collision.WetCollision(headPosition, 10, 10)) {
            switch (tile.LiquidType) {
                case LiquidID.Water:
                    LowPass += 0.1f;
                    maxLowPass = 0.7f;
                    break;
                case LiquidID.Lava:
                    LowPass += 0.1f;
                    maxLowPass = 0.5f;
                    break;
                case LiquidID.Honey:
                    LowPass += 0.1f;
                    maxLowPass = 0.9f;
                    break;
                case LiquidID.Shimmer:
                    HighPass += 0.1f;
                    break;
            }
            
            return;
        }
        
        LowPass -= 0.1f;
        HighPass -= 0.1f;
    }

    private void UpdateAudio() {
        if (LowPass <= 0f && HighPass <= 0f) {
            return;
        }

        SoundSystem.SetParameters(new SoundModifiers { LowPass = LowPass, HighPass = HighPass });
    }
}

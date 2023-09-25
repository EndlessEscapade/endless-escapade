using EndlessEscapade.Common.Systems.Audio;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience.Effects;

public class PlayerWaterEffects : ModPlayer
{
    private float maxLowPass;
    private float lowPass;

    public float LowPass {
        get => lowPass;
        set => lowPass = MathHelper.Clamp(value, 0f, maxLowPass);
    }

    public override void PreUpdate() {
        UpdateIntensity();
        UpdateAudio();
    }

    private void UpdateIntensity() {
        var headPosition = Player.Center - new Vector2(0f, 16f);

        if (!Collision.WetCollision(headPosition, 10, 10) || !Player.HasEquip(ItemID.FishBowl)) {
            LowPass -= 0.1f;
            return;
        }

        var tile = Framing.GetTileSafely(headPosition.ToTileCoordinates());

        switch (tile.LiquidType) {
            case LiquidID.Water:
                maxLowPass = 0.7f;
                break;
            case LiquidID.Lava:
                maxLowPass = 0.5f;
                break;
            case LiquidID.Honey:
                maxLowPass = 0.9f;
                break;
        }

        LowPass += 0.1f;
    }

    private void UpdateAudio() {
        if (LowPass <= 0f) {
            return;
        }

        SoundSystem.SetParameters(new SoundModifiers { LowPass = LowPass });
    }
}

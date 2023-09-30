using EndlessEscapade.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Ambience.Effects;

public sealed class RainBubbleEffects : ModSystem
{
    public override void OnModLoad() {
        On_Rain.Update += RainUpdateHook;
    }

    private static void RainUpdateHook(On_Rain.orig_Update orig, Rain self) {
        orig(self);
        
        if (Collision.WetCollision(self.position, 2, 2) && Main.rand.NextFloat(100f) < Main.gfxQuality * 100f) {
            var dust = Dust.NewDustDirect(self.position, 2, 2, ModContent.DustType<RainBubble>());
            var tile = Framing.GetTileSafely((self.position / 16f).ToPoint());

            switch (tile.LiquidType) {
                case LiquidID.Water:
                    dust.velocity = self.velocity / 4f;
                    break;
                case LiquidID.Lava:
                    dust.velocity = -self.velocity.SafeNormalize(Vector2.Zero);
                    dust.color = new Color(230, 174, 158);
                    break;
                case LiquidID.Honey:
                    dust.velocity = -self.velocity.SafeNormalize(Vector2.Zero) / 2f;
                    dust.color = new Color(230, 227, 158);
                    break;
                case LiquidID.Shimmer:
                    dust.velocity = new Vector2(self.velocity.X, -self.velocity.Y).SafeNormalize(Vector2.Zero) * 2f;
                    dust.color = new Color(250, 212, 246);
                    break;
            }

            self.active = false;
        }
    }
}

using EndlessEscapade.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience.Effects;

[Autoload(Side = ModSide.Client)]
public sealed class RainBubbleEffects : ILoadable
{
    void ILoadable.Load(Mod mod) {
        On_Rain.Update += RainUpdateHook;
    }

    void ILoadable.Unload() { }

    private static void RainUpdateHook(On_Rain.orig_Update orig, Rain self) {
        orig(self);

        var isRainWet = Collision.WetCollision(self.position, 2, 2);
        var canSpawnBubble = Main.rand.NextFloat(250f) > Main.gfxQuality * 100f;

        if (!isRainWet || !canSpawnBubble) {
            return;
        }

        var dust = Dust.NewDustDirect(self.position, 2, 2, ModContent.DustType<Bubble>());
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

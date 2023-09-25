using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Dusts;

public class RainBubble : ModDust
{
    public override void OnSpawn(Dust dust) {
        dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 10, 10);
        dust.noLight = true;
        dust.noGravity = true;
        dust.scale *= Main.rand.NextFloat(0.9f, 1.2f);
        dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        dust.alpha = 100;
    }

    public override bool Update(Dust dust) {
        dust.position += dust.velocity;
        dust.velocity *= 0.99f;
        dust.rotation += dust.velocity.ToRotation() * 0.1f;
        dust.scale -= 0.025f;
        dust.alpha += 2;

        if (!WorldGen.TileEmpty((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f)) || dust.scale <= 0f || dust.alpha >= 255) {
            dust.active = false;
            return false;
        }

        return false;
    }
}

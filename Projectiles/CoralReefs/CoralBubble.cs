using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class CoralBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 48;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.2f;
            Projectile.tileCollide = false;
        }

        private int sinControl;

        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            Projectile.alpha = (int)Projectile.ai[1];
            Projectile.position.Y--;
            sinControl++;
            if (Projectile.ai[1] < 140)
            {
                Projectile.velocity.X += (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / (Projectile.ai[1] / 10);
            }
            else if (Projectile.ai[1] < 160)
            {
                Projectile.position.X += (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / (Projectile.ai[1] / 4);
            }
            else
            {
                Projectile.velocity.X -= (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / (Projectile.ai[1] / 10);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 10; a++)
            {
                Vector2 vector = new Vector2(0, 10).RotatedBy(Math.PI * 0.2 * a, default);
                int index = Dust.NewDust(Projectile.Center, 22, 22, DustID.BlueCrystalShard, vector.X, vector.Y, 0, Color.Blue, 1f);
                Main.dust[index].velocity *= 1.3f;
                Main.dust[index].noGravity = true;
            }
        }
    }
}
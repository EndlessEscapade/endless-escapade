using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class CoralBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 48;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
            projectile.tileCollide = false;
        }

        private int sinControl;

        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.position.Y--;
            sinControl++;
            if (projectile.ai[1] < 140)
                projectile.velocity.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 10);
            else if (projectile.ai[1] < 160)
                projectile.position.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 4);
            else
                projectile.velocity.X -= (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 10);
        }

        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 10; a++)
            {
                Vector2 vector = new Vector2(0, 10).RotatedBy(((Math.PI * 0.2) * a), default);
                int index = Dust.NewDust(projectile.Center, 22, 22, DustID.BlueCrystalShard, vector.X, vector.Y, 0, Color.Blue, 1f);
                Main.dust[index].velocity *= 1.3f;
                Main.dust[index].noGravity = true;
            }
        }
    }
}
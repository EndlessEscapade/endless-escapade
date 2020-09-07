using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class WaterDragonsBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Dragon's Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.timeLeft = 120;
            projectile.penetrate = 3;
            projectile.ranged = true;
            projectile.damage = 5;
            projectile.knockBack = 0;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 5)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                Vector2 offset = new Vector2(xdist, ydist);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 113, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.97f;
                dust.noLight = false;
            }
        }

        public override void AI()
        {
            if (projectile.velocity.Y <= 2)
            {
                projectile.velocity.Y *= 1.02f;
            }

            projectile.rotation = projectile.velocity.Y / 20f;
            projectile.ai[0]++;
            projectile.velocity.X = (float)Math.Sin(projectile.ai[0] / 10f) * 2;
            /*Vector2 position = projectile.Center;
            Dust dust = Dust.NewDustPerfect(position, 111,Vector2.Zero);
            dust.noGravity = true;
            dust.noLight = false;
            dust.fadeIn = 1f;*/
        }
    }
}
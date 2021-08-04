using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class WaterDragonsBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Dragon's Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 3;
            Projectile.ranged = true;
            Projectile.damage = 5;
            Projectile.knockBack = 0;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 5)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                Vector2 offset = new Vector2(xdist, ydist);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, 113, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.97f;
                dust.noLight = false;
            }
        }

        public override void AI()
        {
            if (Projectile.velocity.Y <= 2)
            {
                Projectile.velocity.Y *= 1.02f;
            }

            Projectile.rotation = Projectile.velocity.Y / 20f;
            Projectile.ai[0]++;
            Projectile.velocity.X = (float)Math.Sin(Projectile.ai[0] / 10f) * 2;
            /*Vector2 position = projectile.Center;
            Dust dust = Dust.NewDustPerfect(position, 111,Vector2.Zero);
            dust.noGravity = true;
            dust.noLight = false;
            dust.fadeIn = 1f;*/
        }
    }
}
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class LythenWarhammerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trident Of The Depths");
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 52;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;

            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
        }

        public override void AI()
        {
            projectile.ai[0]++;

            if(projectile.ai[0] < 60)
            {
                projectile.rotation = projectile.velocity.X/8 - MathHelper.PiOver4;
            }
            if (projectile.ai[0] >= 60 && projectile.ai[0] < 480)
            {
                projectile.velocity *= 0.95f;

                if(projectile.ai[1] < 32) projectile.ai[1] += 0.2f;
                Vector2 origin = projectile.Center;
                float radius = 48;
                int numLocations = 32;
                for (int i = 0; i < projectile.ai[1]; i++)
                {
                    Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                    Dust dust = Dust.NewDustPerfect(position, 111);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                }

                projectile.rotation += (projectile.ai[1] / 32);
            }
            if(projectile.ai[0] >= 480)
            {
                projectile.rotation += (projectile.ai[1] / 32);

                projectile.velocity = Vector2.Normalize(projectile.Center - Main.player[projectile.owner].Center) * -16;

                if(Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center) <= 16)
                {
                    Main.player[projectile.owner].velocity += projectile.velocity / 4;
                    projectile.Kill();
                }
            }
        }
    }
}
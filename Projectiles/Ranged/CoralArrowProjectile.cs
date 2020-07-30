using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Ranged
{
    public class CoralArrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
            projectile.arrow = true;
            projectile.aiStyle = 1;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        int bubol = 0;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI/2;
            if(projectile.ai[1] == 1)
            {
                projectile.velocity.Y += projectile.ai[0];
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 5);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 5);
                    Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                    Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 113, offset * 0.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
                projectile.damage = 1000;
            }

            bubol++;
            if(bubol >= 10)
            {
                Projectile.NewProjectile(projectile.position, new Vector2(0, -1), ModContent.ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: projectile.owner);
                bubol = 0;
            }
            
        }
        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] == 1)
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
        }
    }
}

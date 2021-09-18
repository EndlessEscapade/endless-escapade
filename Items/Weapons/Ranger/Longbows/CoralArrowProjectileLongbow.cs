using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class CoralArrowProjectileLongbow : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Arrow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
            Projectile.arrow = true;
            Projectile.aiStyle = -1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private int bubol = 0;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += Projectile.ai[0];
            if (Projectile.ai[1] == 1)
            {
                bubol++;
                if (bubol >= 10)
                {
                    // Projectile.NewProjectile(projectile.position, new Vector2(0, -1), ModContent.ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: projectile.owner);
                    //  bubol = 0;
                }
                if (bubol % 2 == 0)
                    for (int i = 0; i < 360; i += 10)
                    {
                        float xdist = (float)(Math.Sin(i * (Math.PI / 180)) * 2);
                        float ydist = (float)(Math.Cos(i * (Math.PI / 180)) * 1);
                        Vector2 offset = new Vector2(xdist, ydist).RotatedBy(Projectile.rotation);
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, 113, offset);
                        dust.noGravity = true;
                        dust.velocity *= 0.98f;
                        // dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                Projectile.damage = 1000;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] == 1)
            {
                for (int i = 0; i < 360; i += 5)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                    Vector2 offset = new Vector2(xdist, ydist);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, 113, offset * 0.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.97f;
                    // dust.noLight = false;
                }
            }
        }
    }
}
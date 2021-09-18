using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Items.Weapons.Ammo
{ 
    public class CoralArrowProjectile : EEProjectile
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
            Projectile.aiStyle = 1;
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
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 5);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 5);
                    Vector2 offset = new Vector2(xdist, ydist).RotatedBy(Projectile.rotation);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, 113, offset * 0.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    // dust.noLight = false;
                    dust.fadeIn = 1f;
                }
                Projectile.damage = 1000;
            }

            bubol++;
            if (bubol >= 10)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), Projectile.position, new Vector2(0, -1), ModContent.ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: Projectile.owner);
                bubol = 0;
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
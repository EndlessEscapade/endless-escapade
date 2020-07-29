using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.ID;
using EEMod.Items;

namespace EEMod.Projectiles.Ranged
{
    public class LythenHandgunProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Coral");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 10)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * Math.Cos(i/10)*20);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * Math.Sin(i/10)*20);
                Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 111, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.94f;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            for (int i = 0; i < 360; i += 10)
            {
                float xdist = (int)(Math.Sin(Math.Sin(i * (Math.PI / 180))) * 5);
                float ydist = (int)(Math.Cos(Math.Cos(i * (Math.PI / 180))) * 5);
                Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 111, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.94f;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
    }
}

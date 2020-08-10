using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class LythenStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Shell");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 16;
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
            Vector2 origin = projectile.Center;
            float radius = 8;
            int numLocations = 180;
            for (int i = 0; i < numLocations; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust dust = Dust.NewDustPerfect(position, 111);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(dust.position - projectile.Center) * 4;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Vector2 origin = projectile.Center;
            float radius = 4;
            int numLocations = 30;
            Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * projectile.ai[0])) * radius;
            projectile.ai[0]++;
            Dust dust = Dust.NewDustPerfect(position, 111);
            dust.noGravity = true;
            dust.velocity = Vector2.Normalize(dust.position - projectile.Center) * 2;
            dust.noLight = false;
            dust.fadeIn = 1f;

            radius = 16;
            numLocations = 120;
            for (int i = 0; i < projectile.ai[1]; i++)
            {
                position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                dust = Dust.NewDustPerfect(position, 111);
                dust.noGravity = true;
                dust.velocity = projectile.velocity;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
            if (projectile.ai[1] < 120)
                projectile.ai[1]++;
            else if (projectile.ai[1] == 120)
            {
                projectile.damage = projectile.damage * 2;
                projectile.ai[1] = 121;
            }
        }
    }
}

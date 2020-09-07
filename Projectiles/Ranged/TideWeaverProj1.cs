using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Ranged
{
    public class TideWeaverProj1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tennis Ball");
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

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
            projectile.rotation += projectile.velocity.X / 16f;
        }
    }
}
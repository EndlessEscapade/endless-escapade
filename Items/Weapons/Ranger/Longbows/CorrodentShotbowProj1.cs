using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class CorrodentShotbowProj1 : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrodent Shotbow");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            Projectile.velocity.Y += Projectile.ai[0];
            Projectile.rotation += Projectile.velocity.X / 16f;
        }
    }
}
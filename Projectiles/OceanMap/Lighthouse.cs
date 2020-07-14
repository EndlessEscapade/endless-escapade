using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class Lighthouse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lighthouse");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 8;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }

        public override void AI()
        {
            projectile.timeLeft = 1000;
        }
    }
}

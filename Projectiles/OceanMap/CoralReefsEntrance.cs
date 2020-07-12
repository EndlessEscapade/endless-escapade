using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class CoralReefsEntrance : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Land");
        }

        public override void SetDefaults()
        {
            projectile.width = 220;
            projectile.height = 116;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
            projectile.alpha = 180;
        }

        public override void AI()
        {
            projectile.timeLeft = 1000;
        }
    }
}

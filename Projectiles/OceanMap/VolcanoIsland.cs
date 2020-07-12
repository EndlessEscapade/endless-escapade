using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class VolcanoIsland : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Land");
        }

        public override void SetDefaults()
        {
            projectile.width = 118;
            projectile.height = 96;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
        }

        public override void AI()
        {
          projectile.timeLeft = 1000;
        }
    }
}

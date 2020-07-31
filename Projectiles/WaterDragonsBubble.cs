using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class WaterDragonsBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Dragon's Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.timeLeft = 120;
            projectile.penetrate = 3;
            projectile.ranged = true;
            projectile.damage = 5;
            projectile.knockBack = 0;
        }

        public override void AI()
        {
            if (projectile.velocity.Y <= 2)
                projectile.velocity.Y *= 1.02f;
        }
    }
}

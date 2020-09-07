using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Ranged
{
    public class CoralCannonProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Cannon Projectile");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            projectile.velocity.Y = projectile.velocity.Y + 0.25f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
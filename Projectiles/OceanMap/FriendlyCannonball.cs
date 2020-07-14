using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class FriendlyCannonball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cannonball");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}

using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class InkCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("InkCloud");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 256;
        }

        public override void AI()
        {
            projectile.velocity *= 0.98f;
            projectile.alpha++;
        }
    }
}

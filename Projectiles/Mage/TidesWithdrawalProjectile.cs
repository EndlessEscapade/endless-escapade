using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class TidesWithdrawalProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raindrop");
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
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 33, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 150, default, 0.7f);
        }
    }
}

/*using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DruidsVine : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Druid's Vine");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 0;
            projectile.timeLeft = 10;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ModContent.ProjectileType<DruidsVin>(), projectile.damage, 0f, 0, 0, projectile.ai[1]);
                Main.projectile[p].netUpdate = true;
                projectile.localAI[0] = 1;
            }
        }
    }
}*/
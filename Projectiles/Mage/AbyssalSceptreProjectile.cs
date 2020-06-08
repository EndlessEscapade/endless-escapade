using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class AbyssalSceptreProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Fish");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 8;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = 1;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public bool isClone;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!isClone)
            {
                float rot = Main.rand.NextFloat(6.28f);
                Vector2 position = target.Center + Vector2.One.RotatedBy(rot) * 160;
                for(int i=0; i<30; i++)
                    Dust.NewDust(position, projectile.width, projectile.height, 75);
                Vector2 velocity = Vector2.One.RotatedBy(rot) * -1 * 16f;
                int newProj = Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<AbyssalSceptreProjectile>(), projectile.damage, projectile.knockBack);
                Main.projectile[newProj].tileCollide = false;
                Main.projectile[newProj].timeLeft = 60;
                (Main.projectile[newProj].modProjectile as AbyssalSceptreProjectile).isClone = true;
            }
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
        }
    }
}
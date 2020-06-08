using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Archon
{
    public class HadesExplosion : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Explosion");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.penetrate = -1;

            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;

            projectile.timeLeft = 10;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 242,
                projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.velocity += projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }
            for (int i = 0; i < 5; i++)
            {
                Vector2 direction = new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));

                Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), direction * 8, ModContent.ProjectileType<HadesFireball>(), 150, 0f);
            }
        }
    }
}
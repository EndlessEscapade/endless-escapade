using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Archon
{
    public class HadesRing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Ring");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.penetrate = -1;

            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;

            projectile.timeLeft = 120;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            ///projectile.rotation = projectile.velocity.ToRotation();
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 242, projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.velocity += projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }

            projectile.rotation += 0.5f;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<HadesExplosion>(), 150, 0f);
        }
    }
}

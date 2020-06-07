using Terraria;
using Terraria.ModLoader;

namespace InteritosMod.NPCs.Archon
{
    public class HadesFireball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Fireball");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = 1;

            projectile.tileCollide = true;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, 242,
                projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.velocity += projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }

            projectile.velocity *= 1.02f;
        }
    }
}

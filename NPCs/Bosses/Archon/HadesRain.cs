using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Archon
{
    public class HadesRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hades Rain");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 14;
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

            projectile.velocity *= 1.005f;
        }
    }
}
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class TridentOfTheDepthsWhirlpool : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whirlpool");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
        }

        public override void AI()
        {
            projectile.velocity *= 1.002f;
            projectile.frameCounter++;
            if (projectile.frameCounter > 8)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 6)
            {
                projectile.frame = 0;
                projectile.frameCounter = 0;
            }
        }
    }
}
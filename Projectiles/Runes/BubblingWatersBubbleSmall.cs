using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersBubbleSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.timeLeft = 900;
            projectile.penetrate = 3;
            projectile.ranged = true;
            projectile.damage = 5;
            projectile.knockBack = 0;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            projectile.ai[0]++;

            if (projectile.ai[1] == 0)
                projectile.Center = new Vector2((float)Math.Sin(projectile.ai[0] / 20) * 60 + owner.Center.X, (float)Math.Sin(projectile.ai[0] / 40) * 60 + owner.Center.Y);//(float)Math.Sin(projectile.ai[0] / 10f) * 2;

            if (projectile.ai[0] == 630)
            {
                projectile.ai[1] = 1;
                projectile.velocity.Y = -1;
            }


            if (projectile.ai[1] == 1)
            {
                if (projectile.velocity.Y >= -8)
                    projectile.velocity.Y *= 1.03f;
                projectile.velocity.X = (float)Math.Sin(projectile.ai[0] / 20);
            }
            projectile.rotation = projectile.velocity.Y / 20f;
        }
    }
}

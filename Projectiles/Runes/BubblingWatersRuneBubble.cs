using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersRuneBubble : ModProjectile
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
            projectile.timeLeft = 10000;
            projectile.penetrate = -1;
            projectile.damage = 0;
            projectile.knockBack = 0;
        }

        public override void AI()
        {
            Projectile owner = Main.projectile[projectile.owner];
            projectile.ai[1]++;

            if (projectile.ai[0] == 0)
            {
                projectile.Center = new Vector2((float)Math.Sin(projectile.ai[1] / 20) * 60 + owner.Center.X, (float)Math.Sin(projectile.ai[1] / 40) * 60 + owner.Center.Y);
                Main.NewText("i exist");
            }
        }
    }
}
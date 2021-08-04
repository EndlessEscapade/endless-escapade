using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersRuneBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
        }

        public override void AI()
        {
            Projectile owner = Main.projectile[Projectile.owner];
            Projectile.ai[1]++;

            if (Projectile.ai[0] == 0)
            {
                Projectile.Center = new Vector2((float)Math.Sin(Projectile.ai[1] / 20) * 60 + owner.Center.X, (float)Math.Sin(Projectile.ai[1] / 40) * 60 + owner.Center.Y);
                Main.NewText("i exist");
            }
        }
    }
}
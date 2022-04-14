using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersBubbleSmall : EEProjectile
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
            Projectile.timeLeft = 900;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.damage = 5;
            Projectile.knockBack = 0;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.ai[0]++;

            if (Projectile.ai[1] == 0)
            {
                Projectile.Center = new Vector2((float)Math.Sin(Projectile.ai[0] / 20) * 60 + owner.Center.X, (float)Math.Sin(Projectile.ai[0] / 40) * 60 + owner.Center.Y);//(float)Math.Sin(projectile.ai[0] / 10f) * 2;
            }

            if (Projectile.ai[0] == 630)
            {
                Projectile.ai[1] = 1;
                Projectile.velocity.Y = -1;
            }

            if (Projectile.ai[1] == 1)
            {
                if (Projectile.velocity.Y >= -8)
                {
                    Projectile.velocity.Y *= 1.03f;
                }

                Projectile.velocity.X = (float)Math.Sin(Projectile.ai[0] / 20);
            }
            Projectile.rotation = Projectile.velocity.Y / 20f;
        }
    }
}
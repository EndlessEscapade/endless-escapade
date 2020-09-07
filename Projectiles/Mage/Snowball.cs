using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class Snowball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snowball");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.ignoreWater = true;
            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 149, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 150, default, 0.7f);
            }

            projectile.scale *= 1.01f;
            projectile.width = (int)Math.Floor(projectile.width * 1.01f);
            projectile.height = (int)Math.Floor(projectile.height * 1.01f);
            projectile.damage = (int)Math.Floor(projectile.damage * 1.01f);
            projectile.netUpdate = true;

            projectile.velocity.Y = projectile.velocity.Y + 0.25f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.rotation += 6f * projectile.direction;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.Melee
{
    public class AxeLightning : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Warhammer");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.damage = 0;
            projectile.alpha = 255;
            projectile.timeLeft = 120;
            projectile.extraUpdates = 3;
        }

        Vector2 initialVelocity = Vector2.Zero;
        public override void AI()
        {
            if (initialVelocity == Vector2.Zero)
            {
                initialVelocity = projectile.velocity;
            }
            if (projectile.timeLeft % 10 == 0)
            {
                projectile.velocity = initialVelocity.RotatedBy(Main.rand.NextFloat(-1, 1));
            }
            if (projectile.timeLeft % 2 == 0)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 226);
                dust.noGravity = true;
                dust.scale = (float)Math.Sqrt(projectile.timeLeft) / 4;
                dust.velocity = Vector2.Zero;
            }
        }
    }
}
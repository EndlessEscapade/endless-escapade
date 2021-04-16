using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.CoralReefs
{
    public class TeslaCoralProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Bolt");
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
            projectile.timeLeft = iterations;
            projectile.alpha = 0;
            projectile.hide = true;
        }

        public Vector2 target = Vector2.Zero;
        public int iterations = 8;
        public float distance = 24;

        public override void AI()
        {
            if (projectile.ai[0] < iterations)
            {
                Vector2 dir = target - projectile.Center;

                distance = dir.Length() / 16f;

                Vector2 desiredPoint = dir * (projectile.ai[0] / iterations);

                Vector2 desiredVector = desiredPoint + (Vector2.Normalize(dir - projectile.Center).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f)) * distance);

                projectile.Center += desiredVector;

                projectile.ai[0]++;
            }
            else
            {
                projectile.Center = target;
            }
        }
    }
}
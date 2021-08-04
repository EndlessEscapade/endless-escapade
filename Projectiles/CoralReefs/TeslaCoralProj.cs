using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.CoralReefs
{
    public class TeslaCoralProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.melee = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.timeLeft = iterations;
            Projectile.alpha = 0;
            Projectile.hide = true;
        }

        public Vector2 target = Vector2.Zero;
        public int iterations = 8;
        public float distance = 24;

        public override void AI()
        {
            if (Projectile.ai[0] < iterations)
            {
                Vector2 dir = target - Projectile.Center;

                distance = dir.Length() / 16f;

                Vector2 desiredPoint = dir * (Projectile.ai[0] / iterations);

                Vector2 desiredVector = desiredPoint + (Vector2.Normalize(dir - Projectile.Center).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f)) * distance);

                Projectile.Center += desiredVector;

                Projectile.ai[0]++;
            }
            else
            {
                Projectile.Center = target;
            }
        }
    }
}
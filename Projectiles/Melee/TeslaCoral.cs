using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.Melee
{
    public class TeslaCoral : ModProjectile
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
            projectile.timeLeft = 120;
            projectile.alpha = 0;
        }

        public Vector2 target = Vector2.Zero;
        public int iterations = 8;
        public int distance = 24;

        public override void AI()
        {
            projectile.ai[0]++;

            if (projectile.ai[0] < iterations)
            {
                Vector2 dir = target - projectile.Center;

                Vector2 desiredPoint = dir * (projectile.ai[0] / iterations);

                Vector2 desiredVector = desiredPoint + (Vector2.Normalize(dir - projectile.Center).RotatedBy(MathHelper.PiOver2) * distance);

                projectile.Center = desiredVector;

                Main.NewText("Moving!");
            }
            else
            {
                projectile.Center = target;

                Main.NewText("Stopped!");
            }
        }
    }
}
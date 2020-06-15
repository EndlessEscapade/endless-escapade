using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.OceanMap
{
    public class PirateShip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pirate Ship");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 52;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }

        public override void AI()
        {
            Vector2 moveTo = Main.screenPosition + EEMod.position;
            projectile.spriteDirection = 1;
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            float speed = .3f;
            Vector2 move = moveTo - projectile.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 10f;
            move = (projectile.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            projectile.velocity = move;

            projectile.rotation = projectile.velocity.X / 2;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.OceanMap
{
    public class RedDutchman : ModProjectile
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
            projectile.scale = 1.2f;
        }

        private bool sinking;
        private int hp = 5;
        private int invincTime = 60;

        public override void AI()
        {
            if (!sinking)
            {
                Vector2 moveTo = Main.screenPosition + EEMod.instance.position;
                projectile.spriteDirection = 1;
                if (projectile.velocity.X > 0)
                {
                    projectile.spriteDirection = -1;
                }
                float speed = .5f; //a
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

                invincTime--;

                invincTime = (int)MathHelper.Clamp(invincTime, 0, 61);
                for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ModContent.ProjectileType<FriendlyCannonball>())
                    {
                        if (Vector2.DistanceSquared(Main.projectile[j].Center, projectile.Center) < (50 * 50) && invincTime == 0)
                        {
                            invincTime = 60;
                            Main.projectile[j].Kill();
                            Main.PlaySound(SoundID.NPCHit4);
                            hp--;
                        }
                    }
                }
                if (hp <= 0)
                    sinking = true;
            }
            else
                Sink();
        }

        private int sinkTimer = 32;

        private void Sink()
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0.5f;
            projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                projectile.Kill();
            }
        }
    }
}
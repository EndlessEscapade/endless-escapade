using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;

namespace EEMod.Seamap.SeamapAssets
{
    public class RedDutchman : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pirate Ship");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 52;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.2f;
        }

        private bool sinking;
        private int hp = 5;
        private int invincTime = 60;

        public override void AI()
        {
            if (!sinking)
            {
                Vector2 moveTo = Main.screenPosition + SeamapPlayerShip.localship.position;
                Projectile.spriteDirection = 1;
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = -1;
                }
                float speed = .5f; //a
                Vector2 move = moveTo - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                float turnResistance = 10f;
                move = (Projectile.velocity * turnResistance + move) / (turnResistance + 1f);
                magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                Projectile.velocity = move;

                Projectile.rotation = Projectile.velocity.X / 2;

                invincTime--;

                invincTime = (int)MathHelper.Clamp(invincTime, 0, 61);
                for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ModContent.ProjectileType<FriendlyCannonball>())
                    {
                        if (Vector2.DistanceSquared(Main.projectile[j].Center, Projectile.Center) < (50 * 50) && invincTime == 0)
                        {
                            invincTime = 60;
                            Main.projectile[j].Kill();
                            Main.PlaySound(SoundID.NPCHit4);
                            hp--;
                        }
                    }
                }
                if (hp <= 0)
                {
                    sinking = true;
                }
            }
            else
            {
                Sink();
            }
        }

        private int sinkTimer = 32;

        private void Sink()
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0.5f;
            Projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                Projectile.Kill();
            }
        }
    }
}

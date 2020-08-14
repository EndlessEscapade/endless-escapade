using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

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

        private bool sinking;
        public override void AI()
        {
            if (!sinking)
            {
                projectile.ai[0]++;
                Vector2 moveTo = Main.screenPosition + EEMod.instance.position;
                projectile.spriteDirection = 1;
                if (projectile.velocity.X > 0)
                {
                    projectile.spriteDirection = -1;
                }
                float speed = .3f;
                Vector2 move = moveTo - projectile.Center;
                if (projectile.ai[0] % 180 == 0 && move.LengthSquared() < 500 * 500)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Normalize(move) * 3, ModContent.ProjectileType<EnemyCannonball>(), 10, 10f);
                }
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

                for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ModContent.ProjectileType<FriendlyCannonball>())
                    {
                        if (Vector2.DistanceSquared(Main.projectile[j].Center, projectile.Center) < (60 * 60))
                        {
                            sinking = true;
                            Main.projectile[j].Kill();
                            Main.PlaySound(SoundID.NPCHit4);
                        }
                    }
                }
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

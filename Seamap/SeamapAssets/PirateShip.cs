using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class PirateShip : SeamapObject
    {
        public override void OnSpawn()
        {
            texture = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/PirateShip");

            width = 44;
            height = 52;
        }

        public PirateShip(Vector2 pos, Vector2 vel) : base(pos, vel) { }

        //private bool sinking;

        public override void Update()
        {
            /*if (!sinking)
            {*/
                ai[0]++;
                Vector2 moveTo = SeamapPlayerShip.localship.position;

                /*projectile.spriteDirection = 1;
                if (velocity.X > 0)
                {
                    projectile.spriteDirection = -1;
                }*/

                float speed = .3f;
                Vector2 move = moveTo - Center;

                /*if (ai[0] % 180 == 0 && move.LengthSquared() < 500 * 500)
                {
                    Projectile.NewProjectile(Center, Vector2.Normalize(move) * 3, ModContent.ProjectileType<EnemyCannonball>(), 10, 10f);
                }*/

                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                float turnResistance = 10f;
                move = (velocity * turnResistance + move) / (turnResistance + 1f);
                magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > speed)
                {
                    move *= speed / magnitude;
                }
                velocity = move;

                rotation = velocity.X / 2;

                /*for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ModContent.ProjectileType<FriendlyCannonball>())
                    {
                        if (Vector2.DistanceSquared(Main.projectile[j].Center, Center) < (60 * 60))
                        {
                            sinking = true;
                            Main.projectile[j].Kill();
                            Main.PlaySound(SoundID.NPCHit4);
                        }
                    }
                }*/
            /*}
            else
            {
                Sink();
            }*/
        }

        //private int sinkTimer = 32;

        /*private void Sink()
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0.5f;
            projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                projectile.Kill();
            }
        }*/
    }
}

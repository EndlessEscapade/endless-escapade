using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class LargeScrap : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrap");
        }

        public override void SetDefaults()
        {
            Projectile.width = 158;
            Projectile.height = 16;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = true;

            Projectile.damage = 20;

            Projectile.timeLeft = 1000000000;
        }

        public ref float AttackPhase => ref Projectile.ai[0];

        public float desiredRotation;
        public Vector2 desiredPosition;

        public float lastRotation;
        public Vector2 lastPosition;

        public float movementDuration;
        public float movementTimer;
        public float delayDuration = 20;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void AI()
        {
            switch(AttackPhase)
            {
                case 0: //Embedded in the ground
                    lastPosition = Projectile.Center;
                    lastRotation = Projectile.rotation;

                    Projectile.velocity = Vector2.Zero;

                    movementTimer = 0;

                    break;

                case 1: //Moving towards a given position and rotation over a given duration
                    if (movementTimer < movementDuration)
                    {
                        Projectile.Center = Vector2.SmoothStep(lastPosition, desiredPosition, MathHelper.Clamp(movementTimer / movementDuration, 0f, 1f));
                        Projectile.rotation = MathHelper.SmoothStep(lastRotation, desiredRotation, MathHelper.Clamp(movementTimer / movementDuration, 0f, 1f));
                    }
                    if (movementTimer == movementDuration + delayDuration)
                    {
                        AttackPhase = 2;
                    }

                    movementTimer++;

                    Projectile.velocity = Vector2.SmoothStep(lastPosition, desiredPosition, MathHelper.Clamp(movementTimer / movementDuration, 0f, 1f)) - Projectile.Center;

                    break;

                case 2: //Firing
                    Projectile.velocity = Vector2.UnitX.RotatedBy(Projectile.rotation) * 50f;

                    break;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(AttackPhase == 2)
            {
                bool collisionCheck = true;

                while (collisionCheck)
                {
                    if(Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)].HasTile)
                    {
                        Projectile.Center -= Vector2.Normalize(Projectile.oldVelocity);
                    }
                    else
                    {
                        AttackPhase = 0;
                        collisionCheck = false;
                    }
                }
            }

            return false;
        }
    }
}
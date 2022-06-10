using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class ArmrestScrap : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scrap");
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 22;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = true;

            Projectile.damage = 20;

            Projectile.timeLeft = 1000000000;

            Projectile.hide = true;
        }

        public Vector2 offset;
        public Vector2 desiredPosition;

        public Vector2 lastPosition;
        public float lastRotation;

        public float initRotation;
        public float desiredRotation;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public int AttackPhase;

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.ai[0] == 0 ? -1 : 1;

            if (desiredPosition != Vector2.Zero)
            {
                Projectile.Center = desiredPosition + offset.RotatedBy(desiredRotation);
                Projectile.rotation = desiredRotation + initRotation;
            }

            if(AttackPhase == 0)
            {
                lastPosition = Projectile.Center;
                lastRotation = Projectile.rotation;

                Projectile.velocity = Vector2.Zero;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (AttackPhase == 2 || AttackPhase == 3)
            {
                bool collisionCheck = true;

                while (collisionCheck)
                {
                    if (Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)].HasTile && Main.tileSolid[(int)Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)].BlockType])
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
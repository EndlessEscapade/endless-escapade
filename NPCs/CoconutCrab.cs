using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class CoconutCrab : EENPC
    {
        public bool collision = false;
        public bool canTp = false;
        public bool onGround = false;
        public int alpha = 1;
        private readonly float accel = 0.2f;
        private readonly float maxSpeed = 1.4f;
        public Vector2 tilePos;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Crab");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 58;
            NPC.damage = 12;
            NPC.defense = 4;
            NPC.lifeMax = 90;
            NPC.HitSound = SoundID.NPCHit30;
            NPC.DeathSound = SoundID.NPCDeath33;
            NPC.value = 100f;
            NPC.knockBackResist = 0.3f;
            NPC.alpha = 20;
            NPC.behindTiles = true;
        }

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[NPC.target];
            if (player.Center.X - NPC.Center.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }

            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void AI()
        {
            if (NPC.ai[1] > 0)
            {
                NPC.ai[1]--;
            }

            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            onGround = false;
            collision = false;
            NPC.TargetClosest(true);
            float accel2 = Math.Abs(NPC.Center.X - player.Center.X) / 140;
            if (accel2 > 0.7f)
            {
                accel2 = 0.7f;
            }

            if (NPC.Center.X < player.Center.X)
            {
                NPC.velocity.X += accel * accel2;
            }

            if (NPC.Center.X > player.Center.X)
            {
                NPC.velocity.X -= accel * accel2;
            }

            if (Math.Abs(NPC.velocity.X) == maxSpeed)
            {
                NPC.velocity.X = maxSpeed * NPC.spriteDirection;
            }

            int minTilePosX = (int)(NPC.Center.X / 16.0) - 1;
            int maxTilePosX = (int)(NPC.Center.X / 16.0) + 1;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 5;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0);
            if (minTilePosX < 0)
            {
                minTilePosX = 0;
            }

            if (maxTilePosX > Main.maxTilesX)
            {
                maxTilePosX = Main.maxTilesX;
            }

            if (minTilePosY < 0)
            {
                minTilePosY = 0;
            }

            if (maxTilePosY > Main.maxTilesY)
            {
                maxTilePosY = Main.maxTilesY;
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY + 5; ++j)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile?.IsActive is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        tilePos.X = i * 16;
                        tilePos.Y = j * 16;

                        if (Math.Abs(NPC.Center.Y - tilePos.Y) <= 16 + (NPC.height / 2))
                        {
                            onGround = true;
                        }
                    }
                }
            }
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile?.IsActive is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        Vector2 vector2;
                        vector2.X = i * 16;
                        vector2.Y = j * 16;

                        if (Math.Abs(NPC.Center.X - vector2.X) <= 16 + (NPC.width / 2))
                        {
                            collision = true;
                        }
                    }
                }
            }
            if (Math.Abs(NPC.velocity.X) <= accel * 2.5f && NPC.ai[1] == 0)
            {
                if (onGround && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.velocity.Y -= Main.rand.NextFloat(-8f, -5f);
                    NPC.ai[1] = Main.rand.Next(80, 180);
                    NPC.netUpdate = true;
                }
            }
        }
    }
}
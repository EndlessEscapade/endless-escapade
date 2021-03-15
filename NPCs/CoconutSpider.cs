using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class CoconutSpider : ModNPC
    {
        private readonly int coolDown = 600;
        public bool collision = false;
        public bool canTp = false;
        public bool onGround = false;
        public int alpha = 1;
        private readonly float accel = 0.2f;
        private readonly float maxSpeed = 1.4f;
        public Vector2 tilePos;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Spider");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 58;
            npc.damage = 12;
            npc.defense = 4;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.alpha = 20;
            npc.behindTiles = true;
        }

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[npc.target];
            if (player.Center.X - npc.Center.X > 0)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }

            if (npc.frameCounter++ > 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            if (npc.frame.Y >= frameHeight * 4)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void AI()
        {
            if (npc.ai[1] > 0)
            {
                npc.ai[1]--;
            }

            npc.ai[0] = coolDown;
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            onGround = false;
            collision = false;
            npc.TargetClosest(true);
            float accel2 = Math.Abs(npc.Center.X - player.Center.X) / 140;
            if (accel2 > 0.7f)
            {
                accel2 = 0.7f;
            }

            if (npc.Center.X < player.Center.X)
            {
                npc.velocity.X += accel * accel2;
            }

            if (npc.Center.X > player.Center.X)
            {
                npc.velocity.X -= accel * accel2;
            }

            if (Math.Abs(npc.velocity.X) == maxSpeed)
            {
                npc.velocity.X = maxSpeed * npc.spriteDirection;
            }

            int minTilePosX = (int)(npc.Center.X / 16.0) - 1;
            int maxTilePosX = (int)(npc.Center.X / 16.0) + 1;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 5;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0);
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
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        tilePos.X = i * 16;
                        tilePos.Y = j * 16;

                        if (Math.Abs(npc.Center.Y - tilePos.Y) <= 16 + (npc.height / 2))
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
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        Vector2 vector2;
                        vector2.X = i * 16;
                        vector2.Y = j * 16;

                        if (Math.Abs(npc.Center.X - vector2.X) <= 16 + (npc.width / 2))
                        {
                            collision = true;
                        }
                    }
                }
            }
            if (Math.Abs(npc.velocity.X) <= accel * 2.5f && npc.ai[1] == 0)
            {
                if (onGround && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.velocity.Y -= Main.rand.NextFloat(-8f, -5f);
                    npc.ai[1] = Main.rand.Next(80, 180);
                    npc.netUpdate = true;
                }
            }
        }
    }
}
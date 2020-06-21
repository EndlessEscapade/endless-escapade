using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
namespace EEMod.NPCs.CoralReefs
{
    public class Clam : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Calamity be like
            DisplayName.SetDefault("Clam");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 50;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 84;
            npc.height = 53;
            npc.noGravity = false;
            npc.knockBackResist = 0f;

            npc.npcSlots = 1f;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;

            npc.value = Item.sellPrice(0, 0, 0, 75);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.22f);
        }

        public bool CheckIfEntityOnGround(NPC npc)
        {
            Vector2 tilePos;
            int minTilePosX = (int)(npc.position.X / 16.0) - 5;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 5;
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
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0)))
                    {
                        tilePos.X = (float)(i * 16);
                        tilePos.Y = (float)(j * 16);

                        if (Math.Abs(npc.Center.Y - tilePos.Y) <= 16 + (npc.height / 2))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            float dist = Vector2.Distance(player.Center, npc.Center);
            float yChange = npc.Center.Y - player.Center.Y ;
            if (dist < 200)
                npc.ai[2] = 1;
            npc.TargetClosest(true);
            if (npc.ai[2] == 1)
            {
                if (player.Center.X - npc.Center.X > 0)
                    npc.spriteDirection = 1;
                else
                    npc.spriteDirection = -1;
                if (npc.ai[0] % 200 == 0 && npc.ai[1] == 0 && npc.ai[0] != 0)
                {
                    npc.velocity.X += 10 * npc.spriteDirection;
                    npc.velocity.Y -= 10 * (1+yChange/500);
                    npc.ai[1] = 1;
                }
                if (CheckIfEntityOnGround(npc))
                {
                    if (npc.velocity.Y == 0)
                    {
                        npc.velocity.X = 0;
                        npc.ai[1] = 0;
                    }
                    npc.ai[0]++;
                }

                npc.velocity *= .98f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if(npc.ai[2] == 1)
            {
                npc.frame.Y = frameHeight;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    public class GiantClam : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Clam");
            Main.npcFrameCount[NPC.type] = 11;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 50;
            NPC.damage = 13;
            NPC.defense = 3;

            NPC.width = 84;
            NPC.height = 53;
            NPC.noGravity = false;
            NPC.knockBackResist = 0f;

            NPC.npcSlots = 1f;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.lavaImmune = false;
            banner = NPC.type;
            //bannerItem = ModContent.ItemType<Items.Banners.ClamBanner>();
            NPC.value = Item.sellPrice(0, 0, 0, 75);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.22f);
        }

        public bool CheckIfEntityOnGround(NPC npc)
        {
            Vector2 tilePos;
            int minTilePosX = (int)(npc.position.X / 16.0) - 5;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 5;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 5;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0);

            Helpers.Clamp(ref maxTilePosX, 0, Main.maxTilesX);
            Helpers.Clamp(ref maxTilePosY, 0, Main.maxTilesY);

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY + 5; ++j)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        tilePos.X = i * 16f;
                        tilePos.Y = j * 16f;

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
            Player player = Main.player[NPC.target];
            float yChange = NPC.Center.Y - player.Center.Y;
            if (NPC.WithinRange(player.Center, 200))
            {
                NPC.ai[2] = 1;
            }

            NPC.TargetClosest(true);
            if (NPC.ai[2] == 1)
            {
                if (player.Center.X - NPC.Center.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }

                if (NPC.ai[0] % 200 == 0 && NPC.ai[1] == 0 && NPC.ai[0] != 0)
                {
                    NPC.velocity.X += 10 * NPC.spriteDirection;
                    NPC.velocity.Y -= 10 * (1 + yChange / 500);
                    NPC.ai[1] = 1;
                }
                if (CheckIfEntityOnGround(NPC))
                {
                    if (NPC.velocity.Y == 0)
                    {
                        NPC.velocity.X = 0;
                        NPC.ai[1] = 0;
                    }
                    NPC.ai[0]++;
                }

                NPC.velocity *= .98f;
            }
        }

        /*public override void FindFrame(int frameHeight)
        {
            if (npc.ai[2] == 1)
            {
                npc.frame.Y = frameHeight;
            }
        }*/
    }
}
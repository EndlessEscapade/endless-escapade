using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Clam : ModNPC
    {
        public override void SetStaticDefaults()
        {
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
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.ClamBanner>();
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

            Helpers.Clamp(ref maxTilePosX, 0, Main.maxTilesX);
            Helpers.Clamp(ref maxTilePosY, 0, Main.maxTilesY);

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY + 5; ++j)
                {
                    Tile tile = Main.tile[i, j];
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
            Player player = Main.player[npc.target];
            float yChange = npc.Center.Y - player.Center.Y;
            if (npc.WithinRange(player.Center, 200))
            {
                npc.ai[2] = 1;
            }

            npc.TargetClosest(true);
            if (npc.ai[2] == 1)
            {
                if (player.Center.X - npc.Center.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }

                if (npc.ai[0] % 200 == 0 && npc.ai[1] == 0 && npc.ai[0] != 0)
                {
                    npc.velocity.X += 10 * npc.spriteDirection;
                    npc.velocity.Y -= 10 * (1 + yChange / 500);
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
            if (npc.ai[2] == 1)
            {
                npc.frame.Y = frameHeight;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(TextureCache.ClamGlow, npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
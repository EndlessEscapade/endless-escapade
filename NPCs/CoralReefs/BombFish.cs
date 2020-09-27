using EEMod.Items.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class BombFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bomb Fish");
            Main.npcFrameCount[npc.type] = 16;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 50;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 40;
            npc.height = 30;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.npcSlots = 1f;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;
            banner = npc.type;
            bannerItem = ModContent.ItemType<ClamBanner>();
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

            Helpers.Clamp(ref minTilePosX, 0, Main.maxTilesX);
            Helpers.Clamp(ref minTilePosY, 0, Main.maxTilesY);

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

        private Vector2 playerPosition;
        private Vector2 speed;

        public override void AI()
        {
            npc.rotation = npc.velocity.ToRotation() + MathHelper.Pi;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            npc.ai[1]++;

            if (npc.ai[2] == 0)
            {
                if (npc.ai[1] % 180 == 1 && Main.rand.Next(2) == 0)
                {
                    speed = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                }
                npc.velocity.X += (speed.X - npc.velocity.X) / 16f;
                npc.velocity.Y += (speed.Y - npc.velocity.Y) / 16f;
                if (npc.WithinRange(player.Center, 300))
                {
                    npc.ai[2] = 1;
                    npc.ai[0] = 0;
                }
            }
            if (npc.ai[2] == 1)
            {
                npc.ai[0]++;
                if (npc.ai[0] < 120)
                {
                    npc.velocity += new Vector2((float)Math.Sin(npc.ai[0] / 10f) * 0.5f, -(float)Math.Cos(npc.ai[0] / 10f) * 0.5f);
                    playerPosition = player.Center;
                }
                else if (npc.ai[0] < 128)
                {
                    npc.velocity += (playerPosition - npc.Center) / 500f;
                }
                if (npc.ai[0] > 128)
                {
                    npc.velocity *= 0.98f;
                }
                if (npc.ai[0] >= 200)
                {
                    npc.ai[2] = 0;
                    npc.ai[0] = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(EEMod.instance.GetTexture("NPCs/CoralReefs/BombFishGlow"), npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
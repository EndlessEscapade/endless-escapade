using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.LowerReefs
{
    public class BombFish : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bomb Fish");
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 50;
            NPC.damage = 13;
            NPC.defense = 3;

            NPC.width = 40;
            NPC.height = 30;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.npcSlots = 1f;
            NPC.buffImmune[BuffID.Confused] = true;
            // NPC.lavaImmune = false;
            Banner = NPC.type;
            //bannerItem = ModContent.ItemType<ClamBanner>();
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

            Helpers.Clamp(ref minTilePosX, 0, Main.maxTilesX);
            Helpers.Clamp(ref minTilePosY, 0, Main.maxTilesY);

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY + 5; ++j)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile is true && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0))
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
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.Pi;
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            NPC.ai[1]++;

            if (NPC.ai[2] == 0)
            {
                if (NPC.ai[1] % 180 == 1 && Main.rand.Next(2) == 0)
                {
                    speed = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                }
                NPC.velocity.X += (speed.X - NPC.velocity.X) / 16f;
                NPC.velocity.Y += (speed.Y - NPC.velocity.Y) / 16f;
                if (NPC.WithinRange(player.Center, 300))
                {
                    NPC.ai[2] = 1;
                    NPC.ai[0] = 0;
                }
            }
            if (NPC.ai[2] == 1)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] < 120)
                {
                    NPC.velocity += new Vector2((float)Math.Sin(NPC.ai[0] / 10f) * 0.5f, -(float)Math.Cos(NPC.ai[0] / 10f) * 0.5f);
                    playerPosition = player.Center;
                }
                else if (NPC.ai[0] < 128)
                {
                    NPC.velocity += (playerPosition - NPC.Center) / 500f;
                }
                if (NPC.ai[0] > 128)
                {
                    NPC.velocity *= 0.98f;
                }
                if (NPC.ai[0] >= 200)
                {
                    NPC.ai[2] = 0;
                    NPC.ai[0] = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/BombFishGlow").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
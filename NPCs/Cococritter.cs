using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class Cococritter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coco Critter");
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 5;
        }
        public bool isColliding()
        {
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
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        Vector2 vector2;
                        vector2.X = i * 16;
                        vector2.Y = j * 16;

                        if (Math.Abs(npc.Center.X - vector2.X) <= 16 + (npc.width / 2))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool OnGround()
        {
            Vector2 tilePos;
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
                    Tile tile = Main.tile[i, j];
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        tilePos.X = i * 16;
                        tilePos.Y = j * 16;

                        if (Math.Abs(npc.Center.Y - tilePos.Y) <= 16 + (npc.height / 2))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 5;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.height = 29;
            npc.width = 24;
        }

        public override void AI()
        {
            Animate(4, false);
            npc.velocity.X = npc.ai[1];
            if (npc.ai[0] == 0)
                npc.ai[1] = 1;
            npc.ai[0]++;
            if(npc.ai[0] % 180 == 0 && OnGround())
            {
                npc.velocity.Y -= 5;
                if(isColliding())
                {
                    if (npc.ai[1] == -1)
                    {
                        npc.ai[1] = 1;
                    }
                    else
                    {
                        npc.ai[1] = -1;
                    }
                }
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override void OnCatchNPC(Player player, Item item)
        {
            item.stack = 2;

            try
            {
                var npcCenter = npc.Center.ToTileCoordinates();
                Tile tile = Main.tile[npcCenter.X, npcCenter.Y];
                if (!WorldGen.SolidTile(npcCenter.X, npcCenter.Y) && tile.liquid == 0)
                {
                    tile.liquid = (byte)Main.rand.Next(50, 150);
                    tile.lava(true);
                    tile.honey(false);
                    WorldGen.SquareTileFrame(npcCenter.X, npcCenter.Y, true);
                }
            }
            catch
            {
                return;
            }
        }
        public void Animate(int delay, bool flip)
        {
            Player player = Main.player[npc.target];
            if (flip)
            {
                if (player.Center.X - npc.Center.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                else
                    npc.spriteDirection = -1;
            }
            if (npc.frameCounter++ > delay)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);
            }
            if (npc.frame.Y >= (Main.npcTexture[npc.type].Height/ Main.npcFrameCount[npc.type]) * (Main.npcFrameCount[npc.type] - 1))
            {
                npc.frame.Y = 0;
                return;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            
            
        }
    }
}

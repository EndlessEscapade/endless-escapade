using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Items.Materials;

namespace EEMod.NPCs
{
    public class DuneShambler : ModNPC
    {
        private int coolDownMax = 600;
        private int coolDown = 600;
        public bool collision = false;
        public bool canTp = false;
        private int timeInDig = 100;
        public bool onGround = false;
        public int alpha = 1;
        readonly float accel = 0.2f;
        readonly float maxSpeed = 1.4f;
        public Vector2 tilePos;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dune Shambler");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 == true)
            {
                return SpawnCondition.DesertCave.Chance * 0.5f;
            }
            else
            {
                return SpawnCondition.DesertCave.Chance * 0f;
            }
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
                npc.spriteDirection = -1;
            }
            else
                npc.spriteDirection = 1;

            if (npc.ai[0] > 0 || ((canTp && npc.ai[1] >= timeInDig && npc.ai[1] >= timeInDig + 24)))
            {
                if (npc.frameCounter++ > 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
                if (npc.frame.Y >= frameHeight * 5)
                {
                    npc.frame.Y = 0;
                    return;
                }
            }
            if (npc.ai[0] <= 0 && Math.Abs(npc.velocity.X) <= accel * 2.5f && npc.ai[1] < timeInDig)
            {
                if (npc.frameCounter++ > 6)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
                if (npc.frame.Y >= frameHeight * 5)
                {
                    npc.frame.Y = frameHeight * 5;
                    return;
                }
            }
            if (canTp && npc.ai[1] >= timeInDig && npc.ai[1] <= timeInDig + 24)
            {
                alpha = 1;
                if (npc.frameCounter++ > 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y - frameHeight;
                }
                if (npc.frame.Y <= 0)
                {
                    npc.frame.Y = 0;
                    return;
                }
            }

        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MummifiedRag>(), Main.rand.Next(2));
        }

        public override void AI()
        {
            npc.ai[0] = coolDown;
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            Vector2 moveTo = player.Center + new Vector2(npc.spriteDirection * (npc.width * 4), 0);

            onGround = false;
            collision = false;
            npc.TargetClosest(true);
            float accel2 = Math.Abs(npc.Center.X - player.Center.X) / 140;
            if (accel2 > 0.7f)
                accel2 = 0.7f;
            if (npc.Center.X < player.Center.X)
            {
                npc.velocity.X += accel * accel2;
            }

            if (npc.Center.X > player.Center.X)
            {
                npc.velocity.X -= accel * accel2;
            }

            if (Math.Abs(npc.velocity.X) == maxSpeed)
                npc.velocity.X = maxSpeed * npc.spriteDirection;

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
                    Tile tile = Main.tile[i, j];
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
                    Tile tile = Main.tile[i, j];
                    if (tile?.nactive() is true && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0))
                    {
                        Vector2 vector2;
                        vector2.X = i * 16;
                        vector2.Y = j * 16;

                        if (Math.Abs(npc.Center.X - vector2.X) <= 16 + (npc.width / 2))
                        {
                            coolDown--;
                            collision = true;
                            if (coolDown <= 0)
                                coolDown = 0;
                        }
                    }
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Math.Abs(npc.velocity.X) <= accel * 2.5f)
                {
                    npc.ai[2] = Main.rand.NextFloat(12);
                    if (npc.ai[2] <= 1 && onGround)
                    {
                        npc.velocity.Y -= 1.8f;
                    }
                    if (onGround)
                        coolDown -= 2;
                    if (Main.rand.NextFloat(5) <= 1 && onGround && collision)
                        npc.velocity.Y -= 1.7f;

                    npc.netUpdate = true;
                }
            }
            if (npc.ai[0] <= 0 && Math.Abs(npc.velocity.X) <= accel * 2.5f && onGround)
            {
                canTp = true;
            }
            if (canTp)
            {
                if (npc.ai[1] == 0)
                    npc.frame.Y = 0;
                if (npc.ai[1] < 30 || npc.ai[1] > 100 && npc.ai[1] < 130)
                {
                    int num = Dust.NewDust(npc.position + new Vector2(0, 30), npc.width, npc.height, 0, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 6, default, npc.scale);
                    Main.dust[num].noGravity = false;
                    Main.dust[num].velocity.Y = Main.rand.NextFloat(-6, -3);
                    Main.dust[num].velocity.X = Main.rand.NextFloat(-1, 1);
                    Main.dust[num].noLight = false;
                }
                npc.ai[1]++;
                int distFromPlayer = 10;
                int playerTileX = (int)Main.player[npc.target].position.X / 16;
                int playerTileY = (int)Main.player[npc.target].position.Y / 16;
                int tileX = (int)npc.position.X / 16;
                int tileY = (int)npc.position.Y / 16;
                int teleportCheckCount = 0;
                bool hasTeleportPoint = false;
                if (npc.ai[1] < timeInDig)
                    npc.velocity = new Vector2(0, 0);
                //gayass
                //player is too far away, don't teleport.
                //Universe is so stupid and noone likes him. STFU u stupid ass mofo bitch monkey ass mofo bitch ass monkey mofo ass monkey bitch fucking slutbag.
                if (npc.ai[1] == timeInDig)
                {
                    // Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 2000f
                    if (!Main.player[npc.target].WithinRange(npc.Center, 2000))
                    {
                        teleportCheckCount = 100;
                        hasTeleportPoint = true;
                    }
                    while (!hasTeleportPoint && teleportCheckCount < 100)
                    {
                        teleportCheckCount++;
                        Func<int, int, bool> CanTeleportTo = null;
                        int tpTileX = Main.rand.Next(playerTileX - distFromPlayer, playerTileX + distFromPlayer);
                        int tpTileY = Main.rand.Next(playerTileY - 5, playerTileY + 5);
                        for (int tpY = tpTileY; tpY < playerTileY + distFromPlayer; tpY++)
                        {
                            if ((tpY < playerTileY - 4 || tpY > playerTileY + 4 || tpTileX < playerTileX - 4 || tpTileX > playerTileX + 4) && (tpY < tileY - 1 || tpY > tileY + 1 || tpTileX < tileX - 1 || tpTileX > tileX + 1) && (Main.tile[tpTileX, tpY].nactive()))
                            {
                                if ((CanTeleportTo != null && CanTeleportTo(tpTileX, tpY)) || (Main.tileSolid[Main.tile[tpTileX, tpY].type]) && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1))
                                {
                                    npc.position.X = tpTileX * 16f - npc.width / 2 + 8f;
                                    npc.position.Y = tpY * 16f - npc.height;
                                    hasTeleportPoint = true;

                                    npc.netUpdate = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (npc.ai[1] == timeInDig * 2)
                {
                    canTp = false;
                    npc.ai[1] = 0;
                    coolDown = coolDownMax;
                    npc.netUpdate = true;
                }
            }
            if ((canTp && npc.ai[1] < timeInDig))
                npc.velocity.X = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(Main.magicPixel, tilePos - Main.screenPosition, new Rectangle(0, 0, 16, 16), drawColor * alpha, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            // Mod mod = ModLoader.GetMod("EEMod");
            Texture2D texture = TextureCache.DuneShambler;
            Texture2D texture2 = TextureCache.DuneShamblerDig;
            Player player = Main.player[npc.target];
            //Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            if (player.Center.X - npc.Center.X > 0)
            {
                npc.spriteDirection = -1;
            }
            else
                npc.spriteDirection = 1;

            if (!canTp || ((canTp && npc.ai[1] >= timeInDig && npc.ai[1] >= timeInDig + 24)))
                Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, 13), npc.frame, drawColor * alpha, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            else
                Main.spriteBatch.Draw(texture2, npc.Center - Main.screenPosition + new Vector2(0, 13), npc.frame, drawColor * alpha, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}

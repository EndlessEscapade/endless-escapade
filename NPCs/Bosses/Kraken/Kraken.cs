using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Compatibility;
using EEMod.NPCs.Bosses.Hydros;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class KrakenHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kraken");
            Main.npcFrameCount[npc.type] = 6;
        }
        int tentaclesPer = 7;
        private int frameUpdate;
        private int frameUpdate2;
        public override void FindFrame(int frameHeight)
        {
            frameUpdate++;
            if (frameUpdate >= tentaclesPer)
            {
                npc.frame.Y += frameHeight;
                frameUpdate = 0;
            }
            if (npc.frame.Y == frameHeight * 5)
            {
                npc.frame.Y = 0;
            }
            frameUpdate2++;
            if (frameUpdate2 >= tentaclesPer && seperateFrame.Y < frameHeight * 5)
            {
                seperateFrame.Y += frameHeight;
                frameUpdate2 = 0;
            }
            if (seperateFrame.Y == frameHeight * 5 && resetAnim && thrust)
            {
                seperateFrame.Y = 0;
            }
        }
        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.friendly = false;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.lifeMax = 50000;
            npc.defense = 40;
            npc.damage = 95;
            npc.value = Item.buyPrice(0, 8, 0, 0);
            npc.noTileCollide = true;
            npc.width = 568;
            npc.height = 472;
            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;
            musicPriority = MusicPriority.BossMedium;
        }

        bool firstFrame = true;
        float thrustingPower = 9;
        float variablethrustingPower = 5;
        bool thrust = false;
        bool isRightOrLeft = true;
        bool resetAnim = false;
        Vector2 arenaPosition = new Vector2(8000, 19500);
        Rectangle seperateFrame = new Rectangle(0,0, 568, 472);
        float numberOfPushes;
        float tentaclerotation;
        public override void AI()
        {
            npc.ai[2]++;
            tentaclerotation = 0;
            if (npc.ai[2] < 180)
            {
                EEPlayer.FixateCameraOn(npc.Center, 32f, false);
            }
            else if(npc.ai[2] == 181)
            {
                EEPlayer.TurnCameraFixationsOff();
            }
            npc.ai[0]++;
            Vector2 topLeft = arenaPosition - new Vector2(2500, 1200);
            Vector2 topRight = arenaPosition - new Vector2(-2500, 1200);
            Vector2[] holePositions = { new Vector2((int)topLeft.X, (int)topLeft.Y - 100), new Vector2((int)topRight.X, (int)topRight.Y - 100), new Vector2((int)topLeft.X, (int)topLeft.Y + 1200), new Vector2((int)topRight.X, (int)topRight.Y + 1200) };
            Vector2[] geyserPositions = { arenaPosition + new Vector2(-100, 1000), arenaPosition + new Vector2(100, 1000) };
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            npc.rotation = npc.velocity.X / 80f;
            if(firstFrame)
            {
                npc.Center = topLeft;
                firstFrame = false;
                NPC.NewNPC((int)topLeft.X, (int)topLeft.Y- 100, ModContent.NPCType<KHole>(),0, topLeft.X, topLeft.Y - 100);
                NPC.NewNPC((int)topRight.X, (int)topRight.Y - 100, ModContent.NPCType<KHole>(), 0, topRight.X, topRight.Y - 100);
                NPC.NewNPC((int)topRight.X, (int)topRight.Y + 1200, ModContent.NPCType<KHole>(), 0, topRight.X, topRight.Y + 1200);
                NPC.NewNPC((int)topLeft.X, (int)topLeft.Y + 1200, ModContent.NPCType<KHole>(), 0, topLeft.X, topLeft.Y + 1200);
            }
            if (npc.velocity.X > 0)
                npc.spriteDirection = -1;
            else
                npc.spriteDirection = 1;

            switch(npc.ai[1])
            {
                case 0:
                    {
                        npc.ai[1] = 1;
                        break;
                    }
                case 1:
                    {
                        if(isRightOrLeft)
                        {
                            if (!thrust)
                            {
                                variablethrustingPower *= 0.97f;
                            }
                            resetAnim = false;
                            npc.velocity.X = variablethrustingPower;
                            if(variablethrustingPower <= 1f && !thrust)
                            {
                                thrust = true;
                                resetAnim = true;
                                numberOfPushes++;
                            }
                            if(thrust && variablethrustingPower < thrustingPower)
                            {
                                variablethrustingPower += ((thrustingPower - (thrustingPower - variablethrustingPower)) / 13f);
                                if(variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                                {
                                    thrust = false;
                                }
                            }
                            if(numberOfPushes == 4)
                            {
                                npc.ai[1] = 2;
                                npc.ai[0] = 0;
                            }
                        }
                        else
                        {
                            if (!thrust)
                            {
                                variablethrustingPower *= 0.97f;
                            }
                            resetAnim = false;
                            npc.velocity.X = -variablethrustingPower;
                            if (variablethrustingPower <= 1f && !thrust)
                            {
                                thrust = true;
                                resetAnim = true;
                            }
                            if (thrust && variablethrustingPower < thrustingPower)
                            {
                                variablethrustingPower += ((thrustingPower - (thrustingPower - variablethrustingPower)) / 13f);
                                if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                                {
                                    thrust = false;
                                }
                            }
                            if (npc.Center.X > topRight.X && variablethrustingPower <= 1f)
                            {
                                isRightOrLeft = false;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        npc.velocity *= 0.95f;
                        if(npc.ai[0] < 80)
                        EEPlayer.FixateCameraOn((geyserPositions[0] + geyserPositions[1]) / 2, 64f, true);
                        else if(npc.ai[0] == 80)
                        {
                            Projectile.NewProjectile(geyserPositions[0].X, geyserPositions[0].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, .3f, 140);
                            Projectile.NewProjectile(geyserPositions[1].X, geyserPositions[1].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, .3f, 140);
                        }
                        else if(npc.ai[0] == 120)
                        {
                            EEPlayer.TurnCameraFixationsOff();
                            npc.ai[1] = 1;
                            npc.ai[0] = 0;
                        }
                        break;
                    }
                case 3:
                    {
                        float gradient = (arenaPosition - npc.Center).ToRotation();

                        break;
                    }
                case 4:
                    {
                        npc.ai[1] = Main.rand.Next(1, 4);
                        break;
                    }
                case 5:
                    {
                        npc.ai[1] = Main.rand.Next(1, 4);
                        break;
                    }
            }
        }
        public void SpawnProjectileNearPlayerOnTile(int dist)
        {
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[npc.target].position.X / 16;
            int playerTileY = (int)Main.player[npc.target].position.Y / 16;
            int tileX = (int)npc.position.X / 16;
            int tileY = (int)npc.position.Y / 16;
            int teleportCheckCount = 0;
            bool hasTeleportPoint = false;
            //player is too far away, don't teleport.
            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 2000f)
            {
                teleportCheckCount = 100;
                hasTeleportPoint = true;
            }
            while (!hasTeleportPoint && teleportCheckCount < 100)
            {
                teleportCheckCount++;
                int tpTileX = Main.rand.Next(playerTileX - distFromPlayer, playerTileX + distFromPlayer);
                int tpTileY = Main.rand.Next(playerTileY - distFromPlayer, playerTileY + distFromPlayer);
                for (int tpY = tpTileY; tpY < playerTileY + distFromPlayer; tpY++)
                {
                    if ((tpY < playerTileY - 4 || tpY > playerTileY + 4 || tpTileX < playerTileX - 4 || tpTileX > playerTileX + 4) && (tpY < tileY - 1 || tpY > tileY + 1 || tpTileX < tileX - 1 || tpTileX > tileX + 1) && (Main.tile[tpTileX, tpY].nactive()))
                    {
                        if ((Main.tileSolid[Main.tile[tpTileX, tpY].type]) && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1))
                        {
                            Projectile.NewProjectile(tpTileX * 16, tpY * 16, 0, 0, ModContent.ProjectileType<Geyser>(), 1, 0f, Main.myPlayer, .3f, 140);
                            hasTeleportPoint = true;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.KrakenTentacles;
            Main.spriteBatch.Draw(texture, npc.spriteDirection == -1 ? npc.Center - Main.screenPosition + new Vector2(texture.Width / 16,- texture.Height / 96) : npc.Center - Main.screenPosition + new Vector2(texture.Width / 16,- texture.Height / 96), seperateFrame, drawColor, tentaclerotation, seperateFrame.Size() / 2 + new Vector2(texture.Width / 16, -texture.Height/ 96), npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.KrakenGlowMask;
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, 0), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override void NPCLoot()
        {
            EEWorld.EEWorld.downedKraken = true;
        }
    }
}

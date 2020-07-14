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
        private bool mouthOpenConsume;
        public override void FindFrame(int frameHeight)
        {
            frameUpdate++;
            
            if(mouthOpenConsume)
            {
                if (frameUpdate >= tentaclesPer && npc.frame.Y < frameHeight * 2)
                {
                    npc.frame.Y += frameHeight;
                    frameUpdate = 0;
                }
                if (npc.frame.Y == frameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
            }
            else
            {
                if (frameUpdate >= tentaclesPer)
                {
                    npc.frame.Y += frameHeight;
                    frameUpdate = 0;
                }
                if (npc.frame.Y == frameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
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

        public void Reset(int from)
        {
            npc.ai[0] = 0;
            numberOfPushes = 0;
            EEPlayer.TurnCameraFixationsOff();
            while(npc.ai[1] == from)
            {
              npc.ai[1] = Main.rand.Next(1, 4);
            }
            npc.netUpdate = true;
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
        public bool GETHIMBOIS;
        public override void AI()
        {
            
            npc.ai[2]++;
            tentaclerotation = 0;
            mouthOpenConsume = false;
            if (npc.ai[2] < 180)
            {
                EEPlayer.FixateCameraOn(npc.Center, 32f, false);
            }
            else if(npc.ai[2] == 181)
            {
                EEPlayer.TurnCameraFixationsOff();
            }
            Vector2 topLeft = arenaPosition - new Vector2(2500, 1200);
            Vector2 topRight = arenaPosition - new Vector2(-2500, 1200);
            Vector2[] holePositions = { new Vector2((int)topLeft.X + 200, (int)topLeft.Y - 100), new Vector2((int)topRight.X, (int)topRight.Y - 100), new Vector2((int)topLeft.X, (int)topLeft.Y + 1200), new Vector2((int)topRight.X + 200, (int)topRight.Y + 1200) };
            Vector2[] geyserPositions = { arenaPosition + new Vector2(-100, 1000), arenaPosition + new Vector2(100, 1000) };
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            npc.rotation = npc.velocity.X / 80f;
            if(firstFrame)
            {
                npc.ai[1] = 1;
                npc.Center = topLeft;
                firstFrame = false;
                NPC.NewNPC((int)holePositions[0].X, (int)holePositions[0].Y, ModContent.NPCType<KHole>(),0, (int)holePositions[0].X, (int)holePositions[0].Y);
                NPC.NewNPC((int)holePositions[1].X, (int)holePositions[1].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[1].X, (int)holePositions[1].Y);
                NPC.NewNPC((int)holePositions[2].X, (int)holePositions[2].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[2].X, (int)holePositions[2].Y);
                NPC.NewNPC((int)holePositions[3].X, (int)holePositions[3].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[3].X, (int)holePositions[3].Y);
            }
            if (npc.velocity.X > 0)
                npc.spriteDirection = -1;
            else
                npc.spriteDirection = 1;

            switch(npc.ai[1])
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        npc.ai[0]++;
                        if (isRightOrLeft)
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
                                Reset(1);
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
                        npc.ai[0]++;
                        npc.velocity *= 0.95f;
                        if (npc.ai[0] < 80)
                        {
                            EEPlayer.FixateCameraOn((geyserPositions[0] + geyserPositions[1]) / 2, 64f, true);
                        }
                        else if (npc.ai[0] == 80)
                        {
                            Projectile.NewProjectile(geyserPositions[0].X, geyserPositions[0].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 0);
                            Projectile.NewProjectile(geyserPositions[1].X, geyserPositions[1].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 0);
                        }
                        else if (npc.ai[0] == 100)
                        {
                            Reset(2);
                        }
                        break;
                    }
                case 3:
                    {
                        Vector2 gradient = Vector2.Normalize(arenaPosition - npc.Center);
                        if (Vector2.Distance(arenaPosition,npc.Center) > 200 && !GETHIMBOIS)
                        {
                            if (!thrust)
                            {
                                variablethrustingPower *= 0.97f;
                            }
                            resetAnim = false;
                            npc.velocity.X = variablethrustingPower * gradient.X;
                            npc.velocity.Y = variablethrustingPower * gradient.Y;
                            if (variablethrustingPower <= 1f && !thrust)
                            {
                                thrust = true;
                                resetAnim = true;
                                numberOfPushes++;
                            }
                            if (thrust && variablethrustingPower < thrustingPower)
                            {
                                variablethrustingPower += ((thrustingPower - (thrustingPower - variablethrustingPower)) / 13f);
                                if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                                {
                                    thrust = false;
                                }
                            }
                        }
                        else if(!GETHIMBOIS)
                        {
                                npc.ai[0]++;
                                resetAnim = true;
                                npc.velocity *= 0.98f;
                            if(npc.ai[0] == 100)
                            {
                                for(int i = 0; i<holePositions.Length; i++)
                                {
                                    NPC.NewNPC((int)holePositions[i].X + 200, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(),0,0,0,npc.whoAmI);
                                }
                            }
                            if(npc.ai[0] >= 400)
                            {
                                Reset(3);
                            }
                        }
                        if (GETHIMBOIS)
                        {
                            EEPlayer.FixateCameraOn(npc.Center, 64f, false);
                            gradient = Vector2.Normalize((player.Center + new Vector2(300, 0)) - npc.Center);
                            if (Vector2.Distance(player.Center + new Vector2(300, 0), npc.Center) > 200)
                            { 
                            if (!thrust)
                            {
                                variablethrustingPower *= 0.97f;
                            }
                            resetAnim = false;
                            npc.velocity.X = variablethrustingPower * gradient.X;
                            npc.velocity.Y = variablethrustingPower * gradient.Y;
                            if (variablethrustingPower <= 1f && !thrust)
                            {
                                thrust = true;
                                resetAnim = true;
                                numberOfPushes++;
                            }
                            if (thrust && variablethrustingPower < thrustingPower)
                            {
                                variablethrustingPower += ((thrustingPower - (thrustingPower - variablethrustingPower)) / 13f);
                                if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                                {
                                    thrust = false;
                                }
                            }
                            }
                            else
                            {
                                npc.ai[0]++;
                                npc.velocity = (player.Center + new Vector2(300, 0) - npc.Center)/64f;
                                npc.velocity *= .98f;
                                resetAnim = true;
                                mouthOpenConsume = true;
                                for (int i = 0; i < 9; i++)
                                {
                                    int num = Dust.NewDust(npc.Center - new Vector2(110 * npc.spriteDirection, 10), 5, 5, DustID.SolarFlare, Main.rand.NextFloat(-2, -5) * npc.spriteDirection, 0, 6, default, 2);
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].velocity *= 15f;
                                    Main.dust[num].velocity.Y = Main.rand.NextFloat(-2, 2);
                                    Main.dust[num].noLight = false;
                                }
                                if(npc.ai[0] == 280)
                                {
                                    Reset(3);
                                }
                            }
                        }
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

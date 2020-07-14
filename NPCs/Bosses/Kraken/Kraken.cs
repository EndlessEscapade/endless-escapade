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
    [AutoloadBossHead]
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

            if (mouthOpenConsume)
            {
                if (frameUpdate >= tentaclesPer && npc.frame.Y != frameHeight * 2)
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
            while (npc.ai[1] == from)
            {
                npc.ai[1] = Main.rand.Next(1, 5);
            }
            npc.netUpdate = true;
            npc.alpha = 0;
            tentacleAlpha = 1;
            GETHIMBOIS = false;
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
            npc.alpha = 255;
            tentacleAlpha = 0;
        }

        bool firstFrame = true;
        float thrustingPower = 9;
        float variablethrustingPower = 5;
        bool thrust = false;
        bool isRightOrLeft = true;
        bool resetAnim = false;
        Vector2 arenaPosition = new Vector2(8000, 19500);
        Vector2[] dashPositions = new Vector2[5];
        Vector2[] npcFromPositions = new Vector2[5];
        Rectangle seperateFrame = new Rectangle(0, 0, 568, 472);
        float numberOfPushes;
        float tentaclerotation;
        public bool GETHIMBOIS;
        float tentacleAlpha = 1;
        bool hasChains;
        public override bool CheckActive()
        {
            return false;
        }
        public override void AI()
        {

            npc.ai[2]++;
            tentaclerotation = 0;
            mouthOpenConsume = false;
            if (npc.ai[2] < 180)
            {
                tentacleAlpha += 0.01f;
                tentacleAlpha = Helpers.Clamp(tentacleAlpha, 0, 1);
                npc.alpha -= 2;
                EEPlayer.FixateCameraOn(npc.Center, 32f, false, true);
            }
            else if (npc.ai[2] == 181)
            {
                EEPlayer.TurnCameraFixationsOff();
            }
            Vector2 topLeft = arenaPosition - new Vector2(2500, 1200);
            Vector2 topRight = arenaPosition - new Vector2(-2500, 1200);
            Vector2[] holePositions = { new Vector2((int)topLeft.X + 400, (int)topLeft.Y - 100), new Vector2((int)topRight.X- 300, (int)topRight.Y - 100), new Vector2((int)topLeft.X + 400, (int)topLeft.Y + 1200), new Vector2((int)topRight.X - 300, (int)topRight.Y + 1200) };
            Vector2[] geyserPositions = { arenaPosition + new Vector2(-100, 1000), arenaPosition + new Vector2(100, 1000) };
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            npc.rotation = npc.velocity.X / 80f;
            if (firstFrame)
            {
                npc.ai[1] = 1;
                npc.Center = topLeft;
                firstFrame = false;
                NPC.NewNPC((int)holePositions[0].X, (int)holePositions[0].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[0].X, (int)holePositions[0].Y);
                NPC.NewNPC((int)holePositions[1].X, (int)holePositions[1].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[1].X, (int)holePositions[1].Y,1);
                NPC.NewNPC((int)holePositions[2].X, (int)holePositions[2].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[2].X, (int)holePositions[2].Y);
                NPC.NewNPC((int)holePositions[3].X, (int)holePositions[3].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[3].X, (int)holePositions[3].Y,1);
            }
            if (npc.velocity.X > 0)
                npc.spriteDirection = -1;
            else
                npc.spriteDirection = 1;

            switch (npc.ai[1])
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
                            if (numberOfPushes == 4)
                            {
                                Reset(1);
                            }
                            if(npc.Center.X > topRight.X && variablethrustingPower <= 1f)
                            {
                                isRightOrLeft = false;
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
                            if (npc.Center.X < topLeft.X && variablethrustingPower <= 1f)
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
                        if(npc.ai[0] == 20)
                        {
                            for(int i = 0; i<10; i++)
                            SpawnProjectileNearPlayerOnTile(100);
                        }
                        if (npc.ai[0] < 80)
                        {
                            EEPlayer.FixateCameraOn((geyserPositions[0] + geyserPositions[1]) / 2, 64f, true, false);
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
                        if (Vector2.DistanceSquared(arenaPosition, npc.Center) > (200*200) && !GETHIMBOIS)
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
                        else if (!GETHIMBOIS)
                        {
                            npc.ai[0]++;
                            resetAnim = true;
                            npc.velocity *= 0.98f;
                            if (npc.ai[0] == 100)
                            {
                                for (int i = 0; i < holePositions.Length; i++)
                                {
                                    if(i == 0 || i == 2)
                                    NPC.NewNPC((int)holePositions[i].X + 10, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, npc.whoAmI);
                                    if (i == 1 || i == 3)
                                    NPC.NewNPC((int)holePositions[i].X + 10, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, npc.whoAmI,1);
                                }
                            }
                            if (npc.ai[0] >= 400)
                            {
                                Reset(3);
                            }
                        }
                        if (GETHIMBOIS)
                        {
                            EEPlayer.FixateCameraOn(npc.Center, 64f, false, true);
                            gradient = Vector2.Normalize(player.Center + new Vector2(300 * (npc.Center.X - player.Center.X) /Math.Abs(npc.Center.X - player.Center.X), 0) - npc.Center);
                            if (Vector2.DistanceSquared(player.Center + new Vector2(300 * (npc.Center.X - player.Center.X)/ Math.Abs(npc.Center.X - player.Center.X), 0), npc.Center) > (180* 180))
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
                                npc.velocity = (player.Center + new Vector2(300 * (npc.Center.X - player.Center.X) / Math.Abs(npc.Center.X - player.Center.X), 0) - npc.Center) / 64f;
                                npc.velocity *= .98f;
                                resetAnim = true;
                                mouthOpenConsume = true;
                                if(npc.ai[0] == 10)
                                {
                                    CombatText.NewText(npc.getRect(), Colors.RarityBlue, "*How the fuck did you fall for that???", false, false);
                                }
                                if (npc.ai[0] == 80)
                                {
                                    CombatText.NewText(npc.getRect(), Colors.RarityBlue, "Now that you're here", false, false);
                                }
                                if (npc.ai[0] > 140)
                                {
                                    EEPlayer.FixateCameraOn(npc.Center, 64f, true, true);
                                    float projectilespeedX = 10 * -npc.spriteDirection;
                                    float projectilespeedY = Main.rand.NextFloat(-2,2);
                                    float projectileknockBack = 4f;
                                    int projectiledamage = 20;
                                    Projectile.NewProjectile(npc.Center.X + 110 * -npc.spriteDirection, npc.Center.Y + 10, projectilespeedX, projectilespeedY, mod.ProjectileType("WaterSpew"), projectiledamage,projectileknockBack, npc.target, 0f, 0f);
                                    if (npc.ai[0] == 280)
                                    {
                                        Reset(3);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        npc.ai[0]++;
                        npc.alpha++;
                        tentacleAlpha -= 0.025f;
                        int speed = 50;
                        npc.velocity.X = (float)Math.Sin(npc.ai[0] / 10);
                        npc.velocity.Y = (float)Math.Cos(npc.ai[0] / 10);
                        if (npc.alpha > 255)
                        {
                            npc.alpha = 255;
                        }
                        if (npc.ai[0] < (dashPositions.Length) * speed)
                        {
                            if (npc.ai[0] % speed == 0)
                            {
                                hasChains = true;
                                dashPositions[(int)(npc.ai[0] / speed)] = player.Center + new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7));
                                npcFromPositions[(int)(npc.ai[0] / speed)] = arenaPosition - new Vector2(Main.rand.Next(-2000, -1000), Main.rand.Next(-2000, -1000));
                            }

                            for (int j = 0; j < 300; j++)
                            {
                                for (int i = 0; i <= (int)(npc.ai[0] / speed); i++)
                                {
                                    Lighting.AddLight((npcFromPositions[i] - (dashPositions[i] - npcFromPositions[i])) + (Vector2.Normalize(dashPositions[i] - npcFromPositions[i]) * 30) * j, new Vector3(0, .5f, 0));
                                }
                            }
                        }
                        else
                        {
                            Reset(4);
                            int magikalFormula = (int)(npc.ai[0] - (dashPositions.Length - 1) * speed) / 100;
                            if (npc.ai[0] % speed == 0)
                            {
                               // npc.Center = npcFromPositions[magikalFormula];
                            }
                           // npc.velocity = Vector2.Normalize(dashPositions[magikalFormula] - npcFromPositions[magikalFormula]) * 80;
                        }
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
            if (Vector2.DistanceSquared(npc.Center, Main.player[npc.target].Center) > (2000f* 2000f))
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
                            Projectile.NewProjectile(tpTileX * 16, tpY * 16, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, .3f, 140);
                            hasTeleportPoint = true;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
            }
        }
        Vector2[] startingPoint = new Vector2[5];
        Vector2[] endingPoint = new Vector2[5];
        Vector2[] midPoint = new Vector2[5];
        float daFlop;
        float daFlopX;
        int coolDownForCollision;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            coolDownForCollision--;
            if(coolDownForCollision < 0)
            {
                coolDownForCollision = 0;
            }
                daFlop = ((float)Math.Sin(npc.ai[2]/20f) * 100) + 100;
                daFlopX = ((float)Math.Cos(npc.ai[2] / 30f) * 60);
            if (hasChains)
            {
                for (int i = 0; i < npcFromPositions.Length; i++)
                {
                    if (npc.ai[1] != 4 || (npc.ai[0] > (dashPositions.Length - 1) * 50 && npc.ai[1] == 4))
                    {
                        startingPoint[i] -= (startingPoint[i] - npcFromPositions[i]) / 32f;
                        endingPoint[i] = npcFromPositions[i] + (Vector2.Normalize(dashPositions[i] - npcFromPositions[i]) * 5500);
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.1f + new Vector2(daFlopX, daFlop);
                    }
                    else
                    {
                        midPoint[i] = startingPoint[i] + (endingPoint[i] - startingPoint[i]) * 0.5f;
                        startingPoint[i] += (endingPoint[i] - startingPoint[i]) / 64f;
                    }
                    Helpers.DrawBezier(spriteBatch, TextureCache.TentacleChain, "", drawColor, startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], 0.01f, (float)Math.PI/2);
                    Rectangle playerHitBox = new Rectangle((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
                        for (int j = 0; j < Helpers.ReturnPoints(startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], 0.01f, 80,140,5).Length; j++)
                        {
                            if (playerHitBox.Intersects(Helpers.ReturnPoints(startingPoint[i], endingPoint[i], midPoint[i], midPoint[i], 0.01f, 80,140, 3)[j]))
                            {
                            if (coolDownForCollision == 0)
                            {
                                Main.player[npc.target].AddBuff(BuffID.Confused, 60);
                                Main.player[npc.target].velocity *= -2;
                                coolDownForCollision = 60;
                            }
                            }
                        }
                }
            }
            
            Texture2D texture = TextureCache.KrakenTentacles;
            Main.spriteBatch.Draw(texture, npc.spriteDirection == -1 ? npc.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96) : npc.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96), seperateFrame, drawColor * tentacleAlpha, tentaclerotation, seperateFrame.Size() / 2 + new Vector2(texture.Width / 16, -texture.Height / 96), npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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

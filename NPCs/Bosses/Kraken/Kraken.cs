using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        private readonly int tentaclesPer = 7;
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
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            npc.ai[0] = 0;
            numberOfPushes = 0;
            modPlayer.TurnCameraFixationsOff();
            while (npc.ai[1] == from)
            {
                npc.ai[1] = Main.rand.Next(1, 6);
            }
            npc.netUpdate = true;
            npc.alpha = 0;
            tentacleAlpha = 1;
            GETHIMBOIS = false;
            thrust = false;
            resetAnim = false;
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
            npc.damage = 0;
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

        private bool firstFrame = true;
        private readonly float thrustingPower = 9;
        private float variablethrustingPower = 5;
        private bool thrust = false;
        public bool isRightOrLeft = true;
        private bool resetAnim = false;
        public Vector2 arenaPosition = new Vector2(Main.maxTilesX / 2 * 16, (Main.maxTilesY / 2 + 400) * 16);
        public Vector2[] dashPositions = new Vector2[5];
        public Vector2[] npcFromPositions = new Vector2[5];
        private Rectangle seperateFrame = new Rectangle(0, 0, 568, 472);
        public float numberOfPushes;
        public float tentaclerotation;
        public bool GETHIMBOIS;
        public float tentacleAlpha = 1;
        public bool hasChains;
        private Vector2[] geyserPositionsVarUp;
        private Vector2[] geyserPositionsVarDown;
        private Vector2[] geyserPositionsVarRight;
        private Vector2[] geyserPositionsVarLeft;
        public int howMany = 5;
        public Vector2[] smolBloons = new Vector2[2];
        public Vector2[] bigBloons = new Vector2[2];
        public float waterLevel;
        private readonly float speedOfIncline = 0.1f;

        public void UpdateInkBlobs(int chance)
        {
            if (Main.rand.Next(chance) == 0)
            {
                if (smolBloons[0] == Vector2.Zero || smolBloons[1] == Vector2.Zero)
                {
                    int pieCut = Main.rand.Next(3, 4);
                    for (int m = 0; m < pieCut; m++)
                    {
                        int projID = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<InkBlob>(), 50, 0, Main.myPlayer, 0, npc.whoAmI);
                        Main.projectile[projID].velocity = new Vector2(3f, 0f).RotatedBy(m / (float)pieCut * Math.PI * 2);
                        Main.projectile[projID].netUpdate = true;
                    }
                }
                if ((smolBloons[1] == Vector2.Zero) && (smolBloons[0] != Vector2.Zero))
                {
                    smolBloons[1] = npc.Center;
                }
                if (smolBloons[0] == Vector2.Zero)
                {
                    smolBloons[0] = npc.Center;
                }
            }
            if (smolBloons[0] != Vector2.Zero && smolBloons[1] != Vector2.Zero)
            {
                if ((bigBloons[1] == Vector2.Zero) && (bigBloons[0] != Vector2.Zero) && bigBloons[0] != (smolBloons[0] + smolBloons[1]) / 2)
                {
                    bigBloons[1] = (smolBloons[0] + smolBloons[1]) / 2;
                    Projectile.NewProjectile(bigBloons[1], Vector2.Zero, ModContent.ProjectileType<SecondPhaseInkBlob>(), 50, 0, Main.myPlayer, 0, npc.whoAmI);
                }
                if (bigBloons[0] == Vector2.Zero)
                {
                    bigBloons[0] = (smolBloons[0] + smolBloons[1]) / 2;
                    Projectile.NewProjectile(bigBloons[0], Vector2.Zero, ModContent.ProjectileType<SecondPhaseInkBlob>(), 50, 0, Main.myPlayer, 0, npc.whoAmI);
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            waterLevel += speedOfIncline;
            Vector2 topLeft = arenaPosition - new Vector2(2500, 1200);
            Vector2 topRight = arenaPosition - new Vector2(-2500, 1200);
            Vector2[] holePositions = { new Vector2((int)topLeft.X + 400, (int)topLeft.Y - 100), new Vector2((int)topRight.X - 300, (int)topRight.Y - 100), new Vector2((int)topLeft.X + 400, (int)topLeft.Y + 1200), new Vector2((int)topRight.X - 300, (int)topRight.Y + 1200) };
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (firstFrame)
            {
                geyserPositionsVarUp = new Vector2[howMany];
                geyserPositionsVarDown = new Vector2[howMany];
                geyserPositionsVarRight = new Vector2[howMany];
                geyserPositionsVarLeft = new Vector2[howMany];
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<TentacleEdgeHandler>(), 0, npc.whoAmI);
                npc.ai[1] = 1;
                npc.Center = topLeft;
                firstFrame = false;
                NPC.NewNPC((int)holePositions[0].X, (int)holePositions[0].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[0].X, (int)holePositions[0].Y);
                NPC.NewNPC((int)holePositions[1].X, (int)holePositions[1].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[1].X, (int)holePositions[1].Y, 1);
                NPC.NewNPC((int)holePositions[2].X, (int)holePositions[2].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[2].X, (int)holePositions[2].Y);
                NPC.NewNPC((int)holePositions[3].X, (int)holePositions[3].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[3].X, (int)holePositions[3].Y, 1);
            }
            npc.ai[2]++;
            tentaclerotation = 0;
            mouthOpenConsume = false;
            if (npc.ai[2] < 180)
            {
                tentacleAlpha += 0.01f;
                tentacleAlpha = Helpers.Clamp(tentacleAlpha, 0, 1);
                npc.alpha -= 2;
                modPlayer.FixateCameraOn(npc.Center, 32f, false, true, 10);
            }
            else if (npc.ai[2] == 181)
            {
                modPlayer.TurnCameraFixationsOff();
                npc.damage = 150;
            }

            Vector2[] geyserPositions = { arenaPosition + new Vector2(-100, 1000), arenaPosition + new Vector2(100, 1000) };
            npc.rotation = npc.velocity.X / 80f;

            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }

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
                            UpdateInkBlobs(3);
                            numberOfPushes++;
                        }

                        if (thrust && variablethrustingPower < thrustingPower)
                        {
                            variablethrustingPower += (thrustingPower - (thrustingPower - variablethrustingPower)) / 13f;
                            if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                            {
                                thrust = false;
                            }
                        }
                        if (numberOfPushes == 4)
                        {
                            Reset(1);
                        }
                        if (npc.Center.X > topRight.X && variablethrustingPower <= 1f)
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
                            UpdateInkBlobs(3);
                            numberOfPushes++;
                        }
                        if (thrust && variablethrustingPower < thrustingPower)
                        {
                            variablethrustingPower += (thrustingPower - (thrustingPower - variablethrustingPower)) / 13f;
                            if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                            {
                                thrust = false;
                            }
                        }
                        if (numberOfPushes == 4)
                        {
                            Reset(1);
                        }
                        if (npc.Center.X < topLeft.X && variablethrustingPower <= 1f)
                        {
                            isRightOrLeft = true;
                        }
                    }
                    break;
                }
                case 2:
                {
                    npc.ai[0]++;
                    npc.velocity *= 0.95f;
                    if (npc.ai[0] == 10)
                    {
                        geyserPositionsVarUp = SpawnProjectileNearPlayerOnTile(40, howMany, true);
                        geyserPositionsVarDown = SpawnProjectileNearPlayerOnTile(40, howMany, false);
                        geyserPositionsVarRight = SpawnProjectileNearPlayerOnTileSide(40, howMany, true);
                        geyserPositionsVarLeft = SpawnProjectileNearPlayerOnTileSide(40, howMany, false);
                    }
                    if (npc.ai[0] == 30)
                    {
                        for (int i = 0; i < geyserPositionsVarUp.Length; i++)
                        {
                            Projectile.NewProjectile(geyserPositionsVarUp[i].X, geyserPositionsVarUp[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 0);
                            Projectile.NewProjectile(geyserPositionsVarDown[i].X, geyserPositionsVarDown[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 1);
                            Projectile.NewProjectile(geyserPositionsVarRight[i].X, geyserPositionsVarRight[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 2);
                            Projectile.NewProjectile(geyserPositionsVarLeft[i].X, geyserPositionsVarLeft[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 3);
                        }
                    }
                    for (int i = 0; i < geyserPositionsVarUp.Length; i++)
                    {
                        if (npc.ai[0] > 100)
                        {
                            if (npc.ai[0] % 4 == 0)
                            {
                                Projectile.NewProjectile(geyserPositionsVarUp[i].X, geyserPositionsVarUp[i].Y, Main.rand.NextFloat(-.1f, .1f), Main.rand.NextFloat(-10, -15), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(geyserPositionsVarDown[i].X, geyserPositionsVarDown[i].Y, Main.rand.NextFloat(-.1f, .1f), Main.rand.NextFloat(10, 15), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(geyserPositionsVarRight[i].X, geyserPositionsVarRight[i].Y, Main.rand.NextFloat(10f, 15f), Main.rand.NextFloat(-.1f, .1f), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(geyserPositionsVarLeft[i].X, geyserPositionsVarLeft[i].Y, Main.rand.NextFloat(-10f, -15f), Main.rand.NextFloat(-.1f, .1f), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                            }
                        }
                    }
                    if (npc.ai[0] < 80)
                    {
                        modPlayer.FixateCameraOn((geyserPositions[0] + geyserPositions[1]) / 2, 64f, true, false, 10);
                    }
                    else if (npc.ai[0] == 200)
                    {
                        Reset(2);
                    }
                    break;
                }
                case 3:
                {
                    Vector2 gradient = Vector2.Normalize(arenaPosition - npc.Center);
                    if (Vector2.DistanceSquared(arenaPosition, npc.Center) > (280 * 280) && !GETHIMBOIS)
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
                            UpdateInkBlobs(3);
                            numberOfPushes++;
                        }
                        if (thrust && variablethrustingPower < thrustingPower)
                        {
                            variablethrustingPower += (thrustingPower - (thrustingPower - variablethrustingPower)) / 13f;
                            if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                            {
                                thrust = false;
                            }
                        }
                    }
                    else if (!GETHIMBOIS)
                    {
                        npc.ai[0]++;

                        npc.velocity *= 0.98f;
                        if (npc.ai[0] == 100)
                        {
                            for (int i = 0; i < holePositions.Length; i++)
                            {
                                if (i == 0 || i == 2)
                                {
                                    NPC.NewNPC((int)holePositions[i].X + 120, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, npc.whoAmI);
                                }

                                if (i == 1 || i == 3)
                                {
                                    NPC.NewNPC((int)holePositions[i].X + 100, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, npc.whoAmI, 1);
                                }
                            }
                        }
                        if (npc.ai[0] >= 400)
                        {
                            Reset(3);
                        }
                        if (npc.ai[0] > 10)
                        {
                            resetAnim = true;
                            thrust = true;
                            npc.velocity.X = (float)Math.Sin(npc.ai[0] / 20) * 2;
                            npc.velocity.Y = (float)Math.Cos(npc.ai[0] / 20) * 2;
                        }
                    }
                    if (GETHIMBOIS)
                    {
                        Vector2 yeet;
                        if (isRightOrLeft)
                        {
                            yeet = player.Center + new Vector2(400, 0);
                        }
                        else
                        {
                            yeet = player.Center + new Vector2(-400, 0);
                        }
                        modPlayer.FixateCameraOn(npc.Center, 64f, false, true, 10);
                        gradient = Vector2.Normalize(yeet - npc.Center);
                        if (Vector2.DistanceSquared(yeet, npc.Center) > (180 * 180))
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
                                variablethrustingPower += (thrustingPower - (thrustingPower - variablethrustingPower)) / 13f;
                                if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                                {
                                    thrust = false;
                                }
                            }
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.velocity = (yeet - npc.Center) / 64f;
                            npc.velocity *= .98f;
                            resetAnim = true;
                            mouthOpenConsume = true;
                            if (isRightOrLeft)
                            {
                                npc.spriteDirection = 1;
                            }
                            else
                            {
                                npc.spriteDirection = -1;
                            }
                            if (npc.ai[0] == 10)
                            {
                                CombatText.NewText(npc.getRect(), Colors.RarityBlue, "*How the fuck did you fall for that???", false, false);
                            }
                            if (npc.ai[0] == 80)
                            {
                                CombatText.NewText(npc.getRect(), Colors.RarityBlue, "Now that you're here", false, false);
                            }
                            if (npc.ai[0] > 140)
                            {
                                modPlayer.FixateCameraOn(npc.Center, 64f, true, true, 10);
                                float projectilespeedX = 10 * -npc.spriteDirection;
                                float projectilespeedY = Main.rand.NextFloat(-2, 2);
                                float projectileknockBack = 4f;
                                int projectiledamage = 20;
                                Projectile.NewProjectile(npc.Center.X + 110 * -npc.spriteDirection, npc.Center.Y + 10, projectilespeedX, projectilespeedY, ModContent.ProjectileType<WaterSpew>(), projectiledamage, projectileknockBack, npc.target, 0f, 1);
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
                    resetAnim = true;
                    thrust = true;
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
                    if (npc.ai[0] < dashPositions.Length * speed)
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
                                Lighting.AddLight(npcFromPositions[i] + Vector2.Normalize(dashPositions[i] - npcFromPositions[i]) * 30 * j, new Vector3(0, .5f, 0));
                            }
                        }
                    }
                    else
                    {
                        Reset(4);
                    }
                    break;
                }
                case 5:
                {
                    Vector2 gradient = Vector2.Normalize(topRight - npc.Center);
                    if (Vector2.DistanceSquared(topRight, npc.Center) > (200 * 200) && !GETHIMBOIS)
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
                            UpdateInkBlobs(3);
                            numberOfPushes++;
                        }
                        if (thrust && variablethrustingPower < thrustingPower)
                        {
                            variablethrustingPower += (thrustingPower - (thrustingPower - variablethrustingPower)) / 13f;
                            if (variablethrustingPower > thrustingPower || Math.Abs(variablethrustingPower - thrustingPower) < 0.4f)
                            {
                                thrust = false;
                            }
                        }
                    }
                    else
                    {
                        int frequency = 10;
                        npc.ai[0]++;
                        if (npc.ai[0] % frequency == 0)
                        {
                            Projectile.NewProjectile(player.Center + new Vector2(Main.rand.Next(-1000, 1000), -1000), Vector2.Zero, ModContent.ProjectileType<KramkenRocc>(), 40, 1f);
                        }
                        if (npc.ai[0] % frequency * 8 <= frequency * 8 / 2)
                        {
                            modPlayer.FixateCameraOn(player.Center, 64f, true, false, 10);
                        }
                        else
                        {
                            modPlayer.TurnCameraFixationsOff();
                        }
                        if (npc.ai[0] == 400)
                        {
                            Reset(5);
                            modPlayer.TurnCameraFixationsOff();
                        }
                    }
                    break;
                }
            }
        }

        public void SpawnProjectileNearPlayerOnTile(int dist, bool above)
        {
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[npc.target].position.X / 16;
            int playerTileY = (int)Main.player[npc.target].position.Y / 16;
            int tileX = (int)npc.position.X / 16;
            int tileY = (int)npc.position.Y / 16;
            int teleportCheckCount = 0;
            bool hasTeleportPoint = false;
            //player is too far away, don't teleport.
            if (Vector2.DistanceSquared(npc.Center, Main.player[npc.target].Center) > (2000f * 2000f))
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
                    if (Framing.GetTileSafely(tpTileX, tpY).nactive())
                    {
                        if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1) && above)
                        {
                            Projectile.NewProjectile(tpTileX * 16, tpY * 16, 0, -5, ModContent.ProjectileType<WaterSpew>(), 1, 0f, Main.myPlayer, 0, 0);
                            hasTeleportPoint = true;
                            npc.netUpdate = true;
                            break;
                        }
                        if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY + 4, tpY + 1) && !above)
                        {
                            Projectile.NewProjectile(tpTileX * 16, tpY * 16, 0, 5, ModContent.ProjectileType<WaterSpew>(), 1, 0f, Main.myPlayer, 0, 0);
                            hasTeleportPoint = true;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
            }
        }

        public Vector2[] SpawnProjectileNearPlayerOnTile(int dist, int howMany, bool above)
        {
            Vector2[] geyserPoses = new Vector2[howMany];
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[npc.target].position.X / 16;
            int playerTileY = (int)Main.player[npc.target].position.Y / 16;
            int tileX = (int)npc.position.X / 16;
            int tileY = (int)npc.position.Y / 16;
            int teleportCheckCount = 0;
            //player is too far away, don't teleport.
            for (int i = 0; i < howMany; i++)
            {
                bool hasTeleportPoint = false;
                if (Vector2.DistanceSquared(npc.Center, Main.player[npc.target].Center) > (2000f * 2000f))
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
                        if (Framing.GetTileSafely(tpTileX, tpY).nactive())
                        {
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1) && above)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                npc.netUpdate = true;
                                break;
                            }
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY + 1, tpY + 4) && !above)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                npc.netUpdate = true;
                                break;
                            }
                        }
                    }
                }
            }
            return geyserPoses;
        }

        public Vector2[] SpawnProjectileNearPlayerOnTileSide(int dist, int howMany, bool right)
        {
            Vector2[] geyserPoses = new Vector2[howMany];
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[npc.target].position.X / 16;
            int playerTileY = (int)Main.player[npc.target].position.Y / 16;
            int tileX = (int)npc.position.X / 16;
            int tileY = (int)npc.position.Y / 16;
            int teleportCheckCount = 0;
            //player is too far away, don't teleport.
            for (int i = 0; i < howMany; i++)
            {
                bool hasTeleportPoint = false;
                if (Vector2.DistanceSquared(npc.Center, Main.player[npc.target].Center) > (2000f * 2000f))
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
                        if (Framing.GetTileSafely(tpTileX, tpY).nactive())
                        {
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX + 1, tpTileX + 4, tpY - 1, tpY + 1) && right)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                npc.netUpdate = true;
                                break;
                            }
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 4, tpTileX - 1, tpY - 1, tpY + 1) && !right)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                npc.netUpdate = true;
                                break;
                            }
                        }
                    }
                }
            }
            return geyserPoses;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture("NPCs/Bosses/Kraken/KrakenTentacles");
            Main.spriteBatch.Draw(texture, npc.spriteDirection == -1 ? npc.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96) : npc.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96), seperateFrame, drawColor * tentacleAlpha, tentaclerotation, seperateFrame.Size() / 2 + new Vector2(texture.Width / 16, -texture.Height / 96), npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture("NPCs/Bosses/Kraken/KrakenHeadGlowMask");
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override void NPCLoot()
        {
            EEWorld.EEWorld.downedKraken = true;
        }
    }
}
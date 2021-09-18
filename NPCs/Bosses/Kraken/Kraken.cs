using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken
{
    [AutoloadBossHead]
    public class KrakenHead : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kraken");
            Main.npcFrameCount[NPC.type] = 6;
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
                if (frameUpdate >= tentaclesPer && NPC.frame.Y != frameHeight * 2)
                {
                    NPC.frame.Y += frameHeight;
                    frameUpdate = 0;
                }
                if (NPC.frame.Y == frameHeight * 5)
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                if (frameUpdate >= tentaclesPer)
                {
                    NPC.frame.Y += frameHeight;
                    frameUpdate = 0;
                }
                if (NPC.frame.Y == frameHeight * 5)
                {
                    NPC.frame.Y = 0;
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
            NPC.ai[0] = 0;
            numberOfPushes = 0;
            modPlayer.TurnCameraFixationsOff();
            while (NPC.ai[1] == from)
            {
                NPC.ai[1] = Main.rand.Next(1, 6);
            }
            NPC.netUpdate = true;
            NPC.alpha = 0;
            tentacleAlpha = 1;
            GETHIMBOIS = false;
            thrust = false;
            resetAnim = false;
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.lavaImmune = true;
            // NPC.friendly = false;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.defense = 40;
            NPC.damage = 0;
            NPC.value = Item.buyPrice(0, 8, 0, 0);
            NPC.noTileCollide = true;
            NPC.width = 568;
            NPC.height = 472;
            NPC.npcSlots = 24f;
            NPC.knockBackResist = 0f;
            //musicPriority = MusicPriority.BossMedium;
            NPC.alpha = 255;
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
                        int projID = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<InkBlob>(), 50, 0, Main.myPlayer, 0, NPC.whoAmI);
                        Main.projectile[projID].velocity = new Vector2(3f, 0f).RotatedBy(m / (float)pieCut * Math.PI * 2);
                        Main.projectile[projID].netUpdate = true;
                    }
                }
                if ((smolBloons[1] == Vector2.Zero) && (smolBloons[0] != Vector2.Zero))
                {
                    smolBloons[1] = NPC.Center;
                }
                if (smolBloons[0] == Vector2.Zero)
                {
                    smolBloons[0] = NPC.Center;
                }
            }
            if (smolBloons[0] != Vector2.Zero && smolBloons[1] != Vector2.Zero)
            {
                if ((bigBloons[1] == Vector2.Zero) && (bigBloons[0] != Vector2.Zero) && bigBloons[0] != (smolBloons[0] + smolBloons[1]) / 2)
                {
                    bigBloons[1] = (smolBloons[0] + smolBloons[1]) / 2;
                    Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), bigBloons[1], Vector2.Zero, ModContent.ProjectileType<SecondPhaseInkBlob>(), 50, 0, Main.myPlayer, 0, NPC.whoAmI);
                }
                if (bigBloons[0] == Vector2.Zero)
                {
                    bigBloons[0] = (smolBloons[0] + smolBloons[1]) / 2;
                    Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), bigBloons[0], Vector2.Zero, ModContent.ProjectileType<SecondPhaseInkBlob>(), 50, 0, Main.myPlayer, 0, NPC.whoAmI);
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
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (firstFrame)
            {
                geyserPositionsVarUp = new Vector2[howMany];
                geyserPositionsVarDown = new Vector2[howMany];
                geyserPositionsVarRight = new Vector2[howMany];
                geyserPositionsVarLeft = new Vector2[howMany];
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<TentacleEdgeHandler>(), 0, NPC.whoAmI);
                NPC.ai[1] = 1;
                NPC.Center = topLeft;
                firstFrame = false;
                NPC.NewNPC((int)holePositions[0].X, (int)holePositions[0].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[0].X, (int)holePositions[0].Y);
                NPC.NewNPC((int)holePositions[1].X, (int)holePositions[1].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[1].X, (int)holePositions[1].Y, 1);
                NPC.NewNPC((int)holePositions[2].X, (int)holePositions[2].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[2].X, (int)holePositions[2].Y);
                NPC.NewNPC((int)holePositions[3].X, (int)holePositions[3].Y, ModContent.NPCType<KHole>(), 0, (int)holePositions[3].X, (int)holePositions[3].Y, 1);
            }
            NPC.ai[2]++;
            tentaclerotation = 0;
            mouthOpenConsume = false;
            if (NPC.ai[2] < 180)
            {
                tentacleAlpha += 0.01f;
                tentacleAlpha = Helpers.Clamp(tentacleAlpha, 0, 1);
                NPC.alpha -= 2;
                modPlayer.FixateCameraOn(NPC.Center, 32f, false, true, 10);
            }
            else if (NPC.ai[2] == 181)
            {
                modPlayer.TurnCameraFixationsOff();
                NPC.damage = 150;
            }

            Vector2[] geyserPositions = { arenaPosition + new Vector2(-100, 1000), arenaPosition + new Vector2(100, 1000) };
            NPC.rotation = NPC.velocity.X / 80f;

            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }

            switch (NPC.ai[1])
            {
                case 0:
                {
                    break;
                }
                case 1:
                {
                    NPC.ai[0]++;
                    if (isRightOrLeft)
                    {
                        if (!thrust)
                        {
                            variablethrustingPower *= 0.97f;
                        }
                        resetAnim = false;
                        NPC.velocity.X = variablethrustingPower;
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
                        if (NPC.Center.X > topRight.X && variablethrustingPower <= 1f)
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
                        NPC.velocity.X = -variablethrustingPower;
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
                        if (NPC.Center.X < topLeft.X && variablethrustingPower <= 1f)
                        {
                            isRightOrLeft = true;
                        }
                    }
                    break;
                }
                case 2:
                {
                    NPC.ai[0]++;
                    NPC.velocity *= 0.95f;
                    if (NPC.ai[0] == 10)
                    {
                        geyserPositionsVarUp = SpawnProjectileNearPlayerOnTile(40, howMany, true);
                        geyserPositionsVarDown = SpawnProjectileNearPlayerOnTile(40, howMany, false);
                        geyserPositionsVarRight = SpawnProjectileNearPlayerOnTileSide(40, howMany, true);
                        geyserPositionsVarLeft = SpawnProjectileNearPlayerOnTileSide(40, howMany, false);
                    }
                    if (NPC.ai[0] == 30)
                    {
                        for (int i = 0; i < geyserPositionsVarUp.Length; i++)
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarUp[i].X, geyserPositionsVarUp[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 0);
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarDown[i].X, geyserPositionsVarDown[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 1);
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarRight[i].X, geyserPositionsVarRight[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 2);
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarLeft[i].X, geyserPositionsVarLeft[i].Y, 0, 0, ModContent.ProjectileType<KramkenGeyser>(), 1, 0f, Main.myPlayer, 0, 3);
                        }
                    }
                    for (int i = 0; i < geyserPositionsVarUp.Length; i++)
                    {
                        if (NPC.ai[0] > 100)
                        {
                            if (NPC.ai[0] % 4 == 0)
                            {
                                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarUp[i].X, geyserPositionsVarUp[i].Y, Main.rand.NextFloat(-.1f, .1f), Main.rand.NextFloat(-10, -15), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarDown[i].X, geyserPositionsVarDown[i].Y, Main.rand.NextFloat(-.1f, .1f), Main.rand.NextFloat(10, 15), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarRight[i].X, geyserPositionsVarRight[i].Y, Main.rand.NextFloat(10f, 15f), Main.rand.NextFloat(-.1f, .1f), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), geyserPositionsVarLeft[i].X, geyserPositionsVarLeft[i].Y, Main.rand.NextFloat(-10f, -15f), Main.rand.NextFloat(-.1f, .1f), ModContent.ProjectileType<WaterSpew>(), 30, 0f, Main.myPlayer);
                            }
                        }
                    }
                    if (NPC.ai[0] < 80)
                    {
                        modPlayer.FixateCameraOn((geyserPositions[0] + geyserPositions[1]) / 2, 64f, true, false, 10);
                    }
                    else if (NPC.ai[0] == 200)
                    {
                        Reset(2);
                    }
                    break;
                }
                case 3:
                {
                    Vector2 gradient = Vector2.Normalize(arenaPosition - NPC.Center);
                    if (Vector2.DistanceSquared(arenaPosition, NPC.Center) > (280 * 280) && !GETHIMBOIS)
                    {
                        if (!thrust)
                        {
                            variablethrustingPower *= 0.97f;
                        }
                        resetAnim = false;
                        NPC.velocity.X = variablethrustingPower * gradient.X;
                        NPC.velocity.Y = variablethrustingPower * gradient.Y;
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
                        NPC.ai[0]++;

                        NPC.velocity *= 0.98f;
                        if (NPC.ai[0] == 100)
                        {
                            for (int i = 0; i < holePositions.Length; i++)
                            {
                                if (i == 0 || i == 2)
                                {
                                    NPC.NewNPC((int)holePositions[i].X + 120, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, NPC.whoAmI);
                                }

                                if (i == 1 || i == 3)
                                {
                                    NPC.NewNPC((int)holePositions[i].X + 100, (int)holePositions[i].Y + 200, ModContent.NPCType<Tentacle>(), 0, 0, 0, NPC.whoAmI, 1);
                                }
                            }
                        }
                        if (NPC.ai[0] >= 400)
                        {
                            Reset(3);
                        }
                        if (NPC.ai[0] > 10)
                        {
                            resetAnim = true;
                            thrust = true;
                            NPC.velocity.X = (float)Math.Sin(NPC.ai[0] / 20) * 2;
                            NPC.velocity.Y = (float)Math.Cos(NPC.ai[0] / 20) * 2;
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
                        modPlayer.FixateCameraOn(NPC.Center, 64f, false, true, 10);
                        gradient = Vector2.Normalize(yeet - NPC.Center);
                        if (Vector2.DistanceSquared(yeet, NPC.Center) > (180 * 180))
                        {
                            if (!thrust)
                            {
                                variablethrustingPower *= 0.97f;
                            }
                            resetAnim = false;
                            NPC.velocity.X = variablethrustingPower * gradient.X;
                            NPC.velocity.Y = variablethrustingPower * gradient.Y;
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
                            NPC.ai[0]++;
                            NPC.velocity = (yeet - NPC.Center) / 64f;
                            NPC.velocity *= .98f;
                            resetAnim = true;
                            mouthOpenConsume = true;
                            if (isRightOrLeft)
                            {
                                NPC.spriteDirection = 1;
                            }
                            else
                            {
                                NPC.spriteDirection = -1;
                            }
                            if (NPC.ai[0] == 10)
                            {
                                CombatText.NewText(NPC.getRect(), Colors.RarityBlue, "*How the fuck did you fall for that???", false, false);
                            }
                            if (NPC.ai[0] == 80)
                            {
                                CombatText.NewText(NPC.getRect(), Colors.RarityBlue, "Now that you're here", false, false);
                            }
                            if (NPC.ai[0] > 140)
                            {
                                modPlayer.FixateCameraOn(NPC.Center, 64f, true, true, 10);
                                float projectilespeedX = 10 * -NPC.spriteDirection;
                                float projectilespeedY = Main.rand.NextFloat(-2, 2);
                                float projectileknockBack = 4f;
                                int projectiledamage = 20;
                                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), NPC.Center.X + 110 * -NPC.spriteDirection, NPC.Center.Y + 10, projectilespeedX, projectilespeedY, ModContent.ProjectileType<WaterSpew>(), projectiledamage, projectileknockBack, NPC.target, 0f, 1);
                                if (NPC.ai[0] == 280)
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
                    NPC.ai[0]++;
                    NPC.alpha++;
                    tentacleAlpha -= 0.025f;
                    int speed = 50;
                    NPC.velocity.X = (float)Math.Sin(NPC.ai[0] / 10);
                    NPC.velocity.Y = (float)Math.Cos(NPC.ai[0] / 10);
                    if (NPC.alpha > 255)
                    {
                        NPC.alpha = 255;
                    }
                    if (NPC.ai[0] < dashPositions.Length * speed)
                    {
                        if (NPC.ai[0] % speed == 0)
                        {
                            hasChains = true;
                            dashPositions[(int)(NPC.ai[0] / speed)] = player.Center + new Vector2(Main.rand.Next(-7, 7), Main.rand.Next(-7, 7));
                            npcFromPositions[(int)(NPC.ai[0] / speed)] = arenaPosition - new Vector2(Main.rand.Next(-2000, -1000), Main.rand.Next(-2000, -1000));
                        }

                        for (int j = 0; j < 300; j++)
                        {
                            for (int i = 0; i <= (int)(NPC.ai[0] / speed); i++)
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
                    Vector2 gradient = Vector2.Normalize(topRight - NPC.Center);
                    if (Vector2.DistanceSquared(topRight, NPC.Center) > (200 * 200) && !GETHIMBOIS)
                    {
                        if (!thrust)
                        {
                            variablethrustingPower *= 0.97f;
                        }
                        resetAnim = false;
                        NPC.velocity.X = variablethrustingPower * gradient.X;
                        NPC.velocity.Y = variablethrustingPower * gradient.Y;
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
                        NPC.ai[0]++;
                        if (NPC.ai[0] % frequency == 0)
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), player.Center + new Vector2(Main.rand.Next(-1000, 1000), -1000), Vector2.Zero, ModContent.ProjectileType<KramkenRocc>(), 40, 1f);
                        }
                        if (NPC.ai[0] % frequency * 8 <= frequency * 8 / 2)
                        {
                            modPlayer.FixateCameraOn(player.Center, 64f, true, false, 10);
                        }
                        else
                        {
                            modPlayer.TurnCameraFixationsOff();
                        }
                        if (NPC.ai[0] == 400)
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
            int playerTileX = (int)Main.player[NPC.target].position.X / 16;
            int playerTileY = (int)Main.player[NPC.target].position.Y / 16;
            int tileX = (int)NPC.position.X / 16;
            int tileY = (int)NPC.position.Y / 16;
            int teleportCheckCount = 0;
            bool hasTeleportPoint = false;
            //player is too far away, don't teleport.
            if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) > (2000f * 2000f))
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
                    if (!Framing.GetTileSafely(tpTileX, tpY).IsActive)
                    {
                        if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1) && above)
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), tpTileX * 16, tpY * 16, 0, -5, ModContent.ProjectileType<WaterSpew>(), 1, 0f, Main.myPlayer, 0, 0);
                            hasTeleportPoint = true;
                            NPC.netUpdate = true;
                            break;
                        }
                        if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY + 4, tpY + 1) && !above)
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), tpTileX * 16, tpY * 16, 0, 5, ModContent.ProjectileType<WaterSpew>(), 1, 0f, Main.myPlayer, 0, 0);
                            hasTeleportPoint = true;
                            NPC.netUpdate = true;
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
            int playerTileX = (int)Main.player[NPC.target].position.X / 16;
            int playerTileY = (int)Main.player[NPC.target].position.Y / 16;
            int tileX = (int)NPC.position.X / 16;
            int tileY = (int)NPC.position.Y / 16;
            int teleportCheckCount = 0;
            //player is too far away, don't teleport.
            for (int i = 0; i < howMany; i++)
            {
                bool hasTeleportPoint = false;
                if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) > (2000f * 2000f))
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
                        if (!Framing.GetTileSafely(tpTileX, tpY).IsActive)
                        {
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1) && above)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                NPC.netUpdate = true;
                                break;
                            }
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY + 1, tpY + 4) && !above)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                NPC.netUpdate = true;
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
            int playerTileX = (int)Main.player[NPC.target].position.X / 16;
            int playerTileY = (int)Main.player[NPC.target].position.Y / 16;
            int tileX = (int)NPC.position.X / 16;
            int tileY = (int)NPC.position.Y / 16;
            int teleportCheckCount = 0;
            //player is too far away, don't teleport.
            for (int i = 0; i < howMany; i++)
            {
                bool hasTeleportPoint = false;
                if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) > (2000f * 2000f))
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
                        if (!Framing.GetTileSafely(tpTileX, tpY).IsActive)
                        {
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX + 1, tpTileX + 4, tpY - 1, tpY + 1) && right)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                NPC.netUpdate = true;
                                break;
                            }
                            if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 4, tpTileX - 1, tpY - 1, tpY + 1) && !right)
                            {
                                hasTeleportPoint = true;
                                geyserPoses[i] = new Vector2(tpTileX * 16, tpY * 16);
                                NPC.netUpdate = true;
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
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Bosses/Kraken/KrakenTentacles").Value;
            Main.spriteBatch.Draw(texture, NPC.spriteDirection == -1 ? NPC.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96) : NPC.Center - Main.screenPosition + new Vector2(texture.Width / 16, -texture.Height / 96), seperateFrame, drawColor * tentacleAlpha, tentaclerotation, seperateFrame.Size() / 2 + new Vector2(texture.Width / 16, -texture.Height / 96), NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Bosses/Kraken/KrakenHeadGlowMask").Value;
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override void NPCLoot()
        {
            EEWorld.EEWorld.downedKraken = true;
        }
    }
}
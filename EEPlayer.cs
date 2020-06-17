using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles;
using EEMod.Projectiles.OceanMap;
using EEMod.Projectiles.CoralReefs;

namespace EEMod
{
    public class EEPlayer : ModPlayer
    {
        public bool FlameSpirit;
        public bool magmaRune;
        public bool dalantiniumHood;
        public bool hydriteVisage;
        public bool ZoneCoralReefs;
        private int opac;
        public override void UpdateBiomes()
        {
            ZoneCoralReefs = EEWorld.EEWorld.CoralReefsTiles > 200;
            if (ZoneCoralReefs)
            {
                opac++;
                if (opac > 100)
                    opac = 100;
                //Filters.Scene.Activate("EEMod:CR").GetShader().UseOpacity(opac);
            }
            else
            {
                opac--;
                if (opac < 0)
                    opac = 0;
                //	Filters.Scene.Deactivate("EEMod:CR");
            }
        }

        public override bool CustomBiomesMatch(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            return ZoneCoralReefs == modOther.ZoneCoralReefs;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            modOther.ZoneCoralReefs = ZoneCoralReefs;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneCoralReefs;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneCoralReefs = flags[0];
        }

        public static bool godMode = false;
        public bool quartzCrystal = false;
        public bool isQuartzRangedOn = false;
        public bool isQuartzSummonOn = false;
        public bool isQuartzMeleeOn = false;
        public bool isQuartzChestOn = false;
        public int timerForCutscene;
        public static string key1 = "Pyramids";
        public static string key2 = "Sea";
        public static string key3 = "CoralReefs";
        public static string key4 = "Island";
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subText;
        public bool noU;
        public static bool triggerSeaCutscene;
        public static int cutSceneTriggerTimer;
        public static int cutSceneTriggerTimer2;
        public static float cutSceneTriggerTimer3;
        public static int markerPlacer;
        public Vector2 position;
        public Vector2 velocity;
        public static List<Vector2> objectPos = new List<Vector2>();
        public static bool isNearIsland;
        public static bool isNearVolcano;
        public static bool isNearMainIsland;
        public static bool isNearCoralReefs;
        public static string baseWorldName;
        public override void Initialize()
        {
            isSaving = false;
            godMode = false;
            timerForCutscene = 0;
            markerPlacer = 0;
            arrowFlag = false;
            noU = false;
            triggerSeaCutscene = false;
            cutSceneTriggerTimer = 0;
            cutSceneTriggerTimer2 = 500;
            position = player.Center;
            speedOfPan = 0;
            subText = 0;
            EEMod.position = new Vector2(1000, 1000);
            objectPos.Clear();
            EEMod.ShipHelth = EEMod.ShipHelthMax;
        }

        public override void ResetEffects()
        {
            isQuartzChestOn = false;
            isQuartzRangedOn = false;
            isQuartzMeleeOn = false;
            isQuartzSummonOn = false;
            ResetMinionEffect();
            isSaving = false;
        }
        int Arrow;
        int Arrow2;
        int Anchors;
        float positionOfGlacX;
        float positionOfGlacY;
        float speedOfPan = 1;
        int AnchorsVolc;
        int AnchorsMain;
        int AnchorsCoral;

        public override void ModifyScreenPosition()
        {
            base.ModifyScreenPosition();
            if (Main.ActiveWorldFileData.Name == key2)
            {
                Main.screenPosition += new Vector2(0, 1000);
            }

            if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
            {
                Main.screenPosition.X -= cutSceneTriggerTimer;
            }
            if (cutSceneTriggerTimer >= 500)
            {
                speedOfPan += 0.005f;
                Main.screenPosition.X -= cutSceneTriggerTimer2 * speedOfPan;
            }
        }
        readonly SubworldManager SM = new SubworldManager();
        public override void UpdateBiomeVisuals()
        {
            EEMod.isSaving = false;
            string path = $@"{Main.SavePath}\Worlds\CoralReefs.wld";
            Action<string> newWorld = new Action<string>(SubworldManager.EnterSub);
            Action returnToBaseWorld = Return;
            //player.ManageSpecialBiomeVisuals("EEMod:Akumo", NPC.AnyNPCs(ModContent.NPCType<Akumo>()));
            if (triggerSeaCutscene && cutSceneTriggerTimer <= 1000)
            {
                cutSceneTriggerTimer += 6;
                player.position = player.oldPosition;
            }
            if (cutSceneTriggerTimer >= 1000)
            {
                cutSceneTriggerTimer += 2;
            }
            if (godMode)
            {
                timerForCutscene += 20;
            }
            string shad1 = "EEMod:Ripple";
            string shad2 = "EEMod:SunThroughWalls";
            string shad3 = "EEMod:SeaTrans";
            if (Main.ActiveWorldFileData.Name == key1)
            {
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;

                if (titleText <= 0)
                    titleText = 0;

                titleText2 = 1;
                if (!arrowFlag)
                {
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                    arrowFlag = true;
                }
                if (player.Center.X / 16 >= Main.spawnTileX - 5 &&
                    player.Center.X / 16 <= Main.spawnTileX + 5 &&
                    player.Center.Y / 16 >= Main.spawnTileY - 5 &&
                    player.Center.Y / 16 <= Main.spawnTileY + 5)
                {
                    (Main.projectile[Arrow].modProjectile as DesArrowProjectile).visible = true;
                }
                else
                {
                    (Main.projectile[Arrow].modProjectile as DesArrowProjectile).visible = false;
                }
                Filters.Scene.Deactivate(shad2);
            }
            else if (Main.ActiveWorldFileData.Name == key2)
            {
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;
                Filters.Scene[shad2].GetShader().UseOpacity(EEMod.position.X);
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
                {
                    Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
                }
                markerPlacer++;
                float pos1X = Main.screenPosition.X + Main.screenWidth - 900;
                float pos1Y = Main.screenPosition.Y + Main.screenHeight - 100 + 1000;
                float pos2X = Main.screenPosition.X + Main.screenWidth - 400;
                float pos2Y = Main.screenPosition.Y + Main.screenHeight - 400 + 1000;
                float pos3X = Main.screenPosition.X + Main.screenWidth - 700;
                float pos3Y = Main.screenPosition.Y + Main.screenHeight - 300 + 1000;
                float pos4X = Main.screenPosition.X + Main.screenWidth - 500;
                float pos4Y = Main.screenPosition.Y + Main.screenHeight - 200 + 1000;
                float pos5X = Main.screenPosition.X + Main.screenWidth - 1000;
                float pos5Y = Main.screenPosition.Y + Main.screenHeight - 400 + 1000;
                float pos6X = Main.screenPosition.X + Main.screenWidth - 300;
                float pos6Y = Main.screenPosition.Y + Main.screenHeight - 100 + 1000;
                float pos7X = Main.screenPosition.X + Main.screenWidth - 800;
                float pos7Y = Main.screenPosition.Y + Main.screenHeight - 150 + 1000;
                float pos8X = Main.screenPosition.X + Main.screenWidth - 200;
                float pos8Y = Main.screenPosition.Y + Main.screenHeight - 300 + 1000;
                float pos9X = Main.screenPosition.X + Main.screenWidth - 100;
                float pos9Y = Main.screenPosition.Y + Main.screenHeight - 40 + 1000;
                float pos10X = Main.screenPosition.X + Main.screenWidth - 300;
                float pos10Y = Main.screenPosition.Y + Main.screenHeight - 600 + 1000;
                Rectangle rectangle1 = new Rectangle((int)pos3X - 56, (int)pos3Y - 32, 118, 64);
                Rectangle rectangle2 = new Rectangle((int)pos2X - 56, (int)pos2Y - 32, 118, 64);
                Rectangle rectangle3 = new Rectangle((int)pos9X - 115, (int)pos9Y - 49, 330, 98);
                Rectangle rectangle4 = new Rectangle((int)pos10X - 110, (int)pos10Y - 58, 220, 116);
                Rectangle ShipHitBox = new Rectangle((int)Main.screenPosition.X + (int)EEMod.position.X - 30, (int)Main.screenPosition.Y + (int)EEMod.position.Y - 30 + 1000, 60, 60);
                isNearIsland = false;
                isNearVolcano = false;
                isNearMainIsland = false;
                isNearCoralReefs = false;
                if (rectangle1.Intersects(ShipHitBox))
                {
                    isNearIsland = true;
                }
                if (rectangle2.Intersects(ShipHitBox))
                {
                    isNearVolcano = true;
                }
                if (rectangle3.Intersects(ShipHitBox))
                {
                    isNearMainIsland = true;
                }
                if (rectangle4.Intersects(ShipHitBox))
                {
                    isNearCoralReefs = true;
                }
                if (!arrowFlag)
                {
                    Anchors = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos3X, pos3Y);
                    AnchorsVolc = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos2X, pos2Y - 50);
                    AnchorsMain = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos9X, pos9Y - 50);
                    AnchorsCoral = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos10X, pos10Y - 50);
                    arrowFlag = true;
                }
                if (isNearIsland)
                {
                    subText += 0.02f;
                    if (subText >= 1)
                        subText = 1;
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                      //Initialize();
                      SM.SaveAndQuit(key4);
                    }
                }
                else
                {
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = false;
                }
                if (isNearVolcano)
                {
                    subText += 0.02f;
                    if (subText >= 1)
                        subText = 1;
                    (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = true;
                }
                else
                {
                    (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = false;
                }

                if (isNearMainIsland)
                {
                    subText += 0.02f;
                    if (subText >= 1)
                        subText = 1;
                    (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = true;
                }
                else
                {
                    (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = false;
                }
                if (isNearCoralReefs)
                {
                    subText += 0.02f;
                    if (subText >= 1)
                        subText = 1;
                    (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                        SM.SaveAndQuit(key3); // coral reefs
                    }
                }
                else
                {
                    (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = false;
                }
                if (!isNearVolcano && !isNearIsland && !isNearCoralReefs && !isNearMainIsland)
                {
                    subText -= 0.02f;
                    if (subText <= 0)
                        subText = 0;
                }

                for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ModContent.ProjectileType<PirateShip>())
                    {
                        if ((Main.projectile[j].Center - EEMod.position - Main.screenPosition).Length() < 40)
                        {
                            EEMod.ShipHelth -= 20;
                            EEMod.velocity += Main.projectile[j].velocity * 20;
                        }
                    }
                }
                if (markerPlacer == 1)
                {
                    for (int i = 0; i < 400; i++)
                    {
                        int CloudChoose = Main.rand.Next(3);
                        Vector2 CloudPos = new Vector2(Main.rand.NextFloat(Main.screenPosition.X - 200, Main.screenPosition.X + Main.screenWidth), Main.rand.NextFloat(Main.screenPosition.Y + 800, Main.screenPosition.Y + Main.screenHeight + 1000));
                        Vector2 dist = (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight + 1000)) - CloudPos;
                        if (dist.Length() > 1140)
                        {
                            switch (CloudChoose)
                            {
                                case 0:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ModContent.ProjectileType<DarkCloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                                case 1:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ModContent.ProjectileType<DarkCloud2>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                                case 2:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ModContent.ProjectileType<DarkCloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                            }
                        }
                    }
                    //Projectile.NewProjectile(new Vector2(pos3X, pos3X), Vector2.Zero, ModContent.ProjectileType<Land>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos2X, pos2Y), Vector2.Zero, ModContent.ProjectileType<VolcanoIsland>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos3X, pos3Y), Vector2.Zero, ModContent.ProjectileType<Land>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos4X, pos4Y), Vector2.Zero, ModContent.ProjectileType<Lighthouse>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos5X, pos5Y), Vector2.Zero, ModContent.ProjectileType<Lighthouse2>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos6X, pos6Y), Vector2.Zero, ModContent.ProjectileType<Rock1>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos7X, pos7Y), Vector2.Zero, ModContent.ProjectileType<Rock2>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos8X, pos8Y), Vector2.Zero, ModContent.ProjectileType<Rock3>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos9X, pos9Y), Vector2.Zero, ModContent.ProjectileType<MainIsland>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(new Vector2(pos10X, pos10Y), Vector2.Zero, ModContent.ProjectileType<CoralReefsEntrance>(), 0, 0f, Main.myPlayer, 0, 0);
                    objectPos.Add(new Vector2(pos1X, pos1Y));
                    objectPos.Add(new Vector2(pos2X, pos2Y));
                    objectPos.Add(new Vector2(pos3X, pos3Y));
                    objectPos.Add(new Vector2(pos4X, pos4Y));
                    objectPos.Add(new Vector2(pos5X, pos5Y));
                    objectPos.Add(new Vector2(pos6X, pos6Y));
                    objectPos.Add(new Vector2(pos7X, pos7Y));
                    objectPos.Add(new Vector2(pos8X, pos8Y));
                    objectPos.Add(new Vector2(pos9X, pos9Y));
                    objectPos.Add(new Vector2(pos10X, pos10Y));
                    //upgrade, pirates, radial
                    for (int i = 0; i < 2; i++)
                    {
                        int GlacierChoose = Main.rand.Next(3);
                        float positionOfGlacXLast = positionOfGlacX;
                        float positionOfGlacYLast = positionOfGlacY;
                        positionOfGlacX = Main.rand.NextFloat(Main.screenPosition.X, Main.screenPosition.X + Main.screenWidth);
                        positionOfGlacY = Main.rand.NextFloat(Main.screenPosition.Y + 1000, Main.screenPosition.Y + Main.screenHeight + 1000);
                        Vector2 dist = new Vector2(positionOfGlacY - positionOfGlacYLast, positionOfGlacXLast - positionOfGlacX);
                        if (dist.Length() > 300)
                        {
                            switch (GlacierChoose)
                            {
                                case 0:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ModContent.ProjectileType<Glacier>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                                case 1:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ModContent.ProjectileType<Glacier2>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                                case 2:
                                    {
                                        //  Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ModContent.ProjectileType<Glacier3>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                            }
                        }
                    }
                }
                player.position = player.oldPosition;
                player.invis = true;
                player.AddBuff(BuffID.Cursed, 100000);
                if (markerPlacer % 600 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                }
                if (markerPlacer % 20 == 0)
                {
                    int CloudChoose = Main.rand.Next(5);
                    switch (CloudChoose)
                    {
                        case 0:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<Cloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 1:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<Cloud6>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 2:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<Cloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 3:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 4:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ModContent.ProjectileType<Cloud5>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                    }
                }

                if (markerPlacer % 40 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + EEMod.position, Vector2.Zero, ModContent.ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                }
            }
            else if (Main.ActiveWorldFileData.Name == key3)
            {
                player.ClearBuff(BuffID.Cursed);
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;
                if (titleText <= 0)
                    titleText = 0;
                markerPlacer++;
                if (markerPlacer % 40 == 0)
                {
                    int CloudChoose = Main.rand.Next(5);
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ModContent.ProjectileType<CoralBubble>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f,0.5f), Main.rand.Next(100, 180));
                }
            }
            else if (Main.ActiveWorldFileData.Name == key4)
            {
            player.ClearBuff(BuffID.Cursed);
            }
            else
            {
                baseWorldName = Main.ActiveWorldFileData.Name;
                Filters.Scene.Deactivate(shad2);
                EEMod.position = EEMod.start;
                EEMod.velocity = Vector2.Zero;
                titleText2 = 0;
                if (!arrowFlag)
                {
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<OceanArrowProjectile>(), 0, 0, player.whoAmI);
                    arrowFlag = true;
                }
                if (EEWorld.EEWorld.EntracesPosses.Count > 0)
                {
                    DesArrowProjectile arrow = Main.projectile[Arrow].modProjectile as DesArrowProjectile;
                    if (player.Center.X / 16 >= (EEWorld.EEWorld.EntracesPosses[0].X + 12) - 2 &&
                        player.Center.X / 16 <= (EEWorld.EEWorld.EntracesPosses[0].X + 12) + 2 &&
                        player.Center.Y / 16 >= (EEWorld.EEWorld.EntracesPosses[0].Y + 7) - 2 &&
                        player.Center.Y / 16 <= (EEWorld.EEWorld.EntracesPosses[0].Y + 7) + 2 &&
                            EEWorld.EEWorld.EntracesPosses.Count > 0)
                    {
                        if (player.controlUp)
                        {
                            godMode = true;
                        }
                        arrow.visible = true;
                    }
                    else
                    {
                        arrow.visible = false;
                    }
                }
                OceanArrowProjectile oceanarrow = Main.projectile[Arrow2].modProjectile as OceanArrowProjectile;
                if (player.Center.X / 16 >= (EEWorld.EEWorld.ree.X + 2) - 2 &&
                    player.Center.X / 16 <= (EEWorld.EEWorld.ree.X + 2) + 2 &&
                    player.Center.Y / 16 >= (EEWorld.EEWorld.ree.Y + 14) - 2 &&
                    player.Center.Y / 16 <= (EEWorld.EEWorld.ree.Y + 14) + 2 &&
                    EEWorld.EEWorld.shipComplete == true)
                {
                    if (player.controlUp)
                    {
                        triggerSeaCutscene = true;
                    }
                    oceanarrow.visible = true;

                }
                else
                {
                    oceanarrow.visible = false;
                }
            }
            Filters.Scene[shad1].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad1].IsActive())
            {
                Filters.Scene.Activate(shad1, player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                Filters.Scene.Deactivate(shad1);
            }
            Filters.Scene[shad3].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad3].IsActive())
            {
                Filters.Scene.Activate(shad3, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                Filters.Scene.Deactivate(shad3);
            }
            if (timerForCutscene >= 1400)
            {
                Initialize();
                SM.SaveAndQuit(key1); //pyramid
            }
            if (cutSceneTriggerTimer >= 500)
            {
                cutSceneTriggerTimer2 -= 5;
                if (cutSceneTriggerTimer >= 1520)
                {
                    Initialize();
                    SM.SaveAndQuit(key2); //sea
                }
            }
        }
        internal static void PreSaveAndQuit()
        {

            Mod[] mods = ModLoader.Mods;
            for (int i = 0; i < mods.Length; i++)
            {
                mods[i].PreSaveAndQuit();
            }
        }
        public void Return()
        {
            GenerationProgress progress = new GenerationProgress();
            ReturnOnName(baseWorldName, progress);
        }
        private void ReturnOnName(string text, GenerationProgress progress)
        {
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"C:\Users\{Environment.UserName}\Documents\My Games\Terraria\ModLoader\Worlds\{text}.wld", false);
            WorldGen.playWorld();
        }
        public static void SaveAndQuit(Action<string> callback = null)
        {
            Main.PlaySound(SoundID.MenuClose);
            PreSaveAndQuit();
            ThreadPool.QueueUserWorkItem(SaveAndQuitCallBack, callback);
        }
        public static void SaveAndQuitCallBack(object threadContext)
        {
            EEMod.isSaving = true;
            try
            {
                Main.PlaySound(SoundID.Waterfall, -1, -1, 0);
                Main.PlaySound(SoundID.Lavafall, -1, -1, 0);
            }
            catch
            {
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.CacheSaveTime();
            }
            Main.invasionProgress = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0f;
            Main.menuMode = 10;
            Main.gameMenu = true;
            Main.StopTrackedSounds();
            Terraria.Graphics.Capture.CaptureInterface.ResetFocus();
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.saveWorld();
                Main.PlaySound(SoundID.MenuOpen);
            }
            else
            {
                Netplay.disconnect = true;
                Main.netMode = NetmodeID.SinglePlayer;
            }
            Main.fastForwardTime = false;
            Main.UpdateSundial();
            Main.menuMode = 0;
            if (threadContext != null)
            {
                ((Action)threadContext)();
            }
        }





        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {

            if (godMode)
            {

                int getRand = Main.rand.Next(5);
                int healSet = damage / 9;
                if (healSet > 5)
                {
                    healSet = 5;
                }
                if (healSet < 1)
                {
                    healSet = 1;
                }

                if (getRand == 1)
                {
                    player.statLife += healSet;
                    player.HealEffect(healSet);
                }
            }
            if (isQuartzRangedOn && item.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && item.summon)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

            if (isQuartzRangedOn && proj.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && proj.minion)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }
        private void ResetMinionEffect()
        {
            quartzCrystal = false;
        }
    }
}

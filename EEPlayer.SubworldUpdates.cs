using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Friendly;
using EEMod.Projectiles;
using EEMod.Projectiles.OceanMap;
using EEMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        private readonly List<float> _bubbleRoots = new List<float>();

        public List<BubbleClass> bubbles = new List<BubbleClass>();
        public List<SeagullsClass> seagulls = new List<SeagullsClass>();
        public float brightness;
        public float quickOpeningFloat = 5f;

        public void UpdatePyramids()
        {
            if (noU)
            {
                titleText -= 0.005f;
            }
            else
            {
                titleText += 0.005f;
            }

            if (titleText >= 1)
            {
                noU = true;
            }
            else
            {
                titleText = 0;
            }

            titleText2 = 1;

            if (!arrowFlag)
            {
                Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                arrowFlag = true;
            }

            DesArrowProjectile desArrowProj = (DesArrowProjectile)Main.projectile[Arrow].modProjectile;

            if (player.Center.X / 16 >= Main.spawnTileX - 5 && player.Center.X / 16 <= Main.spawnTileX + 5 && player.Center.Y / 16 >= Main.spawnTileY - 5 && player.Center.Y / 16 <= Main.spawnTileY + 5)
            {
                if (player.controlUp)
                {
                    ReturnHome();
                }

                desArrowProj.visible = true;
            }
            else
            {
                desArrowProj.visible = false;
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Deactivate(shad2);
            }
        }

        public void UpdateSea()
        {
            if (Main.dayTime)
            {
                if (Main.time <= 200)
                {
                    brightness += 0.0025f;
                }

                if (Main.time >= 52000 && brightness > 0.1f)
                {
                    brightness -= 0.0025f;
                }

                if (Main.time > 2000 && Main.time < 52000)
                {
                    brightness = 0.5f;
                }
            }
            else
            {
                brightness = 0.1f;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBrightness, brightness);
            }

            if (quickOpeningFloat > 0.01f)
            {
                quickOpeningFloat -= quickOpeningFloat / 20f;
            }
            else
            {
                quickOpeningFloat = 0;
            }

            Filters.Scene["EEMod:SeaOpening"].GetShader().UseIntensity(quickOpeningFloat);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:SeaOpening"].IsActive())
            {
                Filters.Scene.Activate("EEMod:SeaOpening", player.Center).GetShader().UseIntensity(quickOpeningFloat);
            }

            if (noU)
            {
                titleText -= 0.005f;
            }
            else
            {
                titleText += 0.005f;
            }

            if (titleText >= 1)
            {
                noU = true;
            }
            Filters.Scene[shad2].GetShader().UseOpacity(EEMod.instance.position.X);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }

            markerPlacer++;

            if (markerPlacer == 1)
            {
                SeaObject.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/Projectiles/OceanMap/Land")));
                SeaObject.Add(new Island(new Vector2(-400, -400), GetTexture("EEMod/Projectiles/OceanMap/VolcanoIsland"), true));
                SeaObject.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));
                SeaObject.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse")));
                SeaObject.Add(new Island(new Vector2(-1000, -400), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse2")));
                SeaObject.Add(new Island(new Vector2(-300, -100), GetTexture("EEMod/Projectiles/OceanMap/Rock1")));
                SeaObject.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/Projectiles/OceanMap/Rock2")));
                SeaObject.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/Projectiles/OceanMap/Rock3")));
                SeaObject.Add(new Island(new Vector2(-100, -40), GetTexture("EEMod/Projectiles/OceanMap/MainIsland"), true));
                SeaObject.Add(new Island(new Vector2(-300, -600), GetTexture("EEMod/Projectiles/OceanMap/CoralReefsEntrance"), true));
                SeaObject.Add(new Island(new Vector2(-600, -800), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));
                SeaObject.Add(new Island(new Vector2(-300, -250), GetTexture("EEMod/Projectiles/OceanMap/Rock2")));

                if (!Islands.ContainsKey("VolcanoIsland"))
                {
                    Islands.Add("VolcanoIsland", SeaObject[1]);
                }

                if (!Islands.ContainsKey("Island"))
                {
                    Islands.Add("Island", SeaObject[2]);
                }

                if (!Islands.ContainsKey("Lighthouse"))
                {
                    Islands.Add("Lighthouse", SeaObject[3]);
                }

                if (!Islands.ContainsKey("Lighthouse2"))
                {
                    Islands.Add("Lighthouse2", SeaObject[4]);
                }

                if (!Islands.ContainsKey("MainIsland"))
                {
                    Islands.Add("MainIsland", SeaObject[8]);
                }

                if (!Islands.ContainsKey("CoralReefsEntrance"))
                {
                    Islands.Add("CoralReefsEntrance", SeaObject[9]);
                }

                if (!Islands.ContainsKey("UpperLand"))
                {
                    Islands.Add("UpperLand", SeaObject[10]);
                }

                for (int i = 0; i < 400; i++)
                {
                    int CloudChoose = Main.rand.Next(3);
                    Vector2 CloudPos = new Vector2(Main.rand.NextFloat(-200, Main.screenWidth), Main.rand.NextFloat(800, Main.screenHeight + 1000));
                    Vector2 dist = new Vector2(Main.screenWidth, Main.screenHeight + 1000) - CloudPos;

                    if (dist.Length() > 1140)
                    {
                        Texture2D cloudTexture;

                        switch (CloudChoose)
                        {
                            case 0:
                            case 1:
                                cloudTexture = GetTexture("EEMod/Projectiles/OceanMap/DarkCloud" + (CloudChoose + 1));
                                break;

                            default:
                                cloudTexture = GetTexture("EEMod/Projectiles/OceanMap/DarkCloud3");
                                break;
                        }

                        OceanMapElements.Add(new DarkCloud(CloudPos, cloudTexture, Main.rand.NextFloat(.6f, 1), Main.rand.NextFloat(60, 180)));
                    }
                }

                for (int i = 0; i < SeaObject.Count; i++)
                {
                    objectPos.Add(SeaObject[i].posToScreen);
                }

                //upgrade, pirates, radial
            }
            if (EEMod.ShipHelth <= 0)
            {
                if (prevKey == baseWorldName || prevKey == "Main")
                {
                    ReturnHome();
                }
                else
                {
                    Initialize();

                    arrowFlag = false;

                    SM.SaveAndQuit(prevKey);
                }
            }
            if (markerPlacer > 10)
            {
                if (Islands["VolcanoIsland"].isColliding)
                {
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoIsland);

                        prevKey = KeyID.VolcanoIsland;
                    }
                }
                else if (Islands["Island"].isColliding)
                {
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island);

                        prevKey = KeyID.Island;
                    }
                }
                else if (Islands["MainIsland"].isColliding)
                {
                    if (player.controlUp)
                    {
                        ReturnHome();

                        prevKey = baseWorldName;
                    }
                }
                else if (Islands["CoralReefsEntrance"].isColliding)
                {
                    if (player.controlUp)
                    {
                        importantCutscene = true;
                    }
                }
                else if (Islands["UpperLand"].isColliding)
                {
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island2);

                        prevKey = KeyID.Island2;
                    }
                }
                else
                {
                    subTextAlpha -= 0.02f;

                    if (subTextAlpha <= 0)
                    {
                        subTextAlpha = 0;
                    }
                }
            }
            if (!arrowFlag)
            {
                arrowFlag = true;
            }

            foreach (Island island in Islands.Values)
            {
                if (island.isColliding)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                    {
                        subTextAlpha = 1;
                    }
                }
            }

            if (importantCutscene)
            {
                EEMod.Noise2D.Parameters["noiseTexture"].SetValue(TextureCache.Noise);
                Filters.Scene["EEMod:Noise2D"].GetShader().UseOpacity(cutSceneTriggerTimer / 180f);

                if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Noise2D"].IsActive())
                {
                    Filters.Scene.Activate("EEMod:Noise2D", player.Center).GetShader().UseOpacity(0);
                }

                cutSceneTriggerTimer++;

                if (cutSceneTriggerTimer > 180)
                {
                    Initialize();
                    Filters.Scene.Deactivate("EEMod:Noise2D");
                    SM.SaveAndQuit(KeyID.CoralReefs); // coral reefs

                    prevKey = KeyID.CoralReefs;
                }
            }


            for (int j = 0; j < 450; j++)
            {
                if (Main.projectile[j].type == ProjectileType<PirateShip>() || Main.projectile[j].type == ProjectileType<RedDutchman>() || Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                {
                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && Main.projectile[j].type != ProjectileType<EnemyCannonball>())
                    {
                        EEMod.ShipHelth -= 1;
                        EEMod.instance.velocity += Main.projectile[j].velocity * 20;
                    }

                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 30 && Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                    {
                        EEMod.ShipHelth -= 1;
                        EEMod.instance.velocity += Main.projectile[j].velocity;
                    }
                }
                if (Main.projectile[j].type == ProjectileType<Crate>())
                {
                    Crate a = (Crate)Main.projectile[j].modProjectile;

                    if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && !a.sinking)
                    {
                        //Crate loot tables go here
                        if (Main.rand.NextBool())
                        {
                            player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(4, 9));
                        }
                        else
                        {
                            player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(4, 9));
                        }

                        if (Main.rand.NextBool())
                        {
                            player.QuickSpawnItem(ItemID.ApprenticeBait, Main.rand.Next(2, 4));
                        }
                        else
                        {
                            player.QuickSpawnItem(ItemID.JourneymanBait, 1);
                        }

                        player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(0, 2));
                        player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(0, 100));
                        player.QuickSpawnItem(ItemID.CopperCoin, Main.rand.Next(0, 100));

                        a.sinking = true;

                        a.Sink();
                    }
                }
            }

            player.position = player.oldPosition;
            player.invis = true;

            player.AddBuff(BuffID.Cursed, 100000);

            if (markerPlacer % 600 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (markerPlacer % 2400 == 0)
            {
                NPC.NewNPC((int)Main.screenPosition.X + Main.screenWidth - 200, (int)Main.screenPosition.Y + Main.rand.Next(1000), NPCType<MerchantBoat>());
            }

            if (markerPlacer % 7200 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<RedDutchman>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (markerPlacer % 800 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Crate>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (markerPlacer % 150 == 0)
            {
                if (seagulls.Count < 500)
                {
                    GraphicObject.LazyAppendInBoids(ref seagulls, 5);
                }
                else
                {
                    seagulls.RemoveAt(0);
                }
            }

            if (markerPlacer % 20 == 0)
            {
                int CloudChoose = Main.rand.Next(5);
                IOceanMapElement cloud;

                switch (CloudChoose)
                {
                    case 0:
                    {
                        // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 1:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Projectiles/OceanMap/Cloud6"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 144, 42, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud6>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 2:
                    {
                        // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 3:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Projectiles/OceanMap/Cloud4"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 100, 48, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                    case 4:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Projectiles/OceanMap/Cloud5"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 96, 36, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        OceanMapElements.Add(cloud);
                        //Projectile.NewProjectile(new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud5>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));

                        break;
                    }
                }
            }

            if (markerPlacer % 40 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + EEMod.instance.position, Vector2.Zero, ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.instance.velocity.X, EEMod.instance.velocity.Y);
            }
        }

        public void UpdateCR()
        {
            if (player.position.Y >= 800 * 16 && !player.accDivingHelm)
            {
                player.AddBuff(BuffType<WaterPressure>(), 60);
            }

            if (HydrosCheck())
            {
                NPC.NewNPC((int)position.X * 16, (int)position.Y * 16 - 400, NPCType<Hydros>());

                EEWorld.EEWorld.instance.minionsKilled = 0;
            }

            if (noU)
            {
                titleText -= 0.005f;
            }
            else
            {
                titleText += 0.005f;
            }

            if (titleText >= 1)
            {
                noU = true;
            }

            if (titleText <= 0)
            {
                titleText = 0;
            }

            markerPlacer++;

            if (markerPlacer % 80 == 0)
            {
                _bubbleRoots.Add(Main.rand.Next(Main.screenWidth) + player.Center.X - Main.screenWidth / 2);
            }

            if (markerPlacer % 120 == 0)
            {
                for (int i = 0; i < _bubbleRoots.Count; i++)
                {
                    Vector2 BubblePos = new Vector2(_bubbleRoots[i], player.Center.Y + 600);
                    BubbleClass bubble = new BubbleClass
                    {
                        scale = Main.rand.NextFloat(0.5f, 1f),
                        alpha = Main.rand.NextFloat(.2f, .8f),
                        Position = BubblePos,
                        flash = Main.rand.NextFloat(0, 100),
                        Velocity = new Vector2(Main.rand.NextFloat(0.5f, 1), 0)
                    };

                    if (bubbles.Count < 500)
                    {
                        bubbles.Add(bubble);
                    }
                }

                if (bubbles.Count > 500)
                {
                    bubbles.RemoveAt(0);
                }

                //Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<CoralBubble>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
            }

            foreach (BubbleClass bubble in bubbles)
            {
                bubble.flash++;
                bubble.Position -= new Vector2((float)Math.Sin(bubble.flash / (bubble.Velocity.X * 30)), bubble.Velocity.X);
            }

            if (!arrowFlag)
            {
                for (int i = 0; i < EESubWorlds.OrbPositions.Count; i++)
                {
                    NPC.NewNPC((int)EESubWorlds.OrbPositions[i].X * 16, (int)EESubWorlds.OrbPositions[i].Y * 16, NPCType<SpikyOrb>());
                }

                Main.NewText(EESubWorlds.OrbPositions.Count);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
                }

                player.ClearBuff(BuffID.Cursed);
                arrowFlag = true;
            }

            if (EESubWorlds.CoralBoatPos == Vector2.Zero)
            {
                EESubWorlds.CoralBoatPos = new Vector2(200, 48);
            }

            try
            {
                Projectile oceanarrow = Main.projectile[Arrow2];

                if (Helpers.PointInRectangle(player.Center / 16, EESubWorlds.CoralBoatPos.X, EESubWorlds.CoralBoatPos.Y + 12, 4, 4))
                {
                    if (player.controlUp)
                    {
                        Initialize();

                        EEMod.instance.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);

                        SM.SaveAndQuit(KeyID.Sea);
                    }

                    oceanarrow.ai[1] = 1;
                }
                else
                {
                    oceanarrow.ai[1] = 0;
                }
            }
            catch
            {
            }
        }

        public void UpdateIsland()
        {
            player.ClearBuff(BuffID.Cursed);

            if (!arrowFlag)
            {
                NPC.NewNPC(Main.maxTilesX / 2 * 16, 75 * 16, NPCType<AtlantisCore>());

                arrowFlag = true;
            }
        }

        public void UpdateVolcano()
        {
            firstFrameVolcano = true;

            player.ClearBuff(BuffID.Cursed);

            if (!arrowFlag)
            {
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                NPC.NewNPC(600 * 16, 594 * 16, NPCType<VolcanoSmoke>());

                arrowFlag = true;
            }

            if (SubWorldSpecificVolcanoInsidePos == Vector2.Zero)
            {
                SubWorldSpecificVolcanoInsidePos = new Vector2(198, 198);
            }

            VolcanoArrowProj voclanoarrow = (VolcanoArrowProj)Main.projectile[Arrow2].modProjectile;

            if (Helpers.PointInRectangle(player.Center / 16, SubWorldSpecificVolcanoInsidePos.X - 4, SubWorldSpecificVolcanoInsidePos.Y - 4, 8, 8))
            {
                if (player.controlUp)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.VolcanoInside);
                }

                voclanoarrow.visible = true;
            }
            else
            {
                voclanoarrow.visible = false;
            }
        }

        public void UpdateInnerVolcano()
        {
            player.ClearBuff(BuffID.Cursed);

            if (firstFrameVolcano)
            {
                NPC.NewNPC(200, TileCheck(200, TileType<MagmastoneTile>()), NPCType<Akumo>());

                firstFrameVolcano = false;
            }
        }

        public void UpdateCutscene()
        {
            markerPlacer++;

            if (markerPlacer == 5)
            {
                player.AddBuff(BuffID.Cursed, 100000);
                NPC.NewNPC(193 * 16, (120 - 30) * 16, NPCType<SansSlime>());
                NPC.NewNPC(207 * 16, (120 - 30) * 16, NPCType<GreenSlimeGoBurr>());
            }

            if (markerPlacer > 120 * 8)
            {
                if (markerPlacer == 5 && EEModConfigClient.Instance.ParticleEffects)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                }
            }
        }

        public void UpdateWorld()
        {
            /*object tempLights = typeof(Lighting).GetField("tempLights", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            try
            {
                Main.NewText(tempLights.get);
                //Point16[] arrayOfAllKeys = buffer.Keys.ToArray();
                // foreach (Point16 p in arrayOfAllKeys)
                // {
                //    Main.NewText(p);
                // }
            }
            catch
            {
                Main.NewText("few");
            }*/
            if (missingShipTiles != null)
            {
                int lastNoOfShipTiles = missingShipTiles.Count;

                try
                {
                    int DefShipPosX = 100;
                    int DefShipPosY = TileCheckWater(100) - 22;

                    if (Main.LocalPlayer.Center.X < (DefShipPosX + ShipTiles.GetLength(1)) * 16 && Main.LocalPlayer.Center.Y < (DefShipPosY + ShipTiles.GetLength(0)) * 16)
                    {
                        ShipComplete();
                    }
                }
                catch
                {
                }

                if (missingShipTiles.Count != lastNoOfShipTiles)
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    foreach (Vector2 tile in missingShipTiles)
                    {
                        int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8 + (-3 * 16), 8 + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                        WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                        newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                    }
                }
            }
            if (EEModConfigClient.Instance.ParticleEffects)
            {
                markerPlacer++;
            }
            else
            {
                markerPlacer = 0;
            }
            if (markerPlacer == 10 && EEModConfigClient.Instance.ParticleEffects)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), player.whoAmI);
            }
            baseWorldName = Main.ActiveWorldFileData.Name;
            if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Deactivate(shad2);
            }
            EEMod.instance.position = EEMod.start;
            EEMod.instance.velocity = Vector2.Zero;
            titleText2 = 0;
            if (!arrowFlag)
            {
                if (EEModConfigClient.Instance.BetterLighting)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BetterLighting>(), 0, 0f, Main.myPlayer, 0, player.whoAmI);
                }

                player.ClearBuff(BuffID.Cursed);

                Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, player.whoAmI);

                arrowFlag = true;

                for (int i = 0; i < 200; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                    {
                        Main.projectile[i].Kill();
                    }
                }

                foreach (Vector2 tile in missingShipTiles)
                {
                    int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8 + (-3 * 16), 8 + (-6 * 16)), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                    WhiteBlock newProj = (WhiteBlock)Main.projectile[proj].modProjectile;

                    newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                }
            }
            /* if (EntracesPosses.Count > 0)
             {
                 if (Main.projectile[Arrow].modProjectile is DesArrowProjectile arrow)
                 {
                     Vector2 entrace = EntracesPosses[0];
                     if (Helpers.PointInRectangle(player.Center / 16, entrace.X + 10, entrace.Y + 5, 4, 4))
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
             }*/

            Vector2 revisedRee = new Vector2(ree.X == 0 ? 100 : ree.X,
                                             ree.Y == 0 ? TileCheckWater(100) - 22 : ree.Y);
            if (Helpers.PointInRectangle(player.Center / 16, revisedRee.X, revisedRee.Y + 12, 4, 4) && shipComplete)
            {
                if (player.controlUp)
                {
                    triggerSeaCutscene = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = mod.GetPacket();
                        netMessage.Write(triggerSeaCutscene);
                        netMessage.Send();
                    }
                }

                Main.projectile[Arrow2].ai[1] = 1;
            }
            else
            {
                Main.projectile[Arrow2].ai[1] = 0;
            }
        }
    }
}
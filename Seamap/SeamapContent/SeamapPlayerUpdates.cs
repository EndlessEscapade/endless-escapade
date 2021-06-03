using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles;
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
using ReLogic.Graphics;
using EEMod.Seamap.SeamapAssets;
using EEMod.Seamap.SeamapContent;
using EEMod.Autoloading;
using EEMod.Systems.Subworlds.EESubworlds;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public List<SeagullsClass> seagulls = new List<SeagullsClass>();
        public float brightness;
        public bool isStorming;

        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public float cutSceneTriggerTimer3;
        public int coralReefTrans;
        public int seamapUpdateCount;
        public Vector2 position;
        public Vector2 velocity;

        public string baseWorldName;


        public void ReturnHome()
        {
            Initialize();
            SM.Return(KeyID.BaseWorldName);
        }

        public void UpdateSea()
        {
            #region Controlling island brightness
            if (Main.dayTime)
            {
                if (Main.time <= 200)
                    brightness += 0.0025f;

                if (Main.time >= 52000 && brightness > 0.1f)
                    brightness -= 0.0025f;

                if (Main.time > 2000 && Main.time < 52000)
                    brightness = 0.5f;
            }
            else
            {
                brightness = 0.1f;
            }

            if (Main.time % 1000 == 0)
            {
                if (Main.rand.NextBool(10)) isStorming = !isStorming;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                EENet.SendPacket(EEMessageType.SyncBrightness, brightness);
            }
            #endregion

            #region Opening cutscene for seamap
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

            Filters.Scene[shad2].GetShader().UseOpacity(SeamapPlayerShip.localship.position.X);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
            {
                Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            #endregion

            seamapUpdateCount++;

            #region Placing SeamapObjects.Islands
            if (seamapUpdateCount == 1)
            {
                InitializeSeamap();
                //upgrade, pirates, radial
            }
            #endregion

            #region If ship crashes
            if (SeamapPlayerShip.localship.shipHelth <= 0)
            {
                if (prevKey == baseWorldName || prevKey == "Main")
                {
                    ReturnHome();
                }
                else
                {
                    Initialize();

                    arrowFlag = false;

                    SM.Return(prevKey);
                }
            }
            #endregion

            #region Warping to islands
            if (!arrowFlag)
            {
                arrowFlag = true;
            }

            bool isCollidingWithAnyIsland = false;
            foreach (var island in SeamapObjects.IslandEntities)
            {
                if (island.isCollidingWithPlayer)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                    {
                        subTextAlpha = 1;
                    }

                    if (EEMod.Inspect.JustPressed)
                    {
                        switch (island.id)
                        {
                            case IslandID.TropicalIsland1:
                                importantCutscene = true;
                                break;

                            case IslandID.TropicalIsland2:
                                importantCutscene = true;
                                break;

                            case IslandID.VolcanoIsland:
                                importantCutscene = true;
                                break;

                            case IslandID.MoyaiIsland:
                                importantCutscene = true;
                                break;

                            case IslandID.CoralReefs:
                                importantCutscene = true;
                                break;

                            case IslandID.MainIsland:
                                ReturnHome();
                                break;
                        }

                        prevKey = KeyID.Sea;
                    }

                    isCollidingWithAnyIsland = true;

                    player.ClearBuff(BuffID.Cursed);
                    player.ClearBuff(BuffID.Invisibility);
                }
            }

            if (!isCollidingWithAnyIsland)
            {
                subTextAlpha -= 0.02f;

                if (subTextAlpha <= 0)
                {
                    subTextAlpha = 0;
                }
            }
            #endregion

            #region Warp cutscene
            if (importantCutscene)
            {
                EEMod.Noise2D.Parameters["noiseTexture"].SetValue(ModContent.GetTexture("EEMod/Noise/noise"));
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
                    SubworldManager.EnterSubworld<CoralReefs>(); // coral reefs
                }
            }
            #endregion

            //THIS NEEDS CHANGING
            #region Updating entities
            for (int j = 0; j < 400; j++)
            {
                if (Main.projectile[j].type == ProjectileType<RedDutchman>() || Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                {
                    if ((Main.projectile[j].Center - SeamapPlayerShip.localship.position.ForDraw()).Length() < 40 && Main.projectile[j].type != ProjectileType<EnemyCannonball>())
                    {
                        SeamapPlayerShip.localship.shipHelth -= 1;
                        SeamapPlayerShip.localship.velocity += Main.projectile[j].velocity * 20;
                    }

                    if ((Main.projectile[j].Center - SeamapPlayerShip.localship.position.ForDraw()).Length() < 30 && Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                    {
                        SeamapPlayerShip.localship.shipHelth -= 1;
                        SeamapPlayerShip.localship.velocity += Main.projectile[j].velocity;
                    }
                }
            }
            #endregion

            #region Hiding the player
            player.position = player.oldPosition;
            player.invis = true;

            player.AddBuff(BuffID.Cursed, 100000);
            #endregion

            //THIS NEEDS CHANGING
            #region Spawning entities
            if (seamapUpdateCount % 200 == 0)
            {
                SeamapObjects.NewSeamapObject(new PirateShip(new Vector2(1000, 1000), Vector2.One));
            }

            if (seamapUpdateCount % 2400 == 0)
            {
                NPC.NewNPC((int)Main.screenPosition.X + Main.screenWidth - 200, (int)Main.screenPosition.Y + Main.rand.Next(1000), NPCType<MerchantBoat>());
            }

            if (seamapUpdateCount % 7200 == 0)
            {
                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<RedDutchman>(), 0, 0f, Main.myPlayer, 0, 0);
            }

            if (seamapUpdateCount % 200 == 0)
            {
                SeamapObjects.NewSeamapObject(new Crate(new Vector2(500, 550) * 4, new Vector2(0.5f, 0)));
            }

            if (seamapUpdateCount % 200 == 0)
            {
                if (seagulls.Count < 500)
                {
                    GraphicObject.LazyAppendInBoids(ref seagulls, Main.rand.Next(4, 8));
                }
                else
                {
                    seagulls.RemoveAt(0);
                }
            }
            #endregion

            //THIS NEEDS CHANGING
            #region Spawning clouds
            if (seamapUpdateCount % 20 == 0)
            {
                int CloudChoose = Main.rand.Next(5);
                ISeamapEntity cloud;

                switch (CloudChoose)
                {
                    case 0:
                    {
                        break;
                    }
                    case 1:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud6"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 144, 42, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        SeamapObjects.OceanMapElements.Add(cloud);
                        break;
                    }
                    case 2:
                    {
                        break;
                    }
                    case 3:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud4"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 100, 48, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        SeamapObjects.OceanMapElements.Add(cloud);
                        break;
                    }
                    case 4:
                    {
                        cloud = new MCloud(GetTexture("EEMod/Seamap/SeamapAssets/Cloud5"), new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), 96, 36, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                        SeamapObjects.OceanMapElements.Add(cloud);
                        break;
                    }
                }
            }
            #endregion

            #region Moving screen position

            player.position = player.oldPosition;

            SeamapPlayerShip.localship.ModifyScreenPosition(ref Main.screenPosition);
            #endregion
        }

        internal static void UpdateOceanMapElements()
        {
            for (int i = 0; i < SeamapObjects.OceanMapElements.Count; i++)
            {
                SeamapObjects.OceanMapElements[i].Update();
            }

            for (int i = 0; i < SeamapObjects.SeamapEntities.Length; i++)
            {
                if (SeamapObjects.SeamapEntities[i] != null)
                {
                    SeamapObjects.SeamapEntities[i].Update();
                }
            }
        }

        #region Syncing seamap
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            EEPlayer clone = clientPlayer as EEPlayer;

            if (clone.EEPosition != SeamapPlayerShip.localship.position)
            {
                var packet = mod.GetPacket();
                packet.Write(triggerSeaCutscene);
                packet.WriteVector2(EEPosition);
                packet.Send();
            }
        }

        public override void clientClone(ModPlayer clientClone) { }

        public Vector2 EEPosition;

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(triggerSeaCutscene);
            packet.WriteVector2(EEPosition);
            packet.Send(toWho, fromWho);
        }
        #endregion

        public static void InitializeSeamap()
        {
            SeamapObjects.IslandEntities.Add(new TropicalIsland1(new Vector2(500, 500) * 4));

            SeamapObjects.IslandEntities.Add(new TropicalIsland2(new Vector2(700, 300) * 4));

            SeamapObjects.IslandEntities.Add(new VolcanoIsland(new Vector2(300, 600) * 4));

            SeamapObjects.IslandEntities.Add(new MoyaiIsland(new Vector2(300, 600) * 4));

            SeamapObjects.IslandEntities.Add(new CoralReefsIsland(new Vector2(300, 600) * 4));

            SeamapObjects.IslandEntities.Add(new MainIsland(new Vector2(1332, 1423) * 4));


            for (int i = 0; i < 300; i++)
            {
                int CloudChoose = Main.rand.Next(3);
                Vector2 CloudPos = new Vector2(Main.rand.NextFloat(-200, Main.screenWidth * 0.7f), Main.rand.NextFloat(800, Main.screenHeight + 1000));
                Vector2 dist = new Vector2(Main.screenWidth, Main.screenHeight + 1000) - CloudPos;

                if (dist.Length() > 1140)
                {
                    Texture2D cloudTexture;

                    switch (CloudChoose)
                    {
                        case 0:
                        case 1:
                            cloudTexture = GetTexture("EEMod/Seamap/SeamapAssets/DarkCloud" + (CloudChoose + 1));
                            break;

                        default:
                            cloudTexture = GetTexture("EEMod/Seamap/SeamapAssets/DarkCloud3");
                            break;
                    }

                    SeamapObjects.OceanMapElements.Add(new DarkCloud(CloudPos, cloudTexture, Main.rand.NextFloat(.6f, 1), Main.rand.NextFloat(60, 180)));
                }
            }
        }

        public void UpdateCutscenesAndTempShaders()
        {
            Filters.Scene[shad1].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad1].IsActive())
            {
                Filters.Scene.Activate(shad1, player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad1].IsActive())
                {
                    Filters.Scene.Deactivate(shad1);
                }
            }
            Filters.Scene[shad3].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad3].IsActive())
            {
                Filters.Scene.Activate(shad3, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad3].IsActive())
                {
                    Filters.Scene.Deactivate(shad3);
                }
            }
            if (timerForCutscene >= 1400)
            {
                Initialize();
                prevKey = KeyID.BaseWorldName;
                SubworldManager.EnterSubworld<CoralReefs>();                                                      
            }
            if (cutSceneTriggerTimer >= 500)
            {
                Initialize();
                prevKey = KeyID.BaseWorldName;
                SubworldManager.EnterSubworld<Sea>();
            }
        }
    }

    public partial class EEMod : Mod
    {
        public static bool IsPlayerLocalServerOwner(int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                RemoteClient client = Netplay.Clients[i];
                if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
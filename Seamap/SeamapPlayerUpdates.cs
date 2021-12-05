using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
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
using System.Diagnostics;
using EEMod.Seamap.SeamapContent;

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

        public static string prevKey = "Main";

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
            if (Player.GetModPlayer<EEPlayer>().quickOpeningFloat > 0.01f)
            {
                Player.GetModPlayer<EEPlayer>().quickOpeningFloat -= Player.GetModPlayer<EEPlayer>().quickOpeningFloat / 20f;
            }
            else
            {
                Player.GetModPlayer<EEPlayer>().quickOpeningFloat = 0;
            }

            Filters.Scene["EEMod:SeaOpening"].GetShader().UseIntensity(Player.GetModPlayer<EEPlayer>().quickOpeningFloat);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:SeaOpening"].IsActive())
            {
                Filters.Scene.Activate("EEMod:SeaOpening", Player.Center).GetShader().UseIntensity(Player.GetModPlayer<EEPlayer>().quickOpeningFloat);
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
            #endregion

            seamapUpdateCount++;

            #region Placing SeamapObjects.Islands
            if (seamapUpdateCount == 1)
            {
                Debug.WriteLine("initializing");
                InitializeSeamap();
            }
            #endregion

            #region If ship crashes
            if (SeamapObjects.localship.shipHelth <= 0)
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
            foreach (var obj in SeamapObjects.SeamapEntities)
            {
                if(!(obj is Island))
                {
                    return;
                }

                Island island = obj as Island;

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
                                Player.GetModPlayer<EEPlayer>().importantCutscene = true;
                                break;

                            case IslandID.TropicalIsland2:
                                Player.GetModPlayer<EEPlayer>().importantCutscene = true;
                                break;

                            case IslandID.VolcanoIsland:
                                Player.GetModPlayer<EEPlayer>().importantCutscene = true;
                                break;

                            case IslandID.MoyaiIsland:
                                Player.GetModPlayer<EEPlayer>().importantCutscene = true;
                                break;

                            case IslandID.CoralReefs:
                                Player.GetModPlayer<EEPlayer>().importantCutscene = true;
                                break;

                            case IslandID.MainIsland:
                                ReturnHome();
                                break;
                        }

                        prevKey = KeyID.Sea;
                    }

                    isCollidingWithAnyIsland = true;

                    Player.ClearBuff(BuffID.Cursed);
                    Player.ClearBuff(BuffID.Invisibility);
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
            if (Player.GetModPlayer<EEPlayer>().importantCutscene)
            {
                EEMod.Noise2D.NoiseTexture = ModContent.Request<Texture2D>("EEMod/Textures/Noise/noise").Value;
                Filters.Scene["EEMod:Noise2D"].GetShader().UseOpacity(cutSceneTriggerTimer / 180f);

                if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Noise2D"].IsActive())
                {
                    Filters.Scene.Activate("EEMod:Noise2D", Player.Center).GetShader().UseOpacity(0);
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

            Player.position = Player.oldPosition;
            Player.invis = true;

            Player.AddBuff(BuffID.Cursed, 100000);
            Main.GameZoomTarget = 1f;
        }
        public override void clientClone(ModPlayer clientClone) { }

        public Vector2 EEPosition;

        public static void InitializeSeamap()
        {
            SeamapObjects.InitObjects(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 450, Seamap.SeamapContent.Seamap.seamapWidth - 100));

            SeamapObjects.NewSeamapObject(new MainIsland(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 402, Seamap.SeamapContent.Seamap.seamapHeight - 118)));

            for (int i = 0; i < 15; i++)
            {
                switch (Main.rand.Next(0, 6)) 
                {
                    case 0:
                        SeamapObjects.NewSeamapObject(new Rock1(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                    case 1:
                        SeamapObjects.NewSeamapObject(new Rock2(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                    case 2:
                        SeamapObjects.NewSeamapObject(new Rock3(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                    case 3:
                        SeamapObjects.NewSeamapObject(new Rock4(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                    case 4:
                        SeamapObjects.NewSeamapObject(new Rock5(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                    case 5:
                        SeamapObjects.NewSeamapObject(new Rock6(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - (i * 67), Seamap.SeamapContent.Seamap.seamapHeight - 800)));
                        break;
                }
            }

            for (int i = 0; i < 15; i++)
            {
                switch (Main.rand.Next(0, 6))
                {
                    case 0:
                        SeamapObjects.NewSeamapObject(new Rock1(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                    case 1:
                        SeamapObjects.NewSeamapObject(new Rock2(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                    case 2:
                        SeamapObjects.NewSeamapObject(new Rock3(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                    case 3:
                        SeamapObjects.NewSeamapObject(new Rock4(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                    case 4:
                        SeamapObjects.NewSeamapObject(new Rock5(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                    case 5:
                        SeamapObjects.NewSeamapObject(new Rock6(new Vector2(Seamap.SeamapContent.Seamap.seamapWidth - 1000, Seamap.SeamapContent.Seamap.seamapHeight - (i * 53))));
                        break;
                }
            }

            //SeamapObjects.IslandEntities.Add(new TropicalIsland1(new Vector2(500, 500)));

            //SeamapObjects.IslandEntities.Add(new TropicalIsland2(new Vector2(700, 300)));

            //SeamapObjects.IslandEntities.Add(new VolcanoIsland(new Vector2(300, 600)));

            //SeamapObjects.IslandEntities.Add(new MoyaiIsland(new Vector2(300, 600)));

            //SeamapObjects.IslandEntities.Add(new CoralReefsIsland(new Vector2(300, 600)));

            //SeamapObjects.IslandEntities.Add(new MainIsland(new Vector2(200, 200)));
        }

        public void UpdateCutscenesAndTempShaders()
        {
            Filters.Scene[RippleShader].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[RippleShader].IsActive())
            {
                Filters.Scene.Activate(RippleShader, Player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[RippleShader].IsActive())
                {
                    Filters.Scene.Deactivate(RippleShader);
                }
            }
            Filters.Scene[SeaTransShader].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[SeaTransShader].IsActive())
            {
                Filters.Scene.Activate(SeaTransShader, Player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[SeaTransShader].IsActive())
                {
                    Filters.Scene.Deactivate(SeaTransShader);
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

    /*public partial class EEMod : Mod
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
    }*/
}